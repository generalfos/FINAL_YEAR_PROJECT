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

        // External reference to stored text data
        [field: SerializeField]
        private TextAsset jsonData { get; set; }
        // External references to game objects necessary for tutorial progress.
        [field: SerializeField]
        private CameraController _activeCamera;
        [field: SerializeField]
        private GameObject Engineer { get; set; }
        [field: SerializeField]
        private GameObject Canvas { get; set; }
        [field: SerializeField]
        private GameObject PopUpObj { get; set; }
        private PopUp popUp;

        private PopUpRoot data;
        private int tutSeqNo;
        private bool popUpActive;
        private bool tutFinished;

        // Awake is init function. Start before first frame
        private void Awake()
        {
            tutSeqNo = 0;
            popUpActive = false;
            tutFinished = false;
            data = JsonConvert.DeserializeObject<PopUpRoot>(jsonData.text);
            popUp = PopUpObj.GetComponent<PopUp>();
            popUp.attach_listener(HandleOk);
            PopUpDatum datum = data.popUpDatum[0];
            UpdatePrompt(datum.title, datum.content);
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
        private void UpdatePrompt(string title, string content)
        {
            // Get references to new text fields and button
            // Set text
            popUp.update_title(title);
            popUp.update_content(content);
            popUpActive = true;
        }
        
        // Resolver for Ok Acknowledgement
        private void HandleOk()
        {
            popUpActive = false;
            HidePrompt(PopUpObj);
            tutSeqNo += 1;
        }

        // Handles moving to and displaying next tutorial popup
        private void EnterNextTutPhase()
        {
            try {
                PopUpDatum datum = data.popUpDatum[tutSeqNo];
                UpdatePrompt(datum.title, datum.content);
                ShowPrompt(PopUpObj);
                if (tutSeqNo == (int)TutSeqStage.EngineerIntro)
                {
                    _activeCamera.FocusCamera(Engineer.transform.position);
                }
            }
            catch (ArgumentOutOfRangeException) {
                tutFinished = true;
            }
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