using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{

    public class BusStop : MonoBehaviour, Workable
    {
        private List<Unit> inTown = new List<Unit>();

        // Update is called once per frame
        void Update()
        {
            // Check each the inTownCounter for each unit in inTown
            foreach (Unit u in inTown)
            {
                if (u.getInTownCounter() == 0)
                {
                    inTown.Remove(u);
                    // Show the unit
                    u.GetComponent<Renderer>().enabled = true;
                }
            }
        }
        
        /*
         * Adds a unit to the in town list
         */
        public void Work(Unit unit)
        {
            inTown.Add(unit);
            // Hide the unit
            unit.GetComponent<Renderer>().enabled = false;
        }
        
        
    }
}
