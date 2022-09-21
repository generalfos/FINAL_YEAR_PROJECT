using UnityEngine;

namespace BeamMeUpATCA
{
    public abstract class Building : MonoBehaviour
    {
        [field: SerializeField]
        public BuildingAnchor Anchors { get; set; }
    }
}