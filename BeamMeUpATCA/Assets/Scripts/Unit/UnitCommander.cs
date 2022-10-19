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
            if (_selectedUnits.Contains(unit)) return;

            GameManager.UI.ClearModelView();
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
            GameManager.UI.ClearModelView();
            GameManager.UI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        public void CommandUnits<T>(bool queue, Vector2 position) where T : Command
        {
            // cacheRayData to allow Commands to handle how Ray is used, while also saving on performance
            (Ray, float) cacheRayData = (ActiveCamera.ScreenPointToRay(position), ActiveCamera.farClipPlane);

            int offset = 0;
            foreach (Unit unit in _selectedUnits)
            {
                Type commandType = ToggleCommandCheck(typeof(T), unit);
                Command command = (Command) unit.gameObject.AddComponent(commandType);

                // Command initialization
                command.Offset = offset;
                command.RayData = cacheRayData;
                command.ResetQueue = command.SkipQueue = !queue;
                
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
            // Checks if building is workable/enterable for toggle handling
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
