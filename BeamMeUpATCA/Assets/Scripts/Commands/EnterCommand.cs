using UnityEngine;

namespace BeamMeUpATCA
{
    public class EnterCommand : GotoCommand
    {
        // Commands Unit to Enter the building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Enterable
        override protected void CommandAwake()
        {
            Name = "Enter";
        }

        // Check Command conditions. If conditions met but the unit is not 
        // at the building dock, call Pathfinder - Goto(Camera, Vector2)
        // Check in Update() for (IsGotoFinished && CommandConditions).
        // Ensure methods respect the expected call count (single vs multiple calls)
        // of the building interface methods.
        public override void Execute() { base.Execute(); }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() { return base.IsFinished(); }
    }
}