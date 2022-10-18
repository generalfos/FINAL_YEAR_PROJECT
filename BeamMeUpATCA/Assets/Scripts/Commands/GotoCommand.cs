using UnityEngine;

namespace BeamMeUpATCA
{
    public class GotoCommand : Command
    {
        protected bool IsGoingTo = false;
        private bool IsGotoFinished => IsGoingTo && unit.Pathfinder.PathFinished();

        // Commands Unit to Command.Position using Pathfinder.
        // Conditions: 1. Command.Position is a valid walkable position.
        protected override void CommandAwake() { Name = "Goto"; }

        private bool AbleToPath { get; set; } = false;
        
        public override void Execute() 
        {
            // Action which cannot be preformed from inside a building.
            if (unit.BuildingInside) return;
            
            AbleToPath = true;
            Goto(ActiveCamera, Position);
        }

        public override bool IsFinished() 
        {
            return IsGotoFinished || !AbleToPath;
        }

        // Keep checking is a path has been calculated 
        private bool _isPathCalculated = false;
        protected virtual void Update()
        {
            if (!_isPathCalculated) _isPathCalculated = unit.Pathfinder.RunPathing();
        }

        protected void Goto(Camera cam, Vector2 position) 
        {
            IsGoingTo = true;
            unit.Pathfinder.Path(cam, position, Offset);

            unit.Pathfinder.DontDestroyPathOnCancel();
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