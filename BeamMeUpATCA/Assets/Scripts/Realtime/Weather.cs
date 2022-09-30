using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;

namespace BeamMeUpATCA
{
    public class Weather : MonoBehaviour
    {
        // Link below for information on MonoBehaviour
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/MonoBehaviour.html

        const string table_pattern = "<table.*?>(.*?)</table>";
        const string tr_pattern = "<tr>(.*?)</tr>";
        const string td_pattern = "<td.*?>(.*?)</td>";
        private Regex regex = new Regex(@"Australia Telescope (Culgoora)", RegexOptions.IgnoreCase);

        // Awake is init function. Start before first frame
        private void Awake() 
        {
            Debug.Log("Weather start");
            // StartCoroutine(weatherGetRequest("https://ozforecast.com.au/cgi-bin/weatherstation.cgi?station=11001"));
        }

        private IEnumerator weatherGetRequest(string url)
        { 
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();

                switch (req.result)
                {
                    case UnityWebRequest.Result.Success:
                        Debug.Log(":\nReceived: " + req.downloadHandler.text);
                        break;
                    default:
                        Debug.Log("Request Error");
                        break;
                }
                var doc = req.downloadHandler.data;
                // var elem = doc.GetElementById("ozf");
                // Debug.Log(elem);
                // var captured_text = regex.Match(doc);
                // Debug.Log(string.Format("'{0}' found", captured_text.Value));
            }
        }
    }
}

// Australia Telescope (Culgoora)  </TD> <TD> 15:15</TD> <TD> 20.7 </TD> <TD> 6.2 (06:15) / 20.7(15:15) </ TD > < TD > 10 / 5  N </ TD > < TD > 21 / 11 </ TD > < TD > 27 / 15 @ 11:45 </ TD > < TD > 0 </ TD > < TD > 0 </ TD > < TD > 27.2 </ TD > </ TR >