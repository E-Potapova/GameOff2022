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
    int maxX;
    int maxY;
    Node[,] mapGrid;
    public int pixelsPerUnit = 64;
    public float posOffset = 1f/64f; // 1 / pixels per unit of the level sprite

    // mouse input support
    Vector3 mousePos;
    Node currNode;
    Node prevNode;
    int delRadius = 6;
    //make the delete a circle instead of square
    public float editRadius = 6;

    // spawn location of cats
    // must be within the frame of the sprite terrain
    public Transform spawnTransform;
    [HideInInspector]
    public Vector3 spawnPosition;
    [HideInInspector]
    public Node spawnNode;

    //create a cat object
    public Cat currCat;
    CatManager catManager;
    //public List<Units> units = new List<Units>();//convert to cats soon

    UIManager uiManager;

    //make GameManger singleton, restricts class to only one instance
    public static GameManager singleton;
    
    //Ui stuff
    private bool overUIElement;

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
        levelRenderer.sprite = Sprite.Create(tempTexture, tempSpriteRect, Vector2.zero, pixelsPerUnit);
    }

    private void Update()
    {
        overUIElement = EventSystem.current.IsPointerOverGameObject();
        GetMousePosition();
        CheckForUnit();
        uiManager.Tick();
        HandleCat();
        if (Input.GetMouseButton(1)) // primary button (left click)
            HandleMouseClick();
    }

    void HandleMouseClick()
    {
        if (currNode == null) return;

        if (currNode != prevNode) // don't repeat if didn't click in new place
        {
            prevNode = currNode;

            // delete part of map around where mouse was clicked
            Color col = Color.white;
            col.a = 0; // transparent

            //makes it delete in a circle
            Vector3 center = GetWorldPosFromNode(currNode);
            float radius = editRadius *posOffset;

            for (int x = -delRadius; x < delRadius; x++){
                for (int y = -delRadius; y < delRadius; y++){
                    int textureX = x + currNode.x;
                    int textureY = y + currNode.y;

                    // check if node is in range of radius, to remove
                    float dist = Vector3.Distance(center, GetWorldPosFromNode(textureX, textureY));
                    if(dist > radius){
                        continue;
                    }

                    // check node exist, to prevent wrapping fixes this bug
                    // there is a bug where it deletes from the oppisite side too
                    Node node = GetNode(textureX, textureY);
                    if(node == null){
                        continue; //skip this itteration of loop
                    }
                    node.isEmpty = true;
                    tempTexture.SetPixel(textureX, textureY, col);
                }
            }

            tempTexture.Apply(); // update texture after changes
        }
    }

    void CheckForUnit(){
        currCat = catManager.GetClosest(mousePos);
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
                currCat.ChangeAbility(uiManager.targetAbility);
            }

            //added this to make cats walk again when i tell them too
            if(currCat.currAbility != CatManager.Ability.defaultWalk){
                currCat.ChangeAbility(uiManager.targetAbility);
            }
        }

    }

    void GetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        currNode = GetNodeFromWorldPos(mousePos);
    }

    // Gets closest (int,int) position to get Node
    Node GetNodeFromWorldPos(Vector3 worldCoord)
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
        if(node ==null){
            return(-Vector3.one);//dont want negative values
        }

        Vector3 vec = Vector3.zero;
        vec.x = node.x * posOffset;
        vec.y = node.y * posOffset;
        return(vec);
    }
}

// Represents a pixel of the level's map
public class Node
{
    public int x;
    public int y;
    public bool isEmpty;

    public bool isStop;

}
