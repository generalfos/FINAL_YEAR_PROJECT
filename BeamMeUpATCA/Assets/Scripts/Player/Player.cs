using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using IASub = BeamMeUpATCA.InputActionSubscription;
using IASubscriber = BeamMeUpATCA.InputActionSubscriber;


namespace BeamMeUpATCA
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        #region Player Setup

        [SerializeField] private PlayerUI _playerUI;
        [field: SerializeField] private CameraController PlayerCamera { get; set; }
        [field: SerializeField] private UnitCommander Commander { get; set; }

        [SerializeField]
        private CameraMover _cameraMover;
        
        private Camera _camera;
        // private CameraFocus _cameraFocus;
        private InputActionAsset _playerActions;
        private HashSet<Unit> _selectedUnits;

        private void Awake() 
        {
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _actions = _playerInput.actions;

            if (_playerInput.camera == null) {
                Debug.LogWarning("PlayerInput requires a camera to be set. Using MainCamera instead.");
                _playerInput.camera = Camera.main;
            }

            // Setup camera and camera mover script
            _camera = _playerInput.camera;
            _cameraMover.PlayerCamera = _camera;
            // _cameraFocus.PlayerCamera = _camera;

            DefineSubscriptions();
        }
        #endregion // Player Setup

        #region InputAction/Action Subscriptions

        private InputAction _quit;

        public InputAction PrimaryAction { get; private set; }
        public InputAction SecondaryAction { get; private set; }
        public InputAction TertiaryAction { get; private set; }
        public InputAction Pointer { get; private set; }
        public InputAction CameraPan { get; private set; }
        public InputAction CameraScroll { get; private set; }
        public InputAction CameraRotate { get; private set; }
        public InputAction CameraFocus { get; private set;  }

        private Dictionary<InputAction, Command> _commandActions;
        private String _cs = "Default/";
        private String _cmdString = "Command: ";

        private void DefineInputActions()
        {
            PrimaryAction = _playerActions.FindAction(_cs + "Primary Action");
            SecondaryAction = _playerActions.FindAction(_cs + "Secondary Action");
            TertiaryAction = _playerActions.FindAction(_cs + "Tertiary Action");
            Pointer = _playerActions.FindAction(_cs + "Pointer");
            CameraPan = _playerActions.FindAction(_cs + "Pan Camera");
            CameraScroll = _playerActions.FindAction(_cs + "Scroll Camera");
            CameraRotate = _playerActions.FindAction(_cs + "Rotate Camera");
            CameraFocus = _playerActions.FindAction((_cs + "Focus Camera"));

            _commandActions = new Dictionary<InputAction, Command>() 
            {
                {_playerActions.FindAction(_cs + _cmdString + "Cancel"), new CancelCommand()},
                {_playerActions.FindAction(_cs + _cmdString + "Move"), new MoveCommand(this)},
                {_playerActions.FindAction(_cs + _cmdString + "Rotate"), new RotateCommand(this)}
            };

        private Dictionary<IASubscriber, IASub[]> ActionSubscriptions;

        private void DefineSubscriptions()
        {
            // Binds subscribers to subscriptions to allow actions to trigger any actions
            ActionSubscriptions = new Dictionary<IASubscriber, IASub[]>() 
            {
                {new IASubscriber(_actions["Primary Action"]), 
                    new[] { new IASub(ctx => Commander.SelectUnit(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Secondary Action"]), 
                    new[] { new IASub(ctx => Commander.DeselectAllUnits(), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Tertiary Action"]), 
                    new[] { 
                        new IASub(ctx => PlayerCamera.DragRotation = true, (true, false, false)),
                        new IASub(ctx => PlayerCamera.DragRotation = false, (false, false, true))}
                },
                {new IASubscriber(_actions["Pan Camera"]), 
                    new[] { new IASub(ctx => PlayerCamera.Camera2DAdjust = ctx.ReadValue<Vector2>(), IASub.UPDATE)}
                },
                {new IASubscriber(_actions["Scroll Camera"]), 
                    new[] { new IASub(ctx => PlayerCamera.CameraZoomAdjust = ctx.ReadValue<float>(), IASub.UPDATE)}
                },
                {new IASubscriber(_actions["Focus Camera"]), 
                    new[] { new IASub(ctx =>  PlayerCamera.FocusCamera(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Quit"]), 
                    new[] { new IASub(ctx => Application.Quit(), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Cancel"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits(new CancelCommand(this)), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Move"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits(new MoveCommand(this)), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Rotate"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits(new RotateCommand(this)), IASub.PREFORMED)}
                },
            };

            CameraScroll.started += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.performed += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.canceled += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            // TODO
            CameraFocus.performed += ctx => FocusCamera(Pointer.ReadValue<Vector2>());
        }

        private void AddSubscriptions()
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) 
            {
                foreach (IASub sub in ActionSubscriptions[subscriber]) 
                {
                    subscriber.AddSubscription(sub);
                }
            }
        }
        #endregion // Input Action Setup
        
        /*
         * Attempts to translate a Pointer position into a
         * camera position
         */
        private void FocusCamera(Vector2 targetPos)
        {
            GameObject selectedBuilding = _playerSelector.SelectGameObject(_camera, Pointer, PrimaryAction);
            if (selectedBuilding == null) { return; }
            if (selectedBuilding.GetComponent<Building>() == null) { return; }

            // Find current camera location
            Vector3 currentPos = _camera.transform.position;
            Debug.Log("CAMERA CURRENTLY AT: " + currentPos);
            // Find difference between target location object and camera location
            Vector3 buildingPos = selectedBuilding.transform.position;
            Debug.Log(selectedBuilding.name + " positioned at " + buildingPos);
            // Instruct camera to look above object and this diff using CameraMover()
        }

        private void OnEnable() 
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(true);}
        }

        private void OnDisable() 
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(false);}
        }

        #endregion // InputAction/Action Subscriptions
    }
}