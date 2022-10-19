namespace BeamMeUpATCA
{
    public class RestCommand : Command
    {
        // Commands Unit to Rest in the building they are currently working in
        // Conditions:
        // 1. Unit.BuildingInside != null
        // 2. Building is Workable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        // 3. Unit.BuildingInside.WorkingUnit = unit
        public override void Execute()
        {
            Building building = Unit.BuildingInside;

            // If building is null or not enterable (or the unit is not inside) return without setting _conditionsMet
            if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;
            
            // If building is not workable (or currently being worked) return without setting _conditionsMet
            if (!(building is Workable workable && Equals(workable.WorkingUnit, Unit))) return;

            workable.Rest(Unit);
        }

        // Regardless of outcome command exits immediantly
        public override bool IsFinished() => true;
    }
}