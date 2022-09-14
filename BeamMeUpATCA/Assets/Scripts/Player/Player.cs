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

        private PlayerInput _playerInput;
        private InputActionAsset _actions;

        private void Awake() 
        {
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _actions = _playerInput.actions;

            if (_playerInput.camera == null) {
                Debug.LogWarning("PlayerInput requires a camera to be set. Using MainCamera instead.");
                _playerInput.camera = Camera.main;
            }

            // Dependencies for PlayerCamera and Commander.
            PlayerCamera.ActiveCamera = _playerInput.camera;
            Commander.ActiveCamera = _playerInput.camera;
            Commander.PlayerUI = _playerUI;

            DefineSubscriptions();
        }
        #endregion // Player Setup

        #region InputAction/Action Subscriptions

        private Vector2 PointerPosition { get 
        {
            if (_actions == null) { return Vector2.zero; }
            return _actions["Pointer"].ReadValue<Vector2>(); 
        }}
            CameraFocus = _playerActions.FindAction((_cs + "Focus Camera"));

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

            AddSubscriptions();
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

        private void OnEnable() 
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(true);}
        }
>>>>>>> 6828bf297cf7172566c8e167de6e9fc5f83f878f

        private void OnDisable() 
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(false);}
        }

        #endregion // InputAction/Action Subscriptions
    }
}