using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // public Animator transition;

    // public float transitionTime = 1f;

    // //
    // public static LevelLoader singleton;
    // void Awake(){
    //     singleton = this;
    // }
    //just call this where we win the levels
    public static void LoadNextLevel(){
        //automate it so it loads next level
        //StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // //give time for animation to play, develop a co-routine
    // IEnumerator LoadLevel(int levelIndex){
    //     //play animation
    //     transition.SetTrigger("Start");

    //     //wait
    //     yield return new WaitForSeconds(transitionTime);

    //     //load scene
    //     SceneManager.LoadScene(levelIndex);
    // }
}
