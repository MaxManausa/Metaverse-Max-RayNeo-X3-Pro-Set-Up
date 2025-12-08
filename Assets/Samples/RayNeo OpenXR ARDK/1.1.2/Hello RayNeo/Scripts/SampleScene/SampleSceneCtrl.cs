
using RayNeo.API;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneCtrl : MonoBehaviour
{

    public GameObject m_PlaneDetection;
    private void Awake()
    {

        if(m_PlaneDetection!=null) m_PlaneDetection.SetActive(RayNeoInfo.DeviceIsIntegratedType());
    }
    public void OnBtnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
                Application.Quit();
#endif
    }

    public void OpenBatteryInfo()
    {
        PlatformAndroid.OpenSystemMonitoring();
    }
    public void CloseBatteryInfo()
    {
        PlatformAndroid.CloseSystemMonitoring();
    }
}
