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
    public GameObject catPrefab;
    private GameObject catsParent;
    #endregion

    #region spawning
    float delta;
    public float interval =2;
    float timer;
    #endregion

    //win game condition
    bool win = false;
    
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

        //keep spawning cats at set time intervals until all cats are spawned
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

        //go through all cats and check if they made it to the goal
        for(int i = 0; i < catList.Count; i++){
            if(catList[i].isSafe){
                safeCats.Add(catList[i]);
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
        FindObjectOfType<AudioManager>().Play("Meow 1");
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
        defaultWalk, stopper, umbrella, digFoward, digDown, dead, builder, filler
    }
}
