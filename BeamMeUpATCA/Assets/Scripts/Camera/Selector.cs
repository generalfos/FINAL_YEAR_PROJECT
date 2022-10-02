using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

namespace BeamMeUpATCA
{
    public static class Selector
    {       
        public static GameObject SelectGameObject(Camera camera, Vector2 screenPoint, string[] selectableTags) 
        {
            if (Physics.Raycast(camera.ScreenPointToRay(screenPoint), out RaycastHit hit, camera.farClipPlane)) 
            {

                string RayCastHitTag = hit.transform.gameObject.tag;
                foreach (string tag in selectableTags) 
                {
                    if (tag == RayCastHitTag) return hit.transform.gameObject;
                }
            } 
            // No object was found with selectableTags
            return null;
        }

        private static float[]AnglePatternOffset = {0f, 180f, -90f, 90f, -135f, 45f, 135f, -45f};

        public static Vector3 NearestWalkable(Camera camera, Vector2 screenPoint, int unitOffset = 0, float unitSpacing = 0f)
        {
            int patternLength = AnglePatternOffset.Length ;
            
            // Each loop of AnglePatternOffset varies initial angle by 180f ( 0 -> 180 -> 0 -> ...)
            float startingAngle = ((int) unitOffset/patternLength) % 2 == 0 ? 0f : 180f;
            float loopAngle = AnglePatternOffset[unitOffset % patternLength];

            Vector3 result = Vector3.zero;

            if (Physics.Raycast(camera.ScreenPointToRay(screenPoint), out RaycastHit hit))
            {

                // Check for building to grab anchor point
                Building building = hit.transform.gameObject.GetComponent<Building>();
                // Need to uncomment below when `building interface` is merged into main
                // if (building != null)
                // {
                //     return building.Anchors.GetAnchorPoint();
                // }

                NavMeshHit myNavHit;
                if(NavMesh.SamplePosition(hit.point, out myNavHit, 100 , -1))
                {
                    result = myNavHit.position;
                }
                result = hit.point;
            }
            // If this isn't unit 1 then add offset to result destination.
            if (unitOffset != 0) 
            {
            result += Quaternion.Euler(0, startingAngle + loopAngle, 0) * (Vector3.forward * unitSpacing);
            }
            return result;
        }

        // HACK: This function does not return the expected value. Instead it returns
        // A random position within 80f units of the ScreenPointToRay hit.point
        // This is done to avoid NavMeshAgents getting stuck trying to head to the same
        // position. There is also no check for the position being walkable as the method
        // name implies. Please obsolete this method by implementing it above in NearestWalkable().
        [Obsolete("NearestWalkableHACK is deprecated, please use NearestWalkable instead.")]
        public static Vector3 NearestWalkableHACK(Camera camera, Vector2 screenPoint)
        { 
            if (Physics.Raycast(camera.ScreenPointToRay(screenPoint), out RaycastHit hit))
            {
                // Temp change to avoid exact agent collisions.
                float tempX = UnityEngine.Random.Range(-40.0f, 40.0f);
                float tempZ = UnityEngine.Random.Range(-40.0f, 40.0f);
                return new Vector3(hit.point.x + tempX, hit.point.y, hit.point.z + tempZ);
            } 
            else 
            {
                return Vector3.zero;
            }
        }
    }
}