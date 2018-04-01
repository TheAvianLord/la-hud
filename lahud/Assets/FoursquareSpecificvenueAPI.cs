using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FoursquareSpecificvenueAPI
{
    [System.Serializable]
    public class Meta
    {
        public int code;
        public string requestId;
    }

    [System.Serializable]
    public class Response
    {
        public Venue venue;
    }

    [System.Serializable]
    public class Venue
    {
        public string name;
        public Contact contact;
        public Location location;
        public string url;
        public double rating;
        public string description;
        public Hours hours;
    }

    [System.Serializable]
    public class Contact
    {
        public string formattedPhone;
        public string twitter;
        public string instagram;
        public string facebookUsername;
    }


    [System.Serializable]
    public class Location
    {
        public List<string> formattedAddress;
    }

    [System.Serializable]
    public class Hours
    {
        public List<Timeframes> timeframes;
    }

    [System.Serializable]
    public class Timeframes
    {
        public string days;
        public List<Open> open;
    }

    [System.Serializable]
    public class Open
    {
        public string renderedTime;
    }

    [System.Serializable]
    public class RootObject
    {
        public Meta meta;
        public Response response;
    }

}