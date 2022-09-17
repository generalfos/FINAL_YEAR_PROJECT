using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class CameraController : MonoBehaviour
    {

        [Header("Camera Settings")]
        public float CameraSpeed;
        public float CameraZoom;
        //public float RotationSpeed;
        //public float freeLookSensitivity;

        [Header("Boundry Settings")]
        public bool BoundryActive = false;

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
        [field: SerializeField]
        public Vector3 BoundingPoint { get; set; } = Vector3.zero;

        public float BoundryTop;
        public float BoundryTopLeft;
        public float BoundryTopRight;
        public float BoundryBottom;

        public float BoundingTop { get { return BoundryTop + BoundingPoint.z;}}
        public float BoundingLeft { get { return BoundryTopLeft + BoundingPoint.x;}}
        public float BoundingRight { get { return BoundryTopRight + BoundingPoint.x;}}
        public float BoundingBottom { get { return BoundryBottom + BoundingPoint.z;}}

        public float BoundryLowestHeight;
        public float BoundryHighestHeight;

        public float BoundingLowestHeight { get { return BoundryLowestHeight + BoundingPoint.y;}}
        public float BoundingHighestHeight { get { return BoundryHighestHeight + BoundingPoint.y;}}

        // Setters for Camera
        public Camera ActiveCamera { private get; set; }
        public bool DragRotation { private get; set; }
        
        public Vector2 Camera2DAdjust { private get; set; }
        public float CameraZoomAdjust { private get; set; }

        private Vector3 CameraPosition;
        //private bool freeCam;

        private void Start() {
            if (ActiveCamera == null) 
            {
                Debug.Log("Using MainCamera.");
                ActiveCamera = Camera.main;
            }

        }

        public void FocusCamera(Vector2 focusPosition)
        {
            // Offsets positioning camera for fairly tight view of target
            int yOffset = 30;
            int zOffset = -20;
            
            GameObject selectedObject = Selector.SelectGameObject(ActiveCamera, focusPosition, new List<string>{"Building"});
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

        private void Update() 
        {
            // TODO: Need to re-implement this with new hookup to Player.cs
            // if (DragRotation)
            // {
            //     transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * RotationSpeed, -Input.GetAxis("Mouse X") * RotationSpeed, 0));
            //     Camera2DAdjust = new Vector2(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.z);
            //     CameraZoomAdjust = transform.rotation.eulerAngles.y;

            //     transform.rotation = Quaternion.Euler(Camera2DAdjust.x, CameraZoomAdjust, Camera2DAdjust.y);
            // }
            
            if (true) 
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

        }

        private const float BoundryOffset = 86;

        /**
         * This takes a given 3D coordinate position and then constrain it to
         * the Boundry variables. The returned value is the new position.
         */
        private Vector3 BoundCamera(Vector3 cameraPosition) {
            // Dont bound camera if not active
            if (!BoundryActive) return cameraPosition;

            // Calculated manually by placing a sphere in Unity and seeing what
            // coordinates make it the edge of the Game Camera View and then 
            // find the multiple.
            const float BoundryOffset = 86; 

            // Establish viewable positions based on its frustum
            float bottomView = cameraPosition.z;
            float topView = cameraPosition.z + BoundryOffset;
            float leftView = cameraPosition.x - BoundryOffset;
            float rightView = cameraPosition.x + BoundryOffset;

            // Constrain camera Position
            if (bottomView < BoundingBottom)  cameraPosition.z = BoundingBottom;
            if (topView > BoundingTop)        cameraPosition.z = BoundingTop - BoundryOffset;
            if (leftView < BoundingLeft)   cameraPosition.x = BoundingLeft + BoundryOffset;
            if (rightView > BoundingRight) cameraPosition.x = BoundingRight - BoundryOffset;

            // Set new camera position
            return cameraPosition;
        }

        private Vector3 BoundCameraHeight(Vector3 cameraPosition) 
        {
            // Don't bound camera if not active
            if (!BoundryActive) return cameraPosition;

            // Constrain camera Position
            if (cameraPosition.y < BoundingLowestHeight)  cameraPosition.y = BoundingLowestHeight;
            if (cameraPosition.y > BoundingHighestHeight)  cameraPosition.y = BoundingHighestHeight;

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
        // TODO: Need to re-implement this with new hookup to Player.cs
        // private void EnterFreeCam()
        // {
        //     this.freeCam = true;
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        // private void ExitFreeCam()
        // {
        //     this.freeCam = false;
        //     Cursor.visible = true;
        //     Cursor.lockState = CursorLockMode.None;
        // }
    }
}