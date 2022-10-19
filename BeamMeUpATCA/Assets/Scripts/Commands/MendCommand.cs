using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class MendCommand : GotoCommand
    {
        private bool _conditionsMet = false;
        private Mendable _building = null;

        // Commands Unit to Mend the building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Mendable
        public override void Execute()
        {
            if (Unit.BuildingInside) return;
            
            IInteractable interactable = Selector.SelectGameObject(RayData.Item1, RayData.Item2, Mask.Building);

            // If interactable is null or not mendable this will fail and conditions will not be met.
            if (!(interactable is Mendable mendable)) return;
            
            _building = mendable;
            _conditionsMet = true;
            if (_building.Anchors.CanAnchor(Unit.transform.position))
            {
                _building.Mend(Unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(Unit);
            }
            else
            {
                Vector3 position = _building.Anchors.GetAnchorPoint();
                Goto(RayData);
            }
        }

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        protected override void Update()
        {
            base.Update();
            if (!_building) return;
            if (_building.Anchors.CanAnchor(Unit.transform.position))
            {
                _building.Mend(Unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(Unit);
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || (_building && _building.IsRepaired);
        
        
        protected override void OnDisable() 
        {
            base.OnDisable();
            if (!_building || !_conditionsMet) return;
            if (_building.IsMender(Unit)) _building.RemoveMender(Unit);
        }

        protected override void OnEnable() 
        {
            base.OnEnable();
            
            // If command has been paused rerun anchor and building checks for mend attempt
            if (!_building || !_conditionsMet) return;
            if (_building.Anchors.CanAnchor(Unit.transform.position))
            {
                _building.Mend(Unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(Unit);
            }
        }

        protected override void OnDestroy() 
        {
            base.OnDestroy();
            if (!_building || !_conditionsMet) return;
            if (_building.IsMender(Unit)) _building.RemoveMender(Unit);
        }
    }
}