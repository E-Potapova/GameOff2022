using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // sprite info
    // LEVEL SPRITE NEEDS TO BE LOCATED IN 0,0,0
    public Texture2D levelTexture;
    public SpriteRenderer levelRenderer;
    Texture2D tempTexture; // to not write to original sprite

    // map grid support32
    #region map support
    int maxX;
    int maxY;
    Node[,] mapGrid;
    public int pixelsPerUnit = 64;
    public float posOffset = 1f/64f; // 1 / pixels per unit of the level sprite
    #endregion

    // mouse input support
    Vector3 mousePos;

    // spawn location and goal for cats
    #region spawn n Goal
    // must be within the frame of the sprite terrain
    public Transform spawnTransform;
    [HideInInspector]
    public Vector3 spawnPosition;
    [HideInInspector]
    public Node spawnNode;

    //end goal for cats 
    public Transform goalTransform;
    [HideInInspector]
    public Vector3 goalposition;
    [HideInInspector]
    public Node goalNode;
    #endregion

    //create a cat object
    public Cat currCat;
    CatManager catManager;
    //cat building
    public Color buildColor = Color.cyan;

    UIManager uiManager;

    public GameObject pauseMenu;
    bool isPaused = false;
    

    //make GameManger singleton, restricts class to only one instance
    public static GameManager singleton;
    

    // tells gamemanager to start
    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        catManager = CatManager.singleton;
        uiManager = UIManager.singleton;
        InitializeLevel();

        // sets the spawn location of the cats
        spawnNode = GetNodeFromWorldPos(spawnTransform.position);
        spawnPosition = GetWorldPosFromNode(spawnNode);
        //currCat.Init(this); i guess this isnt neede anymore

        //set the goal location for the cats
        goalNode = GetNodeFromWorldPos(goalTransform.position);
        goalposition = GetWorldPosFromNode(goalNode);
        SetupGoalPositions();

        //play music here

    }

    // Initializes map grid to hold each pixel(Node) of the map sprite
    void InitializeLevel()
    {
        maxX = levelTexture.width;
        maxY = levelTexture.height;
        mapGrid = new Node[maxX, maxY];
        tempTexture = new Texture2D(maxX, maxY); // new empty texture
        tempTexture.filterMode = FilterMode.Point; // pixel art sprite

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                Node n = new Node();
                n.x = x;
                n.y = y;
                Color col = levelTexture.GetPixel(x, y);
                tempTexture.SetPixel(x, y, col); // copy original pixel to new texture instance
                n.isEmpty = (col.a == 0); // pixel is transparent
                mapGrid[x, y] = n;
            }
        }
        tempTexture.Apply();
        Rect tempSpriteRect = new Rect(0, 0, maxX, maxY);
        // pass texture as sprite to renderer
        levelRenderer.sprite = Sprite.Create(tempTexture, tempSpriteRect, Vector2.zero, pixelsPerUnit, 0, SpriteMeshType.FullRect);
    }

    private void Update()
    {
        GetMousePosition();
        if (Input.GetMouseButton(0)) // primary button (left click)
            uiManager.HandleMouseClick();
            
        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused){
            Instantiate(pauseMenu);//create object 
            isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape)){
            isPaused = false;
        }
        CheckForCat();
        uiManager.Tick();
        HandleCat();
        BuildListOfNodes();
    }

    void GetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
    }
   
    #region Cat Handelers
    void CheckForCat(){
        currCat = catManager.GetClosestCat(mousePos);
        if (currCat == null){
            uiManager.overCat = false;
            return;
        }
        uiManager.overCat = true;
    }

    void HandleCat(){
        if(currCat == null){
            return;
        }

        if(Input.GetMouseButtonUp(0)){
            // if(uiManager.targetAbility == CatManager.Ability.defaultWalk){
            //     return;
            // } i commented this out so i can make cats walk again
            
            if(currCat.currAbility == CatManager.Ability.defaultWalk){
                currCat.ChangeAbility(uiManager.selectedAbility);
            }

            //added this to make cats walk again when i tell them too
            else if(currCat.currAbility != CatManager.Ability.defaultWalk){
                currCat.ChangeAbility(uiManager.selectedAbility);
            }
        }
    }
    #endregion

    #region Node Getters
    // Gets closest (int,int) position to get Node
    public Node GetNodeFromWorldPos(Vector3 worldCoord)
    {
        int x = Mathf.RoundToInt(worldCoord.x / posOffset);
        int y = Mathf.RoundToInt(worldCoord.y / posOffset);
        return GetNode(x, y);
    }

    // Retreives Node from map grid
    public Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x > maxX - 1 || y > maxY - 1) return null;
        return mapGrid[x, y];
    }

    //used by cats sprite
    //gets world cordinates of node, given an x and y cord
    public Vector3 GetWorldPosFromNode(int x, int y){
        Vector3 vec = Vector3.zero;
        vec.x = x * posOffset;
        vec.y = y * posOffset;
        return(vec);
    }

    //used by cats sprite
    //given a node gets world cordinates of the node
    public Vector3 GetWorldPosFromNode(Node node){
        if(node == null){
            return(-Vector3.one);//dont want negative values
        }

        Vector3 vec = Vector3.zero;
        vec.x = node.x * posOffset;
        vec.y = node.y * posOffset;
        return(vec);
    }
    #endregion
    public void ClearListOfPixels(List<Node> nodeList){
        Color newColor =Color.white;
        newColor.a = 0; //set color transparecny to invisible

        for(int i =0; i < nodeList.Count; i++){
            nodeList[i].isEmpty = true;
            tempTexture.SetPixel(nodeList[i].x, nodeList[i].y, newColor);
        }
        
        tempTexture.Apply();
    }

    #region Nodes Built by cat
    List<Node> buildNodes = new List<Node>();
    public void NodesToBuild(List<Node> nodesToBuild){
        buildNodes.AddRange(nodesToBuild);
    }

    void BuildListOfNodes(){
        if(buildNodes.Count == 0){
            return;
        }

        for(int i =0; i < buildNodes.Count; i++){
            buildNodes[i].isEmpty = false; //confused
            tempTexture.SetPixel(buildNodes[i].x, buildNodes[i].y, buildColor);
        }

        buildNodes.Clear();
        tempTexture.Apply(); //this is done diffrent
    }
    #endregion
    
    //makes the end goal an area instead of singular node
    public void SetupGoalPositions(){
        for(int x = -1; x < 1; x++){
            for(int y =-2; y < 8; y++){
                int targetX = goalNode.x + x;
                int targetY = goalNode.y + y;

                Node tempNode = GetNode(targetX, targetY);

                if(tempNode == null){
                    continue;
                }

                tempNode.isGoal = true;
            }
        }
    }

}

// Represents a pixel of the level's map
public class Node
{
    public int x;
    public int y;
    public bool isEmpty;

    public bool isStop;

    public bool isGoal;

}
