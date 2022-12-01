using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{
     // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
            //changhe to button to go to next;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 1;
            LevelLoader.LoadNextLevel();
            
            //Destroy(gameObject);
        }
    }
}
