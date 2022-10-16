using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class JunctionBox : Mendable
    {
        [field: SerializeField]public Transform ArrangementSlot { get; private set; }
        
        [field: SerializeField] private JunctionBox North { get; set; }
        [field: SerializeField] private JunctionBox South { get; set; }
        [field: SerializeField] private JunctionBox East { get; set; }
        [field: SerializeField] private JunctionBox West { get; set; }

        [field: SerializeField] public bool IsNorthPaired { get; set; }
        [field: SerializeField] public bool IsSouthPaired { get; set; }
        [field: SerializeField] public bool IsEastPaired { get; set; }
        [field: SerializeField] public bool IsWestPaired { get; set; }

        public List<JunctionBox> Options { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            // If ArrangementSlot exists hide it's mesh.
            if (!(ArrangementSlot is null))
            {
                ArrangementSlot.GetComponent<MeshRenderer>().enabled = false;
            }
            
            // If not transform is assigned use this.gameObject's transform
            ArrangementSlot = ArrangementSlot ? ArrangementSlot : transform;
            Options = new List<JunctionBox>() {North, South, East, West};
        }
    }
}