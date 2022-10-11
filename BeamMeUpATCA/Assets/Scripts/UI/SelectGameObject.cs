using UnityEngine;

namespace BeamMeUpATCA
{
    public class SelectGameObject : MonoBehaviour
    {
        public Camera activeCamera;

        void Awake() {
            if (activeCamera == null) {
                if (Camera.main == null) {
                    ErrorMainCamera();
                    ErrorNoCamera();
                    return;
                }

                // Set the Active Camear to the Main one
                activeCamera = Camera.main;
            }
        }

        // Select Dish when UI button is pressed
        public void ObjectSelection (GameObject objectToSelect)
        {
            if (activeCamera == null) {
                ErrorNoCamera();
                return;
            }

            #if UNITY_EDITOR
                // Selects the Object in the inspector
                UnityEditor.Selection.activeGameObject = objectToSelect;
            #endif
            
            // Focus the Camera onto the selected object
            CameraController.CameraFocus(activeCamera, objectToSelect.transform);
        }

        private void ErrorMainCamera() {
            Debug.LogError("SelectGameObject: There is no Camera tagged as 'MainCamera'");
        }
        private void ErrorNoCamera() {
            Debug.LogError("SelectGameObject: There is no Camera linked to `activeCamera`");
        }
    }
}
