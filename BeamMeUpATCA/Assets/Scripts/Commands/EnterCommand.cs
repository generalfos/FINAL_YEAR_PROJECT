using UnityEngine;

namespace BeamMeUpATCA
{
    public class EnterCommand : GotoCommand
    {
        private bool _conditionsMet = false;
        private Enterable _building = null;

        // Commands Unit to Enter the building at the Command.Position
        // Conditions: 
        // 1. Building exists at Command.Position
        // 2. Building is Enterable
        public override void Execute()
        {
            // Action which cannot be preformed from inside a building.
            if (!(Unit.BuildingInside is null)) return;

            IInteractable interactable = Selector.SelectGameObject(RayData.Item1, RayData.Item2, Mask.Building);

            // If interactable is null or not enterable this will fail and conditions will not be met.
            if (!(interactable is Building) || !(interactable is Enterable enterable)) return;

            _building = enterable;
            _conditionsMet = true;
            if (((Building)_building).Anchors.CanAnchor(Unit.transform.position))
            {
                _building.Enter(Unit);
                // Check if enter was successful
                if (_building.IsInside(Unit)) 
                    Unit.EnterBuilding((Building) _building);
                else
                {
                    _conditionsMet = false;
                }
            }
            else
            {
                Debug.Log("Enter! Enter");
                Vector3 position = ((Building)_building).Anchors.GetAnchorPoint();
                Goto(RayData);
            }
        }

        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        protected override void Update()
        {
            base.Update();
            if (!(Building)_building) return;
            if (((Building)_building).Anchors.CanAnchor(Unit.transform.position))
            {
                _building.Enter(Unit);
                // Check if enter was successful
                if (_building.IsInside(Unit)) 
                    Unit.EnterBuilding((Building) _building);
                else
                {
                    _conditionsMet = false;
                }
            }
        }

        // Should return true if the command has finished execution. Goal condition.
        // Consider adding a timeout to the command if it doesn't have an guaranteed end state.
        public override bool IsFinished() => !_conditionsMet || ((Building)_building && _building.IsInside(Unit));
    }
}