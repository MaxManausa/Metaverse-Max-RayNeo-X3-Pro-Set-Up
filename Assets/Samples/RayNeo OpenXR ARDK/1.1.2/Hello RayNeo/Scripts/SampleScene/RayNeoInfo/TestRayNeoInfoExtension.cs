using RayNeo.API;
using UnityEngine;
using UnityEngine.UI;

public class TestRayNeoInfoExtension : MonoBehaviour
{
    public Text DeviceNameText;

    private void Start()
    {
        DeviceNameText.text = RayNeoInfoExtension.GetDeviceName();
    }
}
