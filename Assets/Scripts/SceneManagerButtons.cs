using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerButtons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject sceneManagerObject;
    [SerializeField] GameObject head;
    [SerializeField] GameObject laserBeamDot;

    [SerializeField] GameObject whole3DOFScreen;
    [SerializeField] GameObject hud0DOFScreen;
    [SerializeField] GameObject minimalistScreen;

    public int zoomDistance;
    public int idealDistance;
    [SerializeField] TextMeshProUGUI glassesModeText1;
    [SerializeField] TextMeshProUGUI glassesModeText2;

    [SerializeField] Text text1;
    [SerializeField] Text text2;
    [SerializeField] Text text3;
    [SerializeField] Text text4;



    void Start()
    {
        Go3DOF();
        Go0DOF();
    }

    public void Go0DOF()
    {
        minimalistScreen.SetActive(false);
        whole3DOFScreen.SetActive(false);
        hud0DOFScreen.SetActive(true);
        laserBeamDot.SetActive(false);
        glassesModeText1.text = "0DOF HUD";
        glassesModeText2.text = "0DOF HUD";
    }

    public void Go3DOF()
    {
        minimalistScreen.SetActive(false);
        hud0DOFScreen.SetActive(false);
        whole3DOFScreen.SetActive(true);
        laserBeamDot.SetActive(true);
        glassesModeText1.text = "3DOF HUD";
        glassesModeText2.text = "3DOF HUD";
    }

    public void Go0and3DOF()
    {
        minimalistScreen.SetActive(false);
        hud0DOFScreen.SetActive(true);
        whole3DOFScreen.SetActive(true);
        glassesModeText1.text = "0DOF & 3DOF HUD";
        glassesModeText2.text = "0DOF & 3DOF HUD";
    }

    public void minimalistScreenOnOff()
    {
        hud0DOFScreen.SetActive(false);
        whole3DOFScreen.SetActive(false);
        if (!minimalistScreen.activeSelf)
        {
            whole3DOFScreen.SetActive(false);
            minimalistScreen.SetActive(true);
        }
        else
        {
            whole3DOFScreen.SetActive(true);
            minimalistScreen.SetActive(false);
        }
            
    }

    

    public void Zoom()
    {
        if (zoomDistance == 0)
        {
            zoomDistance = idealDistance;
            sceneManagerObject.transform.position = new Vector3(0, 0, zoomDistance);
            Debug.Log("You zoomed out!");
            return;
        }
        else
        {
            zoomDistance = 0;
            sceneManagerObject.transform.position = new Vector3(0, 0, zoomDistance);
            Debug.Log("You zoomed in!");
        }
    }

    public void AboutRayNeoX3Pro()
    {
        text1.text = "Feature";
        text2.text = "Specification";
        text3.text = "Processor/Chipset\r\nOptical Engine\r\nDisplay Type\r\nDisplay Brightness\r\nField of View (FOV)\r\nRAM\r\nStorage" +
            " (ROM)\r\nBattery Capacity\r\nWeight\r\nFrame Material\r\nSpatial Sensing\r\nCamera\r\nConnectivity\r\nIP Rating\r\nSoftware ";
        text4.text = "Qualcomm Snapdragon AR1 Generation 1 platform\r\nSelf-developed Micro-LED Light Engine with Waveguide Technology\r\n Binocular" +
            "Full-Color Display\r\nUp to 2,500 nits (peak brightness)\r\nApproximately 30 degrees\r\n4GB\r\n32GB\r\n245mAh\r\nApproximately 76g " + 
            "(less than 3 ounces)\r\nAerospace-grade magnesium alloy frame and titanium alloy hinge\r\nRayNeo Imaging Plus system (6DOF tracking, scene" +
            " detection, gesture recognition)\r\nDual-camera system (estimated 12MP for photography/video)\r\nEnhanced Connectivity (Seamless link with other " +
            "devices and cloud)\r\nIPx2 (Water-resistant)\r\nRayNeoAIOS with integrated Google Gemini AI ";
    }

    public void AboutThisApp()
    {
        text1.text = "";
        text2.text = "This app was made by Max Manausa (Metaverse Max) for RayNeo's CES 2026 booth";
        text3.text = "";
        text4.text = "Check out this app! Pretty cool, right? This prototype is a 3DOF, 3 degrees of head rotation freedom, experience where you can view and access 9 unique HUDs. A lot more is coming! Be excited!\r\n\r\nDouble Tap the right side of the glasses to reset the screen, select buttons with a Single Tap on the right side of the glasses, and press the \"Exit App\" button to leave the app, or hold the right side of the glasses down until the system menu opens then you can just click the home button.";

    }

    public void AboutCES()
    {
        text1.text = "CES 2026";
        text2.text = "Welcome to CES 2026! ";
        text3.text = "I haven't put anything here yet";
        text4.text = "CES 2026 is the massive global tech event happening January 6-9, 2026, in Las Vegas, featuring innovations in AI, automotive tech, digital health, robotics, and more, with major announcements from big brands and startups shaping the future of consumer electronics, offering networking and key insights for tech professionals. \r\n\r\nExpect focus on AI PCs, foldables, sustainability tech, and digital transformation across industries.\r\n\r\nExpected Tech Trends & Focus Areas\r\nArtificial Intelligence (AI): AI PCs with dedicated Neural Processing Units (NPUs) for on-device AI, smarter co-pilots, and enhanced privacy.\r\nMobility: Autonomous vehicles, electric vehicles (EVs), and advanced mobility solutions.\r\nFuture of Computing: Foldable/rollable devices (phones, laptops), transparent displays, and new chipsets.\r\nDigital Transformation: AI, robotics, digital health, cybersecurity, and sustainable solutions across industries like manufacturing, retail, and energy.";
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
