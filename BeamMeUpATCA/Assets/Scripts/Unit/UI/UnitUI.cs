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

        private void Awake() 
        {
            clearUnitUI();
        }

        public void clearUnitUI()
        {
            _UnitName.text = String.Empty;
            _UnitIcon.color = Color.white;
            gameObject.SetActive(false);
        }

        public void setUnitUI(string name, Color color, int health) 
        {
            _UnitName.text = name;
            _UnitIcon.color = color;
            _UnitHealthBar.value = (float) health / (float) 100;
            // Debug.Log(_UnitHealthBar.value);
            gameObject.SetActive(true);
        }
    }
}
