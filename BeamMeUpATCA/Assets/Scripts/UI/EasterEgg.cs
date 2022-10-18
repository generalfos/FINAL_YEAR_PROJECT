using System;
using System.Collections;
using System.Collections.Generic;
using BeamMeUpATCA;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    // Seriously I'm so tired I don't know why I bother writing this... but yeah. It's a thing now :)

    [SerializeField] private AudioSource countdown;
    [SerializeField] private AudioSource blastoff;
    [SerializeField] private GameObject smoke;

    private bool commenceTakeoff = false;
    private bool thrustersBooting = false;
    private bool thrustersOnline = false;
    private int dishsOnline = 0;

    private List<Dish> rockets = new List<Dish>();

    public void BlastOff(string status)
    {
        Debug.Log(status);
        if (countdown && blastoff && !commenceTakeoff)
        {
            countdown.Play();
            commenceTakeoff = true;
            dishsOnline = 0;
        }
    }

    private float count;
    private void Update()
    {
        if (commenceTakeoff)
        {
            count += Time.deltaTime;
            
            if (count >= 5.4f && !thrustersBooting)
            {
                blastoff.Play();
                thrustersBooting = true;
            }

            if (count >= 8f && !thrustersOnline)
            {
                FireBoosters();
            }
        }

        if (thrustersOnline)
        {
            foreach (Dish rocket in rockets)
            {
                dishsOnline++;
                Vector3 pos = rocket.transform.position;
                rocket.gameObject.transform.position = new Vector3(pos.x, pos.y + Time.deltaTime, pos.z);
                
                rocket.AltazCoordinates(80, (count * 4));
                
                if (dishsOnline <= rockets.Count)
                {
                    foreach (Transform child in rocket.transform)
                    {
                        if (child.name == "dishBase") Instantiate(smoke, child);
                    }
                }

            }
        }
    }

    private void FireBoosters()
    {
        rockets = GameManager.GetBuildings<Dish>();

        thrustersOnline = true;
    }
}
