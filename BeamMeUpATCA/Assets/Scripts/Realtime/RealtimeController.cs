/** 
 * RealtimeController.cs
 * 
 * This script manages the scene's weather, lighting and array configuration.
 * Updates are either based on real time data (in real time mode) or are arbitrarily made
 * when outside of real time mode.
 * 
 * @author: Joel Foster
 * @last_edited: 18/10/2022 21:38
 */

using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeamMeUpATCA
{
    public class RealtimeController : MonoBehaviour
    {
        [SerializeField] private bool realTimeMode;

        [field: Header("Data Acquisition")]
        [SerializeField] private Weather weatherData;

        [field: Header("Day/Night Cycle")]
        [SerializeField] private GameObject SunLight;

        [field: Header("Weather")]
        [SerializeField] private GameObject rain;
        [SerializeField] private bool rainActive;
        [SerializeField] private GameObject lightRain;
        [SerializeField] private bool lightRainActive;
        [SerializeField] private GameObject heavyRain;
        [SerializeField] private bool heavyRainActive;

        private List<Dish> dishes;
        private List<JunctionBox> boxes;
        private Dictionary<string, JunctionBox> boxDict;
        private float altitude;
        private float azimuth;
        private double rainfall;

        private float timePassed;

        // All current valid array configurations
        private static readonly Dictionary<string, string[]> validConfigs = new Dictionary<string, string[]>()
        {
            { "6A", new string[] { "W4", "W45", "W102", "W173", "W195" } },
            { "6B", new string[] { "W2", "W64", "W147", "W182", "W196" } },
            { "6C", new string[] { "W0", "W10", "W113", "W140", "W182" } },
            { "6D", new string[] { "W8", "W32", "W84", "W168", "W196" } },
            { "1.5A", new string[] { "W100", "W110", "W147", "W168", "W196" } },
            { "1.5B", new string[] { "W111", "W113", "WW163", "W182", "W195" } },
            { "1.5C", new string[] { "W98", "W128", "W173", "W190", "W195" } },
            { "1.5D", new string[] { "W102", "W109", "W140", "W182", "W196" } },
            { "750A", new string[] { "W147", "W163", "W172", "W190", "W195" } },
            { "750B", new string[] { "W98", "W109", "W113", "W140", "W148" } },
            { "750C", new string[] { "W64", "W84", "W100", "W110", "W113" } },
            { "750D", new string[] { "W100", "W102", "W128", "W140", "W147" } },
            { "EW367", new string[] { "W104", "W110", "W113", "W124", "W128" } },
            { "EW352", new string[] { "W102", "W104", "W109", "W112", "W125" } },

            { "H214", new string[] { "W98", "W104", "W113", "N5", "N14" } },
            { "H168", new string[] { "W100", "W104", "W111", "N7", "N11" } },
            { "H75", new string[] { "W104", "W106", "W109", "N2", "N5" } },

            { "EW214", new string[] { "W98", "W102", "W104", "W109", "W112" } },
            { "NS214", new string[] { "W106", "N2", "N7", "N11", "N14" } },
            { "122C", new string[] { "W98", "W100", "W102", "W104", "W106" } },

        };

        private void Start()
        {
            dishes = GameManager.GetBuildings<Dish>();
            boxes = GameManager.GetBuildings<JunctionBox>();
            boxDict = new Dictionary<string, JunctionBox>();
            foreach (JunctionBox box in boxes)
            {
                boxDict.Add(box.Name, box);
            }
            timePassed = 0f;
        }

        private void Update()
        {
            UpdateWeatherState();
            UpdateTimeState();
            if (realTimeMode)
            {
                UpdateArrayPositions();
                UpdateArrayState();
            }
        }

        private void UpdateArrayState()
        {
            // Guard clause to prevent dish updates without data
            if (!weatherData.stowedArraysRetrieved)
            {
                return;
            }
            foreach (Dish dish in dishes)
            {
                if (dish == null)
                {
                    continue;
                }
                // Update stow states
                if (weatherData.currStowedArrays.Contains(dish.dishId))
                {
                    dish.IsStowed = true;
                }
                // Validate azimuth and elevation values
                if (!weatherData.azimuthRetrieved ||
                !weatherData.elevationRetrieved)
                {
                    continue;
                }
                if (weatherData.currElevation < 0 ||
                        weatherData.currElevation > 90)
                {
                    continue;
                }
                if (weatherData.currAzimuth < 0 ||
                        weatherData.currAzimuth > 360)
                {
                    continue;
                }
                altitude = (float)weatherData.currElevation;
                azimuth = (float)weatherData.currAzimuth;
                if (!dish.IsStowed)
                {
                    // Angle dish to look at target
                    dish.AltazCoordinates(altitude, azimuth);
                }
            }
        }

        private void UpdateArrayPositions()
        {
            string[] dishPositions;
            string dishPosition;

            // Validate inputs
            if (!weatherData.configRetrieved)
            {
                return;
            }
            if (boxDict == null)
            {
                return;
            }

            // Retrieve current positions of arrays

            if (weatherData != null && weatherData.currConfig != null)
            {
                if (validConfigs.ContainsKey(weatherData.currConfig))
                {
                    dishPositions = validConfigs[weatherData.currConfig];
                    
                    // Iterate over dishes and place them
                    foreach (Dish dish in dishes)
                    {
                        int id = dish.dishId;
                        if (id == 6)
                        {
                            continue;
                        }
   
                        dishPosition = dishPositions[id-1];

                        JunctionBox box = boxDict[dishPosition];
                        dish.SnapJunctionBox(box);
                    }
                }
                else
                {
                    Debug.LogWarning("Could not get valid data from ATCA Live");
                }
            }
        }

        private void UpdateWeatherState()
        {
            if (realTimeMode)
            {
                if (!weatherData.rainRetrieved || weatherData.windSpdRetrieved)
                {
                    return;
                }
                rainfall = weatherData.currRainfall;
                GameObject activeRain;

                lightRainActive = false;
                rainActive = false;
                heavyRainActive = false;

                // Set rain intensity
                if (rainfall <= 0)
                {
                    rain.SetActive(false);
                    lightRain.SetActive(false);
                    heavyRain.SetActive(false);
                    return;
                }
                else if (rainfall <= 3)
                {
                    activeRain = lightRain;
                    lightRainActive = true;
                    rain.SetActive(false);
                    heavyRain.SetActive(false);
                }
                else if (rainfall <= 10)
                {
                    activeRain = rain;
                    rainActive = true;
                    lightRain.SetActive(false);
                    heavyRain.SetActive(false);
                }
                else
                {
                    activeRain = heavyRain;
                    rain.SetActive(false);
                    lightRain.SetActive(false);
                }
                activeRain.SetActive(true);
            }
            else
            {
                // Update based on stored variable value
                if (rainActive)
                {
                    lightRain.SetActive(false);
                    heavyRain.SetActive(false);
                    rain.SetActive(true);
                }
                else if (heavyRainActive)
                {
                    rain.SetActive(false);
                    lightRain.SetActive(false);
                    heavyRain.SetActive(true);
                }
                else if (lightRainActive)
                {
                    rain.SetActive(false);
                    heavyRain.SetActive(false);
                    lightRain.SetActive(true);
                }
            }
        }

        private void UpdateTimeState()
        {
            if (realTimeMode)
            {
                if (!weatherData.timeRetrieved)
                {
                    return;
                }
                DateTime time = weatherData.currTime;
                DateTime dayStart = weatherData.currTime;
                TimeSpan ts = new TimeSpan(0, 0, 0);
                dayStart = dayStart.Date + ts;
                TimeSpan timeDiff = time - dayStart;
                double hrs = timeDiff.TotalHours;
                // Sun rotates over the site between 6 am and 6 pm
                if (hrs < 6 || hrs > 18)
                {
                    SunLight.transform.eulerAngles = new Vector3(0, 90, 0);
                }
                else
                {
                    SunLight.transform.eulerAngles = new Vector3((float)(hrs - 6) * 15, 90, 0);
                } 
            }
            else
            {
                timePassed += Time.deltaTime;
                if (timePassed > 60f)
                {
                    // Slowly moves sun every 60 seconds 
                    SunLight.transform.Rotate(new Vector3(1, 0, 0));
                    timePassed = 0f;
                }
            }
        }

    }
}