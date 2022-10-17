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

        private void Update()
        {
            foreach (Dish dish in dishes)
            {
                if (!dish.IsStowed)
                {
                    dish.AltazCoordinates(altitude, azimuth);
                }
                
                //altitude += Time.deltaTime;
                //azimuth += Time.deltaTime;
            }
        }
    }
}