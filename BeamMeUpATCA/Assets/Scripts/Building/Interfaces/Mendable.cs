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
        private float maxHealth;
        private float tickDmg;
        private float dmgMultiplier;
        private float tickRepair;
        private float repMultiplier;
        private bool isBroken;
        private float tickCounter;
        private Unit repairer;
        
        [field: SerializeField] private float healthPool;
        [field: SerializeField] public bool isRepaired { get; private set; }

        private void Awake()
        {
            maxHealth = 100f;
            healthPool = 100f;
            tickDmg = 1f;
            dmgMultiplier = 1f;
            tickRepair = 5f;
            repMultiplier = 1f;
            isBroken = false;
            tickCounter = 0;
            repairer = null;
            isRepaired = true;
        }
        
        private void Update() 
        {
            // Are we damaging the building?
            if ((!isBroken || tickDmg < 0) && repairer is null)
            {
                takeTickDamage();
                isRepaired = false;
            }
            
            // Are we repairing the building
            if (repairer != null)
            {
                if (healthPool != maxHealth) 
                {
                    regainHealth();
                }
                // Building is full health!
                else
                {
                    // TODO - Do something when building is at full health
                    isRepaired = true;
                    repairer = null;  // Stops the building getting repaired next Update()
                    setRepMultiplier(1f);
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
            repairer = unit;
            //TODO - if unit type is scientist, then set repMultiplier to 0.75f. Dependant on Unit implementation
            if (unit.UnitClass == Unit.UnitType.Scientist)
            {
                setRepMultiplier(0.75f);
            }
            return unit;
        }

        /*
        * If a unit has been assigned to repair a building, it gains HP
        * Cannot have more HP than maxHealth
        * Returns the health pool following a heal
        */
        private float regainHealth()
        {
            tickCounter += Time.fixedDeltaTime;
            if (tickCounter >= 1)
            {
                float newHp = healthPool + (tickRepair * repMultiplier);
                if (newHp >= maxHealth)
                {
                    newHp = maxHealth;
                }
                isBroken = false;
                healthPool = newHp;
                tickCounter = 0;
            }
            return healthPool;
        }
        
        /*
         * Changes the damage multiplier
         * Typical case, dish takes extra damage during high wind / bushfire unless stowed
         * Returns the new dmgModifier
         */
        public float setDmgMultiplier(float newModifier)
        {
            dmgMultiplier = newModifier;
            return dmgMultiplier;
        }
        
        /*
         * Changes the repair multiplier
         * Typical case, scientist will fix build at 0.75 the rate an engineer does
         * Returns the new repair multiplier
         */
        public float setRepMultiplier(float newModifier)
        {
            repMultiplier = newModifier;
            return repMultiplier;
        }
        
        /*
         * Reduces the health of a building
         * Can't reduce a buildings health to below zero.
         * Returns the health pool following a damage tick
         */
        private float takeTickDamage()
        {
            tickCounter += Time.fixedDeltaTime;
            if (tickCounter >= 1)
            {
                float newHp = healthPool - (tickDmg * dmgMultiplier);
                if (newHp <= 0)
                {
                    newHp = 0;
                    isBroken = true;
                    healthPool = newHp;
                    tickCounter = 0;
                }
            }
            return healthPool;
        }
    }
}