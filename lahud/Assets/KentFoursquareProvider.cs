using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KentFoursquareProvider : MonoBehaviour {

    public string FOURSQUARE_CLIENT_ID = "FACG5HNH254GMRL3GRXQM4DIG5BI50IOMRZVIXFJ434ENDME";
    public string FOURSQUARE_CLIENT_SECRET = "B5WH5CRHSXFY1TPS4V1IXWCFVWECUYEVNAWKJXLTPBAHSWUE";
    public string NEAR_LOCATION = "Cerritos,CA";
    public string version = "20161016";
    public string VENUE_ID;
    public bool location_selected = false;
    bool location_lock = true;
    public bool venue_selected = false;
    bool venue_lock = true;

    public int VENUE_NUM_CHOICE = 0;

    public List<string> VALID_VENUE_IDS;
    public string[] VENUE_PHOTO_URLS = new string[5];

    // Use this for initialization
    void Start ()
    {
        //StartCoroutine(GetFoursquareVenueData());
	}
	
    IEnumerator GetFoursquareVenueData()
    {
        Debug.Log("Starting FoursquareVenueData");

        string search_url = "https://api.foursquare.com/v2/venues/search?" + "v=" + version + "&near=" + NEAR_LOCATION + 
            "&limit=" + "5" + "&client_id=" + FOURSQUARE_CLIENT_ID + "&client_secret=" + FOURSQUARE_CLIENT_SECRET;

        Debug.Log("this is VenueData url: " + search_url);
        using (UnityWebRequest req = UnityWebRequest.Get(search_url))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError)
            {
                Debug.Log(req.error);
            }

            else
            {
                var d = JsonUtility.FromJson<FoursquareAPI.RootObject>(req.downloadHandler.text);
                List<FoursquareAPI.Venue> venues = d.response.venues;

                foreach( FoursquareAPI.Venue v in venues)
                {
                    Debug.Log(v.name + ": ");
                    /*foreach (FoursquareAPI.Category c in v.categories)
                    {
                        Debug.Log(c.name);
                    }*/
                    VENUE_ID = v.id;
                    VALID_VENUE_IDS.Add(VENUE_ID);
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            VENUE_ID = VALID_VENUE_IDS[i];
            StartCoroutine(GetFoursquarePictureinfoData( i ));
        }
    }

    IEnumerator GetFoursquarePictureinfoData( int index )
    {
        string pictureinfo_url = "https://api.foursquare.com/v2/venues/" + VENUE_ID + "/photos?" + "v=" + version +
            "&limit=" + "1" + "&client_id=" + FOURSQUARE_CLIENT_ID + "&client_secret=" + FOURSQUARE_CLIENT_SECRET;

        Debug.Log("this is Pictureinfo url: " + pictureinfo_url);
        using (UnityWebRequest req = UnityWebRequest.Get(pictureinfo_url))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError)
            {
                Debug.Log(req.error);
            }

            else
            {
                var d = JsonUtility.FromJson<FoursquarePictureinfoAPI.RootObject>(req.downloadHandler.text);
                List<FoursquarePictureinfoAPI.Item> pictureitems = d.response.photos.items;

                if (d.response.photos.count == 0)
                {
                    VENUE_PHOTO_URLS[index] = "https://i.imgur.com/q9maqtf.png";
                }
                else
                {
                    foreach (FoursquarePictureinfoAPI.Item i in pictureitems)
                    {
                        Debug.Log(i.prefix + i.width + "x" + i.height + i.suffix);
                        VENUE_PHOTO_URLS[index] = i.prefix + i.width + "x" + i.height + i.suffix;
                    }
                }
            }
        }
    }

    IEnumerator GetFoursquareSpecificvenueData()
    {

        string specificvenue_url = "https://api.foursquare.com/v2/venues/" + VENUE_ID + "?v=" + version + "&client_id=" + FOURSQUARE_CLIENT_ID + "&client_secret=" + FOURSQUARE_CLIENT_SECRET;

        Debug.Log("this is Specificvenue url: " + specificvenue_url);
        using (UnityWebRequest req = UnityWebRequest.Get(specificvenue_url))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError)
            {
                Debug.Log(req.error);
            }
            
            else
            {
                var d = JsonUtility.FromJson<FoursquareSpecificvenueAPI.RootObject>(req.downloadHandler.text);

                FoursquareSpecificvenueAPI.Venue venue = d.response.venue;

                Debug.Log("This is the venue name: " + venue.name);
                Debug.Log("This is the phone number, Twitter, Instragram, and Facebook: " + venue.contact.formattedPhone
                            + venue.contact.twitter + venue.contact.instagram + venue.contact.facebookUsername);

                Debug.Log("This is the formatted address:");
                foreach (string str in venue.location.formattedAddress)
                    Debug.Log(str);
                

                Debug.Log("This is the venue's address: " + venue.url);
                Debug.Log("This is the rating out of 10: " + venue.rating);
                Debug.Log("This is a small description of the venue: " + venue.description);

                foreach (FoursquareSpecificvenueAPI.Timeframes tf in venue.hours.timeframes)
                {
                    Debug.Log("These are the days the venue is open: " + tf.days);
                    foreach (FoursquareSpecificvenueAPI.Open op in tf.open)
                    {
                        Debug.Log("These are the hours the venue is open: " + op.renderedTime);
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update ()
    {
	    if (location_selected && location_lock)
        {
            location_lock = false;
            StartCoroutine(GetFoursquareVenueData());
        }
        if (venue_selected && venue_lock)
        {
            venue_lock = false;
            int venue_index = VENUE_NUM_CHOICE - 1;
            VENUE_ID = VALID_VENUE_IDS[0];
            StartCoroutine(GetFoursquareSpecificvenueData());
        }
	}
}
