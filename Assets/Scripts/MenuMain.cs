using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : MonoBehaviour
{
    private void Update()
    {
        UIManager.singleton.Tick();    
    }

    public void StartGame()
    {
        Debug.Log("Load First Level");
        LevelLoader.LoadNextLevel();
    }

    // Update is called once per frame
    public void Quit()
    {
        Debug.Log("Game has been Quit");
        Application.Quit();
    }
}
