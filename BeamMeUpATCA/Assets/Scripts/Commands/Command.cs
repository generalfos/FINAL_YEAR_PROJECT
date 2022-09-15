using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Command : MonoBehaviour
    {
        public bool SkipQueue { protected set; get; }
        public bool ResetQueue { protected set; get; }
        public string Name { protected set; get; }

        private void Awake()
        {
            // Stops Update(), FixedUpdate(), & OnGUI() from being called.
            // ANY USES of MonoBehaviour outside of the above method should respect this.enabled
            // For example if you use OnCollision() it should guard with `if (!this.enabled) {return;}`
            enabled = false;

            this.hideFlags = HideFlags.HideInInspector; // Prevents Commands showing up in inspector
            DefineCommand();
        }

        protected virtual void DefineCommand() 
        {
            Name = "Command";
            SkipQueue = false;
            ResetQueue = false;
        }

        public abstract void Execute();

        public abstract bool IsFinished();
    }
}
