using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class WeatherStation : Mendable, Enterable, Workable
    {
        [field: Header("Weather Frequency Updates")]
        [SerializeField] private float defaultFrequency = (1/60f); // updates/second
        [SerializeField] private float scientistFrequency = (1/5f); // updates/second
        [SerializeField] private float engineerFrequency = (1/15f); // updates/second

        private float _weatherFrequency;
        public float WeatherFrequency
        {
            // If building is not powered return 0 updates/second for no weather checking
            get => IsPowered ? _weatherFrequency : 0; // Ensure 0 is checked to avoid division by zero error
            private set => _weatherFrequency = value;
        }

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
        public void Work(Unit unit)
        {
            if (!(WorkingUnit is null)) return;
            WorkingUnit = unit;
            
            // Set Observation bonus depending on UnitClass of working unit
            _weatherFrequency = WorkingUnit.UnitClass switch
            {
                Unit.UnitType.Engineer => engineerFrequency,
                Unit.UnitType.Scientist => scientistFrequency,
                _ => _weatherFrequency
            };
        }

        public Unit WorkingUnit { get; set; }
        public void Rest(Unit unit)
        {
            if (WorkingUnit == unit) WorkingUnit = null;

            // If unit leaves building set observation bonus to default bonus
            if (WorkingUnit is null) _weatherFrequency = defaultFrequency;
        }
    }
}