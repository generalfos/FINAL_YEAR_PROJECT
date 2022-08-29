using UnityEngine;
using UnityEngine.UI;

using System;

namespace BeamMeUpATCA 
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] 
        private Text _UnitName;

        [SerializeField] 
        private Image _UnitIcon;

        [field: SerializeField]
        public GameObject DynamicInfo { get; private set; }

        private void Awake() 
        {
            clearUnitUI();
            DynamicInfo.SetActive(false);
        }

        public void clearUnitUI()
        {
            _UnitName.text = String.Empty;
            _UnitIcon.color = Color.white;
        }

        public void setUnitUI(string name, Color color) 
        {
            _UnitName.text = name;
            _UnitIcon.color = color;
        }
    }
}
