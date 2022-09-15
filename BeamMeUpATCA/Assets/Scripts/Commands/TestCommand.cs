using UnityEngine;
using System;

namespace BeamMeUpATCA
{
    public class TestCommand : Command
    {
        Func<Vector2> _worldPosition;

        public TestCommand(Func<Vector2> worldPosition, bool skipQueue = false, bool resetQueue = false) 
            : base(skipQueue, resetQueue) 
            {
                Name = "TestCommand";
                _worldPosition = worldPosition;

            }

        public override void Execute() 
        {
            Debug.Log("World Position: " + _worldPosition());
        }

        public override bool IsFinished()
        {
            return true;
        }
    }
}
