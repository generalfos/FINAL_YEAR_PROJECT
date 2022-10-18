using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

namespace BeamMeUpATCA
{
    public static class Selector
    {       
        public static IInteractable SelectGameObject(Camera camera, Vector2 screenPoint, int selectionMask) 
        {
            Ray ray = camera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, camera.farClipPlane, layerMask: selectionMask)) 
            {
                if (hit.transform.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    // Bitwise operation to check if the IInteractable.Mask is a sub-mask of selectionMask.
                    if (selectionMask == (selectionMask | interactable.Mask)) return interactable;
                }
            } 
            // No 'IIntractables' were found on the 'selectionMask' with the respective matching mask.
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

            if (Physics.Raycast(camera.ScreenPointToRay(screenPoint), out RaycastHit hit,
                    camera.farClipPlane, Mask.Building | Mask.Ground))
            {

                // Check for building to grab anchor point
                Building building = hit.transform.gameObject.GetComponent<Building>();

                if (building)
                {
                    return building.Anchors.GetAnchorPoint();
                }

                result = NavMesh.SamplePosition(hit.point, out NavMeshHit myNavHit, 1000f , -1) ? myNavHit.position : hit.point;

            }
            // If this isn't unit 1 then add offset to result destination.
            if (unitOffset != 0) 
            {
                result += Quaternion.Euler(0, startingAngle + loopAngle, 0) * (Vector3.forward * unitSpacing);
            }
            return result;
        }
        
    }
}