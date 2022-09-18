using UnityEngine;

namespace BeamMeUpATCA
{
    public class StopCommand : Command
    {
        // Command stops any currently running commands and clears the command queue.
        override protected void DefineCommand()
        {
            Name = "Stop";
            SkipQueue = true;
            ResetQueue = true;
        }

        public override void Execute() { return; }

        public override bool IsFinished() { return true; }
    }
}
