using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BeamMeUpATCA 
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, IInteractable
    {
        #region Unit Properties

        // Mask for checking Unit Layer.
        public Mask Mask => Mask.Unit;

        public enum UnitType
        {
            Engineer,
            Scientist
        };

        private static readonly Dictionary<UnitType, Color> ColorDict
            = new Dictionary<UnitType, Color>
            {
                { UnitType.Engineer, Color.red },
                { UnitType.Scientist, Color.blue }
            };

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public UnitType UnitClass { get; private set; } = UnitType.Engineer;

        private float _moral;
        public float UnitMorale { 
            get => _moral; 
            private set {
                _moral = Mathf.Clamp(value, 0f, _maxMorale);

                // Update the UI
                if (!(healthIndicator is null)) 
                    healthIndicator.color = GameManager.UI.GetMendableColour(_moral, _maxMorale, healthScheme);
            } 
        }
        [field: SerializeField] public Canvas UnitCanvas { get; private set; }

        [SerializeField] private UIManager.ColorScheme healthScheme;
        public Image healthIndicator;
        
        private UIManager.UnitState _unitState;
        public UIManager.UnitState UnitState {
            get => _unitState;
            set {
                _unitState = value;

                // Update the UI
                if (!(actionIndicator is null)) 
                    actionIndicator.sprite = GameManager.UI.GetUnitState(_unitState);
            }
        }
        public Image actionIndicator;

        private UnitPathfinder _pathfinder;
        public UnitPathfinder Pathfinder => _pathfinder ??= new UnitPathfinder(this);

        // Sets color to black if UnitClass is not defined.
        public Color UnitColor => ColorDict[UnitClass];
        
        private float _moraleTickDmg;
        private float _maxMorale;

        [Obsolete("This is being replaced in new building feature branch")]
        public float GetInTownCounter() => 0f;

        private void Awake()
        {
            // Sets current layer of this.gameObject to match the appropriate Mask.Unit.
            gameObject.layer = Mask.Layer(Mask);

            _maxMorale = 100;
            UnitMorale = _maxMorale;
            _moraleTickDmg = 1;

            UnitState = UIManager.UnitState.IDLE; // Default to IDLE

            // Fetching the Canvas from Child
            UnitCanvas = this.GetComponentInChildren<Canvas>();
            if (UnitCanvas == null) Debug.LogWarningFormat("Unable to find a world space canvas in unit {0}'s children", this.gameObject.name);
            else {
                // Turn canvas off if it exists, this will be activated by
                // external scripts to display things such as Unit Selection.
                UnitCanvas.gameObject.SetActive(false);
            }

            _commandQueue = new Queue<Command>();
        }

        #endregion // End of 'Unit Properties'
        
        #region Enter Building

        public Building BuildingInside { get; private set; } = null;

        private Renderer _renderer;
        private Renderer Renderer => _renderer ??= GetComponent<Renderer>();
        
        private Collider _collider;
        private Collider Collider => _collider ??= GetComponent<Collider>();

        public void EnterBuilding(Building building)
        {
            // Set self inside building to building arg
            BuildingInside = building;

            UnitState = UIManager.UnitState.REPAIR;
            
            // Hide unit, disable clicking, and set unit inside building.
            Renderer.enabled = false;
            Collider.enabled = false;

            // Set X and Z positions of Unit. Y height is kept to prevent world clipping
            Vector3 buildingPos = BuildingInside.transform.position;
            Transform unitTransform = transform;
            unitTransform.position = new Vector3(buildingPos.x, unitTransform.position.y, buildingPos.z);
        }
        
        public void ExitBuilding()
        {
            // If Building is null then exit building does nothing just return
            if (BuildingInside is null) return;
            
            // Hide unit, disable clicking, and set unit inside building.
            Renderer.enabled = true;
            Collider.enabled = true;

            // Set X and Z positions of Unit. Y height is kept to prevent world clipping
            Vector3 buildingAnchorPos = BuildingInside.Anchors.GetAnchorPoint();
            Transform unitTransform = transform;
            unitTransform.position = new Vector3(buildingAnchorPos.x, unitTransform.position.y, buildingAnchorPos.z);
            
            // Set self inside building to null
            BuildingInside = null;
        }

        #endregion // End of 'Enter Building'
        

        #region Commanding

        private Queue<Command> _commandQueue;
        private Command _activeCommand;
        private Command _priorityCommand;
        private Command _nextCommand;

        public void AddCommand(Command command)
        {

            if (command.ResetQueue)
            {
                // Destroy the entire command queue, next command and active command
                DestroyCommandQueue();
                DestroyStagedCommands();
            }

            if (command.SkipQueue)
            {
                if (_activeCommand != null)
                {
                    _activeCommand.enabled = false;
                }

                // Priority command holds the command queue hostage stopping activeCommand
                // until the priority command has finished executing.
                _priorityCommand = command;
                ExecuteCommand(_priorityCommand);
            }
            else
            {
                _commandQueue.Enqueue(command);
                if (_nextCommand == null)
                {
                    _nextCommand = _commandQueue.Dequeue();
                }
            }
        }

        private void ExecuteCommand(Command command)
        {
            if (command is null) return;

            // Change UI State Graphic to reflect command
            if (command is GotoCommand) UnitState = UIManager.UnitState.MOVE;
            if (command is MendCommand) UnitState = UIManager.UnitState.REPAIR;
            
            // Indicate to the command it can be begin executing.
            command.enabled = true;
            command.Execute();

        }

        private Command ExecuteAndLoadCommand(Command command)
        {
            if (command is null) return null;

            ExecuteCommand(command);

            // If there is a next command then return it. Otherwise return null.
            return _commandQueue.Count != 0 ? _commandQueue.Dequeue() : null;
        }

        // Destroys a command instance.
        private void DestroyCommand(Command command)
        {
            // Guard Clause ensuring command is valid.
            if (command is null)return;

            // Debug.Log("Destroying Command: " + command.Name);
            Destroy(command);
        }

        // Clears Command Queue, NextCommand, and ActiveCommand and ensures Command instances are destroyed.
        private void DestroyCommandQueue()
        {
            while (_commandQueue.Count > 0)
            {
                DestroyCommand(_commandQueue.Dequeue());
            }
        }

        // Clears next and active commands
        private void DestroyStagedCommands()
        {
            DestroyCommand(_nextCommand);
            _nextCommand = null;
            DestroyCommand(_activeCommand);
            _activeCommand = null;
        }

        // Handles execution order of commands
        private void CommandUpdate()
        {
            // No commands should be running while a priority command exists.
            if (!(_priorityCommand is null))
            {
                // Guard Clause to allow priority command to run enabled.
                if (!_priorityCommand.IsFinished()) return;

                DestroyCommand(_priorityCommand);
                _priorityCommand = null;

                // Restore whatever activeCommand was running.
                if (!(_activeCommand is null))
                {
                    _activeCommand.enabled = true;
                }
            }
            else
            {
                // If the active command is null run the next command.
                if (_activeCommand is null && !(_nextCommand is null))
                {
                    _activeCommand = _nextCommand;
                    // ExecuteCommand() returns the next command in queue
                    _nextCommand = ExecuteAndLoadCommand(_activeCommand);
                }

                // Evaluation left to right validates Command.IsFinished() check.
                if (_activeCommand is null || !_activeCommand.IsFinished()) return;
                
                // If Command.IsFinished() delete object and set activeCommand to null.
                DestroyCommand(_activeCommand);
                _activeCommand = null;
            }
        }

        private void UnitStateUpdate() {
            // Check if the Unit is doing nothing (IDLE)
            if (_commandQueue.Count == 0 && _activeCommand is null && _priorityCommand is null && BuildingInside is null) {
                UnitState = UIManager.UnitState.IDLE;
                return;
            }

            // If the player is inside a specific building, display dedicated graphic
            if (BuildingInside is EngineerStation)      { UnitState = UIManager.UnitState.SHED; return;    }
            if (BuildingInside is WeatherStation)       { UnitState = UIManager.UnitState.WEATHER; return; }
            if (BuildingInside is ObservationStation)   { UnitState = UIManager.UnitState.OBS; return;     }
            
            // No Graphic for Rest, default to IDLE "zzz"
            if (BuildingInside is BusStop)   { UnitState = UIManager.UnitState.IDLE; return; }

            // Building has no graphic, default to "Mend"
            if (!(BuildingInside is null))   { UnitState = UIManager.UnitState.REPAIR; return; }
        
        }

        private void Update()
        {
            CommandUpdate();
            UnitStateUpdate();

            if (BuildingInside is BusStop)
            {
                UnitMorale = _maxMorale;
            } else
            {
                TakeTickDamage();
            }
        }

        #endregion // End of 'Commanding'

        #region Morale Degradation

        private void TakeTickDamage()
        {
            float newMorale = UnitMorale - (_moraleTickDmg * Time.deltaTime);
            if (newMorale <= 0)
            {
                newMorale = 0;
            }

            if (newMorale >= _maxMorale)
            {
                newMorale = _maxMorale; //do not heal over full
            }
            UnitMorale = newMorale;
        }
        
        #endregion // End of 'Morale Degradation'
    }
}