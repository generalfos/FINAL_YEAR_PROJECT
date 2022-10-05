using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class MendCommand : GotoCommand
    {
        // Commands Unit to Mend the building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Mendable
        override protected void CommandAwake()
        {
            Name = "Mend";
        }

        private bool conditionsMet = true;
        Mendable building = null;

        public override void Execute() 
        {
            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, Position, new[]{"Building"});

            // Try to get building assuming selected object is not null.
            try { building = selectedObject.GetComponent<Mendable>();
            } catch (NullReferenceException) 
            {
                Debug.Log("Building object is not mendable");
                building = null;
            }

            // The selected object is not a mendable building therefor abort command.
            if (selectedObject == null || building == null) 
            { 
                Debug.Log("found no mendable building");
                conditionsMet = false; 
                return; 
            }

            if (building.Anchors.CanAnchor(unit.transform.position))
            {
                building.Mend(unit);
            } 
            else 
            {
                Vector3 position = building.Anchors.GetAnchorPoint();
                Vector2 positionTemp = new Vector2(position.x, position.y);
                Goto(ActiveCamera, ActiveCamera.WorldToScreenPoint(position));
            }
        }

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update() 
        {
            if (building != null)
            {
                if (building.Anchors.CanAnchor(unit.transform.position))
                {
                    building.Mend(unit);
                } 
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() 
        {
            if (!conditionsMet) return true;
            if (building != null) return building.isRepaired;
            return false;
        }
    }
}