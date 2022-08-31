using UnityEngine;

namespace BeamMeUpATCA
{
    public class MoveCommand : Command
    {
        RaycastHit RayCastHit;
        Ray RayCast;
        Player _player;

        public MoveCommand(Player player, bool skipQueue = false, bool resetQueue = false) 
            : base(skipQueue, resetQueue) 
            {
                Name = "Move";
                _player = player;
            }

        public override void Execute(Unit unit) 
        {
            RayCast = Camera.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
            if (!Physics.Raycast(RayCast, out RayCastHit, 10000f))
            {
                Debug.Log("No hit");
                return;
            }
            unit.gameObject.transform.position = RayCastHit.point;
            Debug.Log("MoveCommand called");
        }
    }
}
