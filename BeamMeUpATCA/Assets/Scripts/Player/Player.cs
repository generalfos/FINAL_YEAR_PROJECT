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
        private InputActionAsset _playerActions;

        private void Awake() 
        {
            //_selectedUnits = new List<Unit>();
            _playerActions = _playerInput.actions;

            DefineInputActions();
        }
        #endregion // Player Setup

        #region Input Action Setup

        private InputAction _quit;

        public InputAction PrimaryAction { get; private set; }
        public InputAction SecondaryAction { get; private set; }
        public InputAction Pointer { get; private set; }
        public InputAction CameraPan { get; private set; }

        private Dictionary<InputAction, Command> _commandActions;
        private String _cs = "Default/";
        private String _cmdString = "Command: ";

        private void DefineInputActions()
        {
            PrimaryAction = _playerActions.FindAction(_cs + "Primary Action");
            SecondaryAction = _playerActions.FindAction(_cs + "Secondary Action");
            Pointer = _playerActions.FindAction(_cs + "Pointer");
            CameraPan = _playerActions.FindAction(_cs + "Pan Camera");

            _commandActions = new Dictionary<InputAction, Command>() 
            {
                {_playerActions.FindAction(_cs + _cmdString + "Cancel"), new CancelCommand()},
                {_playerActions.FindAction(_cs + _cmdString + "Move"), new MoveCommand()}
            };

            _quit = _playerActions.FindAction("Default/Quit");
        }

        private void OnEnable() 
        {
            CommandActionSubscription(true);
            _quit.performed += ctx => Application.Quit();
        }

        private void OnDisable() 
        {
            CommandActionSubscription(false);
            _quit.performed -= ctx => Application.Quit();
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

        // Should be HashSet<Unit> to avoid duplicates. Is List<Unit> for now for Editor serialization.
        [SerializeField]
        private List<Unit> _selectedUnits; 

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