using UnityEngine;

namespace BeamMeUpATCA
{
    /*
     * Describes the functionality required by buildings that lose health, and hence
     * need to be mended.
     * Set out as an abstract class as all Mendable buildings will have the same
     * implementation.
     */
    public abstract class Mendable : MonoBehaviour
    {
        private float maxHealth;
        private float healthPool;
        private float tickDmg;
        private float dmgMultiplier;
        private float tickRepair;
        private float repMultiplier;
        private bool isBroken;
        private float tickCounter;
        private Unit repairer;

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
            Unit repairer = null;
        }
        
        private void FixedUpdate() 
        {
            if ((!isBroken || tickDmg < 0) && repairer is null)
            {
                takeTickDamage();
            }

            if (repairer != null)
            {
                if (healthPool != maxHealth) 
                {
                    regainHealth();
                }
                else
                {
                    // TODO - Do something when building is at full health
                    Debug.Log("Building at full health");
                    repairer = null;
                }
            }
        }

        
        /*
         * Put a unit in the repairer field to kick off the repair process
         * Also adjusts the repMultiplier based on unit type
         */
        public Unit Mend(Unit unit)
        {
            repairer = unit;
            //TODO - if unit type is scientist, then set repMultiplier to 0.75f
            return unit;
        }
        
        
        /*
        * If a unit has been assigned to repair a building, it gains HP
        * Cannot have more HP than maxHealth
        */
        private void regainHealth()
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
        }

        private void takeTickDamage()
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
        }

    }
}