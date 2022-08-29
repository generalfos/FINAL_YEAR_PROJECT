using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace BeamMeUpATCA
{

    public class Player : MonoBehaviour
    {
        #region Setup
        
        [SerializeField]
        private PlayerInput _playerInput;
        private InputActionAsset _playerActions;

        private List<Unit> _selectedUnits;

        private InputAction _stop; 
        private InputAction _quit;

        private void Awake() 
        {
            _selectedUnits = new List<Unit>();
             _playerActions = _playerInput.actions;
            //DefineInputActions();

            _stop = _playerActions.FindAction("Default/Stop");
            _quit = _playerActions.FindAction("Default/Quit");
        }

        #endregion

        // private void CommandUnits(Command command) 
        // {
        //     foreach(Unit unit in _selectedUnits) 
        //     {
        //         unit.AddCommand();
        //     }
        // }

        private void OnEnable() 
        {
            _stop.performed += ctx => Application.Quit();
            _quit.performed += ctx => Application.Quit();
        }

        private void OnDisable() 
        {
            _stop.performed -= ctx => Application.Quit();
            _quit.performed -= ctx => Application.Quit();
        }


        // TODO: Will fix on next commit
        // private Dictionary<Command, InputAction> _issueCommandTriggers;

        // private void DefineInputActions() 
        // {
        //     _issueCommandTriggers = new Dictionary<Command, InputAction>
        //     {
        //         { new StopCommand() , _playerActions.FindAction("Default/Stop") }
        //     };
        // }
    }
}