using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeamMeUpATCA
{
    public class MainMenu : MonoBehaviour
    {
        public void Awake()
        {
            AudioListener.volume = Volume;
        }

        public static float Volume { get; set; } = 0.75f;

        public static void Play(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
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