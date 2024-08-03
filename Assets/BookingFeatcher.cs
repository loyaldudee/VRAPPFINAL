using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class BookingFetcher : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public string bookingKey = "R7PH5B"; // Example booking key

    public BookingData bookingData;

    void Start()
    {
        StartCoroutine(FetchBookingByKey());
    }

    IEnumerator FetchBookingByKey()
    {
        string url = baseURL + "/slots/bookings/key/" + bookingKey;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Booking fetch successful: " + request.downloadHandler.text);
            BookingResponse response = JsonUtility.FromJson<BookingResponse>(request.downloadHandler.text);

            if (response.booking != null)
            {
                bookingData = response.booking;
                Debug.Log("Booking Key: " + response.booking.key);
                foreach (var item in response.booking.watchlist)
                {
                    Debug.Log("Property Name: " + item.propertyName);
                    Debug.Log("Parent Property Name: " + item.parentPropertyName);
                    Debug.Log("Organisation Name: " + item.organisationName);
                    Debug.Log("Date: " + item.date);
                    Debug.Log("Time: " + item.time);
                    Debug.Log("Image URL: " + item.imageURL);
                }
            }
            else
            {
                Debug.LogWarning("No booking found with this key.");
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
        }
    }

    [System.Serializable]
    public class WatchlistItem
    {
        public string propertyName;
        public string parentPropertyName;
        public string organisationName;
        public string date;
        public string time;
        public string imageURL;
    }

    [System.Serializable]
    public class BookingData
    {
        public string key;
        public List<WatchlistItem> watchlist;
    }

    [System.Serializable]
    public class BookingResponse
    {
        public string message;
        public BookingData booking;
    }
}