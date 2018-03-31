using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KentFoursquareProvider : MonoBehaviour {

    public string FOURSQUARE_CLIENT_ID = "FACG5HNH254GMRL3GRXQM4DIG5BI50IOMRZVIXFJ434ENDME";
    public string FOURSQUARE_CLIENT_SECRET = "B5WH5CRHSXFY1TPS4V1IXWCFVWECUYEVNAWKJXLTPBAHSWUE";
    public string NEAR_LOCATION = "Cerritos,CA";
    public string version = "20161016";

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(GetFoursquareVenueData());
	}
	
    IEnumerator GetFoursquareVenueData()
    {
        Debug.Log("Starting FoursquareVenueData");

        string url = "https://api.foursquare.com/v2/venues/search?" + "v=" + version + "&near=" + NEAR_LOCATION + "&client_id=" + FOURSQUARE_CLIENT_ID + "&client_secret=" + FOURSQUARE_CLIENT_SECRET;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError)
            {
                Debug.Log(req.error);
            }

            else
            {
                //Debug.Log(req.downloadHandler.text);
                
                var d = JsonUtility.FromJson<FoursquareAPI.RootObject>(req.downloadHandler.text);

                List<FoursquareAPI.Venue> venues = d.response.venues;

                foreach( FoursquareAPI.Venue v in venues )
                {
                    Debug.Log(v.name);
                }
            }

        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
