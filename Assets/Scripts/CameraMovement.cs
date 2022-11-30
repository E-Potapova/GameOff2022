using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;
    private Transform cameraTransform;

    #region cameraBounds
    //restrict camera movement bounds
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float minZoom;
    public float maxZoom;
    #endregion

    // how fast to move camera
    public float moveSpeed = 0;

    public float zoomSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cameraTransform = GetComponent<Transform>();
        moveSpeed = moveSpeed /100;
    }

    // Update is called once per frame
    void Update()
    {
        //zoom scroll wheel
        if(Input.mouseScrollDelta.y > 0){
            //zoom out
            cam.orthographicSize -= zoomSpeed * Time.deltaTime; //might need to add smoothing
        }
        if(Input.mouseScrollDelta.y < 0){
            //zoom in
            cam.orthographicSize += zoomSpeed * Time.deltaTime;
        }
        //resrict min and max zoom size
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);


        Vector3 targetPosition = cameraTransform.position;
        //horizontial camera movement
        float hMove = Input.GetAxis("Horizontal") * moveSpeed * (cam.orthographicSize / 5);
        targetPosition.x += hMove;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);


        //vertical camera movement
        float vMove = Input.GetAxis("Vertical") * moveSpeed * (cam.orthographicSize / 5);
        targetPosition.y += vMove;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        cameraTransform.position = targetPosition;
    }
}
