using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;

    private Vector3 mouseWorldCord;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //convert from screen to world cordinates
        mouseWorldCord = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //set z cordinate to .... z location
        mouseWorldCord.z = 1f;

        //update the transform to the mouse psotion
        transform.position = mouseWorldCord;

        Debug.Log(mouseWorldCord);
    }
}
