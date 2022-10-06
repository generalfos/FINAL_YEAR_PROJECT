using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace BeamMeUpATCA
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {

        [field: Header("Camera Settings")]
        [SerializeField] private Camera _camera;
        public Camera ActiveCamera {
            get
            {
                if (_camera == null)
                {
                    Debug.LogWarning("CameraController requires a valid 'Camera' inspector field."
                        + "Failing dependencies safely. This may lead to instability.", gameObject);
                    // Return an attached Camera, otherwise attach one and return it.
                    _camera = TryGetComponent<Camera>(out Camera cam) ? cam : gameObject.AddComponent<Camera>();
                }
                return _camera;
            }
        }

        [field: SerializeField] public float CameraSpeed { get; set; } = 20f;
        [field: SerializeField] public float CameraZoom { get; set; } = 5f;


        #region Bounding Settings
        [field: Header("Boundary Settings")]
        [field: SerializeField] public bool BoundaryActive { get; set; } = false;

        /**
         * The Camera Boundaries can be a bit confusing, but here is an example
         * of how they work. Below is a diagram of a projected Camera 
         * perspective viewport and where each of the boundaries sit in the 
         * 3D world space.
         *
         * (top left) --------------- (top) --------------- (top right)
         *      \                                               /
         *       \                                             /
         *        \                                           /
         *         \                                         /
         *          \                                       /
         *           \                                     /
         *            \                                   /
         *             \_____________(bottom)____________/
         *
         */

        // World position that Boundaries define as the (0,0,0) point
        [field: SerializeField] private Vector3 BoundingPoint { get; set; } = Vector3.zero;

        // Top bounds: +Z
        [SerializeField] private float _boundaryTop;
        public float BoundingTop { 
            get { return _boundaryTop + BoundingPoint.z; } 
            set { _boundaryTop = value; } }

        // Bottom bounds: -Z
        [SerializeField] private float _boundaryBottom;
        public float BoundingBottom { 
            get { return _boundaryBottom + BoundingPoint.z; }
            set { _boundaryBottom = value; }}

        // Right bounds: +X
        [SerializeField] private float _boundaryTopRight;
        public float BoundingRight { 
            get { return _boundaryTopRight + BoundingPoint.x; }
            set { _boundaryTopRight = value; }}

        // Left bounds: -X
        [SerializeField] private float _boundaryTopLeft;
        public float BoundingLeft { 
            get { return _boundaryTopLeft + BoundingPoint.x;}
            set { _boundaryTopLeft = value; } }

        // Upper bounds: +Y
        [SerializeField] private float _boundaryHighestHeight;
        public float BoundingHighestHeight { 
            get { return _boundaryHighestHeight + BoundingPoint.y; }
            set { _boundaryHighestHeight = value; }}

        // Lower bounds: -Y
        [SerializeField] private float _boundaryLowestHeight;
        public float BoundingLowestHeight { 
            get { return _boundaryLowestHeight + BoundingPoint.y;}
            set { _boundaryLowestHeight = value; } }

        #endregion // Bounding Settings

        // Public CameraController properties
        public bool DragRotation { private get; set; }
        public Vector2 Camera2DAdjust { private get; set; }
        public float CameraZoomAdjust { private get; set; }

        public void FocusCamera(Vector2 focusPosition)
        {
            // Offsets positioning camera for fairly tight view of target
            int yOffset = 30;
            int zOffset = -20;

            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, focusPosition, new[] { "Building" });
            // Guard clause to check valid return from function.
            if (selectedObject == null) { return; }
            if (selectedObject.GetComponent<Building>() == null) { return; }

            // Find the positions of the building and camera - Use the difference to reposition the camera
            Vector3 objectPos = selectedObject.transform.position;
            Vector3 cameraPos = ActiveCamera.transform.position;
            Vector3 positionDiff = objectPos - cameraPos + new Vector3(0, yOffset, zOffset);

            //TODO
            Debug.Log(selectedObject.name + " is at " + objectPos);
            Debug.Log("Camera is at " + cameraPos);

            ActiveCamera.transform.position += positionDiff;

        }

        // Vector3 for storying the adjustment of the camera position.
        private Vector3 CameraPosition;

        private void Update()
        {
            if (Camera2DAdjust.x != 0 || Camera2DAdjust.y != 0 || CameraZoomAdjust != 0)
            {
                float CameraMoveInc = CameraSpeed * Time.deltaTime;
                float CameraZoomInc = CameraZoom * Time.deltaTime;

                float xIncrement = Camera2DAdjust.x * CameraMoveInc;
                float zIncrement = Camera2DAdjust.y * CameraMoveInc;
                float yIncrement = CameraZoomAdjust * CameraZoomInc;

                CameraPosition = new Vector3(xIncrement, yIncrement, zIncrement);
            }
        }

        // Calculated manually by placing a sphere in Unity and seeing what
        // coordinates make it the edge of the Game Camera View and then 
        // find the multiple.
        private const float BoundaryOffset = 86;

        /**
         * This takes a given 3D coordinate position and then constrain it to
         * the Boundary variables. The returned value is the new position.
         */
        private Vector3 BoundCamera(Vector3 cameraPosition)
        {
            // Don't bound camera if not active
            if (!BoundaryActive) return cameraPosition;

            // Establish viewable positions based on its frustum
            float bottomView = cameraPosition.z;
            float topView = cameraPosition.z + BoundaryOffset;
            float leftView = cameraPosition.x - BoundaryOffset;
            float rightView = cameraPosition.x + BoundaryOffset;

            // Constrain camera Position
            if (bottomView < BoundingBottom) cameraPosition.z = BoundingBottom;
            if (topView > BoundingTop) cameraPosition.z = BoundingTop - BoundaryOffset;
            if (leftView < BoundingLeft) cameraPosition.x = BoundingLeft + BoundaryOffset;
            if (rightView > BoundingRight) cameraPosition.x = BoundingRight - BoundaryOffset;

            // Set new camera position
            return cameraPosition;
        }

        private Vector3 BoundCameraHeight(Vector3 cameraPosition)
        {
            // Don't bound camera if not active
            if (!BoundaryActive) return cameraPosition;

            // Constrain camera Position
            if (cameraPosition.y < BoundingLowestHeight) cameraPosition.y = BoundingLowestHeight;
            if (cameraPosition.y > BoundingHighestHeight) cameraPosition.y = BoundingHighestHeight;

            // Set new camera position
            return cameraPosition;
        }

        private void LateUpdate()
        {
            // Get the current Camera Position
            Vector3 updateCameraPosition = ActiveCamera.transform.position;

            { /* Update Camera Positions */
                updateCameraPosition += CameraPosition;
                updateCameraPosition = BoundCamera(updateCameraPosition);
                updateCameraPosition = BoundCameraHeight(updateCameraPosition);
            }

            // Set the Final Camera Position
            ActiveCamera.transform.position = updateCameraPosition;

            CameraPosition = Vector3.zero;
        }
    }
}