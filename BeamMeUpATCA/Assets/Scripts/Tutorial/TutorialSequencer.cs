/** 
 * TutorialSequencer.cs
 * 
 * This script handles the overall tutorial process from information popups
 * to instructing users what they should do next. Behaves as the macro organiser
 * of in game events during the tutorial scene.
 * 
 * @author: Joel Foster
 * @last_edited: 4/10/2022 21:38
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeamMeUpATCA
{
    enum TutSeqStage
    {
        Welcome,
        EngineerIntro,
        HealthIntro,
        CriticalHealth,
        RepairProgress,
        RailFix,
        MoraleIntro,
        ScientistIntro,
        StationingUnit,
        WeatherStation,
        EngineerReturn,
        RadioSignals,
        Storm,
        MultiUnitCommand,
        MoraleLoss,
        PowerLoss,
        ScientistObserving,
        ArrayConfiguation,
        ScoreIncrease,
        Overheating,
        OverheatingFix,
        End
    }

    public class PopUpDatum
    {
        public int seq_no { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    public class PopUpRoot
    {
        public List<PopUpDatum> popUpDatum { get; set; }
    }

    public class TutorialSequencer : MonoBehaviour
    {
        // External reference to stored text data
        [field: SerializeField] public TextAsset jsonData { get; private set; }
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
        private PopUpRoot data;
        private int tutSeqNo;
        private bool popUpActive;

        // Awake is init function. Start before first frame
        private void Awake() {
            tutSeqNo = 0;
            popUpActive = false;
            data = JsonConvert.DeserializeObject<PopUpRoot>(jsonData.text);
        // PopUpDatum pop = new PopUpDatum();
        // pop.title = "Hello";
        // pop.content = "Content";
        // PopUpRoot popList = new PopUpList();
        // popList.popUpData = new List<PopUpDatum>();
        // popList.popUpData.Add(pop);
        // string jsonString = JsonConvert.SerializeObject(popList);
        // Debug.LogFormat("Json Doc:" + jsonString);
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
            // Set text and button listener
            PopUp popup = newPrompt.GetComponent<PopUp>();
            popup.update_title(title);
            popup.update_content(content);
            popup.attach_listener(HandleOk);
            currPopUp = newPrompt;
            popUpActive = true;
        }
        
        // Resolver for Ok Acknowledgement
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
            PopUpDatum datum = data.popUpDatum[tutSeqNo];
            Debug.Log("Tutorial Sequence: Intro started");
            CreateNewPrompt(datum.title, datum.content);
        }

        // Handles moving to and displaying next tutorial popup
        private void EnterNextTutPhase()
        {
            PopUpDatum datum = data.popUpDatum[tutSeqNo];
            CreateNewPrompt(datum.title, datum.content);
        }

        // Introducing engineers (DEPRECATED)
        private void EngineerTut()
        {
            CameraFocus(Engineer);
            CreateNewPrompt("Meet The Engineer", "The engineer is one of two main unit types " +
                "which you will use while performing an observation with the ATCA. " +
                "\nSelect the engineer now by left clicking the unit with the mouse hovering over it. ");
        }

        private void CameraFocus(GameObject obj)
        {
            Vector3 pos = obj.transform.position;
            Camera.transform.position = Vector3.MoveTowards(CameraStartingPosition, new Vector3(obj.transform.position.x, 0f, obj.transform.position.z), (float) 1);
            Camera.transform.LookAt(obj.transform.position);
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
                EnterNextTutPhase();
            }
            else
            {
                PlayerInput actionSet = Player.GetComponent<PlayerInput>();
            }
        }
    }
}