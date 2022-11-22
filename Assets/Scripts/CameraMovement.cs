using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //camera transform
    public Transform cameraTransform;

    //camera cordinates
    #region cameraCordinates
    float cameraX = 0;
    float cameraY = 0;
    float cameraZ = 0;
    #endregion

    #region cameraBounds
    //restrict camera movement bounds
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    #endregion

    // how fast to move camera
    public float moveSpeed =1;


    // Start is called before the first frame update
    void Start()
    {
        //cameraTransform = GetComponent<Transform>();
        //cameraP = GetComponent<Camera>();

        //Debug.Log(cameraP);
        //Debug.Log(cameraTransform);
    }

    // Update is called once per frame
    void Update()
    {
        //horizontial camera movement
        float hMove = Input.GetAxis("Horizontal");
        Vector3 mp = Vector3.zero;
        mp.x = hMove * moveSpeed;

        Vector3 targetPosition = cameraTransform.position;
        //vertical camera movement
        float vMove = Input.GetAxis("Vertical");
        mp.y = vMove * moveSpeed;

        targetPosition += mp;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX + 0.01f);

        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY + 0.01f);

        cameraTransform.position = targetPosition;

    }
}
