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

        public override void Execute(Unit unit) 
        {
            unit.gameObject.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
            Debug.Log("MoveCommand called");
        }
    }
}
