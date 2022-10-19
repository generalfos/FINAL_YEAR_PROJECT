using UnityEngine;
using UnityEngine.AI;
using System;
using System.IO;
using BeamMeUpATCA.Extensions;

namespace BeamMeUpATCA
{
    public class UnitPathfinder
    {
        // This value was calculated manually to find a distance smaller than two collision
        // bounds on units. If we change the scale of the environnement or want tighter/wider
        // path on multiple unit commands this value will need to be adjusted.
        private const float UnitOffsetSpacing = 2.5f;

        private NavMeshAgent _agent;
        private NavMeshAgent Agent => _unit.SafeComponent<NavMeshAgent>(ref _agent);

        private readonly Unit _unit;

        public UnitPathfinder(Unit unit)
        {
            _unit = unit;
            _agent = unit.GetComponent<NavMeshAgent>();
            endPosition = unit.transform.position;
        }

        private bool _pathFound = false;
        private bool _pathPersistent = false;
        private NavMeshPath _path = new NavMeshPath();
        private Vector3 endPosition;
        
        public void Path((Ray, float) rayCastData, int unitOffset) 
        {
            if (!Agent.isOnNavMesh) return;
            float unitSpacing = UnitOffsetSpacing * (Agent.radius * 2);
            
            Vector3 selectorPosition = Selector.NearestWalkable(rayCastData.Item1, rayCastData.Item2, unitOffset, unitSpacing);
            if (Vector3.Distance(endPosition, selectorPosition) >= 0.15f)
            {
                _pathPersistent = _pathFound = false;
                endPosition = selectorPosition;
                Agent.CalculatePath(endPosition, _path);
            }
        }

        public void DontDestroyPathOnCancel()
        {
            _pathPersistent = true;
        }

        public void CancelPath() 
        {
            if (Agent.isOnNavMesh && !_pathPersistent) Agent.ResetPath();
        }

        public void SetPausePath(bool pause) 
        {
            // Only pause path if it's not persistent
            if (Agent.isOnNavMesh)
            {
                Agent.isStopped = (!_pathPersistent || !pause) && pause;
            }
        }

        public bool RunPathing()
        {
            _pathFound = _path.status == NavMeshPathStatus.PathComplete;
            if (!_pathFound) return false;
            Agent.SetPath(_path);
            return true;
        }

        public bool PathFinished()
        {
            if (!Agent.isOnNavMesh || Agent.pathPending || !_pathFound) return false;
            return Agent.remainingDistance <= Agent.stoppingDistance;
        }
    }
}