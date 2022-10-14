using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class EngineerStation : Mendable, Enterable, Workable, Powerable
    {

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
        public void Work(Unit unit) { WorkingUnit ??= unit; }

        public Unit WorkingUnit { get; set; }
        public void Rest(Unit unit) { if (WorkingUnit == unit) WorkingUnit = null; }

        protected override void Update()
        {
            base.Update();

            if (!(WorkingUnit is null)) Engineer(Time.deltaTime);

            if (IsGeneratorOn)
            {
                // Drain power
                _backupPowerCounter -= Time.deltaTime;
                
                // If power is above 0 keep generator going
                if (_backupPowerCounter > 0) return;
                
                _backupPowerCounter = 0f;
                IsGeneratorOn = false;
            }
            else
            {
                // Restore power at rate to match BackupPowerRecoverTime
                _backupPowerCounter += Time.deltaTime * (BackupPowerMaxDuration / BackupPowerRecoverTime);
                // Clamp value so it doesn't go over MaxDuration.
                _backupPowerCounter = Mathf.Clamp(_backupPowerCounter, min:0, max:BackupPowerMaxDuration);
            }
        }
        
        private void Engineer(float time)
        {
            throw new System.NotImplementedException();
        }

        public void TogglePower() { IsGeneratorOn = !IsGeneratorOn; }
    }
}