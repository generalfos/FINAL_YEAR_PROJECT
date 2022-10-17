using UnityEngine;

namespace BeamMeUpATCA
{
    public sealed class ArrangementSlot : MonoBehaviour, IInteractable
    {
        public Mask Mask => Mask.DishSlot;

        private void Awake() {
            // Sets current layer of this.gameObject to match the appropriate Mask.Unit.
            gameObject.layer = Mask.Layer(Mask);
        }
    }
}