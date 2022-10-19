using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BeamMeUpATCA
{
    public class MainMenu : MonoBehaviour
    {
        public void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
            AudioListener.volume = Volume;
        }

        public static float Volume { get; set; } = 0.75f;

        public static bool isLoading = false;

        public static void Play(int sceneIndex)
        {
            if (!isLoading)
            {
                isLoading = true;
                SceneManager.LoadSceneAsync(sceneIndex);
            }
        }
        
        public static void Play(string sceneName)
        {
            if (!isLoading)
            {
                isLoading = true;
                SceneManager.LoadSceneAsync(sceneName);
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