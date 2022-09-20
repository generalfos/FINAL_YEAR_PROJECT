using UnityEngine;

namespace BeamMeUpATCA
{
    public class MoveCommand : Command
    {
        // Commands Unit to Move a building to the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Moveable.
        // 3. Building.Lock = false;
        // 4. Building is Enterable & this.unit has entered building
        override protected void CommandAwake()
        {
            Name = "Move";
        }

        // Check Command conditions and call the relevant building interface method
        // Ensure methods respect the expected call count (single vs multiple calls)
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
