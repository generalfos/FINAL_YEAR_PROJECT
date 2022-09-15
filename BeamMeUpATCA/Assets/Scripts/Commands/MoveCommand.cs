using UnityEngine;

namespace BeamMeUpATCA
{
    public class MoveCommand : Command
    {
        RaycastHit RayCastHit;
        Ray RayCast;
        Player _player;

        override protected void DefineCommand()
        {
            Name = "Move";
            SkipQueue = false;
            ResetQueue = false;
        }

        // public override void Execute() 
        // {
        //     // RayCast = Player.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
        //     // if (!Physics.Raycast(RayCast, out RayCastHit, 10000f))
        //     // {
        //     //     Debug.Log("No hit");
        //     //     return;
        //     // }
        //     // string RayCastHitTag = RayCastHit.transform.gameObject.tag;
        //     // Debug.Log(RayCastHitTag);
        //     // if (RayCastHitTag == "ConfigSlot")
        //     // {
        //     //     if (unit.UnitClass == Unit.UnitType.Array)
        //     //     {
        //     //         unit.gameObject.transform.position = RayCastHit.point;
        //     //     }
        //     // } else
        //     // {
        //     //     if (!(unit.UnitClass == Unit.UnitType.Array))
        //     //     {
        //     //         unit.gameObject.transform.position = RayCastHit.point;
        //     //     }
        //     // }
        //     // Debug.Log("MoveCommand called");
        // }

        public override void Execute()
        {
            Debug.Log(this.Name + " Command Executed");
        }

        float counter = 0f;

        private void Update()
        {
            counter += Time.deltaTime;
            Debug.Log("Doing Updates...");
        }

        public override bool IsFinished()
        {
            return counter >= 4f ? true : false;
        }
    }
}
