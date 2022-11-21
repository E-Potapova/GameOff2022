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

    //falling variables
    public bool falling;
    public bool isUmbrella;
    public bool isDigFoward;


    // the movement speed of the sprite
    public float lerpSpeed = 1;
    // used to update the sprites current location
    bool initLerp;
    // another speed variable i guess??
    float baseSpeed; 
    // fall speed
    float fallSpeed = 5f;
    float umbrellaSpeed = 0.5f;
    float airTime =0;
    float digDownSpeed = 0.1f;
    float buildTime= 0.5f;
    float buildSpeed = 0.05f;
    int maxBuildAmount = 100;
    int builtAmount = 0;
    float bTimer = 0;

    // check if on ground
    bool onGround;
    bool prevGround;
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
                DigFoward(delta);
                break;
            case CatManager.Ability.digDown:
                DigDown(delta);
                break;
            case CatManager.Ability.builder:
                Builder(delta);
                break;
            case CatManager.Ability.filler:
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
            bool hasPath = Pathfind();
            //Pathfind();
            if(hasPath){
                Vector3 tp = gameManager.GetWorldPosFromNode(targetNode);
                targetPos = tp;
            }
            float dist = Vector3.Distance(targetPos, startPos);
           
            if (onGround) {
                baseSpeed = lerpSpeed / dist;
            }
            else {
                if(isUmbrella){
                    baseSpeed = umbrellaSpeed / dist;
                }
                else{
                    baseSpeed = fallSpeed / dist;
                }
            }
            
        }
        else {
            LerpIntoPosition(delta);
        }
    }

    void Stopper(){

    }

