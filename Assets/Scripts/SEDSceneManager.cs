using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject GameUIScreen;
    [SerializeField] private GameObject Game3DOFScene;
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private GameObject laserPointer;

    public bool gamePlaying = false;
    public bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        HomeScreen.SetActive(true);
        GameUIScreen.SetActive(false);
        Game3DOFScene.SetActive(false);
        gamePlaying = false;
        gamePaused = false;
        laserPointer.SetActive(false);
    }


    public void StartGame()
    {
        HomeScreen.SetActive(false);
        GameUIScreen.SetActive(true);
        Game3DOFScene.SetActive(true);
        gamePlaying = true;
        gamePaused = false;
        laserPointer.SetActive(true);
    }

    public void PauseGame()
    {
        if (!HomeScreen.activeInHierarchy && GameUIScreen.activeInHierarchy && Game3DOFScene.activeInHierarchy)
        {
            gamePaused = true;
            gamePlaying = false;
            laserPointer.SetActive(false);
            PauseScreen.SetActive(true);
            //game scene and game ui screen need to be paused
        }
    }

    public void ContinueGame()
    {
        if(PauseScreen.activeInHierarchy)
        {
            gamePaused = false;
            gamePlaying = true;
            laserPointer.SetActive(true);
            PauseScreen.SetActive(false);
            //game scene and ui screen unpaused
        }
    }

    public void GoHome ()
    {
        HomeScreen.SetActive(true);
        GameUIScreen.SetActive(false);
        Game3DOFScene.SetActive(false);
        PauseScreen.SetActive(false);
        gamePaused = false;
        gamePlaying = false;
        laserPointer.SetActive(false);
    }

    
}
