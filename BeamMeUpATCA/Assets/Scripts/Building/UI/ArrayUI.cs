using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class ArrayUI : MonoBehaviour
    {
        [SerializeField]
        private string _ArrayName;

        [SerializeField]
        private int _ArrayHealth;

        [SerializeField]
        private Image _ArrayHealthIndicator;
        
        // Update for loop per frame. FixedUpdate for loop per physics step.
        // Update() counts in Time.deltaTime. FixedUpdate counts in Time.fixedDeltaTime.
        private void Update() 
        {
            if (_ArrayHealth > 75)
            {
                _ArrayHealthIndicator.color = Color.green;
            } 
            else if (_ArrayHealth > 25)
            {
                _ArrayHealthIndicator.color = Color.yellow;
            }
            else
            {
                _ArrayHealthIndicator.color = Color.red;
            }
        }
    }
}