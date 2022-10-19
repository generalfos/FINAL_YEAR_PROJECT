namespace BeamMeUpATCA
{
    public class LeaveCommand : Command
    {
        private bool _conditionsMet = false;
        private Enterable _building = null;
        
        // Commands Unit to Leave the building they are currently inside
        // Conditions:
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        public override void Execute()
        {
            Building building = Unit.BuildingInside;

            // If building is null or not enterable (or the unit is not inside) return without setting _conditionsMet
            if (!(building is Enterable enterable) || !(enterable.IsInside(Unit))) return;

            _building = enterable;
            _conditionsMet = true;
            enterable.Leave(Unit);
            // Check if enter was successful
            if (!enterable.IsInside(Unit)) 
                Unit.ExitBuilding();
            if (building is Workable workable && Equals(workable.WorkingUnit, Unit))
            {
                workable.Rest(Unit);
            }

            else
            {
                _conditionsMet = false;
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || ((Building)_building && !_building.IsInside(Unit));
    }
}