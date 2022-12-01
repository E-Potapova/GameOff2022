using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    // if want to make tick faster or slower change timescale
    // CHANGE to be unity time scale library function thingy

    #region Cats
    public float maxCats = 1;
    List<Cat> catList = new List<Cat>();
    List<Cat> safeCats = new List<Cat>();
    public int catsSafe;
    public int catsSpawned = 0;
    List<Cat> deadCats = new List<Cat>();
    public int deadCatsCount = 0;
    public int maxDeadCats = 2;
    public GameObject catPrefab;
    private GameObject catsParent;
    #endregion

    #region spawning
    float delta;
    public float interval =2;
    float timer;
    public float spawnBuffer = 5;
    float spawnBufferTime = 0;

    public bool beginGame = false;
    #endregion

    #region Ability counter
    //when player runs out of that ability just disable the button...
    public int catBuildersUp =5;
    public int catBuildersFoward = 5;
    public int catDigDown = 5;
    public int catDigFoward = 5;
    public int catUmbrella = 5;
    public int catStopper = 5;
    #endregion

    //win game condition
    bool win = false;
    bool lose =false;
    
    public static CatManager singleton;
    void Awake(){
        singleton = this;
    }

    private void Start()
    {
        catsParent = new GameObject();
        catsParent.name = "Cats";
        catsParent.transform.position = GameManager.singleton.spawnPosition;
    }


    // Update is called once per frame
    void Update() {
        //increase time passed
        delta = Time.deltaTime;
        spawnBufferTime += delta;
        //keep spawning cats at set time intervals until all cats are spawned
        if(spawnBuffer < spawnBufferTime){
            if (catsSpawned < maxCats) {
                timer -= delta;
                if(timer < 0){
                    //create new unit
                    timer = interval;
                    SpawnCat();
                }
            }

            //adding new stuff for removing units
            safeCats.Clear();
            deadCats.Clear();

            //go through all cats and check if they made it to the goal, or are dead
            for(int i = 0; i < catList.Count; i++){
                if(catList[i].isSafe){
                    safeCats.Add(catList[i]);
                    continue;
                }
                else if(catList[i].isDead){
                    deadCats.Add(catList[i]);
                    continue;
                }
                catList[i].Tick(delta);
            }

            //if cat is in the goal remove it from list of spawned cats
            for(int i = 0; i < safeCats.Count; i++){
                if(catList.Contains(safeCats[i])){
                    catList.Remove(safeCats[i]);
                    catsSafe++; //keep track of cats that made it to the goal
                }
            }

            //if enough cats made it to goal you win!!
            if(catsSafe == catsSpawned && !win){
                win = true;
                Debug.Log("You Win!");
            }
            
            for(int i =0; i< deadCats.Count; i++){
                if(catList.Contains(deadCats[i])){
                    catList.Remove(deadCats[i]);
                    deadCatsCount++;
                }
            }

            //if too many cats died you lose
            if(deadCatsCount >= maxDeadCats && !lose){
                lose = true;
                Debug.Log("You Lose");
            }
        }
    }

    //this spawns cats at the spawn point, hence the name
    void SpawnCat() {
        GameObject gameObj = Instantiate(catPrefab);
        gameObj.transform.parent = catsParent.transform;
        Cat cat = gameObj.GetComponent<Cat>();
        cat.Init(GameManager.singleton);
        catList.Add(cat);
        cat.move = true;
        catsSpawned++; //keep track of how many cats have been spawned

        //play sound
        //FindObjectOfType<AudioManager>().Play("Meow 1");
        AudioManager.instance.Play("Meow 1");
    }

    //find the cat closest to the mouse
    public Cat GetClosestCat(Vector3 vectClosest){
        Cat closestCat = null;

        //go through ever cat and calulate its distance to the mouse
        float minDist  = 0.3f;
        for(int i =0; i < catList.Count; i++){
            float tempDist = Vector3.Distance(vectClosest, catList[i].transform.position);
            
            //we only care about the cat closest to mouse
            if(tempDist < minDist){
                minDist = tempDist;
                closestCat = catList[i];
            }
        }
        return(closestCat);

    }
   
    public enum Ability{
        //enter the abilities here
        defaultWalk, stopper, umbrella, digFoward, digDown, dead, buildUp, buildFoward
    }
}
