using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using IASub = BeamMeUpATCA.InputActionSubscription;
using IASubscriber = BeamMeUpATCA.InputActionSubscriber;


namespace BeamMeUpATCA
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        #region Player Setup

        [SerializeField] private PlayerUI _playerUI;
        [field: SerializeField] private CameraController PlayerCamera { get; set; }
        [field: SerializeField] private UnitCommander Commander { get; set; }

        private PlayerInput _playerInput;
        private InputActionAsset _actions;

        private void Awake() 
        {
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _actions = _playerInput.actions;

            if (_playerInput.camera == null) {
                Debug.LogWarning("PlayerInput requires a camera to be set. Using MainCamera instead.");
                _playerInput.camera = Camera.main;
            }

            // Dependencies for PlayerCamera and Commander.
            PlayerCamera.ActiveCamera = _playerInput.camera;
            Commander.ActiveCamera = _playerInput.camera;

            // Set codependency
            Commander.PlayerUI = _playerUI;
            _playerUI.commander = Commander;

            DefineSubscriptions();
        }
        #endregion // Player Setup

        #region InputAction/Action Subscriptions

        private Vector2 PointerPosition { get 
        {
            if (_actions == null) { return Vector2.zero; }
            return _actions["Pointer"].ReadValue<Vector2>(); 
        }}

        private Dictionary<IASubscriber, IASub[]> ActionSubscriptions;

        private void DefineSubscriptions()
        {
            // Binds subscribers to subscriptions to allow actions to trigger any actions
            // https://gitlab.com/teamnamefinal/Beammeupatca/-/wikis/Unity/Guides/Creating-new-InputAction-event-handles
            ActionSubscriptions = new Dictionary<IASubscriber, IASub[]>() 
            {
                {new IASubscriber(_actions["Primary Action"]), 
                    new[] { new IASub(ctx => Commander.SelectUnit(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Secondary Action"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<GotoCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Tertiary Action"]), 
                    new[] { 
                        new IASub(ctx => PlayerCamera.DragRotation = true, (true, false, false)),
                        new IASub(ctx => PlayerCamera.DragRotation = false, (false, false, true))}
                },
                {new IASubscriber(_actions["Pan Camera"]), 
                    new[] { new IASub(ctx => PlayerCamera.Camera2DAdjust = ctx.ReadValue<Vector2>(), IASub.UPDATE)}
                },
                {new IASubscriber(_actions["Scroll Camera"]), 
                    new[] { new IASub(ctx => PlayerCamera.CameraZoomAdjust = ctx.ReadValue<float>(), IASub.UPDATE)}
                },
                {new IASubscriber(_actions["Focus Camera"]), 
                    new[] { new IASub(ctx =>  PlayerCamera.FocusCamera(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Quit"]), 
                    new[] { new IASub(ctx => Application.Quit(), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Stop"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<StopCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Enter"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<EnterCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Lock"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<LockCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Mend"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<MendCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Move"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<MoveCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Power"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<PowerCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Stow"]), 
                    new[] { new IASub(ctx => Commander.CommandUnits<StowCommand>(PointerPosition), IASub.PREFORMED)}
                },
                {new IASubscriber(_actions["Command: Work"]), 
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
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(true);}
        }

        private void OnDisable() 
        {
            foreach (IASubscriber subscriber in ActionSubscriptions.Keys) { subscriber.RegisterSubscriptions(false);}
        }

        #endregion // InputAction/Action Subscriptions
    }
}
