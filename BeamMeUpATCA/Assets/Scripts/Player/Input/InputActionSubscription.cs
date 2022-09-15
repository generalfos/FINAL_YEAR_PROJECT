using System;
using UnityEngine.InputSystem;

namespace BeamMeUpATCA
{
    public readonly struct InputActionSubscription
    {
        public readonly static (bool, bool, bool) HOLD = (true, false, true);
        public readonly static (bool, bool, bool) PREFORMED = (false, true, false);
        public readonly static (bool, bool, bool) UPDATE = (true, true, true);

        public readonly Action<InputAction.CallbackContext> CallbackAction;
        public readonly bool Started { get; }
        public readonly bool Preformed { get; }
        public readonly bool Canceled { get; }

        public InputActionSubscription(Action<InputAction.CallbackContext> action, (bool, bool, bool) actionCallback)
        {
            CallbackAction = action;
            Started = actionCallback.Item1;
            Preformed = actionCallback.Item2;
            Canceled = actionCallback.Item3;
        }
    }
}