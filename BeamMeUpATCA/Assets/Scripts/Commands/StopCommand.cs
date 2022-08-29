using UnityEngine;

namespace BeamMeUpATCA
{
    public class StopCommand : Command
    {
        public override void Execute() 
        {
            Debug.Log("StopCommand called");
            IsFinished = true;
        }
    }
}
