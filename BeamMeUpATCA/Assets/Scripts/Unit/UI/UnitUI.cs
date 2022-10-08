using UnityEngine;
using UnityEngine.UI;

using System;

namespace BeamMeUpATCA 
{
    public class UnitUI : MonoBehaviour
    {
        [SerializeField] 
        private Text _UnitName;

        [SerializeField] 
        private Image _UnitIcon;

        [SerializeField]
        private Slider _UnitHealthBar;

        [SerializeField]
        private Text _UnitHealthType;

        private Unit currentUnit;

        private void Awake() 
        {
            clearUnitUI();
        }

        public void clearUnitUI()
        {
            _UnitName.text = String.Empty;
            _UnitHealthType.text = String.Empty;
            _UnitIcon.color = Color.white;
            currentUnit = null;
            gameObject.SetActive(false);
        }

        public void setUnitUI(Unit unit) 
        {
            currentUnit = unit;
            _UnitName.text = unit.Name;
            _UnitIcon.color = unit.UnitColor;
            _UnitHealthBar.value = (float) unit.UnitMorale / (float) 100;
            _UnitHealthType.text = "Morale";
            // Debug.Log(_UnitHealthBar.value);
            gameObject.SetActive(true);
        }

        public void Update() {
            if(currentUnit) {
                setUnitUI(currentUnit);
            }
        }
    }
}
