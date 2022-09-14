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

            if (!selectableTags.Contains(RayCastHitTag)) { return null; } 

            Debug.Log("Hit: " + RayCastHit.transform.gameObject.name);
            return RayCastHit.transform.gameObject;
        }
    }
}