using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Building : MonoBehaviour, IInteractable
    {
        public Mask Mask => Mask.Building;

        [field: SerializeField]
        public BuildingAnchor Anchors { get; set; }


        protected virtual void Awake() {
            // Sets current layer of this.gameObject to match the appropriate Mask.Unit.
            gameObject.layer = Mask.Layer(Mask);
        }
    }
}