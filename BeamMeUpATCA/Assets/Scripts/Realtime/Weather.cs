/** 
 * Weather.cs
 * 
 * This script handles retrieving the latest weather and array state data 
 * utilising the BOM API, ATCA Live Webpage and OZForecast website.
 *
 * 
 * @author: Joel Foster
 * @last_edited: 16/10/2022 22:03
 */

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using HtmlAgilityPack;

namespace BeamMeUpATCA
{
    public class Weather : MonoBehaviour
    {
        /* Classes to serialise JSON data */
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

        public bool windSpdRetrieved;
        public bool gustSpdRetrieved;
        public bool tempRetrieved;
        public bool rainRetrieved;
        public bool azimuthRetrieved;
        public bool elevationRetrieved;
        public bool targetRetrieved;
        public bool configRetrieved;
        public bool windDirRetrieved;
        public bool timeRetrieved;
        public bool stowedArraysRetrieved;

        private bool CSIROReqFin;
        private bool OZReqFin;

        public int currWindSpd;
        public int currGustSpd;
        public double currTemp;
        public double currRainfall;
        public double currAzimuth;
        public double currElevation;
        public string currObservationTarget;
        public string currConfig;
        public string currWindDir;
        public DateTime currTime;
        public List<int> currStowedArrays;

        private float timePassed;

        private void Awake() 
        {
            timePassed = 0f;
            ResetState();
            UpdateData();
        }

        private void ResetState()
        {
            windSpdRetrieved = false;
            gustSpdRetrieved = false;
            tempRetrieved = false;
            rainRetrieved = false;
            azimuthRetrieved = false;
            elevationRetrieved = false;
            targetRetrieved = false;
            configRetrieved = false;
            windDirRetrieved = false;
            timeRetrieved = false;
            stowedArraysRetrieved = false;
            CSIROReqFin = false;
            OZReqFin = false;
            currStowedArrays = new List<int>();
        }

        private IEnumerator DataCSIROGetRequest(string url)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                string weatherStatus;
                string antennaStates;
                string observationTarget;
                string observationState;
                string configState;
                bool success = false;
                currStowedArrays.Clear();

                yield return req.SendWebRequest();

                success = CheckReqStatus(req);
                if (!success)
                {
                    Debug.LogError("DataCSIROGetRequest: HTTP Request Failed");
                    CSIROReqFin = true;
                    yield break;
                }

