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
        public int sort_order { get; set; }
        public int wmo { get; set; }
        public string name { get; set; }
        public string history_product { get; set; }
        public string local_date_time { get; set; }
        public string local_date_time_full { get; set; }
        public string aifstime_utc { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double apparent_t { get; set; }
        public string cloud { get; set; }
        public int? cloud_base_m { get; set; }
        public int? cloud_oktas { get; set; }
        public string cloud_type { get; set; }
        public object cloud_type_id { get; set; }
        public double delta_t { get; set; }
        public int gust_kmh { get; set; }
        public int gust_kt { get; set; }
        public double air_temp { get; set; }
        public double dewpt { get; set; }
        public double press { get; set; }
        public double press_msl { get; set; }
        public double press_qnh { get; set; }
        public string press_tend { get; set; }
        public string rain_trace { get; set; }
        public int rel_hum { get; set; }
        public string sea_state { get; set; }
        public string swell_dir_worded { get; set; }
        public object swell_height { get; set; }
        public object swell_period { get; set; }
        public string vis_km { get; set; }
        public string weather { get; set; }
        public string wind_dir { get; set; }
        public int wind_spd_kmh { get; set; }
        public int wind_spd_kt { get; set; }
    }

    public class Header
    {
        public string refresh_message { get; set; }
        public string ID { get; set; }
        public string main_ID { get; set; }
        public string name { get; set; }
        public string state_time_zone { get; set; }
        public string time_zone { get; set; }
        public string product_name { get; set; }
        public string state { get; set; }
    }

    public class Notice
    {
        public string copyright { get; set; }
        public string copyright_url { get; set; }
        public string disclaimer_url { get; set; }
        public string feedback_url { get; set; }
    }

    public class Observations
    {
        public List<Notice> notice { get; set; }
        public List<Header> header { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Root
    {
        public Observations observations { get; set; }
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
                var record = JsonConvert.DeserializeObject<Root>(jsonString);
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