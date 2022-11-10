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
    //direction to move
    public bool movingLeft;

    //the movement speed of the sprite
    public float lerpSpeed = 1;
    //used to update the sprites current location
    bool initLerp;
    //another spead variable i guess??
    float baseSpeed; 
    //location to move to
    Vector3 targetPos;
    //how high up cat can move
    int heightMoveUp =3;

    //location of sprite
    Vector3 startPos;
    float time;

    //refrence to sprite
    public SpriteRenderer catSprite;

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

        catSprite.flipX = !movingLeft;
    }

    void Pathfind(){
        targetX = currNode.x;
        targetY = currNode.y;

        bool downIsAir = IsAir(targetX, targetY -1);
        bool fowardIsAir = IsAir(targetX, targetY);

        //if node below sprite is air fall
        if(downIsAir){
            targetX = currNode.x;
            targetY -=1;
        }
        else{
            //if node infront of sprite is air move foward
            if(fowardIsAir){
                targetX = (movingLeft) ? targetX-1 : targetX +1;
                targetY = currNode.y;
            }
            else{
                int moveUP = 0;
                bool isValid = false;
                while(moveUP < heightMoveUp){
                    moveUP++;
                    fowardIsAir = IsAir(targetX, targetY + moveUP); //might need to change variable fowardIsAir
                    if(fowardIsAir){
                        isValid = true;
                        break;
                    }
                }
                if(isValid){
                    targetY += moveUP;
                }
                else{
                    //move other direction
                    movingLeft = !movingLeft;
                    targetX = (movingLeft) ? targetX-1 : targetY +1;
                }

            }
        }

        targetNode = gameManager.GetNode(targetX, targetY);
    }

    //check if node location is null/air
    bool IsAir(int x, int y){
        Node node = gameManager.GetNode(x, y);
        if(node == null){
            return(true);
        }
        return(node.isEmpty);
    }
}
