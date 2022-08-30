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

        public void setUnitUI(string name, Color color) 
        {
            _UnitName.text = name;
            _UnitIcon.color = color;
            gameObject.SetActive(true);
        }
    }
}
