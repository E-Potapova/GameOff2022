using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    // if want to make tick faster or slower change timescale
    // CHANGE to be unity time scale library function thingy
    public float maxCats = 1;
    public float timeScale = 1;
    float delta;
    public float interval =2;
    float timer;
    public GameObject catPrefab;
    GameObject catsParent;
    List<Cat> catList = new List<Cat>();
    GameManager gameManager;

    public static CatManager singleton;
    void Awake(){
        singleton = this;
    }

    // Start is called before the first frame update
    void Start() {
        catsParent = new GameObject();
        catsParent.name = "cats parent";
        gameManager = GameManager.singleton;
    }

    // Update is called once per frame
    void Update() {
        delta = Time.deltaTime;
        delta *= timeScale;

        if (catList.Count < maxCats) {
            timer -= delta;
            if(timer < 0){
                //create new unit
                timer = interval;
                SpawnCat();
            }
        }

        //go through all cats
        for(int i = 0; i < catList.Count; i++){
            catList[i].Tick(delta);
        }
    }

    void SpawnCat() {
        GameObject gameObj = Instantiate(catPrefab);
        gameObj.transform.parent = catsParent.transform;
        Cat cat = gameObj.GetComponent<Cat>();
        cat.Init(gameManager);
        catList.Add(cat);
        cat.move = true;
    }

    //find the cat closest to the mouse
    public Cat GetClosest(Vector3 vectClosest){
        Cat closestCat = null;

        float minDist  = 0.5f;
        for(int i =0; i < catList.Count; i++){
            float tempDist = Vector3.Distance(vectClosest, catList[i].transform.position);
            if(tempDist < minDist){
                minDist = tempDist;
                closestCat = catList[i];
            }
        }

        return(closestCat);

    }

    public enum Ability{
        //enter the abilities here
        defaultWalk, stopper, umbrella, digFoward, digDown
    }
}
