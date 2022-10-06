using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using IASub = BeamMeUpATCA.InputActionSubscription;
using IASubscriber = BeamMeUpATCA.InputActionSubscriber;


namespace BeamMeUpATCA
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        #region Player Initialization
  
        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput Input {
            get
            {
                if (_playerInput == null)
                {
                    Debug.LogWarning("Player requires a valid 'PlayerInput' inspector field."
                        + "Failing dependencies safely. This may lead to instability.", gameObject);
                    // Return an attached PlayerInput, otherwise attach one and return it.
                    _playerInput = TryGetComponent<PlayerInput>(out PlayerInput pi) 
                    ? pi : gameObject.AddComponent<PlayerInput>();
                }
                return _playerInput;
            }
        }

        // Dependencies assigned by active Scene - via the Inspector
        [SerializeField] private PlayerUI _playerUI;
        [field: SerializeField] public CameraController PlayerCamera { get; private set; }
        [field: SerializeField] private UnitCommander Commander { get; set; }

        private void Awake() 
        {
            PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();

            // Set camera references to the Player's CameraController camera. Accessing property
            // Is okay as it handles the null checking and handles any missing inspector elements.
            Commander.ActiveCamera = playerInput.camera = PlayerCamera.ActiveCamera;

            // Set codependency TODO: Refactor into single wrapper class.
            Commander.PlayerUI = _playerUI;
            _playerUI.commander = Commander;

            DefineSubscriptions();
        }

        #endregion // End of 'Player Initialization'

        #region Player InputAction Callbacks

        private Vector2 PointerPosition
        {
            get
            {
                if (Input.actions == null) { return Vector2.zero; }
                return Input.actions["Pointer"].ReadValue<Vector2>();
            }
        }

        private Dictionary<IASubscriber, IASub[]> ActionSubscriptions;

        private void DefineSubscriptions()
        {
            // Check if PlayerInput hasn't been defined yet and fail safely.
            if (Input.actions == null) 
            {
                Debug.LogWarning("PlayerInput.actions isn't defined"
                + "Failing dependencies safely. This will break input.", gameObject);
                return;
            }

            // Binds subscribers to subscriptions to allow actions to trigger any actions
            // https://gitlab.com/teamnamefinal/Beammeupatca/-/wikis/Unity/Guides/Creating-new-InputAction-event-handles
            ActionSubscriptions = new Dictionary<IASubscriber, IASub[]>()
            {
                {new IASubscriber(Input.actions["Primary Action"]),
                    new[] { new IASub(ctx => Commander.SelectUnit(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Secondary Action"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<GotoCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Tertiary Action"]),
                    new[] {
                        new IASub(ctx => PlayerCamera.DragRotation = true, (true, false, false)),
                        new IASub(ctx => PlayerCamera.DragRotation = false, (false, false, true))}
                },
                {new IASubscriber(Input.actions["Pan Camera"]),
                    new[] { new IASub(ctx => PlayerCamera.Camera2DAdjust = ctx.ReadValue<Vector2>(), IASub.UPDATE)}
                },
                {new IASubscriber(Input.actions["Scroll Camera"]),
                    new[] { new IASub(ctx => PlayerCamera.CameraZoomAdjust = ctx.ReadValue<float>(), IASub.UPDATE)}
                },
                {new IASubscriber(Input.actions["Focus Camera"]),
                    new[] { new IASub(ctx =>  PlayerCamera.FocusCamera(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Quit"]),
                    new[] { new IASub(ctx => Application.Quit(), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Stop"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<StopCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Enter"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<EnterCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Lock"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<LockCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Mend"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<MendCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Move"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<MoveCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Power"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<PowerCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Stow"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<StowCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(Input.actions["Command: Work"]),
                    new[] { new IASub(ctx => Commander.CommandUnits<WorkCommand>(PointerPosition), IASub.PREFORMED)}
                }
            };

            AddSubscriptions();
        }

        private void AddSubscriptions()
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys)
            {
                foreach (IASub sub in ActionSubscriptions[subscriber])
                {
                    subscriber.AddSubscription(sub);
                }
            }
        }

        private void OnEnable()
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(true); }
        }

        private void OnDisable()
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(false); }
        }

        #endregion // End of 'Player InputAction Callbacks'
    }
}
