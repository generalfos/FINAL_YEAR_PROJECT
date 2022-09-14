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
    }
}