                #region HTML Parsing
                // Parse HTML
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(req.downloadHandler.text);
                /* 
                 * NOTE: Current HTML retrieval method is extremely volatile to deviations
                 * in usual/expected HTML structure of target website 
                 */
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='box']/*");
                try
                {
                    weatherStatus = (nodes[3].NextSibling).InnerText;
                    antennaStates = (nodes[8].NextSibling).InnerText;
                    observationTarget = (nodes[9]).InnerText;
                    observationState = (nodes[9].NextSibling).InnerText;
                    configState = (nodes[10]).InnerText;
                }
                catch (Exception)
                {
                    Debug.LogError("dataCSIROGetRequest: Invalid data page format retrieved from website");
                    CSIROReqFin = true;
                    yield break;
                }
                #endregion

                #region Data Retrieval
                // Retrieve Current Temp
                Regex tempRegex = new Regex(@"\d+ degrees Celsius", RegexOptions.IgnoreCase);
                Match tempMatch = tempRegex.Match(weatherStatus);
                string tempString = Regex.Match(tempMatch.Value, @"\d+").Value;
                success = Double.TryParse(tempString, out currTemp);
                if (!success)
                {
                    int tempInt;
                    success = Int32.TryParse(tempString, out tempInt);
                    if (!success)
                    {
                        Debug.LogError("dataCSIROGetRequest: Invalid temperature format retrieved from website");
                        CSIROReqFin = true;
                        yield break;
                    }
                    currTemp = (double)tempInt;
                }
                tempRetrieved = true;

                // Retrieve which antennas are stowed
                // Handle all antennas are stowed edge case
                if (antennaStates.Replace("\n", "") == "All antennas are stowed.") {
                    // All arrays are stowed
                    currStowedArrays.Add(1);
                    currStowedArrays.Add(2);
                    currStowedArrays.Add(3);
                    currStowedArrays.Add(4);
                    currStowedArrays.Add(5);
                    currStowedArrays.Add(6);
                    // Update true config position in HTML
                    try
                    {
                        configState = nodes[9].InnerText;
                    }
                    catch (Exception)
                    {
                        Debug.LogError("dataCSIROGetRequest: Invalid data page format retrieved from website");
                        CSIROReqFin = true;
                        yield break;
                    }
                }
                Regex stowRegex = new Regex(@"antenna \d+(([ ]*,[ ]*\d+)+)? is stowed", RegexOptions.IgnoreCase);
                Match stowMatch = stowRegex.Match(antennaStates);
                MatchCollection stowedArrays = Regex.Matches(stowMatch.Value, @"\d");
                var matchList = (from Match m in Regex.Matches(stowMatch.Value, @"\d") select m.Value);
                foreach (string array in matchList)
                {
                    int arrayNo;
                    success = Int32.TryParse(array, out arrayNo);
                    if (!success)
                    {
                        Debug.LogError("dataCSIROGetRequest: Invalid stowed array no retrieved from website");
                        CSIROReqFin = true;
                        yield break;
                    }
                    currStowedArrays.Add(arrayNo);
                }
                stowedArraysRetrieved = true;

                // Retrieve current config
                Regex configRegex = new Regex(@"\w+ array", RegexOptions.IgnoreCase);
                Match configMatch = configRegex.Match(configState);
                string configString = Regex.Match(configMatch.Value, @"\w+ ").Value;
                if (configString == null)
                {
                    Debug.LogError("dataCSIROGetRequest: Invalid array config retrieved from website");
                    CSIROReqFin = true;
                    yield break;
                }
                currConfig = configString.Trim();
                configRetrieved = true;

                // Retrieve observation target
                currObservationTarget = observationTarget;
                targetRetrieved = true;

                // Retrieve azimuth
                Regex azimuthRegex = new Regex(@"azimuth of \d+\.\d+ degrees", RegexOptions.IgnoreCase);
                Match azimuthMatch = azimuthRegex.Match(observationState);
                string azimuthString = Regex.Match(azimuthMatch.Value, @"\d+\.\d+").Value;
                success = Double.TryParse(azimuthString, out currAzimuth);
                if (!success)
                {
                    int tempInt;
                    success = Int32.TryParse(azimuthString, out tempInt);
                    if (!success)
                    {
                        Debug.LogError("dataCSIROGetRequest: Invalid azimuth retrieved from website");
                        CSIROReqFin = true;
                        yield break;
                    }
                    currAzimuth = (double)tempInt;
                }
                azimuthRetrieved = true;

                // Retrieve elevation
                Regex elevationRegex = new Regex(@"elevation of \d+\.\d+ degrees", RegexOptions.IgnoreCase);
                Match elevationMatch = elevationRegex.Match(observationState);
                string elevationString = Regex.Match(elevationMatch.Value, @"\d+\.\d+").Value;
                success = Double.TryParse(elevationString, out currElevation);
                if (!success)
                {
                    int tempInt;
                    success = Int32.TryParse(azimuthString, out tempInt);
                    if (!success)
                    {
                        Debug.LogError("dataCSIROGetRequest: Invalid azimuth retrieved from website");
                        CSIROReqFin = true;
                        yield break;
                    }
                    currElevation = (double)tempInt;
                }
                elevationRetrieved = true;

                #endregion
                CSIROReqFin = true;
            }
        }

        private IEnumerator BOMWeatherGetRequest(string url)
        {
            yield return new WaitUntil(() => OZReqFin == true);

            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                Root record;
                bool success;

                req.SetRequestHeader("user-agent", "");
                yield return req.SendWebRequest();

                success = CheckReqStatus(req);
                if (!success)
                {
                    Debug.LogError("BOMWeatherGetRequest: HTTP Request Failed");
                    yield break;
                }
                string jsonString = req.downloadHandler.text;
                
                try
                {
                    record = JsonConvert.DeserializeObject<Root>(jsonString);
                }
                catch (Exception)
                {
                    Debug.LogError("BOMWeatherGetRequest: Error occurred while deserialising JSON");
                    yield break;
                }
                #region Data Parsing
                Datum datum = record.observations.data[0];
                if (!tempRetrieved)
                {
                    currTemp = datum.air_temp;
                    tempRetrieved = true;
                }
                if (!gustSpdRetrieved) {
                    currGustSpd = datum.gust_kt;
                    gustSpdRetrieved = true;
                }
                if (!windDirRetrieved)
                {
                    currWindDir = datum.wind_dir;
                    windDirRetrieved = true;
                }
                if (!windSpdRetrieved)
                {
                    currWindSpd = datum.wind_spd_kmh;
                    windSpdRetrieved = true;
                }
                if (!rainRetrieved)
                {
                    success = Double.TryParse(datum.rain_trace, out currRainfall);
                    if (!success)
                    {
                        Debug.LogError("BOMWeatherGetRequest: Invalid rainfall format stored in JSON data");
                        yield break;
                    }
                    rainRetrieved = true;
                }
                #endregion
            }
        }

        private IEnumerator OZWeatherGetRequest(string url)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                bool success;

                yield return new WaitUntil(() => CSIROReqFin == true);

                yield return req.SendWebRequest();

                success = CheckReqStatus(req);
                if (!success)
                {
                    Debug.LogError("OZWeatherGetRequest: HTTP Request Failed");
                    OZReqFin = true;
                    yield break;
                }

                #region Data Retrieval
                // Read in HTML
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(req.downloadHandler.text);
                HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");
                // Retrieve data string
                string tableString = tables[5].InnerText;
                Regex ozDataRegex = new Regex(@"(?: Today).*");
                string dataString = ozDataRegex.Match(tableString).Value;
                
                // Split data string into an arg array
                string[] dataArgs = dataString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // Validate that the data string is in the correct format
                if (dataArgs.Length != 12)
                {
                    Debug.LogError("OZWeatherGetRequest: Invalid format for latest observation retrieved from website");
                    yield break;
                }
                string timeString = dataArgs[1];
                string tempString = dataArgs[2];
                string windSpeedString = dataArgs[4];
                string gustSpeedString = dataArgs[7];
                string windDirString = dataArgs[10];
                string rainString = dataArgs[11];
                #endregion

                #region Data Parsing
                // Parse data strings and store in global variables
                try
                {
                    currTime = DateTime.Parse(timeString);
                }
                catch (Exception)
                {
                    Debug.LogError("OZWeatherGetRequest: Invalid time retrieved from website");
                    OZReqFin = true;
                    yield break;
                }
                timeRetrieved = true;
                if (!tempRetrieved)
                {
                    success = Double.TryParse(tempString, out currTemp);
                    if (!success)
                    {
                        Debug.LogError("OZWeatherGetRequest: Invalid temp format retrieved from website");
                        OZReqFin = true;
                        yield break;
                    }
                    tempRetrieved = true;
                }
                success = Int32.TryParse(windSpeedString, out currWindSpd);
                if (!success)
                {
                    Debug.LogError("OZWeatherGetRequest: Invalid wind speed retrieved from website");
                    OZReqFin = true;
                    yield break;
                }
                windSpdRetrieved = true;
                success = Int32.TryParse(gustSpeedString, out currGustSpd);
                if (!success)
                {
                    Debug.LogError("OZWeatherGetRequest: Invalid gust speed retrieved from website");
                    OZReqFin = true;
                    yield break;
                }
                gustSpdRetrieved = true;
                currWindDir = windDirString;
                windDirRetrieved = true;
                success = Double.TryParse(rainString, out currRainfall);
                if (!success)
                {
                    Debug.LogError("OZWeatherGetRequest: Invalid rainfall retrieved from website");
                    OZReqFin = true;
                    yield break;
                }
                rainRetrieved = true;

                #endregion
                OZReqFin = true;
            }
        }

        private bool CheckReqStatus(UnityWebRequest req)
        {
            switch (req.result)
            {
                case UnityWebRequest.Result.Success:
                    return true;
                default:
                    Debug.LogError("UnityWebRequest Error");
                    break;
            }
            return false;
        }

        private void UpdateData()
        {
            StartCoroutine(DataCSIROGetRequest("https://www.narrabri.atnf.csiro.au/cgi-bin/Public/atca_live.cgi/"));
            StartCoroutine(OZWeatherGetRequest("https://ozforecast.com.au/cgi-bin/weatherstation.cgi?station=11001"));
            StartCoroutine(BOMWeatherGetRequest("http://www.bom.gov.au/fwo/IDN60801/IDN60801.95734.json"));
        }

        private void Update()
        {
            timePassed += Time.deltaTime;
            // Update data every 15 mins.
            if (timePassed > 900)
            {
                ResetState();
                UpdateData();
                timePassed = 0f;
            }
        }
    }
}