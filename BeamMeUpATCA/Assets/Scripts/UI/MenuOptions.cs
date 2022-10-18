using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class MenuOptions : MonoBehaviour
    {
        [SerializeField] private Button[] Options;
        [field: SerializeField] public AudioSource ButtonOnHoverSound { get; set; }
        [field: SerializeField] public AudioSource ButtonOnClickSound { get; set; }

        private void Awake()
        {
            Options = gameObject.GetComponentsInChildren<Button>(true);
            
            foreach (Button button in Options)
            {
                // Return EventTrigger, if none found attach and return a new one
                MenuOptionTrigger menuTrigger = button.TryGetComponent(out MenuOptionTrigger trigger)
                    ? trigger : button.gameObject.AddComponent<MenuOptionTrigger>();

                menuTrigger.Button = button;
                menuTrigger.ClickSound = ButtonOnClickSound;
                menuTrigger.HoverSound = ButtonOnHoverSound;
                
                
                Transform child = null;
                // Find child text object. If it doesn't exist create a new dummy text object
                foreach (Transform transform in menuTrigger.GetComponentInChildren<Transform>(true))
                {
                    if (transform == menuTrigger.transform) continue;
                    child = transform;
                    break;
                }

                if (!child) child = Instantiate(new GameObject("Dummy Text")).transform;
                
                child.SetParent(menuTrigger.transform);
                
                menuTrigger.Text = child.TryGetComponent(out TextMeshProUGUI text) 
                    ? text : child.gameObject.AddComponent<TextMeshProUGUI>();
            }
        }
    }
}