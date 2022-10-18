using System;
using System.Collections.Generic;
using BeamMeUpATCA.Extensions;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class GameManager : MonoBehaviour
    {
        #region  Singleton Management
        [SerializeField] private bool persistent = false; // Set to true if manager should persist between scenes.
        
        private static readonly object ManagerLock = new object();

        private static GameManager _gmInstance;
        public static GameManager Instance { get
            {
                if (!IsQuitting)
                    lock (ManagerLock)
                    {
                        if (!(_gmInstance is null)) return _gmInstance;
                        GameManager[] gmList = FindObjectsOfType<GameManager>();
                        
                        switch (gmList.Length)
                        {
                            case 0: // No managers found. Creating and returning new GameObject + instance.
                                return _gmInstance = new GameObject("GameManager").AddComponent<GameManager>();
                            case 1: // 1 Manager found. Returning found manager.
                                return _gmInstance = gmList[0];
                            default:
                                Debug.LogWarning("{gmList.Length} GameManagers were found. Returning first instance. Destroying all other instances");
                                // Destroy all other GameManager objects except first
                                for (int i = 1; i < gmList.Length; i++) Destroy(gmList[i]);

                                // Return first GameManager object
                                return _gmInstance = gmList[0];
                        }
                    }
                Debug.LogWarning("Not returning GameManager instance as this application is quitting.");
                return null;
            }
        }

        private static bool IsQuitting { get; set; }
        private void OnApplicationQuit() { IsQuitting = true; }
        #endregion // End of 'Singleton Management'

        // Returns the first player found and sets the private representation of player to match.
        // If this system was to support multiplayer searches for players would have to be indexed
        private Player _player;
        
        private static Player Player => Instance._player ??= FindObjectsOfType<Player>()[0];
        
        public static CameraController CameraController => Player.PlayerCamera;
        
        public static UnitCommander UnitCommander => Player.Commander;

        [SerializeField] private UIManager playerUI;
        public static UIManager UI => Instance.SafeComponent<UIManager>(ref Instance.playerUI);
        
        private Building[] _buildings;
        public static Building[] Buildings => Instance._buildings ??= FindObjectsOfType<Building>();

        public static List<T> GetBuildings<T>() where T : Building
        {
            List<T> buildingList = new List<T>();
            foreach (Building building in Buildings) 
            {
                if (building is T buildingOfT) {
                    buildingList.Add(buildingOfT);
                }
            }
            return buildingList;
        }

        private void Awake()
        {
            // For singleton management
            if (persistent)DontDestroyOnLoad(gameObject);
        }
        
    }
}