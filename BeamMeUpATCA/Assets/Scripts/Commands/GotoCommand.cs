using UnityEngine;

namespace BeamMeUpATCA
{
    public class GotoCommand : Command
    {
        protected bool isGoingTo = false;
        private bool IsGotoFinished => isGoingTo && Unit.Pathfinder.PathFinished();
        private bool AbleToPath { get; set; } = false;

        // Commands Unit to Command.Position using Pathfinder.
        // Conditions: 1. Command.Position is a valid walkable position.
        public override void Execute() 
        {
            // Action which cannot be preformed from inside a building.
            if (Unit.BuildingInside) return;
            
            AbleToPath = true;
            Goto(RayData);
        }

        public override bool IsFinished() 
        {
            return IsGotoFinished || !AbleToPath;
        }

        // Keep checking is a path has been calculated 
        private bool _isPathCalculated = false;
        protected virtual void Update()
        {
            if (!_isPathCalculated) _isPathCalculated = Unit.Pathfinder.RunPathing();
        }

        protected void Goto((Ray, float) rayCastData) 
        {
            isGoingTo = true;
            Unit.Pathfinder.Path(rayCastData, Offset);

            Unit.Pathfinder.DontDestroyPathOnCancel();
        }

        protected virtual void OnDisable() 
        {
            // Pauses path if command is disabled
            if (isGoingTo) Unit.Pathfinder.SetPausePath(true);
        }

        protected virtual void OnEnable() 
        {
            // Unpauses path (if path existed) if command is enabled
            if (isGoingTo) Unit.Pathfinder.SetPausePath(false);
        }

        protected virtual void OnDestroy() 
        {
            // Destroy path is command is canceled.
            if (isGoingTo) Unit.Pathfinder.CancelPath();
        }
    }
}