using RayNeo.API;
using UnityEngine;
using UnityEngine.UI;
using static com.rayneo.xr.extensions.XRCamera;

public class ShareCameraCtrl : MonoBehaviour
{
    #region Data

    /// <summary>
    /// TAG
    /// </summary>
    const string TAG = "[ShareCameraCtrl] ";

    /// <summary>
    /// UI 
    /// </summary>
    public RawImage m_RI;
    public Button StartPreviewButton;
    public Button StopPreviewButton;

    /// <summary>
    /// XRCameraHandler
    /// </summary>
    private XRCameraHandler m_CameraHandler;

    /// <summary>
    /// 当前 XRCameraType
    /// </summary>
    private XRCameraType m_CurXRCameraType = XRCameraType.RGB;
    #endregion

    #region LifeCycle Function

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {

        StartPreviewButton.onClick.AddListener(StartPreview); 
        StopPreviewButton.onClick.AddListener(StopPreview); 

        PrintCameraSupportResolutions(m_CurXRCameraType);
    }

    /// <summary>
    /// OnDestroy
    /// </summary>
    private void OnDestroy()
    {
        StartPreviewButton.onClick.RemoveListener(StartPreview);
        StopPreviewButton.onClick.RemoveListener(StopPreview);
    }

    #endregion


    #region Logic Function

    /// <summary>
    /// 开启预览
    /// </summary>
    private void StartPreview()
    {
        if (m_CameraHandler != null)
        {
            return;
        }
        m_CameraHandler = ShareCamera.OpenCamera(m_CurXRCameraType, m_RI);
    }

    /// <summary>
    /// 停止预览
    /// </summary>
    private void StopPreview()
    {
        if(m_CameraHandler!=null) ShareCamera.CloseCamera(m_CameraHandler);
        m_CameraHandler = null;
    }

    /// <summary>
    /// 打印对应支持的分辨率
    /// </summary>
    /// <param name="cameraTpye"></param>
    private void PrintCameraSupportResolutions(XRCameraType cameraTpye) {
        XRResolution[] rlt = ShareCamera.getSupportResolutions(cameraTpye);
        Debug.LogWarning(TAG+ "PrintCameraSupportResolutions()");
        foreach (XRResolution r in rlt)
        {
            Debug.LogWarning(TAG + $"PrintCameraSupportResolutions() width={r.width}, height={r.height}");
        }
    }
    #endregion

}
