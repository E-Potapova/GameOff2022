using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CatManager;

public class UIManager : MonoBehaviour
{
    //mouse location
    public Transform mouseTrans;

    #region mouseCrosshairs
    public Image mouse;
    public Sprite cross1;
    public Sprite cross2;
    public Sprite box;
    #endregion
    
    //check if over ui element
    public bool overCat;

    //switch ability
    public bool switchAbility;

    //what ability is assigned to the button
    public Ability targetAbility;

    public UIButton currButton;

    //button colors
    public Color selectTint;
    Color defColor;

    public static UIManager singleton;
    void Awake(){
        singleton = this;
    }

    //hide default mouse cursor
    void Start(){
        Cursor.visible = false;
    }

    //change mouse based off what its over
    public void Tick(){
        mouseTrans.transform.position = Input.mousePosition;
        if(overCat){
            mouse.sprite = box;
        }
        else{
            mouse.sprite = cross1;
        }
    }

    //get ability when mouse clicks button, duh
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
