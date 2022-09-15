using UnityEngine;

namespace BeamMeUpATCA
{
    public class CancelCommand : Command
    {
        override protected void DefineCommand()
        {
            Name = "Cancel";
            SkipQueue = true;
            ResetQueue = true;
        }

        public override void Execute() 
        {
            Debug.Log("Cancel Command called");
        }

        public override bool IsFinished()
        {
            return true;
        }
    }
}
