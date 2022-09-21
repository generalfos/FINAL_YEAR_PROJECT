using UnityEngine;
using UnityEngine.AI;
using System;

namespace BeamMeUpATCA
{
    public class UnitPathfinder
    {
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

        public void Path(Camera camera, Vector2 position) 
        {
            if (!agent.isOnNavMesh) return;
            
            // See Selector.NearestWalkableHACK() Comments for more information.
            agent.SetDestination(Selector.NearestWalkableHACK(camera, position));
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