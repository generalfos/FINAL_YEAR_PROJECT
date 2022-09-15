using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Command : MonoBehaviour
    {
        public bool SkipQueue { private set; get; }
        public bool ResetQueue { private set; get; }
        public string Name { protected set; get; }

        protected Command(bool skipQueue = false, bool resetQueue = false) 
        {
            SkipQueue = skipQueue;
            ResetQueue = resetQueue;
            Name = "Command";
        }

        private void Awake()
        {
            // Stops Update(), FixedUpdate(), & OnGUI() from being called.
            // ANY USES of MonoBehaviour outside of the above method should respect this.enabled
            // For example if you use OnCollision() it should guard with `if (!this.enabled) {return;}`
            enabled = false;
        }

        public abstract void Execute();

        public abstract bool IsFinished();
    }
}
