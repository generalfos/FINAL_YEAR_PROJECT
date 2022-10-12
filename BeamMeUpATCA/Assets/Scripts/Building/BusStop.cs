using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{

    public class BusStop : Building, Enterable, Workable
    {
        private List<Unit> inTown = new List<Unit>();

        void Update()
        {
            // Check each the inTownCounter for each unit in inTown
            foreach (Unit u in inTown)
            {
                if (u.GetInTownCounter() == 0)
                {
                    Leave(u);
                    // Show the unit
                    //TODO Now that work() explicitly requires entering a building, this should be changed
                    u.GetComponent<Renderer>().enabled = true;
                }
            }
        }
        
        /*
         * Adds a unit to the in town list
         * This _should not_ be called directly for bus stop - Call Work() instead
         */
        public void Enter(Unit unit)
        {
            inTown.Add(unit);
            // TODO Hide unit and place an icon above the bus stop
        }

        /*
         * Return true is a specific unit is in town, else false
         */
        public bool IsInside(Unit unit)
        {
            if (inTown.Contains(unit))
            {
                return true;
            }
            return false;
        }

        /*
        * Removes a unit to the in town list
        * This _should not_ be called directly for bus stop - Call Work() instead
        */
        public void Leave(Unit unit)
        {
            inTown.Remove(unit);
        }
        
        
        // TODO: Upon further thinking as BusStop defines it's own Enter functionality we could just
        // TODO: Have BusStops only be enterable. Having Right-Clicks on buildings automatically call Enter
        /*
         * This is what gets called when we want to send a unit to town
         */
        public void Work(Unit unit)
        {
            Enter(unit);
            // Hide the unit
            //TODO Now that work() explicitly requires entering a building, this should be changed
            unit.GetComponent<Renderer>().enabled = false;
        }

        public Unit WorkingUnit { get; set; }
        public void Rest(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}
