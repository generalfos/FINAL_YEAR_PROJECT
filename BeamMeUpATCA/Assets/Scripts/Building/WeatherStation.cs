using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class WeatherStation : Mendable, Enterable, Workable
    {
        [field: Header("Weather Frequency Updates")]
        [SerializeField] [Tooltip("Updates/Second")] private float defaultFrequency = 1f;

        private List<Unit> _unitsInside;

        protected override void Awake()
        {
            base.Awake();
            _unitsInside = new List<Unit>();
        }

        // If unit is not already in Building add them to the list.
        public void Enter(Unit unit) {if (!(IsInside(unit))) _unitsInside.Add(unit); }

        // Check if unit list contains the unit
        public bool IsInside(Unit unit) => _unitsInside.Contains(unit);

        // If unit is inside the building remove them from the list.
        public void Leave(Unit unit) { if (IsInside(unit)) _unitsInside.Remove(unit); }

        // If unit is not working, and work slot is empty add them to the slot
        public void Work(Unit unit) { WorkingUnit ??= unit; }

        public Unit WorkingUnit { get; set; }
        public void Rest(Unit unit) { if (WorkingUnit == unit) WorkingUnit = null; }

        protected override void Update()
        {
            base.Update();

            if (!(WorkingUnit is null)) Weather(Time.deltaTime);
        }
        
        private void Weather(float time)
        {
            throw new System.NotImplementedException();
        }
    }
}