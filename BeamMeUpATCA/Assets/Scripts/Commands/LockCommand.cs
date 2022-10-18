using UnityEngine;

namespace BeamMeUpATCA
{
    public class LockCommand : GotoCommand
    {
        // Commands Unit to Lock a building at the Command.Position
        // Conditions:
        // 1. Building exists at Command.Position
        // 2. Building is Lockable
        // OR
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable && Lockable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        protected override void CommandAwake() { Name = "Lock"; }
        
        private bool _isWaiting = false;
        private Moveable _building = null;
        
        // Check if unit is currently in building. If so just call the lock command, otherwise
        // call the unit pathfinder to navigate to the building and then attempt to lock.
        public override void Execute()
        {
            if (!(unit.BuildingInside is null))
            {
                Building building = unit.BuildingInside;

                // If building is not enterable (or the unit is not inside) return without setting _conditionsMet
                if (!(building is Enterable enterable) || !(enterable.IsInside(unit))) return;
            
                // If building is not lockable return without setting _conditionsMet
                if (!(building is Moveable moveable)) return;

                _building = moveable;
                moveable.ToggleLock();
                
            }
            else
            {
                IInteractable interactable = Selector.SelectGameObject(ActiveCamera, Position, Mask.Building);

                // If interactable is null or not moveable this will fail and conditions will not be met.
                if (!(interactable is Moveable moveable)) return;
            
                _building = moveable;

                if (((Building)_building).Anchors.CanAnchor(unit.transform.position))
                {
                    _building.ToggleLock();
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
                _building.ToggleLock();
                _isWaiting = false;
            }
        }

        // If we're still waiting for pathfinding return false
        public override bool IsFinished() => !_isWaiting;
    }
}
