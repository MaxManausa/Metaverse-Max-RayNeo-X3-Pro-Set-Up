using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkStatus : MonoBehaviour
{
    private TextMeshProUGUI networkText;

    void Start()
    {
        networkText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("UpdateNetworkStatus", 0f, 5f); // Check every 5 seconds
    }

    void UpdateNetworkStatus()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                networkText.text = "Offline";
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                networkText.text = "Data/Cellular";
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                networkText.text = "WiFi";
                break;
        }
    }
}