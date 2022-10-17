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
        protected override void CommandAwake()
        {
            Name = "Mend";
        }

        private bool _conditionsMet = false;
        private Mendable _building = null;

        public override void Execute() 
        {
            IInteractable interactable = Selector.SelectGameObject(ActiveCamera, Position, Mask.Building);

            // If interactable is null or not mendable this will fail and conditions will not be met.
            if (!(interactable is Mendable mendable)) return;
            
            _building = mendable;
            _conditionsMet = true;
            if (_building.Anchors.CanAnchor(unit.transform.position))
            {
                _building.Mend(unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(unit);
            }
            else
            {
                Vector3 position = _building.Anchors.GetAnchorPoint();
                Goto(ActiveCamera, ActiveCamera.WorldToScreenPoint(position));
            }
        }

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update()
        {
            if (!_building) return;
            if (_building.Anchors.CanAnchor(unit.transform.position))
            {
                _building.Mend(unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(unit);
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || (_building && _building.IsRepaired);
        
        
        protected override void OnDisable() 
        {
            base.OnDisable();
            if (!_building || !_conditionsMet) return;
            if (_building.IsMender(unit)) _building.RemoveMender(unit);
        }

        protected override void OnEnable() 
        {
            base.OnEnable();
            
            // If command has been paused rerun anchor and building checks for mend attempt
            if (!_building || !_conditionsMet) return;
            if (_building.Anchors.CanAnchor(unit.transform.position))
            {
                _building.Mend(unit);
                // Check if mend was successful
                _conditionsMet = _building.IsMender(unit);
            }
        }

        protected override void OnDestroy() 
        {
            base.OnDestroy();
            if (!_building || !_conditionsMet) return;
            if (_building.IsMender(unit)) _building.RemoveMender(unit);
        }
    }
}