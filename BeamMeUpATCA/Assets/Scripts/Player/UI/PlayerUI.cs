using UnityEngine;
using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public class PlayerUI : MonoBehaviour
    {
        [field: SerializeField] public GameObject UnitUIPrefab {get; private set;}

        private List<UnitUI> unitUIList;
        private int selectedUnits;

        private void Awake() {
            unitUIList = new List<UnitUI>();
            CreateNewUnitUI();
        }

        private void CreateNewUnitUI() 
        {
            GameObject newUnitUI = Instantiate(UnitUIPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            unitUIList.Add(newUnitUI.GetComponent<UnitUI>());
        }

        public void SelectUnit(Unit unit) 
        {
            unitUIList[0].setUnitUI(unit.Name, unit.UnitColor);
        }

        public void DeselectUnit(Unit unit) 
        {
            unitUIList[0].clearUnitUI();
        }

        public void DeselectAllUnits() 
        {
            unitUIList[0].clearUnitUI();
        }
    }
}