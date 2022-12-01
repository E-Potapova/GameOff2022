using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : MonoBehaviour
{
    private Animator animator;

    private void Update()
    {
        UIManager.singleton.Tick();    
    }

    public void StartGame()
    {
        Debug.Log("Load First Level");
        LevelLoader.LoadNextLevel();

        //instatiate new obj
        //controls screen
    }

    // Update is called once per frame
    public void Quit()
    {
        Debug.Log("Game has been Quit");
        Application.Quit();
    }

    public void LogoClick()
    {
        if (!animator)
        {
            animator = GameObject.Find("Logo").GetComponent<Animator>();
        }
        animator.SetTrigger("click");
        AudioManager.instance.PlayRandMeow();
    }
}
