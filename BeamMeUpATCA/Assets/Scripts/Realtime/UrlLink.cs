/** 
 * UrlLink.cs
 * 
 * This script provides a method to call which opens relevant Webpages
 * in the users default browser so that they have avenues to learn more about
 * ATCA outside of purely our experience.
 *
 * 
 * @author: Joel Foster
 * @last_edited: 15/10/2022 21:30
 */

using UnityEngine;

namespace BeamMeUpATCA
{
    public class UrlLink : MonoBehaviour
    {
        public void OpenURLs()
        {
            Application.OpenURL("https://www.narrabri.atnf.csiro.au/observing/users_guide/html/atug.html");
            Application.OpenURL("https://www.csiro.au/en/about/facilities-collections/atnf/australia-telescope-compact-array");
        }
    }
}