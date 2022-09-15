using UnityEngine;
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
            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, screenPoint, new List<string>{"Unit", "Array"});
            // Guard clause to check valid return from function.
            if (selectedObject == null) { return; }
            if (selectedObject.GetComponent<Unit>() == null) { return; }

            Unit selectedUnit = selectedObject.GetComponent<Unit>();
            if (selectedUnit.UnitClass == Unit.UnitType.Array) { DeselectAllUnits(); }
            this.PlayerUI.SelectUnit(selectedUnit);
            _selectedUnits.Add(selectedUnit);
        }

        public void DeselectAllUnits() 
        {
            this.PlayerUI.DeselectAllUnits();
            _selectedUnits.Clear();
        }

        public void CommandUnits(Command command)
        {
            foreach (Unit unit in _selectedUnits) 
            {
                Debug.Log("Commanding " + unit.name + " to preform the " + command.Name + " command");
                unit.AddCommand(command);
            }
        }
    }
}