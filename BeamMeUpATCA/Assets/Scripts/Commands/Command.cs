using UnityEngine;

namespace BeamMeUpATCA
{
    [RequireComponent(typeof(Unit))]
    public abstract class Command : MonoBehaviour
    {
        public bool SkipQueue { set; get; } = false;
        public bool ResetQueue { set; get; } = false;
        public string Name { protected set; get; } = "None";

        protected Unit unit;

        // Public properties
        // Initializers ensure behaviour will fail safely
        public Camera ActiveCamera { set; protected get; }
        public Vector3 Position { set; protected get; } = Vector3.zero;
        public int Offset { set; protected get; } = 0;

        private void Awake()
        {
            // RequireComponent ensures this GetComponent should not return null.
            // As commands should not exist outside the context of a unit.
            unit = gameObject.GetComponent<Unit>();
            ActiveCamera = Camera.main;

            // Stops Update(), FixedUpdate(), & OnGUI() from being called.
            // ANY USES of MonoBehaviour outside of the above method should respect this.enabled
            // For example if you use OnCollision() it should guard with `if (!this.enabled) {return;}`
            enabled = false;

            // Prevents Commands showing up in inspector
            hideFlags = HideFlags.HideInInspector; 

            CommandAwake();
        }

        protected abstract void CommandAwake();

        public abstract void Execute();

        public abstract bool IsFinished();
    }
}
