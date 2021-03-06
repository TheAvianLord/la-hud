﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FoursquarePictureinfoAPI
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
        public Photo photos;
    }

    [System.Serializable]
    public class Photo
    {
        public int count;
        public List<Item> items;
        public int dupesRemoved;
    }

    [System.Serializable]
    public class Item
    {
        public string id;
        public int createdAt;
        public Source source;
        public string prefix;
        public string suffix;
        public int width;
        public int height;
        public User user;
        public Checkin checkin;
        public string visibility;
    }

    [System.Serializable]
    public class Source
    {
        public string name;
        public string url;
    }

    [System.Serializable]
    public class User
    {
        public string id;
        public string firstName;
        public string lastName;
        public string gender;
    }

    [System.Serializable]
    public class Checkin
    {
        public string id;
        public string createdAt;
        public string type;
        public int timeZoneOffset;

    }

    [System.Serializable]
    public class RootObject
    {
        public Meta meta;
        public Response response;
    }

}