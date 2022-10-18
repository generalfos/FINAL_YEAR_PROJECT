namespace BeamMeUpATCA
{
    public class LeaveCommand : Command
    {
        // Commands Unit to Leave the building they are currently inside
        // Conditions:
        // 1. Unit.BuildingInside != null
        // 2. Building is Enterable
        // 3. Unit.BuildingInside.IsInside(unit) == true
        protected override void CommandAwake() { Name = "Leave"; }
        
        private bool _conditionsMet = false;
        private Enterable _building = null;

        // Check Command conditions and call the relevant building interface method
        // Ensure methods respect the expected call count (single vs multiple calls)
        public override void Execute()
        {
            Building building = unit.BuildingInside;

            // If building is null or not enterable (or the unit is not inside) return without setting _conditionsMet
            if (!(building is Enterable enterable) || !(enterable.IsInside(unit))) return;

            _building = enterable;
            _conditionsMet = true;
            enterable.Leave(unit);
            // Check if enter was successful
            if (!enterable.IsInside(unit)) 
                unit.ExitBuilding();
            if (building is Workable workable && Equals(workable.WorkingUnit, unit))
            {
                workable.Rest(unit);
            }

            else
            {
                _conditionsMet = false;
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || ((Building)_building && !_building.IsInside(unit));
    }
}