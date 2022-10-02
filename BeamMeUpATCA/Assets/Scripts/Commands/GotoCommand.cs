using UnityEngine;

namespace BeamMeUpATCA
{
    public class GotoCommand : Command
    {
        protected bool IsGoingTo = false;
        protected bool IsGotoFinished { 
            get {return IsGoingTo && unit.Pathfinder.PathFinished();}}

        // Commands Unit to Command.Position using Pathfinder.
        // Conditions: 1. Command.Position is a valid walkable position.
        override protected void CommandAwake()
        {
            Name = "Goto";
        }

        public override void Execute() 
        {
            Goto(ActiveCamera, Position);
        }

        public override bool IsFinished() 
        {
            return IsGotoFinished;
        }
         
        protected void Goto(Camera camera, Vector2 position) 
        {
            IsGoingTo = true;
            unit.Pathfinder.Path(camera, position, Offset);
        }

        protected virtual void OnDisable() 
        {
            // Pauses path if command is disabled
            if (IsGoingTo) unit.Pathfinder.SetPausePath(true);
        }

        protected virtual void OnEnable() 
        {
            // Unpauses path (if path existed) if command is enabled
            if (IsGoingTo) unit.Pathfinder.SetPausePath(false);
        }

        protected virtual void OnDestroy() 
        {
            // Destroy path is command is canceled.
            if (IsGoingTo) unit.Pathfinder.CancelPath();
        }
    }
}