using RayNeo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoubleTabBackSceneCtrl : MonoBehaviour
{

    public void OnDoubleTapCallBack()
    {
        SceneManager.LoadScene("Entry");
    }

    void Start()
    {
        SimpleTouchForLite.Instance.OnDoubleTap.AddListener(OnDoubleTapCallBack);
    }

    private void OnDestroy()
    {
        if (SimpleTouchForLite.SingletonExist)
        {
            SimpleTouchForLite.Instance.OnDoubleTap.RemoveListener(OnDoubleTapCallBack);
        }

    }
}
