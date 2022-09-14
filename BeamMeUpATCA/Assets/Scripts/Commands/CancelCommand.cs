using UnityEngine;

namespace BeamMeUpATCA
{
    public class CancelCommand : Command
    {
        public CancelCommand(bool skipQueue = true, bool resetQueue = true) 
            : base(skipQueue, resetQueue) 
            {
                Name = "Cancel";
            }

        public override void Execute(Unit unit) 
        {
            Debug.Log("CancelCommand called");
        }
    }
}
