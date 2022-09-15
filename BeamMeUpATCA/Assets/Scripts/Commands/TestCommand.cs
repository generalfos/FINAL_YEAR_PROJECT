using UnityEngine;
using System;

namespace BeamMeUpATCA
{
    public class TestCommand : Command
    {
        Func<Vector2> _worldPosition;

        override protected void DefineCommand() 
        {
            Name = "Test";
            SkipQueue = false;
            ResetQueue = false;
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
