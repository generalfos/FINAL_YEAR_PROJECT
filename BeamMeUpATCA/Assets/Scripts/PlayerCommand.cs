
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeUpATCA 
{
    public class PlayerCommand : MonoBehaviour
    {
        [SerializeField]
        private Player _player;

        [SerializeField]
        private PlayerUI _playerUI;

        private Unit targetUnit;

        [field: SerializeField]
        public GameObject DynamicInfo { get; private set; }

        private void Awake() 
        {
            DynamicInfo.SetActive(false);
        }
        

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_player.Pointer.ReadValue<Vector2>());
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                // Hover on Object
                if (hit.transform.gameObject.tag == "Unit" || hit.transform.gameObject.tag == "ObservationHut" || hit.transform.gameObject.tag == "EngineerShed" || hit.transform.gameObject.tag == "Array") {
                    DynamicInfo.transform.position = _player.Pointer.ReadValue<Vector2>();
                    DynamicInfo.GetComponentInChildren<Text>().text = hit.transform.gameObject.tag;
                    DynamicInfo.SetActive(true);
                } else
                {
                    DynamicInfo.SetActive(false);
                }

                // Click on Object 
                if (_player.PrimaryAction.ReadValue<float>() > 0)
                {

                    if (hit.transform.gameObject.tag == "Unit")
                    {
                        Debug.Log("I've hit a Unit: " + _player.Pointer.ReadValue<Vector2>());
                        targetUnit = hit.transform.gameObject.GetComponent<Unit>();

                        Debug.Log("Name: " + targetUnit.Name + "; Class: " + targetUnit.UnitClass.ToString());
                        _playerUI.SelectUnit(targetUnit);
                    }
                    else
                    {
                        // Clear Detailed View (TODO: check state if not issuing a command)
                        if (targetUnit != null) 
                        {
                            _playerUI.DeselectUnit(targetUnit);
                        }
                    }
                }

            }

                
            
        }
    }
}
