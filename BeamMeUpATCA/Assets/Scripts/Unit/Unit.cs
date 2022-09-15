using UnityEngine;
using System;
using System.Collections.Generic;


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
            UnitMorale = maxMorale;
            moraleTickDmg = 1;
            tickCounter = 0;

            commandQueue = new Queue<Command>();
        }

        #endregion // Unit Properties

        #region Commanding

        private Queue<Command> commandQueue;
        private Command activeCommand;
        private Command priorityCommand;
        private Command nextCommand;
        
        public void AddCommand(Command command)
        {

            if (command.ResetQueue) 
            {
                // Destroy the entire command queue, next command and active command
                DestroyCommandQueue();
                DestroyStagedCommands();
            }

            if (command.SkipQueue) 
            {
                if (activeCommand != null) 
                {
                    activeCommand.enabled = false;
                }

                // Priority command holds the command queue hostage stopping ExecuteCommand()
                // from being called until the priority command has finished executing.
                priorityCommand = command;
                priorityCommand.enabled = true;
                priorityCommand.Execute();
            } 
            else 
            {
                commandQueue.Enqueue(command);
                if (nextCommand == null) 
                {
                    nextCommand = commandQueue.Dequeue();
                }
            }


        }

        private Command ExecuteCommand(Command command) 
        {
            // Guard Clause for determining if there is no next command.
            if (command == null) { return null; }

            // Indicate to the command it can be begin executing.
            command.enabled = true;
            command.Execute();

            // If there is a next command then return it. Otherwise return null.
            try { return commandQueue.Dequeue(); }
            catch (InvalidOperationException) { return null; }
        }

        // Destroys a command instance.
        private void DestroyCommand(Command command) 
        {
            // Guard Clause ensuring command is valid.
            if (command == null ) { return; }

            Debug.Log("Destroying Command: " + command.Name);

            Destroy(command);
        }

        // Clears Command Queue, NextCommand, and ActiveCommand and ensures Command instances are destroyed.
        private void DestroyCommandQueue() 
        {
            while (commandQueue.Count > 0)
            {
                DestroyCommand(commandQueue.Dequeue());
            }
        }

        private void DestroyStagedCommands() 
        {
            DestroyCommand(nextCommand);
            nextCommand = null;
            DestroyCommand(activeCommand);
            activeCommand = null;
        }

        private void CommandUpdate() 
        {
            // No commands should be running while a priority command exists.
            if (priorityCommand != null) 
            {
                // Guard Clause to allow priority command to run enabled.
                if (!priorityCommand.IsFinished()) { return; }

                DestroyCommand(priorityCommand);
                priorityCommand = null;

                // Restore whatever activeCommand was running.
                if (activeCommand != null) 
                {
                    activeCommand.enabled = true;
                }
            } 
            else 
            {
                // If the active command is null run the next command.
                if (activeCommand == null && nextCommand != null)
                {
                    activeCommand = nextCommand;
                    // ExecuteCommand() returns the next command in queue
                    nextCommand = ExecuteCommand(activeCommand);
                }
                // Evaluation left to right validates Command.IsFinished() check.
                if (activeCommand != null && activeCommand.IsFinished()) 
                {
                    // If Command.IsFinished() delete object and set activeCommand to null.
                    DestroyCommand(activeCommand);
                    activeCommand = null;
                }

            }

            
        }

        private void Update() 
        {
            CommandUpdate();
        }
        #endregion // Commanding

        #region TickUpdates

        private void FixedUpdate() {
            takeTickDamage();
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

        #endregion //TickUpdates
    }
}