//might not actually need as it is same as walker code, basically
    void Umbrella(){

    }

    void DigDown(float delta){
        if(!initLerp){
            initLerp = true;
            startPos = transform.position;
            time = 0;

            //list of nodes below and next to cat
            List<Node> foundGroundNodes = CheckNodesDown();

            //make sure there is stuff to dig
            if(foundGroundNodes.Count == 0){
                //if there is nothing below cat, cat should fall
                //change cat back to default walk
                ChangeAbility(CatManager.Ability.defaultWalk);
                return;
            }
            //if there are pixels to dig remove them
            gameManager.ClearListOfPixels(foundGroundNodes);

            Node tempNode = gameManager.GetNode(currNode.x, currNode.y -1);

            if(tempNode == null){ //we should never have this condition be true, but this is extra safety
                ChangeAbility(CatManager.Ability.defaultWalk);
                return;
            }
            targetNode = tempNode;
            targetPos = gameManager.GetWorldPosFromNode(targetNode);
            float dist = Vector3.Distance(targetPos, startPos);

            baseSpeed = digDownSpeed/dist;
        }
        else{
            LerpIntoPosition(delta);
        }
    }

    List<Node> CheckNodesDown(){

        List<Node> groundNodeList = new List<Node>();

        //also check the x nodes to dig a wide tunnel
        for(int x = -5; x < 5; x++){
            int targetX = currNode.x +x;
            for(int y = 0; y < 2; y++){ // check nodes below cat
                //make it dig circular not square
                if(x == 5 ){
                    if(y ==1){
                        continue;
                    }
                }
                int targetY = currNode.y - y; //cats are digging down
                Node tempNode = gameManager.GetNode(targetX, targetY);
                if(tempNode == null){
                    continue;
                }
                else{ //there is something below the cat that can be destroyed
                    if(!tempNode.isEmpty){
                        groundNodeList.Add(tempNode);
                    }
                }
            }
        }
        return groundNodeList;
    }

    void DigFoward(float delta){
        if(!initLerp){
            initLerp = true;
            startPos = transform.position;
            time = 0;

            //list of nodes below and next to cat
            List<Node> foundGroundNodes = CheckNodesForward();

            //make sure there is stuff to dig
            if(foundGroundNodes.Count == 0){
                //if there is nothing below cat, cat should fall
                //change cat back to default walk
                ChangeAbility(CatManager.Ability.defaultWalk);
                isDigFoward = false;
                return;
            }
            //if there are pixels to dig remove them
            gameManager.ClearListOfPixels(foundGroundNodes);

            //can just move into node statment
            int targetX = (movingLeft) ? currNode.x -1 : currNode.x +1;

            Node tempNode = gameManager.GetNode(targetX, currNode.y);

            if(tempNode == null){ //we should never have this condition be true, but this is extra safety
                ChangeAbility(CatManager.Ability.defaultWalk);
                isDigFoward = false;
                return;
            }
            targetNode = tempNode;
            targetPos = gameManager.GetWorldPosFromNode(targetNode);
            float dist = Vector3.Distance(targetPos, startPos);

            baseSpeed = digDownSpeed/dist;
        }
        else{
            LerpIntoPosition(delta);
        }
    }

    List<Node> CheckNodesForward(){
        List<Node> groundNodeList = new List<Node>();

        //change y to loop to cat height, this is how tall of tunnel to dig
        for(int y = 0; y< 15; y++){
            for(int x =0; x < 3; x++){
                // if(x==2){ //makes dig circular
                //     if(y == 0 || y ==6){
                //         continue;
                //     }
                // }

                int targetX = currNode.x;
                targetX = (movingLeft) ? targetX - x : targetX + x;

                int targetY = currNode.y + y;

                Node tempNode = gameManager.GetNode(targetX, targetY);
                if(tempNode == null){
                    continue;
                }
                else{ //there is something below the cat that can be destroyed
                    if(!tempNode.isEmpty){
                        groundNodeList.Add(tempNode);
                    }
                }
            }
        }
        return groundNodeList;
    }

    void Builder(float delta){
        if(!initLerp){
            bTimer += delta;
            if(bTimer > buildTime){
                bTimer = 0;
                initLerp = true;
                bool interupt = false; //make sure not to build through a wall
                builtAmount++;

                if(builtAmount > maxBuildAmount){
                    interupt = true;
                }

                int targetX = currNode.x;
                int targetY = currNode.y;

                targetX = (movingLeft) ? targetX - 1 : targetX + 1;
                targetY = targetY +1; //always build 1 up, makes it diagnol


                startPos = transform.position;
                targetNode = gameManager.GetNode(targetX, targetY);

                if(targetNode.isEmpty || interupt){
                    ChangeAbility(CatManager.Ability.defaultWalk);
                    return;
                }

                targetPos = gameManager.GetWorldPosFromNode(targetNode.x, targetNode.y);
                float dist = Vector3.Distance(startPos, targetPos);
                baseSpeed = buildSpeed/dist;

                List<Node> buildNodes = new List<Node>();
                for(int i =0; i < 5; i++){
                    int xHeight = targetX + i;
                    Node checkNode = gameManager.GetNode(xHeight, currNode.y);
                    if(checkNode.isEmpty){
                        buildNodes.Add(checkNode);
                    }
                }

                gameManager.NodesToBuild(buildNodes);
            }
        }
        else{
            LerpIntoPosition(delta);
        }
    }

    //lerp is linear interpolation, basically this code moves the node location of the cat to the next node after a set time
    void LerpIntoPosition(float delta){
        time += delta * baseSpeed;
            if (time > 1) {
                time = 1;
                initLerp = false;  
                currNode = targetNode;
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, time);
            transform.position = tp;
    }

    public bool ChangeAbility(CatManager.Ability newAbility){

        //set booleans to false
        isUmbrella = false;
        //currAbility = newAbility;
        switch(newAbility){
            case CatManager.Ability.defaultWalk:
                currAbility = newAbility;
                ClearStopNodes(); //this works correctly !!
                break;
            case CatManager.Ability.stopper:
                if(prevGround){ //make sure we are on ground inorder to perform action
                    FindStopNodes();
                    currAbility = newAbility;
                    return(true);
                }
                else{
                    return(false);
                }
            case CatManager.Ability.umbrella:
                isUmbrella = true;
                break;
            case CatManager.Ability.digDown:
                if(prevGround){
                    FindStopNodes();
                    currAbility = newAbility;
                    return(true);
                }
                else{
                    return false;
                }
            case CatManager.Ability.digFoward:
                isDigFoward = true;
                break;
            case CatManager.Ability.builder:
                //play animation
                currAbility = newAbility;
                builtAmount = 0;
                break;
            case CatManager.Ability.filler:
                currAbility = newAbility;
                break;
            case CatManager.Ability.dead:
                currAbility = newAbility;
                break;
            default:
                break;
        }

        return true;
    }

    bool Pathfind() { //change to bool to exit early

        if(currNode == null){
            targetPos = transform.position;
            targetPos.y = -50;
            prevGround = onGround;
            return false;
        }

        targetX = currNode.x;
        targetY = currNode.y;

        bool downIsAir = IsAir(targetX, targetY -1);
        bool fowardIsAir = IsAir(targetX, targetY);

        //if node below sprite is air fall
        if (downIsAir) { //cat is falling
            targetX = currNode.x;
            targetY -=1;
            airTime++;

            if(onGround){
                if(airTime > 4){
                    onGround =false;
                }
            }
        }
        else { //cat is on ground
            //if node infront of sprite is air move foward
            onGround = true;
            
            //check if cat fell to far and is now dead
            if(onGround && !prevGround){
                if(airTime > 80 && !isUmbrella){
                    targetNode = currNode;
                    ChangeAbility(CatManager.Ability.dead);
                    prevGround = onGround;
                    return true;
                }
                else{
                    //landed safe
                    targetNode = currNode;
                    prevGround =onGround;
                    airTime = 0;
                    return true;
                }
            }
            airTime = 0;

            bool stop = IsStop((movingLeft) ? targetX -1 : targetX +1, targetY);
           
            if(stop){
                movingLeft = !movingLeft;
                targetX = (movingLeft) ? targetX -1 : targetX +1;
                targetY = currNode.y;
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
                        //if cat should dig 
                        if(isDigFoward){
                            currAbility = CatManager.Ability.digFoward;
                            //animation here
                            return(false);
                        }
                        else{
                            //move other direction
                            movingLeft = !movingLeft;
                            targetX = (movingLeft) ? targetX -1 : targetX +1;
                        }
                    }

                }
            }
        }
        targetNode = gameManager.GetNode(targetX, targetY);
        prevGround = onGround;
        return true;
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
        Debug.Log(stopNodes);
        for(int i = 0; i < stopNodes.Count; i++){
            stopNodes[i].isStop = false;
        }
        stopNodes.Clear();
    }

}
