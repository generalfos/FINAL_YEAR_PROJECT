using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class ObservationStation : Mendable, Enterable, Workable
    {

        [field: Header("Observation Multiplier Bonuses")]
        [SerializeField] private float defaultBonus = 1f;
        [SerializeField] private float engineerBonus;
        [SerializeField] private float scientistBonus;
        public float ObservationBonus { get; private set; }

        private List<Unit> _unitsInside;

        protected override void Awake()
        {
            base.Awake();
            _unitsInside = new List<Unit>();
            ObservationBonus = defaultBonus;
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
            ObservationBonus = WorkingUnit.UnitClass switch
            {
                Unit.UnitType.Engineer => engineerBonus,
                Unit.UnitType.Scientist => scientistBonus,
                _ => ObservationBonus
            };
        }

        public Unit WorkingUnit { get; set; }

        public void Rest(Unit unit)
        {
            if (WorkingUnit == unit) WorkingUnit = null;

            // If unit leaves building set observation bonus to default bonus
            if (WorkingUnit is null) ObservationBonus = defaultBonus;
        }
    }
}