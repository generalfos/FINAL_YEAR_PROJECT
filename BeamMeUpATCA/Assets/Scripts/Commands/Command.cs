using BeamMeUpATCA.Extensions;
using UnityEngine;

namespace BeamMeUpATCA
{
    [RequireComponent(typeof(Unit))]
    public abstract class Command : MonoBehaviour
    {
        public virtual bool SkipQueue { set; get; } = false;
        public virtual bool ResetQueue { set; get; } = false;
        protected Unit Unit => this.SafeComponent<Unit>(ref unit);
        
        // Command.Position, and Command.Offset should be set prior to execution
        public (Ray, float) RayData { set; protected get; }
        public int Offset { set; protected get; } = 0;
        
        public Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            
            // Stops Update(), FixedUpdate(), & OnGUI() from being called.
            // ANY USES of MonoBehaviour outside of the above method should respect this.enabled
            // For example if you use OnCollision() it should guard with `if (!this.enabled) {return;}`
            enabled = false;

            // Prevents Commands showing up in inspector
            hideFlags = HideFlags.HideInInspector;
        }

        public abstract void Execute();

        public abstract bool IsFinished();
    }
}
