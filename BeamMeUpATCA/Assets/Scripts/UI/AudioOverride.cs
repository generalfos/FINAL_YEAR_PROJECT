using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class AudioOverride : MonoBehaviour
    {
        [SerializeField] private AudioSource ClickSound;

        private void Awake()
        {
            if (ClickSound)
            {
                foreach (Button button in GetComponentsInChildren<Button>())
                {
                    ButtonTrigger bt = button.transform.gameObject.AddComponent<ButtonTrigger>();
                    bt.clickSound = ClickSound;
                }
            }
        }
    }

    public class ButtonTrigger : EventTrigger
    {
        public AudioSource clickSound;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!clickSound) return;
            clickSound.Play();
        } 
    }
}