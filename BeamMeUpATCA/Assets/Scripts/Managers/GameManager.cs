using System.Collections.Generic;
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

        public static bool IsQuitting { get; private set; }
        private void OnApplicationQuit() { IsQuitting = true; }
        #endregion // End of 'Singleton Management'

        // Returns the first player found and sets the private representation of player to match.
        // If this system was to support multiplayer searches for players would have to be indexed
        private Player _player;
        public Player Player => _player ??= FindObjectsOfType<Player>()[0];
        // Shortcut to get GameManager's active player. Again if multiple player exist this would break.
        // Systems would have to get UI for the correct player object.
        public PlayerUI UI => Player.UI; 

        private Building[] _buildings;
        public Building[] Buildings => _buildings ??= FindObjectsOfType<Building>();

        public List<T> GetBuildings<T>() where T : Building
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