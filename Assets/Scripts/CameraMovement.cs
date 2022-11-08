using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //camera transform
    Transform cameraTransform;

    //camera component
    Camera cameraP;

    //camera cordinates
    float cameraX = 0;
    float cameraY =0;
    float cameraZ =0;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        cameraP = GetComponent<Camera>();

        Debug.Log(cameraP);
        Debug.Log(cameraTransform);
        
    }

    // Update is called once per frame
    void Update()
    {
        c   
    }
}
