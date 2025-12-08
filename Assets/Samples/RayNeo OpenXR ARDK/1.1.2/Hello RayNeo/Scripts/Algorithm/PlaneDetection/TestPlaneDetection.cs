using RayNeo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.rayneo.xr.extensions;
using RayNeo.API;
using FfalconXR;

public class TestPlaneDetection : MonoBehaviour
{

    XRPlaneInfo[] m_infoArrays = new XRPlaneInfo[3];

    private List<GameObject> m_planeObjs = new List<GameObject>();

    public Text m_tips;

    public Material m_m;

    public Text m_PlaneResult;

    private void Awake()
    {
#if UNITY_EDITOR
        m_tips.text = "请在眼镜端执行.";
#else
        Algorithm.EnableSlamHeadTracker();
        Algorithm.EnablePlaneDetection();
#endif
        for (int i = 0; i < m_infoArrays.Length; i++)
        {
            var go = new GameObject("Plane" + i);
            m_planeObjs.Add(go);
        }
    }



    private void Update()
    {

#if UNITY_EDITOR
        //以下是编辑器中的测试代码
        int res = 1;
        m_infoArrays[0].local_polygon = new float[] { 2f, 2f, -2f, 2f, -2f, -2f, 2f, -2f };
        Vector3 pos = new Vector3(-0.24f, -2.00f, 0.06f);
        m_infoArrays[0].local_polygon_size = 4;
        m_infoArrays[0].pose.position.x = pos.x;
        m_infoArrays[0].pose.position.y = pos.y;
        m_infoArrays[0].pose.position.z = pos.z;
        var q = new Quaternion(0.00000f, -0.64147f, 0.00000f, 0.76714f);
        m_infoArrays[0].pose.rotation.x = q.x;
        m_infoArrays[0].pose.rotation.y = q.y;
        m_infoArrays[0].pose.rotation.z = q.z;
        m_infoArrays[0].pose.rotation.w = q.w;

#else
        int res = Algorithm.GetPlaneInfo(m_infoArrays);
 
#endif

        m_PlaneResult.text = "当前平面数量:" + res;
        Log.Debug("TestPlaneDetection获取平面信息---z序列:" + res + ":" + m_infoArrays.Length);
        for (int i = 0; i < m_infoArrays.Length; i++)
        {
            if (i < res)
            {
                m_planeObjs[i].SetActive(true);
                XRPlaneInfo info = m_infoArrays[i];
                Log.Debug("TestPlaneDetection 开始创建模型:" + info.local_polygon.Length + ":" + info.local_polygon_size);

                GameObject obj = Algorithm.CreatePlaneMesh(info, m_planeObjs[i], false, m_m);
                obj.transform.localPosition = Algorithm.ConvertPlanePosition(info);
                obj.transform.localRotation = Algorithm.ConvertPlaneRotation(info);
                var mesh = obj.GetComponent<MeshFilter>().mesh;

                Log.Debug("TestPlaneDetection 模型创建完毕:" + obj.transform.localPosition + ":" + obj.transform.localRotation + ":" + obj.transform.localRotation.eulerAngles);

            }
            else
            {
                m_planeObjs[i].SetActive(false);
            }
        }
        Log.Debug("当前相机朝向:" + Camera.main.transform.rotation + "  e:" + Camera.main.transform.rotation.eulerAngles + "  相机位置:" + Camera.main.transform.position);



    }

    private void OnDestroy()
    {
#if UNITY_EDITOR

#else
        Algorithm.DisablePlaneDetection();
        Algorithm.DisableSlamHeadTracker();

#endif

    }


}
