using UnityEngine;

namespace BeamMeUpATCA
{
    public class StopCommand : Command
    {
        // Command stops any currently running commands and clears the command queue.
        public override bool SkipQueue { set; get; } = true;
        public override bool ResetQueue { set; get; } = true;

        public override void Execute() { return; }
        public override bool IsFinished() { return true; }
    }
}
