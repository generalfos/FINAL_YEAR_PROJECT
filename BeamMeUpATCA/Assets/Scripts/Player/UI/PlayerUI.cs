using UnityEngine;
using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public class PlayerUI : MonoBehaviour
    {
        #region UnitUI
        // TODO: Currently this is a 'HACK' implementation. The Select Unit just looks for the first UnitUI.
        // TODO: Instead each Unit should have it's own unitUI element which is selected/deselected as needed.
        [field: SerializeField] private GameObject UnitUIPrefab { get; set;}

        private List<UnitUI> _unitUIList;

        private void Awake() {
            _unitUIList = new List<UnitUI>();

            // TODO: UnitUI setup should be done in Start() to allow prefab awakes to trigger.
            if (!(UnitUIPrefab is null)) CreateNewUnitUI(UnitUIPrefab);
        }

        private readonly Vector3 _unitUIScreenPosition = new Vector3(83, 0, 0);

        // TODO: Following internal TODO changes this function should take UnityEngine.Object instead of GameObject
        private void CreateNewUnitUI(GameObject prefab)
        {
            // TODO: The following should be replaced with PrefabUtility.InstantiatePrefab().
            // https://docs.unity3d.com/ScriptReference/PrefabUtility.InstantiatePrefab.html
            GameObject newUnitUI = Instantiate(prefab, _unitUIScreenPosition, Quaternion.identity, gameObject.transform);
            // TODO: After refactor, remove check for object existence as prefab should be not-nullable.
            if (UnitUIPrefab) _unitUIList.Insert(0, newUnitUI.GetComponent<UnitUI>());
                
        }

        public void SelectUnit(Unit unit) 
        {
            // TODO: Read notes under "#region UnitUI"
            if (_unitUIList.Count > 0) _unitUIList[0].setUnitUI(unit);
        }

        public void DeselectUnit(Unit unit) 
        {
            // TODO: Read notes under "#region UnitUI"
            if (_unitUIList.Count > 0) _unitUIList[0].clearUnitUI();
        }

        public void DeselectAllUnits() 
        {
            // TODO: Read notes under "#region UnitUI"
            // TODO: For all in _unitUIList -> item.clearUnitUI()
            if (_unitUIList.Count > 0) _unitUIList[0].clearUnitUI();
        }

        #endregion // End of 'UnitUI'
    }
}