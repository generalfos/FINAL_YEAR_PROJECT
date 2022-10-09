using UnityEngine;

namespace BeamMeUpATCA
{
    public class EnterCommand : GotoCommand
    {
        // Commands Unit to Enter the building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Enterable
        protected override void CommandAwake() { Name = "Enter"; }

        private bool _conditionsMet = false;
        private Enterable _building = null;
        
        // Check Command conditions. If conditions met but the unit is not 
        // at the building dock, call Pathfinder - Goto(Camera, Vector2)
        // Check in Update() for (IsGotoFinished && CommandConditions).
        // Ensure methods respect the expected call count (single vs multiple calls)
        // of the building interface methods.
        public override void Execute()
        {
            // Action which cannot be preformed from inside a building.
            if (unit.IsInsideBuilding) return;
            
            IInteractable interactable = Selector.SelectGameObject(ActiveCamera, Position, Mask.Building);

            // If interactable is null or not enterable this will fail and conditions will not be met.
            if (!(interactable is Building) || !(interactable is Enterable enterable)) return;

            _building = enterable;
            _conditionsMet = true;
            if (((Building)_building).Anchors.CanAnchor(unit.transform.position))
            {
                _building.Enter(unit);
            }
            else
            {
                Vector3 position = ((Building)_building).Anchors.GetAnchorPoint();
                Goto(ActiveCamera, ActiveCamera.WorldToScreenPoint(position));
            }
        }

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update()
        {
            if (!(Building)_building) return;
            if (((Building)_building).Anchors.CanAnchor(unit.transform.position))
            {
                _building.Enter(unit);
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || ((Building)_building && _building.IsInside(unit));
    }
}