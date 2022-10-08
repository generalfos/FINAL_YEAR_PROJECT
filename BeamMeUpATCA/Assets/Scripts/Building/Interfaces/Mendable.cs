using System;
using UnityEngine;

namespace BeamMeUpATCA
{
    /*
     * Describes the functionality required by buildings that lose health, and hence
     * need to be mended.
     * Set out as an abstract class as all Mendable buildings will have the same
     * implementation.
     */
    public abstract class Mendable : Building
    {
        private float _maxHealth;
        private float _tickDmg;
        private float _dmgMultiplier;
        private float _tickRepair;
        private float _repMultiplier;
        private bool _isBroken;
        private float _tickCounter;
        private Unit _repairer;
        
        [field: SerializeField] private float healthPool;
        [field: SerializeField] public bool IsRepaired { get; private set; }

        protected override void Awake()
        {
            // Call base.Awake to ensure Building Layer is set
            base.Awake();
            _maxHealth = 100f;
            healthPool = 100f;
            _tickDmg = 1f;
            _dmgMultiplier = 1f;
            _tickRepair = 5f;
            _repMultiplier = 1f;
            _isBroken = false;
            _tickCounter = 0;
            _repairer = null;
            IsRepaired = true;
        }
        
        private void Update() 
        {
            // Are we damaging the building?
            if ((!_isBroken || _tickDmg < 0) && _repairer is null)
            {
                TakeTickDamage();
                IsRepaired = false;
            }
            
            // Are we repairing the building
            if (!(_repairer is null))
            {
                if (Math.Abs(_maxHealth - healthPool) > float.Epsilon) 
                {
                    RegainHealth();
                }
                // Building is full health!
                else
                {
                    // TODO - Do something when building is at full health
                    healthPool = _maxHealth; // Accounts for floating point precision errors.
                    IsRepaired = true;
                    _repairer = null;  // Stops the building getting repaired next Update()
                    SetRepMultiplier(1f);
                }
            }
        }
        
        /*
         * Put a unit in the repairer field to kick off the repair process
         * Also adjusts the repMultiplier based on unit type
         * Returns the mending unit aka the repairer
         */
        public Unit Mend(Unit unit)
        {
            _repairer = unit;
            //TODO - if unit type is scientist, then set repMultiplier to 0.75f. Dependant on Unit implementation
            if (unit.UnitClass == Unit.UnitType.Scientist)
            {
                SetRepMultiplier(0.75f);
            }
            return unit;
        }

        /*
        * If a unit has been assigned to repair a building, it gains HP
        * Cannot have more HP than maxHealth
        * Returns the health pool following a heal
        */
        private float RegainHealth()
        {
            _tickCounter += Time.fixedDeltaTime;
            if (_tickCounter < 1) return healthPool;
            float newHp = healthPool + (_tickRepair * _repMultiplier);
            if (newHp >= _maxHealth)
            {
                newHp = _maxHealth;
            }
            _isBroken = false;
            healthPool = newHp;
            _tickCounter = 0;
            return healthPool;
        }
        
        /*
         * Changes the damage multiplier
         * Typical case, dish takes extra damage during high wind / bushfire unless stowed
         * Returns the new dmgModifier
         */
        public float SetDmgMultiplier(float newModifier)
        {
            _dmgMultiplier = newModifier;
            return _dmgMultiplier;
        }
        
        /*
         * Changes the repair multiplier
         * Typical case, scientist will fix build at 0.75 the rate an engineer does
         * Returns the new repair multiplier
         */
        private float SetRepMultiplier(float newModifier)
        {
            _repMultiplier = newModifier;
            return _repMultiplier;
        }
        
        /*
         * Reduces the health of a building
         * Can't reduce a buildings health to below zero.
         * Returns the health pool following a damage tick
         */
        private float TakeTickDamage()
        {
            _tickCounter += Time.fixedDeltaTime;
            if (_tickCounter >= 1)
            {
                float newHp = healthPool - (_tickDmg * _dmgMultiplier);
                if (newHp <= 0)
                {
                    newHp = 0;
                    _isBroken = true;
                    healthPool = newHp;
                    _tickCounter = 0;
                }
            }
            return healthPool;
        }
    }
}