using UnityEngine;
using System;
using System.Collections.Generic;
using BeamMeUpATCA.Extensions;

namespace BeamMeUpATCA
{
    public class UnitCommander : MonoBehaviour
    {
        private Camera _camera;
        public Camera ActiveCamera { 
            private get { return this.SafeComponent<Camera>(ref _camera); }
            set { _camera = value; }}

        // List of units that are currently selected.
        private List<Unit> _selectedUnits;

        private void Awake() {
            _selectedUnits = new List<Unit>();
        }

        public void SelectUnit(Unit unit)
        {
            // Prevent selecting the same unit multiple times
            if (_selectedUnits.Contains(unit)) return;

            GameManager.UI.SelectUnit(unit);
            _selectedUnits.Add(unit);
        }

        public void SelectUnit(Vector2 screenPoint)
        {
            IInteractable interactable = Selector.SelectGameObject(ActiveCamera, screenPoint, Mask.Unit);

            if (interactable is Unit selectedUnit)
            {
                SelectUnit(selectedUnit);
            }
            else
            {
                DeselectAllUnits();
            }
        }

        public void DeselectAllUnits() 
        {
            GameManager.UI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        public void CommandUnits<T>(Vector2 position) where T : Command
        {
            int offset = 0;
            foreach (Unit unit in _selectedUnits)
            {
                Type commandType = ToggleCommandCheck(typeof(T), unit);
                Command command = (Command) unit.gameObject.AddComponent(commandType);

                // Command initialization
                command.ActiveCamera = ActiveCamera;
                command.Offset = offset;
                // TODO: Vector2 to Vector3 should be calculated on init, rather than when needed (like with pathfinder)
                // TODO: This would require Pathfinder to also be refactored to take a Vector3 (instead of Vector2)
                command.Position = position; 
                
                #if UNITYEDITOR
                Debug.Log("Commanding " + _selectedUnits[i].name + " to preform the " + command.Name + " command");
                #endif
                
                unit.AddCommand(command);

                // Iterate for following units
                offset++;
            }
        }

        public Type ToggleCommandCheck(Type command, Unit unit)
        {
            Building building = unit.BuildingInside;
            if (command == typeof(EnterCommand))
            {
                if (building) command = typeof(LeaveCommand);
            }
            if (command == typeof(WorkCommand))
            {
                if (building && building is Workable && Equals((building as Workable).WorkingUnit, unit))
                {
                    command = typeof(RestCommand);
                }
            }
            return command;
        }
    }
}
