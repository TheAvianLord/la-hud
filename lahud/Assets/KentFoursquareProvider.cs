using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KentFoursquareProvider : MonoBehaviour
{
    public string FOURSQUARE_CLIENT_ID;
    public string FOURSQUARE_CLIENT_SECRET;
    public string NEAR_LOCATION;
    public string version;
    public string VENUE_ID;
    public bool location_selected;
    bool location_lock;
    public bool venue_selected;
    public bool RESET_ALL;


    public int VENUE_NUM_CHOICE;
    public string[] VALID_VENUE_IDS;
    public string[] VENUE_VARIETY;
    public string[] VENUE_PHOTO_URLS;
    public string[] SPECIFIC_VENUE_INFO;

    // Use this for initialization
    void Start()
    {
        RESET_ALL = false;
        FOURSQUARE_CLIENT_ID = "1JBOAMSLJ2PQ11PL0PL2XAZ0FLTMBIZLMTVQT1BJZ2XPU2PP";
        FOURSQUARE_CLIENT_SECRET = "LXA4PSHWDADNO3344HRBWJ0USVJNZV045RJ0YRPL4WQQVLLR";
        NEAR_LOCATION = "Cerritos,CA";
        version = "20161016";
        location_selected = false;
        location_lock = true;

        venue_selected = false;

        VENUE_NUM_CHOICE = 1;
        VALID_VENUE_IDS = new string[5];
        VENUE_VARIETY = new string[5];
        VENUE_PHOTO_URLS = new string[5];
        SPECIFIC_VENUE_INFO = new string[6];
    }
    
    //StartCoroutine(GetFoursquareVenueData());
	
    IEnumerator GetFoursquareVenueData()
    {
        int idx = 0;

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
                    string astr = "";
                    Debug.Log(v.name + ": ");
                    astr += v.name + " ";
                    foreach (FoursquareAPI.Category c in v.categories)
                    {
                        Debug.Log(c.name);
                        astr += c.name + " ";
                    }
                    VENUE_VARIETY[idx] = astr;
                    VENUE_ID = v.id;
                    VALID_VENUE_IDS[idx] = VENUE_ID;
                    idx++;
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
                SPECIFIC_VENUE_INFO[0] = venue.name;

                Debug.Log("This is the phone number, Twitter, Instragram, and Facebook: " + venue.contact.formattedPhone
                            + venue.contact.twitter + venue.contact.instagram + venue.contact.facebookUsername);
                SPECIFIC_VENUE_INFO[1] = venue.contact.formattedPhone + " " + venue.contact.twitter + " " + venue.contact.instagram
                                    + " " + venue.contact.facebookUsername;

                Debug.Log("This is the formatted address:");
                string bigAddress = "";
                foreach (string str in venue.location.formattedAddress)
                {
                    Debug.Log(str);
                    bigAddress += str + " ";
                }
                SPECIFIC_VENUE_INFO[2] = bigAddress;
                

                Debug.Log("This is the venue's address: " + venue.url);
                SPECIFIC_VENUE_INFO[3] = venue.url;
                Debug.Log("This is the rating out of 10: " + venue.rating);
                SPECIFIC_VENUE_INFO[4] = "" + venue.rating;
                Debug.Log("This is a small description of the venue: " + venue.description);
                SPECIFIC_VENUE_INFO[5] = venue.description;
                /*
                foreach (FoursquareSpecificvenueAPI.Timeframes tf in venue.hours.timeframes)
                {
                    Debug.Log("These are the days the venue is open: " + tf.days);
                    SPECIFIC_VENUE_INFO[6] = tf.days;
                    foreach (FoursquareSpecificvenueAPI.Open op in tf.open)
                    {
                        Debug.Log("These are the hours the venue is open: " + op.renderedTime);
                        SPECIFIC_VENUE_INFO[7] = op.renderedTime;
                    }
                }*/
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
        if (venue_selected)
        {
            venue_selected = false;
            int venue_index = VENUE_NUM_CHOICE - 1;
            VENUE_ID = VALID_VENUE_IDS[venue_index];
            StartCoroutine(GetFoursquareSpecificvenueData());
        }
        if (RESET_ALL)
        {
            Start();
        }

	}
}
