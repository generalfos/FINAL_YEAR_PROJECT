using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace BeamMeUpATCA 
{
    public class InputActionSubscriber
    {
        private List<InputActionSubscription> Subscriptions;
        public InputAction IAction {get; private set; }

        public InputActionSubscriber(InputAction inputAction) 
        {
            IAction = inputAction;
            Subscriptions = new List<InputActionSubscription>();
        }

        public void AddSubscription(InputActionSubscription subscription) 
        {
            Subscriptions.Add(subscription);
        }

        public void RegisterSubscriptions(bool subscribe) 
        {
            foreach (InputActionSubscription subscription in Subscriptions) { Subscribe(subscription, subscribe); }
        }

        private void Subscribe(InputActionSubscription subscription, bool subscribe) 
        {
            if (subscribe) 
            {
                if (subscription.Started) { IAction.started += ctx => subscription.CallbackAction(ctx); }
                if (subscription.Preformed) { IAction.performed += ctx => subscription.CallbackAction(ctx); }
                if (subscription.Canceled) { IAction.canceled += ctx => subscription.CallbackAction(ctx); }
            }
            else 
            {
                if (subscription.Started) { IAction.started -= ctx => subscription.CallbackAction(ctx); }
                if (subscription.Preformed) { IAction.performed -= ctx => subscription.CallbackAction(ctx); }
                if (subscription.Canceled) { IAction.canceled -= ctx => subscription.CallbackAction(ctx); }
            }
        }
    }
}