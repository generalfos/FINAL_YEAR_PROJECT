using UnityEngine;

namespace BeamMeUpATCA
{
    public class WorkCommand : Command
    {
        // Commands Unit to Work at the building at the Command.Position
        // Conditions:
        override protected void DefineCommand()
        {
            Name = "Work";
        }


        // Called once when command is first executed
        // Similar to Start()/Awake() but executed after both.
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