using UnityEngine;

namespace BeamMeUpATCA
{
    public class GotoCommand : Command
    {
        protected bool IsGoingTo = false;
        private bool IsGotoFinished => IsGoingTo && unit.Pathfinder.PathFinished();

        // Commands Unit to Command.Position using Pathfinder.
        // Conditions: 1. Command.Position is a valid walkable position.
        protected override void CommandAwake()
        {
            Name = "Goto";
        }
        
        private bool _conditionsMet = false;

        public override void Execute() 
        {
            // Action which cannot be preformed from inside a building.
            if (!(unit.BuildingInside is null)) return;
            
            _conditionsMet = true;
            Goto(ActiveCamera, Position);
        }

        public override bool IsFinished() 
        {
            return IsGotoFinished || !_conditionsMet;
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