using UnityEngine;
using System.Collections.Generic;
using System;

namespace BeamMeUpATCA
{
    public class PlayerUI : MonoBehaviour
    {

        private delegate void CommanderDelegate<T>(Vector2 position);
        public UnitCommander commander { private get; set; }

        public void IssueCommandToUnits<T>(Vector2 position) 
        {
            if (commander == null) 
            {
                Debug.LogWarning("UnitCommander is null. Ensure dependency is met.");
                return;
            }

            if (typeof(T).IsSubclassOf(typeof(Command))) 
            {
                CommanderDelegate<T> commandDel = ctx => commander.CommandUnits<T>(ctx);
                commandDel(position);
            } 
            else Debug.LogWarning("Type must be a 'Command' type");
        }

        private void Awake() {
            unitUIList = new List<UnitUI>();
            CreateNewUnitUI();
        }

        #region UnitUI

        [field: SerializeField] public GameObject UnitUIPrefab {get; private set;}

        private List<UnitUI> unitUIList;
        private int selectedUnits;
        private Vector3 unitUIScreenPosition = new Vector3(83, 0, 0);

        private void CreateNewUnitUI() 
        {
            GameObject newUnitUI = Instantiate(UnitUIPrefab, unitUIScreenPosition, Quaternion.identity, gameObject.transform);
            unitUIList.Add(newUnitUI.GetComponent<UnitUI>());
        }

        public void SelectUnit(Unit unit) 
        {
            unitUIList[0].setUnitUI(unit);
        }

        public void DeselectUnit(Unit unit) 
        {
            unitUIList[0].clearUnitUI();
        }

        public void DeselectAllUnits() 
        {
            unitUIList[0].clearUnitUI();
        }

        #endregion
    }
}