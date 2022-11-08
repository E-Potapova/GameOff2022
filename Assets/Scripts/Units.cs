using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//How it works
//Unit must be within the image
//checks all the pixels, if the pixel/node is empty it can move to that spot
//otherwise it treats filled in pixels/nodes as a collision
public class Units : MonoBehaviour
{

    bool isInit;

    //current node of sprite
    Node currNode;
    //location of node where sprite wants to move
    Node targetNode;

    //can disable this later
    //boolean function to allow the sprite to move
    public bool move;

    //the movement speed of the sprite
    public float lerpSpeed = 1;

    //used to update the sprites current location
    bool initLerp;
    //another spead variable i guess??
    float baseSpeed;
    
    //location to move to
    Vector3 targetPos;

    //location of sprite
    Vector3 startPos;
    float time;
    public bool movingLeft;

    GameManager gameManager;

    //target location sprite wants to move to
    int targetX;
    int targetY;

    public void Init(GameManager gm){
        gameManager = gm;
        PlaceOnNode();
        isInit = true;
    }

    //spawn units
    void PlaceOnNode(){
        currNode = gameManager.spawnNode;
        transform.position = gameManager.spawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInit){
            return;
        }
        // if(currNode == null){
        //     return;
        // }
        if(!move){
            return;
        }

        if(!initLerp){
            initLerp = true;
            startPos = transform.position;
            time = 0;
            Pathfind();
            Vector3 tp = gameManager.GetWorldPosFromNode(targetNode);
            targetPos = tp;
            float d = Vector3.Distance(targetPos, startPos);
            baseSpeed = lerpSpeed / d;
        }
        else{
            time += Time.deltaTime * baseSpeed;
            if(time > 1){
                time = 1;
                initLerp = false;  
                currNode = targetNode;
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, time);
            transform.position = tp;
        }
    }

    void Pathfind(){
        targetX = currNode.x;
        targetY = currNode.y;

        Node nextDown = gameManager.GetNode(targetX, targetY-1);
        
        if(nextDown == null){
            return;
        }
         
        if(!nextDown.isEmpty){
           targetY = currNode.y;
        }
        else{
            targetY -=1;
        }

        targetNode = gameManager.GetNode(targetX, targetY);
    }
}
