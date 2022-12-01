using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
     // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        //AudioManager.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
           // AudioManager.UnPause();
            Destroy(gameObject);
           
        }
    }

    public void Retry()
    {
        LevelLoader.Reload();
    }

    public void NextLevel()
    {
        LevelLoader.LoadNextLevel();
    }

    public void Quit()
    {
        LevelLoader.LoadMainMenu();
    }

    public void DisplayWinText()
    {
        GameObject.Find("Button - Next").GetComponent<Button>().interactable = true;
        GameObject.Find("Text - You win!").GetComponent<Text>().enabled = true;
        GameObject.Find("Text - You win! Subtext").GetComponent<Text>().enabled = true;
    }

    public void DisplayLoseText()
    {
        GameObject.Find("Text - You lose :(").GetComponent<Text>().enabled = true;
        GameObject.Find("Text - You lose :( Subtext").GetComponent<Text>().enabled = true;
    }
}
