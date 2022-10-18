using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public class UIManager : MonoBehaviour
    {

        #region MendableMoralIndicators

            /**
             * These values shouldn't be editable in the inspector. The colour
             * scheme enum should be used by external components to select the
             * resulting colour scheme.
             */
            public enum ColorScheme { DISH = 0, BUILDING = 1  };
            private Color[][] colour_schemes = {
                new Color[] { new Color(0.0f / 255.0f, 255.0f / 255.0f, 82.0f / 255.0f), new Color(241.0f / 255.0f, 196.0f / 255.0f, 15.0f / 255.0f), new Color(230.0f / 255.0f, 126.0f / 255.0f, 34.0f / 255.0f), new Color(231.0f / 255.0f, 76.0f / 255.0f, 60.0f / 255.0f) },
                new Color[] { new Color(52.0f / 255.0f, 152.0f / 255.0f, 219.0f / 255.0f), new Color(231.0f / 255.0f, 76.0f / 255.0f, 60.0f / 255.0f) }
            };

            /**
             * Using a ratio between current/max health, select the correct 
             * colour from the given scheme index. The following is an example
             * of how to use it externally:
             *
             *      using BeamMeUpATCA; // Only necessary if not in the namespace 
             *
             *      Colour result = GameManager.UI.setMendableColour(currentVal, 
             *                          maxVal, 
             *                          UIManager.ColorScheme.DISH);
             *
             * @param current   - The current value between 0.0f and max.
             * @param max       - The maximum value current can be.
             * @param scheme    - What colour scheme should the resulting colour
             *                    be from?
             * @return - A resulting colour indexed by current/max ratio in the 
             *           given colour scheme selected.
             */
            public Color GetMendableColour(float current, float max, ColorScheme scheme) {
                float ratio = Mathf.Clamp(current, 0.0f, max) / max;
                int scheme_len = colour_schemes[(int)scheme].Length;
                int colour_index = Mathf.Clamp(scheme_len - Mathf.CeilToInt(ratio * scheme_len), 0, scheme_len - 1);
                return colour_schemes[(int)scheme][colour_index];
            }
        #endregion

        #region UnitState 
            [Header("Unit States")]
            [field: SerializeField] private Sprite ShedIcon;
            [field: SerializeField] private Sprite IdleIcon;
            [field: SerializeField] private Sprite MoveIcon;
            [field: SerializeField] private Sprite ObservationIcon;
            [field: SerializeField] private Sprite RepairIcon;
            [field: SerializeField] private Sprite WeatherIcon;

            public enum UnitState { SHED, IDLE, MOVE, OBS, REPAIR, WEATHER };

            /**
             * Using a given state, fetch the correct Icon Sprite for the given 
             * state. This allows external scripts to consistently get the right 
             * icons uniformally. An example:
             *
             *      using BeamMeUpATCA; // Only necessary if not in the namespace 
             *
             *      Image result = GameManager.UI.GetUnitState(UIManager.UnitState.MOVE); 
             *
             * @param state - The state the Unit is currently in.
             * @return - The dedicated icon sprite for the given state.
             */
            public Sprite GetUnitState(UnitState state) {
                switch (state) {
                    case UnitState.SHED:        return ShedIcon;
                    case UnitState.MOVE:        return MoveIcon;
                    case UnitState.OBS:         return ObservationIcon;
                    case UnitState.REPAIR:      return RepairIcon;
                    case UnitState.WEATHER:     return WeatherIcon;
                }

                // Any unexpected State should be Idle
                return IdleIcon;
            }


        #endregion


        #region UnitSelection
            [Header("Unit Selection")] 
            [field: SerializeField] private List<Unit> selected_units;
            [field: SerializeField] private Text selected_text;

            /**
             * Sets the Select Indicator for a given unit to be visible or 
             * invisible.
             *
             * @param unit  - A unit that has a UnitCanvas set. Nothing will
             *                happen if there is no UnitCanvas set.
             * @param visable
             */
            private void setUnitsIndicator(Unit unit, bool visable) {
                // Check if the unit has a select indicator
                if (unit.UnitCanvas == null) return;

                unit.UnitCanvas.gameObject.SetActive(visable);
            }

            private void updateSelectedTest() {
                if (selected_text is null) return;

                string combined_text = "";

                foreach (Unit unit in selected_units) 
                    combined_text += (combined_text == "") ? unit.Name : "\n" + unit.Name;
                
                selected_text.text = combined_text;
            }

            public void SelectUnit(Unit unit) {
                setUnitsIndicator(unit, true);
                selected_units.Add(unit); // Keep track of selected units

                updateSelectedTest();
            }

            public void DeselectUnit(Unit unit) {
                setUnitsIndicator(unit, false);
            }

            public void DeselectAllUnits() {
                foreach (Unit unit in selected_units) DeselectUnit(unit);
                selected_units.Clear(); // Clear all selected units

                updateSelectedTest();
            }

            public void CameraFocusSelect(Unit target) {
                if (target is null) return;

                GameManager.UnitCommander.DeselectAllUnits();
                GameManager.UnitCommander.SelectUnit(target);

                CameraFocus(target.gameObject);
            }

            public void CameraFocus(GameObject target) {
                if (target is null) return;

                CameraController.CameraFocus(
                    GameManager.CameraController.ActiveCamera,
                    target.transform
                );
            }
        #endregion

        private void Awake() {
            // Initialising empty list
            selected_units = new List<Unit>();
        }

    }
}