using UnityEngine;

namespace BeamMeUpATCA
{
    public class PowerCommand : GotoCommand
    {
        private bool _isWaiting = false;
        private Powerable _building = null;
        
        // Commands Unit to Power a building at the Command.Position
        // Conditions:
        // 1. Building exists at Command.Position
        // 2. Building is Powerable
        // OR
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable && Powerable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        public override void Execute()
        {
            if (!(Unit.BuildingInside is null))
            {
                Building building = Unit.BuildingInside;

                // If building is not enterable (or the unit is not inside) return without setting _conditionsMet
                if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;
            
                // If building is not powerable return without setting _conditionsMet
                if (!(building is Powerable powerable)) return;

                _building = powerable;
                powerable.TogglePower();
            }
            else
            {
                IInteractable interactable = Selector.SelectGameObject(RayData.Item1, RayData.Item2, Mask.Building);

                // If interactable is null or not powerable this will fail and conditions will not be met.
                if (!(interactable is Powerable powerable)) return;
            
                _building = powerable;

                if (((Building)_building).Anchors.CanAnchor(Unit.transform.position))
                {
                    _building.TogglePower();
                }
                else
                {
                    Vector3 position = ((Building)_building).Anchors.GetAnchorPoint();
                    Goto(RayData);
                    _isWaiting = true;
                }
            }
        }
        
        // If still waiting to get to the building.position keep checking in update
        protected override void Update()
        {
            base.Update();
            if (!(Building)_building || !(Unit.BuildingInside is null)) return;
            if (((Building)_building).Anchors.CanAnchor(Unit.transform.position) && isGoingTo)
            {
                _building.TogglePower();
                _isWaiting = false;
            }
        }

        // If we're still waiting for pathfinding return false
        public override bool IsFinished() => !_isWaiting;
    }
}
