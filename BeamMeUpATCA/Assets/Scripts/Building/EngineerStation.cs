using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class EngineerStation : Mendable, Enterable, Workable, Powerable
    {
        [field: Header("Engineer Global Damage Reductions")]
        [SerializeField] private float defaultReductionPercent = 0.25f;
        [SerializeField] private float scientistReductionPercent = 0.50f;
        [SerializeField] private float engineerReductionPercent = 0.75f;
        
        private float _reductionPercent;
        public float ReductionPercent
        {
            // If building is not powered return 0f (No observation)
            get => IsPowered ? _reductionPercent : 0f;
            private set => _reductionPercent = value;
        }

        [field: Header("Backup Power")]
        [field: SerializeField] private float BackupPowerMaxDuration { get; set; } = 60f;
        [field: SerializeField] private float BackupPowerRecoverTime { get; set; } = 60f;
        private float _backupPowerCounter = 60f;
        
        public bool IsGeneratorOn { get; private set; } = false;

        private List<Unit> _unitsInside;

        protected override void Awake()
        {
            base.Awake();
            _unitsInside = new List<Unit>();

            // Set the _backupPowerCounter to the max duration (full at start)
            _backupPowerCounter = BackupPowerMaxDuration;
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
            _reductionPercent = WorkingUnit.UnitClass switch
            {
                Unit.UnitType.Engineer => engineerReductionPercent,
                Unit.UnitType.Scientist => scientistReductionPercent,
                _ => _reductionPercent
            };
        }

        public Unit WorkingUnit { get; set; }

        public void Rest(Unit unit)
        {
             if (WorkingUnit == unit) WorkingUnit = null;
             // If unit leaves building set observation bonus to default bonus
             if (WorkingUnit is null) _reductionPercent = defaultReductionPercent;
        }

        protected override void Update()
        {
            base.Update();

            // Run/Restore Generator
            Generator(Time.deltaTime);
        }

        public void TogglePower() { IsGeneratorOn = !IsGeneratorOn; }

        private void Generator(float time)
        {
            if (IsGeneratorOn)
            {
                // Drain power
                _backupPowerCounter -= time;
                
                // If power is above 0 keep generator going
                IsGeneratorOn = _backupPowerCounter > 0;
            }
            else
            {
                // Restore power at rate to match BackupPowerRecoverTime
                _backupPowerCounter += time * (BackupPowerMaxDuration / BackupPowerRecoverTime);
            }
            // Clamp power between 0 and BackupPowerMaxDuration
            _backupPowerCounter = Mathf.Clamp(_backupPowerCounter, min: 0, max: BackupPowerMaxDuration);
        }
    }
}