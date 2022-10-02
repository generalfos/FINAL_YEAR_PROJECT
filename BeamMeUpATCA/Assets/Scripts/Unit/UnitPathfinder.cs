using UnityEngine;
using UnityEngine.AI;
using System;

namespace BeamMeUpATCA
{
    public class UnitPathfinder
    {
        // This value was calculated manually to find a distance smaller than two collision
        // bounds on units. If we change the scale of the environnement or want tighter/wider
        // path on multiple unit commands this value will need to be adjusted.
        float UNIT_OFFSET_SPACING = 35f;

        NavMeshAgent agent;

        public UnitPathfinder(Unit unit) 
        {
            try 
            {
                agent = unit.GetComponent<NavMeshAgent>();
            }
            catch (NullReferenceException) 
            {
                Debug.LogWarning("Unable to find a NavMeshAgent. Failing safely.");
                agent = unit.gameObject.AddComponent<NavMeshAgent>();
            }
        }

        public void Path(Camera camera, Vector2 position, int unitOffset) 
        {
            if (!agent.isOnNavMesh) return;
            float unitSpacing = UNIT_OFFSET_SPACING * (agent.radius * 2);
            // See Selector.NearestWalkableHACK() Comments for more information.
            agent.SetDestination(Selector.NearestWalkable(camera, position, unitOffset, unitSpacing));
        }

        public void CancelPath() 
        {
            if (!agent.isOnNavMesh || !agent.hasPath ) return;
            agent.ResetPath();
        }

        public void SetPausePath(bool pause) 
        {
            if (!agent.isOnNavMesh || !agent.hasPath) return;
            agent.isStopped = pause;
        }

        public bool PathFinished() 
        {
            if (!agent.isOnNavMesh || agent.pathPending) return false;
            if (agent.remainingDistance > agent.stoppingDistance) return false;
            if (agent.hasPath || agent.velocity.sqrMagnitude != 0f) return false;
            return true;
        }
    }
}