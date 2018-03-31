using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FourSquareProvider : MonoBehaviour {

    public string FOURSQUARE_CLIENT_ID = "FACG5HNH254GMRL3GRXQM4DIG5BI50IOMRZVIXFJ434ENDME";
    public string FOURSQUARE_CLIENT_SECRET = "B5WH5CRHSXFY1TPS4V1IXWCFVWECUYEVNAWKJXLTPBAHSWUE";
    public string City = "Cerritos";
    public string State = "CA";

    void Start(){
        Debug.Log("Im Running");
        StartCoroutine(GetFoursquareVenueData());
    }

    IEnumerator GetFoursquareVenueData() {
        // string Query = "food";
        //string FOURSQUARE_CLIENT_ID = "FACG5HNH254GMRL3GRXQM4DIG5BI50IOMRZVIXFJ434ENDME";
        //string FOURSQUARE_CLIENT_SECRET = "B5WH5CRHSXFY1TPS4V1IXWCFVWECUYEVNAWKJXLTPBAHSWUE";
        //string City = "Cerritos";
       // string State = "CA";
        string url = "https://api.foursquare.com/v2/venues/search?v=20161016&near=" + City + "," + State + "&intent=browse&radius=1000&client_id=" + FOURSQUARE_CLIENT_ID + "&client_secret=" + FOURSQUARE_CLIENT_SECRET;
        Debug.Log(url);
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {

                var d = JsonUtility.FromJson<FoursquareAPI.RootObject>(www.downloadHandler.text);

                List<FoursquareAPI.Venue> venues = d.response.venues;

                foreach (FoursquareAPI.Venue v in venues)
                {
                    double lat = v.location.lat;
                    double lng = v.location.lng;
                    Debug.Log(lat + "," + lng);
                }
            }
        }
    }

public class FoursquareAPI {
    [System.Serializable]
    public class Meta
    {
        public int code;
        public string requestId;
    }

    [System.Serializable]
    public class Contact
    {
        public string phone;
        public string formattedPhone;
        public string twitter;
        public string facebook;
        public string facebookUsername;
        public string facebookName;
        public string instagram;
    }

    [System.Serializable]
    public class LabeledLatLng
    {
        public string label;
        public double lat;
        public double lng;
    }

    [System.Serializable]
    public class Location
    {
        public string address;
        public string crossStreet;
        public double lat;
        public double lng;
        public int distance;
        public string postalCode;
        public string cc;
        public string city;
        public string state;
        public string country;
        public List<string> formattedAddress;
        public List<LabeledLatLng> labeledLatLngs;
    }

    [System.Serializable]
    public class Icon
    {
        public string prefix;
        public string suffix;
    }

    [System.Serializable]
    public class Category
    {
        public string id;
        public string name;
        public string pluralName;
        public string shortName;
        public Icon icon;
        public bool primary;
    }

    [System.Serializable]
    public class Stats
    {
        public int checkinsCount;
        public int usersCount;
        public int tipCount;
    }

    [System.Serializable]
    public class Menu
    {
        public string type;
        public string label;
        public string anchor;
        public string url;
        public string mobileUrl;
        public string externalUrl;
    }

    [System.Serializable]
    public class BeenHere
    {
        public int lastCheckinExpiredAt;
    }

    [System.Serializable]
    public class Specials
    {
        public int count;
        public List<object> items;
    }

    [System.Serializable]
    public class HereNow
    {
        public int count;
        public string summary;
        public List<object> groups;
    }

    [System.Serializable]
    public class VenuePage
    {
        public string id;
    }

    [System.Serializable]
    public class Provider
    {
        public string name;
    }

    [System.Serializable]
    public class Delivery
    {
        public string id;
        public string url;
        public Provider provider;
    }

    [System.Serializable]
    public class Venue
    {
        public string id;
        public string name;
        public Contact contact;
        public Location location;
        public List<Category> categories;
        public bool verified;
        public Stats stats;
        public string url;
        public bool hasMenu;
        public Menu menu;
        public BeenHere beenHere;
        public Specials specials;
        public HereNow hereNow;
        public string referralId;
        public List<object> venueChains;
        public bool hasPerk;
        public bool? venueRatingBlacklisted;
        public bool? allowMenuUrlEdit;
        public VenuePage venuePage;
        public Delivery delivery;
    }

    [System.Serializable]
    public class Response
    {
        public List<Venue> venues;
        public bool confident;
    }

    [System.Serializable]
    public class RootObject
    {
        public Meta meta;
        public Response response;
    }

}

// Update is called once per frame
void Update () {
		
	}
}
