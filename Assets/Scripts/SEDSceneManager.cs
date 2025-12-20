using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDSceneManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject GameUIScreen;
    [SerializeField] private GameObject Game3DOFScene;
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private GameObject laserPointer;

    [SerializeField] private AsteroidSpawner asteroidSpawner;

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
        scoreManager.ResetVisuals();
    }


    public void StartGame()
    {
        HomeScreen.SetActive(false);
        GameUIScreen.SetActive(true);
        Game3DOFScene.SetActive(true);
        gamePlaying = true;
        gamePaused = false;
        laserPointer.SetActive(true);
        ClearAllUAPs();
        asteroidSpawner.StartSpawning();
        scoreManager.ResetVisuals();
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
            asteroidSpawner.StopSpawning();
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
            asteroidSpawner.StartSpawning();
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
        asteroidSpawner.StopSpawning();
        ClearAllUAPs();
    }

    public void ClearAllUAPs()
    {
        GameObject[] uaps = GameObject.FindGameObjectsWithTag("UAP");
        foreach (GameObject uap in uaps)
        {
            Destroy(uap);
        }
    }


}
