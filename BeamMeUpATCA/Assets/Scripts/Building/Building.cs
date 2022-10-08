using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Building : MonoBehaviour, IInteractable
    {
        public Mask Mask => Mask.Building;

        [field: SerializeField]
        public BuildingAnchor Anchors { get; set; }
    }
}