using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

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
        [field: SerializeField] public float UnitMorale { get; private set; }
        [field: SerializeField] public Canvas UnitCanvas { get; private set; }


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
        private Command _nextCommand;
        private Command _activeCommand;
        private Command _stashedCommand;
        
        private Command _executingCommand;

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
                    _stashedCommand = _activeCommand;
                }
                
                // Holds stashed command hostage until it's complete
                _activeCommand = command;
            }
            else
            {
                _commandQueue.Enqueue(command);
            }
        }

        private void ExecuteCommand(Command command)
        {
            if (!command) return;

            // Indicate to the command it can be begin executing.
            command.enabled = true;
            command.Execute();
        }

        // Destroys a command instance.
        private void DestroyCommand(ref Command command)
        {
            // Guard Clause ensuring command is valid.
            if (!command) return;

            command.enabled = false;
            Destroy(command);
            command = null;
        }

        // Clears Command Queue, NextCommand, and ActiveCommand and ensures Command instances are destroyed.
        private void DestroyCommandQueue()
        {
            while (_commandQueue.Count > 0)
            {
                Command command = _commandQueue.Dequeue();
                DestroyCommand(ref command);
            }
        }

        // Clears next and active commands
        private void DestroyStagedCommands()
        {
            DestroyCommand(ref _nextCommand);
            DestroyCommand(ref _activeCommand);
            DestroyCommand(ref _stashedCommand);
        }

        private void CommandUpdate()
        {
            // If command is ready to execute run it
            if (_executingCommand)
            {
                ExecuteCommand(_executingCommand);
                _executingCommand = null;
            } 
            // If there is an active command loaded
            else if (_activeCommand)
            {
                if (!_activeCommand.enabled)
                {
                    _executingCommand = _activeCommand;

                }
                else if (_activeCommand.IsFinished())
                {
                    DestroyCommand(ref _activeCommand);
                }
            }
            else
            {
                // If there is a stashed command pop it out
                if (_stashedCommand)
                {
                    _activeCommand = _stashedCommand;
                    _stashedCommand = null;
                }
                else
                {
                    _activeCommand = _commandQueue.Count > 0 ? _commandQueue.Dequeue() : null;
                }
            }
        }

        private void Update()
        {
            CommandUpdate();

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
            float newMorale = UnitMorale - _moraleTickDmg;
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