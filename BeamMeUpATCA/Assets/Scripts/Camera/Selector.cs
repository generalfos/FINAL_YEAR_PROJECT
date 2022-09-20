using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public static class Selector
    {       
        public static GameObject SelectGameObject(Camera camera, Vector2 screenPoint, List<string> selectableTags) 
        {
            Ray RayCast = camera.ScreenPointToRay(screenPoint);
            RaycastHit RayCastHit;

            // Guard clause to exit and return null if nothing is collided with the raycast.
            if (!Physics.Raycast(RayCast, out RayCastHit, camera.farClipPlane)) { return null; } 

            string RayCastHitTag = RayCastHit.transform.gameObject.tag;            

            // Guard clause to exit and return null if object hit doesn't have a tag from selectableTags
            if (!selectableTags.Contains(RayCastHitTag)) { return null; } 

            return RayCastHit.transform.gameObject;
        }

        public static Vector3 NearestWalkable(Camera camera, Vector2 screenPoint) 
        {
            throw new System.NotImplementedException();
        }

        // HACK: This function does not return the expected value. Instead it returns
        // A random position within 80f units of the ScreenPointToRay hit.point
        // This is done to avoid NavMeshAgents getting stuck trying to head to the same
        // position. There is also no check for the position being walkable as the method
        // name implies. Please obsolete this method by implementing it above in NearestWalkable().
        public static Vector3 NearestWalkableHACK(Camera camera, Vector2 screenPoint)
        { 
            if (Physics.Raycast(camera.ScreenPointToRay(screenPoint), out RaycastHit hit))
            {
                // Temp change to avoid exact agent collisions.
                float tempX = Random.Range(-40.0f, 40.0f);
                float tempZ = Random.Range(-40.0f, 40.0f);
                return new Vector3(hit.point.x + tempX, hit.point.y, hit.point.z + tempZ);
            } 
            else 
            {
                return Vector3.zero;
            }
        }
    }
}