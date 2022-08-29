using UnityEngine;
using System;
using System.Collections.Generic;


namespace BeamMeUpATCA 
{
    public class Unit : MonoBehaviour
    {
        #region Unit Properties
        public enum UnitClass
        {
            Engineer,
            Scientist
        };

        [field: SerializeField]
        public string Name;

        [field: SerializeField]
        public UnitClass Class;

        #endregion

        #region Commanding
        private Queue<Command> commandQueue;
        private Command nextCommand;
        
        private void Awake() 
        {
            commandQueue = new Queue<Command>();
        }

        public void AddCommand(Command command, bool skipQueue= false, bool resetQueue= false) 
        {
            // Guard Clause for determining if the command queue should be reset.
            if (resetQueue) { commandQueue.Clear(); }

            commandQueue.Enqueue(command);

            // Sets next command to front of queue or the command parameter if skipQueue is true.
            nextCommand = skipQueue ? command : commandQueue.Dequeue();
        }

        public void ExecuteNextCommand() 
        {
            // Guard Clause for determining if there is no next command.
            if (nextCommand == null) { return; }
            
            nextCommand.Execute();

            try 
            {
                nextCommand = commandQueue.Dequeue();
            }
            catch (InvalidOperationException) 
            {
                nextCommand = null;
            }
        }

        private void Update() 
        {
            // HACK: Commands should call ExecuteNextCommand() as an event instead
            // Checking each Update() call. Also double checking for null values here :/
            if (nextCommand != null) 
            {
                if (nextCommand.IsFinished) 
                {
                    ExecuteNextCommand();
                }   
            }
        }
        #endregion
    }
}