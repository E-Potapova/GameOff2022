using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // sprite info
    // LEVEL SPRITE NEEDS TO BE LOCATED IN 0,0,0
    public Texture2D levelTexture;
    public SpriteRenderer levelRenderer;
    Texture2D tempTexture; // to not write to original sprite

    // map grid support
    int maxX;
    int maxY;
    Node[,] mapGrid;
    public int pixelsPerUnit = 32;
    public float posOffset = 1f/32f; // 1 / pixels per unit of the level sprite

    // mouse input support
    Vector3 mousePos;
    Node currNode;
    Node prevNode;
    int delRadius = 4;

    void Start()
    {
        InitializeLevel();
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
        GetMousePosition();
        if (Input.GetMouseButton(0)) // primary button (left click)
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
            for (int x = -delRadius; x < delRadius; x++)
                for (int y = -delRadius; y < delRadius; y++)
                    tempTexture.SetPixel(currNode.x + x, currNode.y + y, col);

            tempTexture.Apply(); // update texture after changes
        }
    }

    void GetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        currNode = GetNodeFromWorldCoord(mousePos);
    }

    // Gets closest (int,int) position to get Node
    Node GetNodeFromWorldCoord(Vector3 worldCoord)
    {
        int x = Mathf.RoundToInt(worldCoord.x / posOffset);
        int y = Mathf.RoundToInt(worldCoord.y / posOffset);
        return GetNode(x, y);
    }

    // Retreives Node from map grid
    Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x > maxX - 1 || y > maxY - 1) return null;
        return mapGrid[x, y];
    }
}

// Represents a pixel of the level's map
public class Node
{
    public int x;
    public int y;
    public bool isEmpty;
}
