using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CatManager;

public class UIButton : MonoBehaviour
{

    //ui button elements go here
    public Ability ability;
    public Image buttonImg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press(){
        UIManager.singleton.PressAbilityButton(this);
    }
}
