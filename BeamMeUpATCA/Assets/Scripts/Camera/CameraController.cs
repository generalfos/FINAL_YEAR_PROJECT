using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using BeamMeUpATCA.Extensions;
using UnityEngine;

namespace BeamMeUpATCA
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {

        [field: Header("Camera Settings")]
        [SerializeField] private Camera _camera;
        public Camera ActiveCamera => this.SafeComponent<Camera>(ref _camera);

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

            IInteractable interactable = Selector.SelectGameObject(ActiveCamera, focusPosition, Mask.Building);

            if (interactable is Building building)
            {
                // Find the positions of the building and camera - Use the difference to reposition the camera
                Vector3 cameraPos = ActiveCamera.transform.position;
                Vector3 buildingPos = building.transform.position;
                Vector3 positionDiff = buildingPos - cameraPos + new Vector3(0, yOffset, zOffset);

                //TODO
                Debug.Log(building.name + " is at " + buildingPos);
                Debug.Log("Camera is at " + cameraPos);

                cameraPos += positionDiff;
                ActiveCamera.transform.position = cameraPos;
            }
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


        /**
         * This is a function that can be called from anywhere without context.
         * It will re-align its positioning to have a `target` object in the 
         * middle of the viewport. To call this function you can do the 
         * following:
         *
         *      using BeamMeUpATCA; // Only necessary if not in the namespace
         *
         *      Transform target = ... // The thing you want to focus on.
         *      CameraController.CameraFocus(Camera.main, target.transform);
         *
         * @param camera        - The source camera to re-align.
         * @param target        - A target Transform to focus on.
         * @param camera_angle  - An optional field that defaults to 50deg on
         *                        the X-Axis. This is used to calculate how far
         *                        the camera should be away on the X-Axis
         * @param camera_height - An optional field that defaults to 300 on the
         *                        Y-Axis, for the height to set the camera to.
         * @param camera_offset - An optional field that defaults to 80 on
         *                        the X-Axis. This is used to fine tune the 
         *                        focused object on the X-Axis.
         */
        public static void CameraFocus(Camera camera, Transform target, 
                float camera_angle = 50.0f, 
                float camera_height = 300.0f, 
                float camera_offset = 80.0f)
        {
            // Calculate the Offset using Trig
            float offset = camera_height * Mathf.Tan(90.0f - camera_angle);
            offset += camera_offset;

            // Calculate the new camera's position
            Vector3 new_position = new Vector3(target.position.x, 
                                               camera_height, 
                                               target.position.z + offset);

            // Set the new position to the camera
            camera.transform.position = new_position;
        }
    }
}