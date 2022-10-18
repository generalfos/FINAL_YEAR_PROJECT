using UnityEngine;

namespace BeamMeUpATCA
{
    public enum WeatherStates
    {

    }
    public class WeatherController : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        [field: SerializeField]
        private GameObject SunLight { get; set; }
        private Quaternion currRotation;

        [SerializeField] private float rotationSpeed = 2f;

        // Awake is init function. Start before first frame
        private void Awake() {}
        
        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update() {
            currRotation = SunLight.transform.rotation;
            SunLight.transform.Rotate(new Vector3(currRotation.x + (Time.deltaTime * rotationSpeed), 0, 0));
        }

        private void FixedUpdate() {}
    }
}