using System.Collections.Generic;
using UnityEngine;

namespace BeamMeUpATCA
{
    public class DishUpdateData : MonoBehaviour
    {

        [SerializeField] private float altitude; // 0 - 90
        [SerializeField] private float azimuth; //0 - 360
        private List<Dish> dishes;
        
        private void Start()
        {
            dishes = GameManager.GetBuildings<Dish>();
        }

        private float altitudeOld;
        private float azimuthOld;
        public float randomSampleTimer = 100f;
        private void Update()
        {

            if (randomSampleTimer >= 100f)
            {
                altitudeOld = altitude;
                azimuthOld = azimuth;
                altitude = Random.Range(0, 90f);
                azimuth = Random.Range(0, 360f);
                randomSampleTimer = 0f;
            }
            else
            {
                randomSampleTimer += Time.deltaTime;
                foreach (Dish dish in dishes)
                {
                    if (!dish.IsStowed)
                    {

                        float alt = Mathf.Lerp(altitudeOld, altitude, randomSampleTimer / 100f);
                        float az = Mathf.Lerp(azimuthOld, azimuth, randomSampleTimer / 100f);
                        dish.AltazCoordinates(alt, az);
                    }
                }
            }
        }
    }
}