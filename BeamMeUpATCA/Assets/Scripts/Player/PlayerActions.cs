//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Scripts/Player/PlayerActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace BeamMeUpATCA
{
    public partial class @PlayerActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""ec40c2f7-5c62-4a07-87ee-047e54d37703"",
            ""actions"": [
                {
                    ""name"": ""Pointer"",
                    ""type"": ""Value"",
                    ""id"": ""86893b24-9f48-41d3-8e93-b8248519ba92"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Pan Camera"",
                    ""type"": ""Value"",
                    ""id"": ""bb78b506-5ec9-4317-8526-2eaeda4b9380"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Command: Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""49bd28d1-0852-4b9e-91ba-1435c4970561"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Command: Move"",
                    ""type"": ""Button"",
                    ""id"": ""0a33ba2d-c6ad-4a6e-ae17-04fb4cd37fc3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""6c1defbc-86af-4db2-83de-ee85bcda12da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Primary Action"",
                    ""type"": ""Button"",
                    ""id"": ""ca447b67-fac8-4b42-bb22-ae43ed5c6979"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Secondary Action"",
                    ""type"": ""Button"",
                    ""id"": ""e3718e7f-3943-4519-b694-a19bb53dbd6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tertiary Action"",
                    ""type"": ""Button"",
                    ""id"": ""66482325-3237-40ea-8682-410ab3b40f6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll Camera"",
                    ""type"": ""Button"",
                    ""id"": ""a82e8ecb-40bc-46a1-b21a-8478bbfe070b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate Camera"",
                    ""type"": ""Button"",
                    ""id"": ""690f3e88-9edd-4d8d-b729-75dda3de3338"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ea250e9a-1584-47b8-a0fa-c4b7293812c2"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35abdebe-12d3-4859-8b2d-499490893d19"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Command: Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""9797eb56-8eb5-400a-8ef6-9717c2d821d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b51e9ccf-426e-4f76-93a4-59ceb872d709"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""886bf388-0cca-491b-bc41-14b3f3690999"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""332c3a6d-9d4e-489f-9b99-57476574d733"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""da52facf-8dc1-4fb3-93e4-a64a45c7d273"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""7b878d34-dfd6-48c9-b644-b6f21504f077"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e636a119-619b-4537-85bf-331399d046c3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0e41d6ba-187b-4e19-baa5-5b29a967fa06"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""afede523-528e-4869-b2e9-deaa3930aad1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ddee7220-5f64-4360-92c4-3e471c94c63f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""48e6bef7-1258-4564-b082-30c913a8245a"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c68f632-763f-45bd-a57c-faf9465a77a9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4c304a6-1f79-414d-96fd-acd2d0cf08a8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Secondary Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9637febd-848f-443d-8ef9-d7ae196efed4"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Command: Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95ce353f-b6c1-4d39-8bba-164c2ac1978c"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tertiary Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""bac367a3-b6ce-4e32-bbcb-f57b11f911fc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b297897d-864c-4848-a1a2-3fc5e2aa18cb"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""953aad81-1117-4bc9-a76d-d3ec3ec1931f"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""36347e74-9612-46df-8ded-50f5be405708"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Default
            m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
            m_Default_Pointer = m_Default.FindAction("Pointer", throwIfNotFound: true);
            m_Default_PanCamera = m_Default.FindAction("Pan Camera", throwIfNotFound: true);
            m_Default_CommandCancel = m_Default.FindAction("Command: Cancel", throwIfNotFound: true);
            m_Default_CommandMove = m_Default.FindAction("Command: Move", throwIfNotFound: true);
            m_Default_Quit = m_Default.FindAction("Quit", throwIfNotFound: true);
            m_Default_PrimaryAction = m_Default.FindAction("Primary Action", throwIfNotFound: true);
            m_Default_SecondaryAction = m_Default.FindAction("Secondary Action", throwIfNotFound: true);
            m_Default_TertiaryAction = m_Default.FindAction("Tertiary Action", throwIfNotFound: true);
            m_Default_ScrollCamera = m_Default.FindAction("Scroll Camera", throwIfNotFound: true);
            m_Default_RotateCamera = m_Default.FindAction("Rotate Camera", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Default
        private readonly InputActionMap m_Default;
        private IDefaultActions m_DefaultActionsCallbackInterface;
        private readonly InputAction m_Default_Pointer;
        private readonly InputAction m_Default_PanCamera;
        private readonly InputAction m_Default_CommandCancel;
        private readonly InputAction m_Default_CommandMove;
        private readonly InputAction m_Default_Quit;
        private readonly InputAction m_Default_PrimaryAction;
        private readonly InputAction m_Default_SecondaryAction;
        private readonly InputAction m_Default_TertiaryAction;
        private readonly InputAction m_Default_ScrollCamera;
        private readonly InputAction m_Default_RotateCamera;
        public struct DefaultActions
        {
            private @PlayerActions m_Wrapper;
            public DefaultActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Pointer => m_Wrapper.m_Default_Pointer;
            public InputAction @PanCamera => m_Wrapper.m_Default_PanCamera;
            public InputAction @CommandCancel => m_Wrapper.m_Default_CommandCancel;
            public InputAction @CommandMove => m_Wrapper.m_Default_CommandMove;
            public InputAction @Quit => m_Wrapper.m_Default_Quit;
            public InputAction @PrimaryAction => m_Wrapper.m_Default_PrimaryAction;
            public InputAction @SecondaryAction => m_Wrapper.m_Default_SecondaryAction;
            public InputAction @TertiaryAction => m_Wrapper.m_Default_TertiaryAction;
            public InputAction @ScrollCamera => m_Wrapper.m_Default_ScrollCamera;
            public InputAction @RotateCamera => m_Wrapper.m_Default_RotateCamera;
            public InputActionMap Get() { return m_Wrapper.m_Default; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
            public void SetCallbacks(IDefaultActions instance)
            {
                if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
                {
                    @Pointer.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPointer;
                    @Pointer.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPointer;
                    @Pointer.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPointer;
                    @PanCamera.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPanCamera;
                    @PanCamera.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPanCamera;
                    @PanCamera.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPanCamera;
                    @CommandCancel.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandCancel;
                    @CommandCancel.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandCancel;
                    @CommandCancel.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandCancel;
                    @CommandMove.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandMove;
                    @CommandMove.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandMove;
                    @CommandMove.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCommandMove;
                    @Quit.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnQuit;
                    @Quit.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnQuit;
                    @Quit.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnQuit;
                    @PrimaryAction.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPrimaryAction;
                    @PrimaryAction.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPrimaryAction;
                    @PrimaryAction.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPrimaryAction;
                    @SecondaryAction.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSecondaryAction;
                    @SecondaryAction.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSecondaryAction;
                    @SecondaryAction.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSecondaryAction;
                    @TertiaryAction.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTertiaryAction;
                    @TertiaryAction.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTertiaryAction;
                    @TertiaryAction.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTertiaryAction;
                    @ScrollCamera.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScrollCamera;
                    @ScrollCamera.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScrollCamera;
                    @ScrollCamera.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScrollCamera;
                    @RotateCamera.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRotateCamera;
                    @RotateCamera.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRotateCamera;
                    @RotateCamera.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRotateCamera;
                }
                m_Wrapper.m_DefaultActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Pointer.started += instance.OnPointer;
                    @Pointer.performed += instance.OnPointer;
                    @Pointer.canceled += instance.OnPointer;
                    @PanCamera.started += instance.OnPanCamera;
                    @PanCamera.performed += instance.OnPanCamera;
                    @PanCamera.canceled += instance.OnPanCamera;
                    @CommandCancel.started += instance.OnCommandCancel;
                    @CommandCancel.performed += instance.OnCommandCancel;
                    @CommandCancel.canceled += instance.OnCommandCancel;
                    @CommandMove.started += instance.OnCommandMove;
                    @CommandMove.performed += instance.OnCommandMove;
                    @CommandMove.canceled += instance.OnCommandMove;
                    @Quit.started += instance.OnQuit;
                    @Quit.performed += instance.OnQuit;
                    @Quit.canceled += instance.OnQuit;
                    @PrimaryAction.started += instance.OnPrimaryAction;
                    @PrimaryAction.performed += instance.OnPrimaryAction;
                    @PrimaryAction.canceled += instance.OnPrimaryAction;
                    @SecondaryAction.started += instance.OnSecondaryAction;
                    @SecondaryAction.performed += instance.OnSecondaryAction;
                    @SecondaryAction.canceled += instance.OnSecondaryAction;
                    @TertiaryAction.started += instance.OnTertiaryAction;
                    @TertiaryAction.performed += instance.OnTertiaryAction;
                    @TertiaryAction.canceled += instance.OnTertiaryAction;
                    @ScrollCamera.started += instance.OnScrollCamera;
                    @ScrollCamera.performed += instance.OnScrollCamera;
                    @ScrollCamera.canceled += instance.OnScrollCamera;
                    @RotateCamera.started += instance.OnRotateCamera;
                    @RotateCamera.performed += instance.OnRotateCamera;
                    @RotateCamera.canceled += instance.OnRotateCamera;
                }
            }
        }
        public DefaultActions @Default => new DefaultActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IDefaultActions
        {
            void OnPointer(InputAction.CallbackContext context);
            void OnPanCamera(InputAction.CallbackContext context);
            void OnCommandCancel(InputAction.CallbackContext context);
            void OnCommandMove(InputAction.CallbackContext context);
            void OnQuit(InputAction.CallbackContext context);
            void OnPrimaryAction(InputAction.CallbackContext context);
            void OnSecondaryAction(InputAction.CallbackContext context);
            void OnTertiaryAction(InputAction.CallbackContext context);
            void OnScrollCamera(InputAction.CallbackContext context);
            void OnRotateCamera(InputAction.CallbackContext context);
        }
    }
}
