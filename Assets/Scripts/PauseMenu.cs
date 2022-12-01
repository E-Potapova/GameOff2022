using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
           // AudioManager.UnPause();
            Destroy(gameObject);
           
        }
    }
}
