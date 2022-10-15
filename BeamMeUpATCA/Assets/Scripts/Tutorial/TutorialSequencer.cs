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
using System;

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
        [field: SerializeField] private Player ActivePlayer { get; set; }
        private CameraController _activeCamera;
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
        private bool tutFinished;

        // Awake is init function. Start before first frame
        private void Awake()
        {
            _activeCamera = ActivePlayer.PlayerCamera;
            tutSeqNo = 0;
            popUpActive = false;
            tutFinished = false;
            data = JsonConvert.DeserializeObject<PopUpRoot>(jsonData.text);
        }

        // Tear down prompts after use
        private void DestroyPrompt(GameObject prompt)
        {
            Destroy(prompt);
        }

        // Temporarily hide a prompt
        static private void HidePrompt(GameObject prompt)
        {
            prompt.SetActive(false);
        }

        // Unhide a prompt
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

        // Handles moving to and displaying next tutorial popup
        private void EnterNextTutPhase()
        {
            try {
                PopUpDatum datum = data.popUpDatum[tutSeqNo];
                CreateNewPrompt(datum.title, datum.content);
            }
            catch (ArgumentOutOfRangeException) {
                tutFinished = true;
            }
        }

        private void ResetCamera() 
        {
            //TODO move this to CameraController
            _activeCamera.transform.position = CameraStartingPosition;
            _activeCamera.transform.rotation = CameraStartingRotation;
        }

        // Update to next tutorial stage on completion of a stage
        private void Update()
        {
            if (!popUpActive && !tutFinished) {
                EnterNextTutPhase();
            }
        }
    }
}