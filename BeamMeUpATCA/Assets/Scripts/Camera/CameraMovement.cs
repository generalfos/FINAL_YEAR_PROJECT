using System;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class CameraMovement : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        private Vector3 CameraPosition;
        private bool freeCam;
        private bool dragRotation;
        private float x;
        private float y;
        private float z;

        // Awake is init function. Start before first frame
        [Header("Camera Settings")]
        public float CameraSpeed;
        public float RotationSpeed;
        public float freeLookSensitivity;

        private void Awake() {
            CameraPosition = this.transform.position;
        }

        private void Start() {}
        
        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                dragRotation = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                dragRotation = false;
            }
            if (dragRotation)
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * RotationSpeed, -Input.GetAxis("Mouse X") * RotationSpeed, 0));
                x = transform.rotation.eulerAngles.x;
                y = transform.rotation.eulerAngles.y;
                z = transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(x, y, z);
            }
        }

        private void FixedUpdate() {
            if (!freeCam)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    CameraPosition.z += CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    CameraPosition.z -= CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    CameraPosition.x += CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    CameraPosition.x -= CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    CameraPosition.y += CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    CameraPosition.y -= CameraSpeed / Time.fixedDeltaTime;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    dragRotation = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnterFreeCam();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                ExitFreeCam();
            }
            this.transform.position = CameraPosition;
        }

        private void EnterFreeCam()
        {
            this.freeCam = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ExitFreeCam()
        {
            this.freeCam = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}