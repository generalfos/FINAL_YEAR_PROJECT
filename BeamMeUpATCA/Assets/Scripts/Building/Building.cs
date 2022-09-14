using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class Building : MonoBehaviour
    {
        // Setup
        [SerializeField]
        private float maxHealth;

        [SerializeField]
        private float healthPool;
        [SerializeField]
        private float tickDmg;
        [SerializeField]
        private float repairHP;
        [SerializeField]
        private bool isBroken;
        private float tickCounter;
        private Unit repairer;

        private List<Unit> storedUnits;

        private void Awake() {
            maxHealth = 100f;
            healthPool = 100f;
            tickDmg = 1f;
            repairHP = 5f;
            isBroken = false;
            tickCounter = 0;
            Unit repairer = null;
        }

        private void FixedUpdate() 
        {
            // Reduce HP if not broken or not being repaired
            if ((!isBroken || tickDmg < 0) && repairer is null)
            {
                takeTickDamage();
            }
            // If a unit has been assigned to repair, gain HP
            if (repairer != null)
            {
                regainHealth();
            }
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
                float newHp = healthPool + repairHP; // TODO: Multiplier based on unit repairing
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
                float newHp = healthPool - tickDmg;
                if (newHp <= 0)
                {
                    newHp = 0;
                    isBroken = true;
                    healthPool = newHp;
                    tickCounter = 0;
                }
            }
        }

        public void setTickDmg(float dmg) {
            tickDmg = dmg;
        }

        public float getHealth() {
            return healthPool;
        }

        public List<Unit> getUnits() {
            return storedUnits;
        }

        public void storeUnit(Unit unit) {
            storedUnits.Add(unit);
        }

        public Unit removeUnit(Unit unit) {
            storedUnits.Remove(unit);
            return unit;
        }
    }
}
