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