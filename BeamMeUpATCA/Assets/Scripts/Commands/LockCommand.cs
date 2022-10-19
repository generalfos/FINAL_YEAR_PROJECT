using UnityEngine;

namespace BeamMeUpATCA
{
    public class LockCommand : GotoCommand
    {
        private bool _isWaiting = false;
        private Moveable _building = null;
        
        // Commands Unit to Lock a building at the Command.Position
        // Conditions:
        // 1. Building exists at Command.Position
        // 2. Building is Lockable
        // OR
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable && Lockable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        public override void Execute()
        {
            if (!(Unit.BuildingInside is null))
            {
                Building building = Unit.BuildingInside;

                // If building is not enterable (or the unit is not inside) return without setting _conditionsMet
                if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;
            
                // If building is not lockable return without setting _conditionsMet
                if (!(building is Moveable moveable)) return;

                _building = moveable;
                moveable.ToggleLock();
                
            }
            else
            {
                IInteractable interactable = Selector.SelectGameObject(RayData.Item1, RayData.Item2, Mask.Building);

                // If interactable is null or not moveable this will fail and conditions will not be met.
                if (!(interactable is Moveable moveable)) return;
            
                _building = moveable;

                if (((Building)_building).Anchors.CanAnchor(Unit.transform.position))
                {
                    _building.ToggleLock();
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
                _building.ToggleLock();
                _isWaiting = false;
            }
        }

        // If we're still waiting for pathfinding return false
        public override bool IsFinished() => !_isWaiting;
    }
}
