/** 
 * TutorialSequencer.cs
 * 
 * This script handles the overall tutorial process from information popups
 * to instructing users what they should do next. Behaves as the macro organiser
 * of in game events during the tutorial scene.
 * 
 * @author: Joel Foster
 * @last_edited: 12/09/2022 21:38
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace BeamMeUpATCA
{
    public class TutorialSequencer : MonoBehaviour
    {
        // External references to game objects necessary for tutorial progress.
        [field: SerializeField] public GameObject TutPopUpPrefab { get; private set; }
        [field: SerializeField] public GameObject Player { get; private set; }
        [field: SerializeField] public GameObject Camera { get; private set; }
        [field: SerializeField] public GameObject Engineer { get; private set; }
        [field: SerializeField] public GameObject Canvas { get; private set; }

        [field: SerializeField] public Vector3 CameraStartingPosition { get; private set; }
        [field: SerializeField] public Quaternion CameraStartingRotation { get; private set; }

        // Position popup directly in centre of screen
        private Vector3 screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        private GameObject currPopUp;
        private int tutSeqNo;
        private bool popUpActive;

        // TODO: Introduce serialised fields for these elements or another more appropriate method
        private const int titleIndex = 0;
        private const int contentIndex = 1;
        private const int btnIndex = 2;

        // Awake is init function. Start before first frame
        private void Awake() {
            tutSeqNo = 0;
            popUpActive = false;
            Intro();
        }

        // Tear down prompts after use
        private void DestroyPrompt(GameObject prompt)
        {
            Destroy(prompt);
        }

        // Temporarily hide prompts
        static private void HidePrompt(GameObject prompt)
        {
            prompt.SetActive(false);
        }

        // Unhide prompt
        static private void ShowPrompt(GameObject prompt)
        {
            prompt.SetActive(true);
        }

        // General method for instantiating a new prompt
        private void CreateNewPrompt(string title, string content)
        {
            // Only allow one active popup at any given time
            if (currPopUp)
            {
                DestroyPrompt(currPopUp);
                currPopUp = null;
            }
            GameObject newPrompt = Instantiate(TutPopUpPrefab, screenCenter, Quaternion.identity, Canvas.transform);
            // Get references to new text fields and button
            // Set text and button listner
            PopUp popup = newPrompt.GetComponent<PopUp>();
            popup.update_title(title);
            popup.update_content(content);
            popup.attach_listener(HandleOk);
            currPopUp = newPrompt;
            popUpActive = true;
        }
        
        // Resolver for Tutorial Ok clicked
        private void HandleOk()
        {
            Debug.Log("Button Clicked");
            DestroyPrompt(currPopUp);
            currPopUp = null;
            popUpActive = false;
            tutSeqNo += 1;
        }

        // Welcome message
        private void Intro()
        {
            Debug.Log("Tutorial Sequence: Intro started");
            CreateNewPrompt("Welcome to the Beam Me Up ATCA Tutorial",
                "In this tutorial you will manage the ATCA whilst it performs an observation of" +
                " Betelguese");
        }

        // Introducing engineers
        private void EngineerTut()
        {
            Camera.transform.LookAt(Engineer.transform);
            CreateNewPrompt("Meet The Engineer", "The engineer is one of two main unit types " +
                "which you will use while performing an observation with the ATCA. " +
                "\nSelect the engineer now by left clicking the unit with the mouse hovering over it. ");
        }

        private void ResetCamera() 
        {
            Camera.transform.position = CameraStartingPosition;
            Camera.transform.rotation = CameraStartingRotation;
        }

        // Update to next tutorial stage on completion of a stage
        private void Update()
        {
            if (!popUpActive)
            {
                switch (tutSeqNo)
                {
                    case 0:
                        Intro();
                        break;
                    case 1:
                        EngineerTut();
                        break;
                    case 2:
                        ResetCamera();
                        tutSeqNo++;
                        break;
                }
            }
            else
            {
                PlayerInput actionSet = Player.GetComponent<PlayerInput>();
            }
        }
    }
}