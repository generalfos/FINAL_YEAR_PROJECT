using System;
using UnityEngine;

namespace BeamMeUpATCA
{
    [Serializable]
    public class BuildingAnchor
    {
        [SerializeField]
        private Transform[] anchorPoints; // All possible anchor points

        [SerializeField]
        private float anchorRadius = 1f; // Distance to anchor point to be considered docked

        /**
         * This function should be called by external scripts/objects. It will
         * return a random anchor point position or the position of the object
         * this script is attached to. Below is an example on how to use it:
         *
         *      // Call the function externally
         *      GameObject target = ... // Target is this object the script is on. 
         *      Vector3 pos = target.GetComponent<BuildingAnchor>().GetAnchorPoint();
         *      
         *      // Do something with the new position
         *      Debug.Log(pos);
         *
         */
        public Vector3 GetAnchorPoint() {

            // If nothing is set, return the current objects transform
            if (anchorPoints.Length == 0) 
            {
                Debug.LogError("No anchor points were specified. Using default anchor Vector3.zero");
                return Vector3.zero;
            }

            // Get Random Anchor Point
            return this.anchorPoints[UnityEngine.Random.Range(0, this.anchorPoints.Length - 1)].position;
        }

        // Checks if position is within the anchorpoint radius
        public bool CanAnchor(Vector3 pos) 
        {
            foreach (Transform anchor in anchorPoints) 
            {
                if (Vector3.Distance(anchor.position, pos) <= anchorRadius) return true;
            }
            return false;
        }
    }
}