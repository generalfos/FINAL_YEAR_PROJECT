using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    /*
     * Describes the functionality required by buildings that lose health, and hence
     * need to be mended.
     * Set out as an abstract class as all Mendable buildings will have the same
     * implementation.
     */
    public class Mendable : Building
    {
        [Header("Health")] 
        [SerializeField] public float maxHealth = 100f;
        private float _health;
        private float Health
        {
            get => _health;
            set => _health = Mathf.Clamp(value, 0f, maxHealth);
        }
        [field: Header("Damage Rates")]
        [SerializeField] private float dmgMultiplier = 1f;
        [SerializeField] private float damageInterval = 3f;
        [SerializeField] private float repairInterval = 3f;
        
        [field: Header("Repair Rates")]
        [SerializeField] private float defaultRepairRate = 1f;
        [SerializeField] private float scientistRepairPercent = 0.75f;
        [SerializeField] private float engineerRepairPercent = 1f;
        
        public bool IsRepaired => (Health >= maxHealth);
        private bool IsBroken => (Health <= 0f);

        protected override void Awake()
        {
            base.Awake();
            Repairers = new List<Unit>();
            Health = maxHealth;
        }

        protected virtual void Update() 
        {
            // Are we damaging the building?
            if (!IsBroken && (Repairers.Count <= 0))
            {
                TakeDamage(Time.deltaTime);
                _repairTick = 0f;
            }
            else // We repairing the building
            {
                _damageTick = 0f;
                foreach (Unit unit in Repairers)
                {
                    if (Health < maxHealth) 
                    {
                        RegainHealth(unit, Time.deltaTime);
                    }
                    else
                    {
                        // Second iteration to remove all repairers
                        foreach (Unit unitSecond in Repairers)
                        {
                            RemoveMender(unitSecond);
                        }
                    }
                }

            }
            
        }

        private List<Unit> Repairers { get; set; }
        
        public virtual void Mend(Unit unit) { if (!(Repairers.Contains(unit))) Repairers.Add(unit); }

        public bool IsMender(Unit unit) => Repairers.Contains(unit);
        
        public void RemoveMender(Unit unit) { if (Repairers.Contains(unit)) Repairers.Remove(unit); }
        
        private float _repairTick = 0f;
        /*
        * If a unit has been assigned to repair a building, it gains HP
        * Cannot have more HP than maxHealth
        * Returns the health pool following a heal
        */
        private void RegainHealth(Unit unit, float time)
        {
            float unitRepairSpeed = defaultRepairRate;
                
            // Set Observation bonus depending on UnitClass of working unit
            unitRepairSpeed = unit.UnitClass switch
            {
                Unit.UnitType.Engineer => engineerRepairPercent * unitRepairSpeed,
                Unit.UnitType.Scientist => scientistRepairPercent * unitRepairSpeed,
                _ => unitRepairSpeed
            };
            _repairTick += Time.deltaTime * unitRepairSpeed;
            if (_repairTick < repairInterval) return;
            
            _repairTick = 0f;
            Health += repairInterval;
        }
        
        /*
         * Changes the damage multiplier
         * Typical case, dish takes extra damage during high wind / bushfire unless stowed
         * Returns the new dmgModifier
         */
        public float SetDmgMultiplier(float newModifier)
        {
            dmgMultiplier = newModifier;
            return dmgMultiplier;
        }

        private float _damageTick = 0f;
        /*
         * Reduces the health of a building
         * Can't reduce a buildings health to below zero.
         * Returns the health pool following a damage tick
         */
        private void TakeDamage(float time)
        {
            _damageTick += Time.deltaTime * dmgMultiplier;
            if (_damageTick < damageInterval) return;
            
            _damageTick = 0f;
            Health -= damageInterval;
        }
    }
}