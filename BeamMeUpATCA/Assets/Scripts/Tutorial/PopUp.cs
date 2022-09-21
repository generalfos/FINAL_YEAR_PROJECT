using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class PopUp : MonoBehaviour
    {
        [field: SerializeField] public Text Title { get; private set; }
        [field: SerializeField] public Text Content { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }

        public void update_title(string title)
        {
            Title.text = title;
        }

        public void update_content(string content)
        {
            Content.text = content;
        }

        public void attach_listener(UnityEngine.Events.UnityAction handler)
        {
            Button.onClick.AddListener(handler);
        }
    }
}