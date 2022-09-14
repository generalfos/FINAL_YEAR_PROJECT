using UnityEngine;

namespace BeamMeUpATCA
{
    public class CameraFocus : MonoBehaviour
    {
        public Camera PlayerCamera { private get; set; }
        public Vector2 targetLocation;
        
  
        private void Awake() {}
        private void Start() {}
        
        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update()
        {
            Vector3 targetLocation3D = targetLocation;
            PlayerCamera.transform.position = targetLocation3D;
        }
        private void LateUpdate() {}
    }
}