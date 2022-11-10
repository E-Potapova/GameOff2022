using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    //if want to make tick faster or slower change timescale
    public float maxUnits = 10;
    public float timeScale = 1;
    float delta;
    public float interval =2;
    float timer;
    public GameObject unitsPrefab;
    GameObject unitsParent;
    List<Units> unitList = new List<Units>();
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        unitsParent = new GameObject();
        unitsParent.name = "units parent";
        gameManager = GameManager.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        delta = Time.deltaTime;
        delta *= timeScale;

        if(unitList.Count < maxUnits){
            timer -= delta;
            if(timer < 0){
                //create new unit
                timer = interval;
                SpawnCat();
            }
        }

        //go through all units
        for(int i = 0; i < unitList.Count; i++){
            unitList[i].Tick(delta);
        }
    }

    void SpawnCat(){
        GameObject gameO = Instantiate(unitsPrefab);
        gameO.transform.parent = unitsParent.transform;
        Units cat = gameO.GetComponent<Units>();
        cat.Init(gameManager);
        unitList.Add(cat);
        cat.move = true;
    }
}
