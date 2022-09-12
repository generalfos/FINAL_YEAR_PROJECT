using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class TutorialSequencer : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        [field: SerializeField] public GameObject TutPopUpPrefab { get; private set; }
        // Position popup directly in centre of screen
        private Vector3 screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        private GameObject currPopUp;
        static private int titleIndex = 0;
        static private int contentIndex = 1;

        // Awake is init function. Start before first frame
        private void Awake() {
            Intro();
        }

        private void DestroyPrompt(GameObject prompt)
        {
            Destroy(prompt, 1.0f);
        }

        static private void HidePrompt(GameObject prompt)
        {
            prompt.SetActive(false);
        }

        static private void ShowPrompt(GameObject prompt)
        {
            prompt.SetActive(true);
        }

        private void CreateNewPrompt(string title, string content)
        {
            // Only one active popup allowed at any given time
            if (currPopUp)
            {
                DestroyPrompt(currPopUp);
                currPopUp = null;
            }
            GameObject newPrompt = Instantiate(TutPopUpPrefab, screenCenter, Quaternion.identity, gameObject.transform);
            Text Title = newPrompt.transform.GetChild(0).GetChild(titleIndex).GetComponentInChildren<Text>();
            Text Content = newPrompt.transform.GetChild(0).GetChild(contentIndex).GetComponentInChildren<Text>();
            Title.text = title;
            Content.text = content;
            currPopUp = newPrompt;
        }
        
        private void Intro()
        {
            CreateNewPrompt("Hello", "Welcome");
        }
    }
}