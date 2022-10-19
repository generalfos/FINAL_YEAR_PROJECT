using UnityEngine;

namespace BeamMeUpATCA
{
    public class MoveCommand : Command
    {
        // Commands Unit to Move a building to the Command.Position
        // Conditions: 
        // 1. unit.BuildingInside != null
        // 2. unit.BuildingInside is Enterable
        // 3. unit.BuildingInside is Moveable
        // 4. Building.Lock = false;
        public override void Execute()
        {
            Building building = Unit.BuildingInside;

            // If building is null or not enterable (or the unit is not inside) return without setting _conditionsMet
            if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;
            
            // If building is not workable (or currently being worked) return without setting _conditionsMet
            if (building is Moveable moveable)
            {
                // Don't bother trying to move dish if it's locked.
                if (moveable.IsLocked) return;
                
                IInteractable interactable = Selector.SelectGameObject(RayData.Item1, RayData.Item2, Mask.DishSlot);

                // If interactable is null or not an ArrangementSlot this will fail and conditions will not be met.
                if (interactable is ArrangementSlot dishSlot) moveable.Move(Unit, dishSlot.transform);
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => true;
        
    }
}
