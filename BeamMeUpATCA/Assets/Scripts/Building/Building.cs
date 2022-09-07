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
        private float tickDmg; // We could make the engineers temporarily change tickDmg to a negative value
        [SerializeField]
        private bool isBroken;
        private float tickCounter;

        private Queue<Unit> storedUnits;

        private void Awake() {
            maxHealth = 100f;
            healthPool = 100f;
            tickDmg = 1f;
            isBroken = false;
            tickCounter = 0;
        }

        private void FixedUpdate() {
            //If it's not broken, or if it is broken and is being healed
            if(!isBroken || tickDmg < 0) {takeTickDamage();}
        }

        private void takeTickDamage() {
            tickCounter += Time.fixedDeltaTime;
            if(tickCounter >= 1) {
                float newHp = healthPool - tickDmg;
                if(newHp <= 0) {
                    newHp = 0;
                    isBroken = true;
                } else if (isBroken) {
                    isBroken = false; //if being healed from broken, fix
                }
                if(newHp >= maxHealth) {
                    newHp = maxHealth; //do not heal over full
                }
                healthPool = newHp;
                tickCounter = 0;
            }
        }
        public void setTickDmg(float dmg) {
            tickDmg = dmg;
        }

        public void addHealth(float hp) {
            healthPool += hp;
        }

        public void setBreak(bool flag) {
            isBroken = flag;
        }

        public float getHealth() {
            return healthPool;
        }

        public Queue<Unit> getUnits() {
            return storedUnits;
        }

        public void storeUnit(Unit unit) {
            storedUnits.Enqueue(unit);
        }

        public Unit removeUnit() {
            return storedUnits.Dequeue();
        }
    }
}
