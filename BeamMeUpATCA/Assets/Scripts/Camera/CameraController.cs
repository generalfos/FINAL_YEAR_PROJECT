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

        private void LateUpdate() 
        {
            // Debug.Log("CameraPosition: " + CameraPosition.ToString());
            ActiveCamera.transform.position += CameraPosition;
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