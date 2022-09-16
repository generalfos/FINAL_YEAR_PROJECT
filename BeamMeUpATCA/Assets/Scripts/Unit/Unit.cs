using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;


namespace BeamMeUpATCA 
{
    public class Unit : MonoBehaviour
    {
        #region Unit Properties

        public enum UnitType
        {
            Engineer,
            Scientist,
            Array
        };

        [field: SerializeField] public string Name {get; private set;}
        [field: SerializeField] public UnitType UnitClass {get; private set;}
        [field: SerializeField] public int UnitHealth { get; private set; }

        [field: SerializeField] public float UnitMorale {get; private set; }
        [field: SerializeField] private float inTownCounter;


        public Color UnitColor {get; private set;}
        
        private float tickCounter;
        private float moraleTickDmg;

        private float maxMorale;

        private void Awake() {
            switch (UnitClass) 
            { 
                case Unit.UnitType.Engineer:
                    UnitColor = Color.red;
                    break;

                case Unit.UnitType.Scientist:
                    UnitColor = Color.blue;
                    break;
                case Unit.UnitType.Array:
                    UnitColor = Color.green;
                    break;
            }

            maxMorale = 100;
            inTownCounter = 0;
            UnitMorale = maxMorale;
            moraleTickDmg = 1;
            tickCounter = 0;

            commandQueue = new Queue<Command>();
        }

        #endregion // Unit Properties

        #region Commanding

        private Queue<Command> commandQueue;
        private Command nextCommand;
        
        public void AddCommand(Command command)
        {
            // Guard Clause for determining if the command queue should be reset.
            if (command.ResetQueue) { commandQueue.Clear(); }

            commandQueue.Enqueue(command);

            // Sets next command to front of queue or the command parameter if skipQueue is true.
            nextCommand = command.SkipQueue ? command : commandQueue.Dequeue();
        }

        public Command ExecuteCommand(Command command) 
        {
            // Guard Clause for determining if there is no next command.
            if (command == null) { return null; }

            // Pass self
            command.Execute(this);

            // If there is a next command then return it. Otherwise return null.
            try { return commandQueue.Dequeue(); }
            catch (InvalidOperationException) { return null; }
        }

        private void Update() 
        {
            // ExecuteCommand() returns the next command in the queue.
            nextCommand = ExecuteCommand(nextCommand);
        }
        #endregion // Commanding

        #region TickUpdates

        private void FixedUpdate() {
            if (inTownCounter == 0)  // Not in town
            {
                takeTickDamage();
            }
            else  // In town
            {
                decrementTownCounter();
                UnitMorale = maxMorale;
            }
        }

        private void takeTickDamage() {
            tickCounter += Time.fixedDeltaTime;
            if(tickCounter >= 3) {
                float newMorale = UnitMorale - moraleTickDmg;
                if(newMorale <= 0) {
                    newMorale = 0;
                }
                if(newMorale >= maxMorale) {
                    newMorale = maxMorale; //do not heal over full
                }
                UnitMorale = newMorale;
                tickCounter = 0;
            }
        }
    
        /*
         * If a unit is in town, countdown the inTownCounter
         */
        private void decrementTownCounter()
        {
            tickCounter += Time.fixedDeltaTime;
            if (tickCounter >= 3)
            {
                inTownCounter--;
                tickCounter = 0;
            }
        }
        
        /*
         * Sets the inTownCounter to send a unit to town
         */
        
        private void goToTown()
        {
            inTownCounter = 20;
        }

        #endregion //TickUpdates
        
    public float getInTownCounter()
    {
        return inTownCounter;
    }
    

    }
}