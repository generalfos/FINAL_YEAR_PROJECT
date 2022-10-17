using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Building : MonoBehaviour, IInteractable
    {

        public Mask Mask => Mask.Building;

        [field: SerializeField]
        public BuildingAnchor Anchors { get; set; }

        private bool _isPowered;
        public bool IsPowered
        {
            get => _isPowered;
            set
            {
                // TODO: Set powered status in UI
                _isPowered = value;
            }
        }

        protected virtual void Awake() {
            // Sets current layer of this.gameObject to match the appropriate Mask.Unit.
            gameObject.layer = Mask.Layer(Mask);
        }
    }
}