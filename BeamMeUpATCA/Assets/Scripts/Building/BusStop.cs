using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{

    public class BusStop : Building, Enterable
    {
        [field: Header("Town Settings")]
        [SerializeField] [Tooltip("In seconds")] private float defaultTimeAway = 60f;
        
        private Dictionary<Unit, float> _inTown;

        protected override void Awake()
        {
            // Call base.Awake to ensure Building Layer is set
            base.Awake();
            _inTown = new Dictionary<Unit, float>();
        }

        /*
         * Adds a unit to the in town list
         * This _should not_ be called directly for bus stop - Call Work() instead
         */
        public void Enter(Unit unit) { if (!(IsInside(unit))) _inTown.Add(unit, defaultTimeAway); }

        /*
         * Return true is a specific unit is in town, else false
         */
        public bool IsInside(Unit unit) => _inTown.ContainsKey(unit);

        /*
        * Removes a unit to the in town list
        * This _should not_ be called directly for bus stop - Call Work() instead
        */
        public void Leave(Unit unit) { if (IsInside(unit)) _inTown.Remove(unit); }
        
        private void Update()
        {
            // Check each the inTownCounter for each unit in inTown
            foreach (Unit unit in _inTown.Keys)
            {
                if (!(_inTown[unit] <= 0))
                {
                    Leave(unit);
                } else
                {
                    // Unit isn't ready to leave so subtract time.
                    _inTown[unit] -= Time.deltaTime;
                }
            }
        }
    }
}
