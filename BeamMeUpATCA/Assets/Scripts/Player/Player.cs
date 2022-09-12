using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace BeamMeUpATCA
{

    public class Player : MonoBehaviour
    {
        #region Player Setup
        
        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private PlayerUI _playerUI;

        [SerializeField]
        private PlayerSelector _playerSelector;

        [SerializeField]
        private CameraMover _cameraMover;

        private Camera _camera;
        private InputActionAsset _playerActions;
        private HashSet<Unit> _selectedUnits;

        private void Awake() 
        {
            _playerActions = _playerInput.actions;

            // Setup camera and camera mover script
            _camera = _playerInput.camera;
            _cameraMover.PlayerCamera = _camera;

            _selectedUnits = new HashSet<Unit>();
            DefineInputActions();
        }
        #endregion // Player Setup

        #region Input Action Setup

        private InputAction _quit;

        public InputAction PrimaryAction { get; private set; }
        public InputAction SecondaryAction { get; private set; }
        public InputAction TertiaryAction { get; private set; }
        public InputAction Pointer { get; private set; }
        public InputAction CameraPan { get; private set; }
        public InputAction CameraScroll { get; private set; }
        public InputAction CameraRotate { get; private set; }

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

            _commandActions = new Dictionary<InputAction, Command>() 
            {
                {_playerActions.FindAction(_cs + _cmdString + "Cancel"), new CancelCommand()},
                {_playerActions.FindAction(_cs + _cmdString + "Move"), new MoveCommand(this)},
                {_playerActions.FindAction(_cs + _cmdString + "Rotate"), new RotateCommand(this)}
            };

            _quit = _playerActions.FindAction("Default/Quit");
        }

        private void OnEnable() 
        {
            CommandActionSubscription(true);
            _quit.performed += ctx => Application.Quit();
            PrimaryAction.performed += ctx => SelectUnits();
            SecondaryAction.performed += ctx => 
            {
                // If right click is on something then command action. Otherwise deselect
                DeselectAllUnits();
            };
            TertiaryAction.started += ctx => _cameraMover.DragRotation = true;
            TertiaryAction.canceled += ctx => _cameraMover.DragRotation = false;

            CameraPan.started += ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();
            CameraPan.performed += ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();
            CameraPan.canceled += ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();

            CameraScroll.started += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.performed += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.canceled += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
        }

        private void OnDisable() 
        {
            CommandActionSubscription(false);
            _quit.performed -= ctx => Application.Quit();
            PrimaryAction.performed -= ctx => SelectUnits();
            SecondaryAction.performed -= ctx => 
            {
                // If right click is on something then command action. Otherwise deselect
                DeselectAllUnits();
            };
            TertiaryAction.started -= ctx => _cameraMover.DragRotation = true;
            TertiaryAction.canceled -= ctx => _cameraMover.DragRotation = false;

            CameraPan.started -= ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();
            CameraPan.performed -= ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();
            CameraPan.canceled -= ctx => _cameraMover.Camera2DAdjust = ctx.ReadValue<Vector2>();

            CameraScroll.started += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.performed += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
            CameraScroll.canceled += ctx => _cameraMover.CameraZoomAdjust = CameraScroll.ReadValue<float>();
        }

        // Registers/De-registers Command Action 
        private void CommandActionSubscription(bool subscribe) 
        {
            foreach (InputAction action in _commandActions.Keys) 
            {
                if (subscribe) 
                {
                    action.performed += ctx => CommandSelectedUnits(_commandActions[action]);
                } 
                else
                {
                    action.performed -= ctx => CommandSelectedUnits(_commandActions[action]);
                }
            }

        }
        #endregion // Input Action Setup

        private void SelectUnits() 
        {
            GameObject selectedObject = _playerSelector.SelectGameObject(_camera, Pointer, PrimaryAction);
            // Guard clause to check valid return from function.
            if (selectedObject == null) { return; }
            if (selectedObject.GetComponent<Unit>() == null) { return; }

            Unit selectedUnit = selectedObject.GetComponent<Unit>();
            if (selectedUnit.UnitClass == Unit.UnitType.Array) { DeselectAllUnits(); }
            _playerUI.SelectUnit(selectedUnit);
            _selectedUnits.Add(selectedUnit);
        }

        private void DeselectAllUnits() 
        {
            _playerUI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        private void CommandSelectedUnits(Command command)
        {
            foreach (Unit unit in _selectedUnits) 
            {
                Debug.Log("Commanding " + unit.name + " to preform the " + command.Name + " command");
                unit.AddCommand(command);
            }
        }
    }
}