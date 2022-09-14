using UnityEngine;

namespace BeamMeUpATCA
{
    public class RotateCommand : Command
    {
        RaycastHit RayCastHit;
        Ray RayCast;
        Player _player;

        public RotateCommand(Player player, bool skipQueue = false, bool resetQueue = false)
            : base(skipQueue, resetQueue)
        {
            Name = "Rotate";
            _player = player;
        }

        public override void Execute(Unit unit)
        {
            // RayCast = Camera.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
            // if (!Physics.Raycast(RayCast, out RayCastHit, 10000f))
            // {
            //     Debug.Log("No hit");
            //     return;
            // }
            // Debug.Log("Hit: " + RayCastHit.transform.gameObject.name);
            // if (unit.UnitClass == Unit.UnitType.Array)
            // {
            //     // Debug.Log(RayCastHit.point);
            //     GameObject dish = unit.gameObject.transform.GetChild(2).gameObject;
            //     Vector3 targetPos = RayCastHit.point;
            //     targetPos.y = 0;
            //     dish.transform.LookAt(targetPos);
            // }
            
            // Debug.Log("RotateCommand called");

        }
    }
}