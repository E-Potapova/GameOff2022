using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //add to scnene manager in unity
        //on play button, go to script section of button add added this script and call play game
        //load are next level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Load First Level");
    }

    public void QuitGame(){
        Debug.Log("Game has been Quit");
        Application.Quit();

    }

}
