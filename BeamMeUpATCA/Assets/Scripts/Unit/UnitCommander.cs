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

        public void CommandUnits<T>(Vector2 position)
        {
            foreach (Unit unit in _selectedUnits) 
            {
                if (typeof(T).IsSubclassOf(typeof(Command))) 
                {
                    // Can't cast T directly to Command so we cast to object first. 
                    // This is a safe cast as T is type checked at runtime to be a Command
                    Command command = (Command)(object) unit.gameObject.AddComponent(typeof(T));
                    Debug.Log("Commanding " + unit.name + " to preform the " + command.Name + " command");

                    command.Position = position;
                    unit.AddCommand(command);
                }
            }
        }
    }
}
