
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA 
{
    public class PlayerCommand : MonoBehaviour
    {
        [SerializeField]
        private PlayerUI _playerUI;
        [SerializeField]
        private Player _player;

        private GameObject target;

        void setUI(Unit props)
        {
            String _unitName = props.name;
            Color _unitColor = props.Class == Unit.UnitClass.Engineer ? Color.red : Color.blue;
            
            _playerUI.setUnitUI(_unitName, _unitColor);
        }

        void clearUI()
        {
            _playerUI.clearUnitUI();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                // Hover on Object
                if (hit.transform.gameObject.tag == "Unit" || hit.transform.gameObject.tag == "ObservationHut" || hit.transform.gameObject.tag == "EngineerShed") {
                    _playerUI.DynamicInfo.transform.position = _player.Pointer.ReadValue<Vector2>();
                    _playerUI.DynamicInfo.GetComponentInChildren<Text>().text = hit.transform.gameObject.tag;
                    _playerUI.DynamicInfo.SetActive(true);
                } else
                {
                    _playerUI.DynamicInfo.SetActive(false);
                }

                // Click on Object 
                if (_player.PrimaryAction.ReadValue<float>() > 0)
                {

                    if (hit.transform.gameObject.tag == "Unit")
                    {
                        Debug.Log("I've hit a Unit: " + _player.Pointer.ReadValue<Vector2>());
                        Unit props = hit.transform.gameObject.GetComponent<Unit>();

                        Debug.Log("Name: " + props.Name + "; Class: " + props.Class.ToString());
                        setUI(props);
                    }
                    else
                    {
                        // Clear Detailed View (TODO: check state if not issuing a command)
                        clearUI();
                    }
                }

            }

                
            
        }
    }
}
