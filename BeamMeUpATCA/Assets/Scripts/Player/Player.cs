using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using IASub = BeamMeUpATCA.InputActionSubscription;
using IASubscriber = BeamMeUpATCA.InputActionSubscriber;
using BeamMeUpATCA.Extensions;
using UnityEngine.EventSystems;


namespace BeamMeUpATCA
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        #region Player Initialization

        // Public Properties
        [SerializeField] private CameraController playerCamera;
        public CameraController PlayerCamera => this.SafeComponent<CameraController>(ref playerCamera);

        // Private Properties
        [SerializeField] private PlayerInput playerInput;
        private PlayerInput Input => this.SafeComponent<PlayerInput>(ref playerInput);

        [SerializeField] private UnitCommander commander;
        private UnitCommander Commander => this.SafeComponent<UnitCommander>(ref commander);

        private void Awake()
        {
            // Set camera references to the Player's CameraController camera. Accessing property
            // Is okay as it handles the null checking and handles any missing inspector elements.
            Commander.ActiveCamera = Input.camera = PlayerCamera.ActiveCamera;

            DefineSubscriptions();
        }

        #endregion // End of 'Player Initialization'

        #region Player InputAction Callbacks

        private Vector2 PointerPosition => Input.actions ? Input.actions["Pointer"].ReadValue<Vector2>() : Vector2.zero;
        
        private bool ModKey => Input.actions && Input.actions["ModKey"].IsPressed();

        private Dictionary<IASubscriber, IASub[]> _actionSubscriptions;

        private void DefineSubscriptions()
        {
            _actionSubscriptions = new Dictionary<IASubscriber, IASub[]>();
            
            // Check if PlayerInput exists. Fail safely if doesn't not.
            if (!Input.actions) 
            {
                Debug.LogWarning("PlayerInput.actions isn't defined"
                + "Failing dependencies safely. This will break input.", gameObject);
                return;
            }
            // Binds subscribers to subscriptions to allow actions to trigger any actions
            // https://gitlab.com/teamnamefinal/Beammeupatca/-/wikis/Unity/Guides/Creating-new-InputAction-event-handles
            
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Primary Action"]),
                new[] { new IASub(ctx =>
                { if (!IsOnUI) 
                    { Commander.SelectUnit(PointerPosition); }}, IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Secondary Action"]), 
                new[] { new IASub(ctx =>
                { if (!IsOnUI) 
                    { Commander.CommandUnits<GotoCommand>(ModKey, PointerPosition); }}, IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Tertiary Action"]), new[] {
                new IASub(ctx => PlayerCamera.DragRotation = true, (true, false, false)),
                new IASub(ctx => PlayerCamera.DragRotation = false, (false, false, true))});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Pan Camera"]), 
                new[] { new IASub(ctx => PlayerCamera.Camera2DAdjust = ctx.ReadValue<Vector2>(), IASub.UPDATE)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Scroll Camera"]), 
                new[] { new IASub(ctx => PlayerCamera.CameraZoomAdjust = ctx.ReadValue<float>(), IASub.UPDATE)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Focus Camera"]), 
                new[] { new IASub(ctx =>  PlayerCamera.FocusCamera(PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Quit"]), 
                new[] { new IASub(ctx => Application.Quit(), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Stop"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<StopCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Enter"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<EnterCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Lock"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<LockCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Mend"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<MendCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Move"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<MoveCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Power"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<PowerCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Stow"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<StowCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Work"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<WorkCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            _actionSubscriptions.Add(new IASubscriber(Input.actions["Command: Leave"]), 
                new[] { new IASub(ctx => Commander.CommandUnits<LeaveCommand>(ModKey, PointerPosition), IASub.PREFORMED)});
            AddSubscriptions();
        }

        private void AddSubscriptions()
        {
            foreach (IASubscriber subscriber in _actionSubscriptions.Keys)
            {
                foreach (IASub sub in _actionSubscriptions[subscriber])
                {
                    subscriber.AddSubscription(sub);
                }
            }
        }

        private void Update()
        {
            IsOnUI = EventSystem.current.IsPointerOverGameObject();
        }

        private static bool IsOnUI { get; set; }

        private void OnEnable()
        {
            foreach (IASubscriber subscriber in _actionSubscriptions.Keys) { subscriber.RegisterSubscriptions(true); }
        }

        private void OnDisable()
        {
            // Prevents NullReference leak on early exit (Game quit)
            if (!(_actionSubscriptions is null))
            {
                foreach (IASubscriber subscriber in _actionSubscriptions.Keys) { subscriber.RegisterSubscriptions(false); }
            }
        }
        #endregion // End of 'Player InputAction Callbacks'
    }
}
