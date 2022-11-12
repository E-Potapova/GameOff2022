using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CatManager;

public class UIManager : MonoBehaviour
{
    public Transform mouseTrans;
    public Image mouse;
    public Sprite cross1;
    public Sprite cross2;
    public Sprite box;
    
    //check if over ui element
    public bool overCat;

    //switch ability
    public bool switchAbility;

    //imported CaManager as static
    //might need to change
    public Ability targetAbility;

    public UIButton currButton;
    public Color selectTint;
    Color defColor;

    public static UIManager singleton;
    void Awake(){
        singleton = this;
    }

    void Start(){
        Cursor.visible = false;
    }

    public void Tick(){
        mouseTrans.transform.position = Input.mousePosition;
        if(overCat){
            mouse.sprite = box;
        }
        else{
            mouse.sprite = cross1;
        }
    }

    public void PressAbilityButton(UIButton button){
        if(currButton){
            currButton.buttonImg.color = defColor;
        }

        currButton = button;
        defColor = currButton.buttonImg.color;
        currButton.buttonImg.color = selectTint;

        targetAbility = currButton.ability;
    }
}
