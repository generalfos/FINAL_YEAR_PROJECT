using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace BeamMeUpATCA
{
    public class JunctionBox : Mendable
    {
        [field: SerializeField] public Transform ArrangementSlot { get; private set; }
        [field: SerializeField] private JunctionBox North { get; set; }
        [field: SerializeField] private JunctionBox South { get; set; }
        [field: SerializeField] private JunctionBox East { get; set; }
        [field: SerializeField] private JunctionBox West { get; set; }

        [field: SerializeField] private bool IsNorthPaired { get; set; }
        [field: SerializeField] private bool IsSouthPaired { get; set; }
        [field: SerializeField] private bool IsEastPaired { get; set; }
        [field: SerializeField] private bool IsWestPaired { get; set; }
        

        protected override void Awake()
        {
            base.Awake();

            // If ArrangementSlot exists hide it's mesh and set it's layer to dishslot
            #if !UNITY_EDITOR
            if (ArrangementSlot)
            {
                ArrangementSlot.GetComponent<MeshRenderer>().enabled = false;
                ArrangementSlot.gameObject.layer = Mask.Layer(Mask.DishSlot);
            }
            #endif

            // If not transform is assigned use this.gameObject's transform
            ArrangementSlot = ArrangementSlot ? ArrangementSlot : transform;
        }
        
        private Dish _dockedDish = null;
        public void Dock(Dish dish)
        {
            // Set current docked dish to dish calling function
            _dockedDish = dish;
        }

        public void Undock() { _dockedDish = null; }

        private static readonly object Lock = new object();
        
        public Queue<JunctionBox> DockPath(JunctionBox dishBox)
        {
            lock (Lock)
            {
                Queue<JunctionBox> dockPath = Pathfind(this, dishBox, dishBox._dockedDish);;
                foreach (JunctionBox box in dockPath)
                {
                    // Dock on all values to lock junction boxes for other arrays moving.
                    box.Dock(dishBox._dockedDish);
                }

                return dockPath;
            }
        }
        
        // TODO: Reduce complexity/size of this method. It gets the job done, but not easy to evaluate
        private Queue<JunctionBox> Pathfind(JunctionBox startBox, JunctionBox endBox, Dish dish)
        {
            List<JunctionBox> exploredBoxes = new List<JunctionBox>();
            Queue<JunctionBoxNode> frontierBoxNodes = new Queue<JunctionBoxNode>();
            Queue<JunctionBox> result = new Queue<JunctionBox>();
            
            // If the provided box exists enqueue it to the frontier nodes
            if (startBox) frontierBoxNodes.Enqueue(new JunctionBoxNode(startBox, null));

            while (frontierBoxNodes.Count > 0)
            {
                JunctionBoxNode currentJunctionBoxNode = frontierBoxNodes.Dequeue();
                JunctionBox currentBox = currentJunctionBoxNode.Value;

                // Discard the box if it has another dish (not the parameter dish) or it's too close to another dish
                if (currentBox._dockedDish && currentBox._dockedDish != dish) continue;
                if (currentBox.CompassCheck(dish)) continue;

                // Exit if we've found the correct box, return list of nodes.
                if (currentBox == endBox)
                {
                    while (!(currentJunctionBoxNode is null))
                    {
                        result.Enqueue(currentJunctionBoxNode.Value);
                        currentJunctionBoxNode = currentJunctionBoxNode.Parent;
                    }
                }
                exploredBoxes.Add(currentBox);

                // Add directional nodes to frontierBoxNodes if they're valid
                JunctionBox[] directions = { currentBox.North, currentBox.South, currentBox.East, currentBox.West };
                foreach (JunctionBox dir in directions)
                {
                    if (dir && !exploredBoxes.Contains(dir))
                    {
                        frontierBoxNodes.Enqueue(new JunctionBoxNode(dir, currentJunctionBoxNode));
                    }
                }
            }
            return result;
        }
        
        // Checks if a dish exists in the current direction that isn't the supplied dish
        bool CompassCheck(Dish dish)
        {
            return (IsNorthPaired && North._dockedDish && North._dockedDish != dish)
                   || (IsSouthPaired && South._dockedDish && South._dockedDish != dish)
                   || (IsEastPaired && East._dockedDish && East._dockedDish != dish)
                   || (IsWestPaired && West._dockedDish && West._dockedDish != dish);
        }

        private class JunctionBoxNode
        {
            public JunctionBox Value { get; }
            public JunctionBoxNode Parent { get; }

            public JunctionBoxNode(JunctionBox value, JunctionBoxNode parent)
            {
                Value = value;
                Parent = parent;
            }
        }
    }
}