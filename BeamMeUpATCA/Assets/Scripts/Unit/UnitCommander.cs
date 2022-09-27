using UnityEngine;
using System;
using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public class UnitCommander : MonoBehaviour
    {
        private List<Unit> _selectedUnits;

        public PlayerUI PlayerUI { private get; set; }
        public Camera ActiveCamera { private get; set; }

        private void Awake() {
            _selectedUnits = new List<Unit>();
        }
        
        public void SelectUnit(Vector2 screenPoint) 
        {
            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, screenPoint, new [] {"Unit"});

            // Left clicking on empty space will deselect all selected units.
            if (selectedObject == null) {
                DeselectAllUnits();
                return;
            }

            Unit selectedUnit = selectedObject.GetComponent<Unit>();

            // Guard clause to check if selected unit is actually a unit.
            if (selectedUnit == null) { return; }

            // Prevent selecting the same unit multiple times
            if (_selectedUnits.Contains(selectedUnit)) { return; }

            this.PlayerUI.SelectUnit(selectedUnit);
            _selectedUnits.Add(selectedUnit);
        }

        public void DeselectAllUnits() 
        {
            this.PlayerUI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        public void CommandUnits<T>(Vector2 position)
        {
            for(var i = 0; i < _selectedUnits.Count; i ++) 
            {
                if (typeof(T).IsSubclassOf(typeof(Command))) 
                {
                    // Can't cast T directly to Command so we cast to object first. 
                    object obj = (object) _selectedUnits[i].gameObject.AddComponent(typeof(T));

                    // This is a safe cast as T is type checked at runtime to be a Command
                    Command command = (Command) obj;

                    // Command initialization
                    command.Position = position;
                    command.ActiveCamera = ActiveCamera;
                    command.Offset = i;

                    Debug.Log("Commanding " + _selectedUnits[i].name + " to preform the " + command.Name + " command");
                    _selectedUnits[i].AddCommand(command);
                }
            }
        }
    }
}
