using UnityEngine;

namespace BeamMeUpATCA
{
    public class PowerCommand : GotoCommand
    {
        // Commands Unit to Power a building at the Command.Position
        // Conditions:
        // 1. Building exists at Command.Position
        // 2. Building is Powerable
        // OR
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable && Powerable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        protected override void CommandAwake() { Name = "Power"; }
        
        private bool _isWaiting = false;
        private Powerable _building = null;
        
        // Check if unit is currently in building. If so just call the power command, otherwise
        // call the unit pathfinder to navigate to the building and then attempt to power.
        public override void Execute()
        {
            if (!(unit.BuildingInside is null))
            {
                Building building = unit.BuildingInside;

                // If building is not enterable (or the unit is not inside) return without setting _conditionsMet
                if (!(building is Enterable enterable) || !(enterable.IsInside(unit))) return;
            
                // If building is not powerable return without setting _conditionsMet
                if (!(building is Powerable powerable)) return;

                _building = powerable;
                powerable.TogglePower();
            }
            else
            {
                IInteractable interactable = Selector.SelectGameObject(ActiveCamera, Position, Mask.Building);

                // If interactable is null or not powerable this will fail and conditions will not be met.
                if (!(interactable is Powerable powerable)) return;
            
                _building = powerable;

                if (((Building)_building).Anchors.CanAnchor(unit.transform.position))
                {
                    _building.TogglePower();
                }
                else
                {
                    Vector3 position = ((Building)_building).Anchors.GetAnchorPoint();
                    Goto(ActiveCamera, ActiveCamera.WorldToScreenPoint(position));
                    _isWaiting = true;
                }
            }
        }
        
        // If still waiting to get to the building.position keep checking in update
        private void Update()
        {
            if (!(Building)_building || !(unit.BuildingInside is null)) return;
            if (((Building)_building).Anchors.CanAnchor(unit.transform.position) && IsGoingTo)
            {
                _building.TogglePower();
                _isWaiting = false;
            }
        }

        // If we're still waiting for pathfinding return false
        public override bool IsFinished() => !_isWaiting;
    }
}
