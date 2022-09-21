using UnityEngine;

namespace BeamMeUpATCA
{
    public class LockCommand : Command
    {
        // Commands Unit to Lock/Unlock a building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position 
        // 2. Building is Moveable.
        // 3. Building is Enterable & this.unit has entered building
        override protected void CommandAwake()
        {
            Name = "Lock";
        }

        // Check Command conditions. If conditions met but the unit is not 
        // at the building dock, call Pathfinder - Goto(Camera, Vector2)
        // Check in Update() for (IsGotoFinished && CommandConditions).
        // Ensure methods respect the expected call count (single vs multiple calls)
        // of the building interface methods.
        public override void Execute() {}

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update() {}
        private void FixedUpdate() {}

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() { return true; }

        // Any code that needs to be called if command is paused should go here.
        private void OnDisable() {}

        // Any Code that needs to be called if command is unpause should go here.
        private void OnEnable() {}

        // Any cleanup code that needs to be called if command is destroyed should go here.
        private void OnDestroy() {}
    }
}
