using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BeamMeUpATCA
{
    public class MainMenu : MonoBehaviour
    {
        static bool notPlaying = true;
        static bool mainMenu = true;
        public void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
            AudioListener.volume = Volume;
        }

        public static float Volume { get; set; } = 0.75f;

        public static void Play(int sceneIndex)
        {
            if(notPlaying) {
                SceneManager.LoadSceneAsync(sceneIndex);
                notPlaying = false;
                mainMenu = true;
            }
        }

        public static void MainMenuButton() 
        {
            if(mainMenu) {
                notPlaying = true;
                SceneManager.LoadSceneAsync(0);
                mainMenu = false;
            }          
        }
        
        public static void Play(string sceneName)
        {
            bool notPlaying = true;
            if(notPlaying) {
                SceneManager.LoadSceneAsync(sceneName);
                notPlaying = false;
                mainMenu = true;
            }
        }
        
        public static void SetVolume(float volumeAmount)
        {
            Volume = volumeAmount;
            AudioListener.volume = Volume;
        }
        
        public static void Quit(int exitCode)
        {
            Application.Quit(exitCode);
        }
    }
}