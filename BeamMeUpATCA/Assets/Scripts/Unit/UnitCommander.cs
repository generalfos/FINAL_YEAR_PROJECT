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

        private PlayerUI _playerUI;
        public PlayerUI UI { 
            private get { return this.SafeComponent<PlayerUI>(ref _playerUI); }
            set { _playerUI = value; }}
  
        // List of units that are currently selected.
        private List<Unit> _selectedUnits;

        private void Awake() {
            _selectedUnits = new List<Unit>();
        }
        
        public void SelectUnit(Vector2 screenPoint) 
        {
            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, screenPoint, new [] {"Unit"});

            // Left clicking on empty space will deselect all selected units.
            if (!selectedObject) {
                DeselectAllUnits();
                return;
            }

            Unit selectedUnit = selectedObject.GetComponent<Unit>();

            // Guard clause to check if selected unit is actually a unit.
            if (selectedUnit == null) { return; }

            // Prevent selecting the same unit multiple times
            if (_selectedUnits.Contains(selectedUnit)) { return; }

            this.UI.SelectUnit(selectedUnit);
            _selectedUnits.Add(selectedUnit);
        }

        public void DeselectAllUnits() 
        {
            this.UI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        public void CommandUnits<T>(Vector2 position) where T : Command
        {
            for(var i = 0; i < _selectedUnits.Count; i ++) 
            {
                Command command = _selectedUnits[i].gameObject.AddComponent<T>();

                // Command initialization
                command.ActiveCamera = ActiveCamera;
                command.Offset = i;
                // TODO: Vector2 to Vector3 should be calculated on init, rather than when needed (like with pathfinder)
                // TODO: This would require Pathfinder to also be refactored to take a Vector3 (instead of Vector2)
                command.Position = position; 


                Debug.Log("Commanding " + _selectedUnits[i].name + " to preform the " + command.Name + " command");
                _selectedUnits[i].AddCommand(command);
            }
        }
    }
}
