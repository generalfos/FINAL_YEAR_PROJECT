using UnityEngine;

namespace BeamMeUpATCA
{
    public class MoveCommand : Command
    {
        public MoveCommand(bool skipQueue = false, bool resetQueue = false) 
            : base(skipQueue, resetQueue) 
            {
                Name = "Move";
            }

        public override void Execute() 
        {
            Debug.Log("MoveCommand called");
        }
    }
}
