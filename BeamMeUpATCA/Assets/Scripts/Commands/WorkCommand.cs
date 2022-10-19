using UnityEngine;

namespace BeamMeUpATCA
{
    public class WorkCommand : Command
    {
        // Commands Unit to Work at the building at the Command.Position
        // Conditions:
        // 1. Building exists at Command.Position
        // 2. Building is Workable
        // 3. Building is Enterable & this.unit has entered building
        public override void Execute()
        {
            Building building = Unit.BuildingInside;

            // If building is null or not enterable (or the unit is not inside) return without setting _conditionsMet
            if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;
            
            // If building is not workable (or currently being worked) return without setting _conditionsMet
            if (!(building is Workable { WorkingUnit: null } workable)) return;

            workable.Work(Unit);
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => true;
    }
}