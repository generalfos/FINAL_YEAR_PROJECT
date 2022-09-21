using UnityEngine;

namespace BeamMeUpATCA
{
    public class BuildingAnchor : MonoBehaviour
    {
        [SerializeField]
        private Transform[] anchorPoints; // All possible anchor points

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
            if (anchorPoints.Length == 0) return this.transform.position;

            // Get Random Anchor Point
            return this.anchorPoints[Random.Range(0, this.anchorPoints.Length - 1)].position;
        }
    }
}