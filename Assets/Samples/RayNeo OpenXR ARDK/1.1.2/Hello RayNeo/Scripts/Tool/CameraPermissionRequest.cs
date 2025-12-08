using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPermissionRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PermissionUtil.TryQueryPermission(UnityEngine.Android.Permission.Camera);
    }

}
