/** 
 * Weather.cs
 * 
 * This script handles retrieving the latest weather data utilising the BOM api
 * and then modifying the game state to reflect this.
 * 
 * @author: Joel Foster
 * @last_edited: 4/10/2022 22:03
 */

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
// using System.Text.RegularExpressions;

namespace BeamMeUpATCA
{
    public class Datum
    {
        public int sort_order;
        public int wmo;
        public string name;
        public string history_product;
        public string local_date_time;
        public string local_date_time_full;
        public string aifstime_utc;
        public double lat;
        public double lon;
        public double apparent_t;
        public string cloud;
        public int? cloud_base_m;
        public int? cloud_oktas;
        public string cloud_type;
        public object cloud_type_id;
        public double delta_t;
        public int gust_kmh;
        public int gust_kt;
        public double air_temp;
        public double dewpt;
        public double press;
        public double press_msl;
        public double press_qnh;
        public string press_tend;
        public string rain_trace;
        public int rel_hum;
        public string sea_state;
        public string swell_dir_worded;
        public object swell_height;
        public object swell_period;
        public string vis_km;
        public string weather;
        public string wind_dir;
        public int wind_spd_kmh;
        public int wind_spd_kt;
    }

    public class Header
    {
        public string refresh_message;
        public string ID;
        public string main_ID;
        public string name;
        public string state_time_zone;
        public string time_zone;
        public string product_name;
        public string state;
    }

    public class Notice
    {
        public string copyright;
        public string copyright_url;
        public string disclaimer_url;
        public string feedback_url;
    }

    public class Observations
    {
        public List<Notice> notice;
        public List<Header> header;
        public List<Datum> data;
    }

    public class ObservationsRoot
    {
        public Observations observations;
    }


    public class Weather : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        // const string table_pattern = "<table.*?>(.*?)</table>";
        // const string tr_pattern = "<tr>(.*?)</tr>";
        // const string td_pattern = "<td.*?>(.*?)</td>";
        // private Regex regex = new Regex(@"Australia Telescope (Culgoora)", RegexOptions.IgnoreCase);
        [SerializeField]
        TextAsset jsonData;

        private void Awake() 
        {
            Debug.Log("Weather start");
            // http://www.bom.gov.au/fwo/IDN60801/IDN60801.95734.json
            // https://ozforecast.com.au/cgi-bin/weatherstation.cgi?station=11001
            StartCoroutine(weatherGetRequest("http://www.bom.gov.au/fwo/IDN60801/IDN60801.95734.json"));
        }

        private IEnumerator weatherGetRequest(string url)
        { 
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                req.SetRequestHeader("user-agent", "");
                yield return req.SendWebRequest();

                switch (req.result)
                {
                    case UnityWebRequest.Result.Success:
                        Debug.Log(":\nReceived: " + req.downloadHandler.text);
                        break;
                    default:
                        Debug.LogError("Request Error; " + req.result);
                        break;
                }
                string jsonString = req.downloadHandler.text;
                var record = JsonConvert.DeserializeObject<ObservationsRoot>(jsonString);
                Debug.Log(record);
                // var elem = doc.GetElementById("ozf");
                // Debug.Log(elem);
                // var captured_text = regex.Match(doc);
                // Debug.Log(string.Format("'{0}' found", captured_text.Value));
            }
        }
    }
}

// Australia Telescope (Culgoora)  </TD> <TD> 15:15</TD> <TD> 20.7 </TD> <TD> 6.2 (06:15) / 20.7(15:15) </ TD > < TD > 10 / 5  N </ TD > < TD > 21 / 11 </ TD > < TD > 27 / 15 @ 11:45 </ TD > < TD > 0 </ TD > < TD > 0 </ TD > < TD > 27.2 </ TD > </ TR >