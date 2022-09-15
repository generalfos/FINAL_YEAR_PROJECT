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

        public override void Execute() 
        {
            // RayCast = Player.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
            // if (!Physics.Raycast(RayCast, out RayCastHit, 10000f))
            // {
            //     Debug.Log("No hit");
            //     return;
            // }
            // string RayCastHitTag = RayCastHit.transform.gameObject.tag;
            // Debug.Log(RayCastHitTag);
            // if (RayCastHitTag == "ConfigSlot")
            // {
            //     if (unit.UnitClass == Unit.UnitType.Array)
            //     {
            //         unit.gameObject.transform.position = RayCastHit.point;
            //     }
            // } else
            // {
            //     if (!(unit.UnitClass == Unit.UnitType.Array))
            //     {
            //         unit.gameObject.transform.position = RayCastHit.point;
            //     }
            // }
            // Debug.Log("MoveCommand called");

        }

        public override bool IsFinished()
        {
            return true;
        }
    }
}
