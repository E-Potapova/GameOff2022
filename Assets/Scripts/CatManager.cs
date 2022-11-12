using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    // if want to make tick faster or slower change timescale
    // CHANGE to be unity time scale library function thingy
    public float maxCats = 10;
    public float timeScale = 1;
    float delta;
    public float interval =2;
    float timer;
    public GameObject catPrefab;
    GameObject catsParent;
    List<Cat> catList = new List<Cat>();
    GameManager gameManager;

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
}
