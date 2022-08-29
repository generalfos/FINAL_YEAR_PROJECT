using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class Building : MonoBehaviour
    {
        // Setup
        [SerializeField]
        private float healthPool;
        [SerializeField]
        private float tickDmg; // We could make the engineers temporarily change tickDmg to a positive value
        [SerializeField]
        private bool isBroken;
        private float tickCounter;


        private void Awake() {
            healthPool = 100f;
            tickDmg = 1f;
            isBroken = false;
            tickCounter = 0;
        }

        private void FixedUpdate() {
            if(!isBroken) {takeTickDamage();}
        }

        private void takeTickDamage() {
            tickCounter += Time.fixedDeltaTime;
            if(tickCounter >= 1) {
                float newHp = healthPool - tickDmg;
                if(newHp <= 0) {
                    newHp = 0;
                    isBroken = true;
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
    }
}
