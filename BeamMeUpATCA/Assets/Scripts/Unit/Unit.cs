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
        #endregion // Unit Properties

        #region Commanding

        private Queue<Command> commandQueue;
        private Command nextCommand;
        
        private void Awake() 
        {
            commandQueue = new Queue<Command>();
        }

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

            command.Execute();

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
    }
}