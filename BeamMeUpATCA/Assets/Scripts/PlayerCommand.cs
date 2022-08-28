using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommand : MonoBehaviour
{
    public Text UI_UnitName;
    public Image UI_UnitIcon;

    public GameObject UI_DynamicInfo;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        clearUI();
        UI_DynamicInfo.SetActive(false);
    }

    void setUI(UnitProperties props)
    {
        UI_UnitName.text = props.Name;
        UI_UnitIcon.color = props.Class == UnitProperties.UnitClass.Engineer ? Color.red : Color.blue;
    }

    void clearUI()
    {
        UI_UnitName.text = "";
        UI_UnitIcon.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            // Hover on Object
            if (hit.transform.gameObject.tag == "Unit" || hit.transform.gameObject.tag == "ObservationHut" || hit.transform.gameObject.tag == "EngineerShed") {
                UI_DynamicInfo.transform.position = Input.mousePosition;
                UI_DynamicInfo.GetComponentInChildren<Text>().text = hit.transform.gameObject.tag;
                UI_DynamicInfo.SetActive(true);
            } else
            {
                UI_DynamicInfo.SetActive(false);
            }

            // Click on Object 
            if (Input.GetMouseButtonDown(0))
            {

                if (hit.transform.gameObject.tag == "Unit")
                {
                    Debug.Log("I've hit a Unit: " + Input.mousePosition);
                    UnitProperties props = hit.transform.gameObject.GetComponent<UnitProperties>();

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
