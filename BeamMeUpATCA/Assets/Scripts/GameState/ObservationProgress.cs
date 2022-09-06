using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class ObservationProgress : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        [field: SerializeField] public int SignalStrength { get; private set; }
        [field: SerializeField] public float Completeness { get; private set; }

        [SerializeField] private Slider _SignalStrength;
        [SerializeField] private Slider _Completeness;

        // Awake is init function. Start before first frame
        private void Awake() {}
        private void Start() {}
        
        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update() {
            if (SignalStrength > 0)
            {
                Completeness += (float)0.1 / ((float)100 / (float)SignalStrength);
            }
            _SignalStrength.value = (float)SignalStrength/(float)100;
            _Completeness.value = (float)Completeness/(float)100;
        }
        private void FixedUpdate() {}
    }
}