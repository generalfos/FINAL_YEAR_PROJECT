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

        public override void Execute(Unit unit) 
        {
            Debug.Log("World Position: " + _worldPosition());
        }
    }
}
