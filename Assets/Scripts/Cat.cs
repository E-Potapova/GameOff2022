using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// How it works
// Cat must be within the image
// checks all the pixels, if the pixel/node is empty it can move to that spot
// otherwise it treats filled in pixels/nodes as a collision
public class Cat : MonoBehaviour
{
    bool isInit;
    // current node of sprite
    Node currNode;
    // location of node where sprite wants to move
    Node targetNode;

    // can disable this later
    // boolean function to allow the sprite to move
    public bool move;
    // direction to move
    public bool movingLeft;

    // the movement speed of the sprite
    public float lerpSpeed = 1;
    // used to update the sprites current location
    bool initLerp;
    // another speed variable i guess??
    float baseSpeed; 
    // fall speed
    float fallSpeed = 5;
    // check if on ground
    bool onGround;
    // location to move to
    Vector3 targetPos;
    // how high up cat can move
    int heightMoveUp =3;

    //set cat current ability
    public CatManager.Ability currAbility;

    // location of sprite
    Vector3 startPos;
    float time;

    // reference to sprite
    public SpriteRenderer catSprite;

    GameManager gameManager;

    // target location sprite wants to move to
    int targetX;
    int targetY;

    //keeptrack of all stop nodes
    List<Node> stopNodes = new List<Node>();

    public void Init(GameManager gm) {
        gameManager = gm;
        PlaceOnNode();
        isInit = true;
        currAbility = CatManager.Ability.defaultWalk;
    }

    // spawn cats
    void PlaceOnNode() {
        currNode = gameManager.spawnNode;
        transform.position = gameManager.spawnPosition;
    }

    // Update is called once per frame
    // no longer is an update change to a public void tick
    public void Tick(float delta) {
        if (!isInit) {
            return;
        }
        if (!move)
        {
            return;
        }
        catSprite.flipX = !movingLeft;

        //call method for correct cat ability
        switch(currAbility){
            case CatManager.Ability.defaultWalk:
                Walk(delta);
                break;
            case CatManager.Ability.stopper:
                Stopper();
                break;
            case CatManager.Ability.umbrella:

                break;
            case CatManager.Ability.digFoward:

                break;
            case CatManager.Ability.digDown:

                break;
            default:
                //default could just be set to walk
                break;
        }

    }

    //functions for diffrent cat abilities
    void Walk(float delta){
          if (!initLerp) {
            initLerp = true;
            startPos = transform.position;
            time = 0;
            Pathfind();
            Vector3 tp = gameManager.GetWorldPosFromNode(targetNode);
            targetPos = tp;
            float d = Vector3.Distance(targetPos, startPos);
            if (onGround) {
                baseSpeed = lerpSpeed / d;
            }
            else {
                baseSpeed = fallSpeed / d;
            }
            
        }
        else {
            time += delta * baseSpeed;
            if (time > 1) {
                time = 1;
                initLerp = false;  
                currNode = targetNode;
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, time);
            transform.position = tp;
        }
    }

    void Stopper(){

    }

    public bool ChangeAbility(CatManager.Ability newAbility){
        //currAbility = newAbility;
        switch(newAbility){
            case CatManager.Ability.defaultWalk:
                currAbility = newAbility;
                break;
            case CatManager.Ability.stopper:
                if(onGround){
                    FindStopNodes();
                    currAbility = newAbility;
                    return(true);
                }
                else{
                    return(false);
                }
            default:
                break;
        }

        return true;
    }

    void Pathfind() {
        targetX = currNode.x;
        targetY = currNode.y;

        bool downIsAir = IsAir(targetX, targetY -1);
        bool fowardIsAir = IsAir(targetX, targetY);

        //if node below sprite is air fall
        if (downIsAir) {
            targetX = currNode.x;
            targetY -=1;
            onGround = false;
        }
        else {
            //if node infront of sprite is air move foward
            onGround = true;
            
            bool stop = IsStop((movingLeft) ? targetX -1 : targetX +1, targetY);
           
            if(stop){
                movingLeft = !movingLeft;
                targetX = (movingLeft) ? targetX -1 : targetX +1;
            }
            else{

                if (fowardIsAir) {
                    targetX = (movingLeft) ? targetX - 1 : targetX + 1;
                    targetY = currNode.y;
                }
                else {
                    int moveUP = 0;
                    bool isValid = false;
                    while (moveUP < heightMoveUp) {
                        moveUP++;
                        fowardIsAir = IsAir(targetX, targetY + moveUP); //might need to change variable fowardIsAir
                        if(fowardIsAir){
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid) {
                        targetY += moveUP;
                    }
                    else {
                        //move other direction
                        movingLeft = !movingLeft;
                        targetX = (movingLeft) ? targetX -1 : targetX +1;
                    }

                }
            }
        }

        targetNode = gameManager.GetNode(targetX, targetY);
    }

    //check if node location is null/air
    bool IsAir(int x, int y) {
        Node node = gameManager.GetNode(x, y);
        if (node == null) {
            return (true);
        }
        return (node.isEmpty);
    }

    bool IsStop(int x, int y){
        Node node = gameManager.GetNode(x, y);
        if (node == null) {
            return (false);
        }
        return (node.isStop);
    }

    void FindStopNodes(){
        for(int x = -2; x < 2; x++){//how far to check in front of lemming 
            for(int y = 0; y < 5; y++){ //needs to be higher then climb height
                Node tempNode = gameManager.GetNode(currNode.x + x, currNode.y + y);
                if( tempNode == null){
                    continue;
                }

                tempNode.isStop = true;
                stopNodes.Add(tempNode);
            }
        }
    }

    //clear stopNodes list and set all nodes back to false for stopping
    public void ClearStopNodes(){
        for(int i = 0; i < stopNodes.Count; i++){
            stopNodes[i].isStop = false;
        }
        stopNodes.Clear();
    }

}
