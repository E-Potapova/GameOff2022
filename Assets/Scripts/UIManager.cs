using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CatManager;

public class UIManager : MonoBehaviour
{
    public Transform mouseTrans;
    
    public bool overCat;
    public bool holdingMouseDown;

    //what ability is assigned to the button
    public Ability selectedAbility;

    public UIButton currButton;

    private Animator animator;


    public static UIManager singleton;
    void Awake(){
        singleton = this;
    }

    //hide default mouse cursor
    void Start(){
        Cursor.visible = false;
        animator = GameObject.FindGameObjectWithTag("Mouse").GetComponent<Animator>();
    }

    //change mouse based off what its over
    public void Tick(){
        // shift so cursor appears in correct position
        mouseTrans.transform.position = Input.mousePosition + new Vector3(35, -20, 0);

        if (overCat && currButton){
            // hovering over a cat and have power/button selected
            animator.SetBool("abilitySelected", true);
            // now want sprite centered on mouse corner
            mouseTrans.transform.position += new Vector3(-35, 20, 0);
        }
        else
        {
            animator.SetBool("abilitySelected", false);
        }

        if (holdingMouseDown)
        {
            animator.SetBool("holdingMouseDown", true);
            holdingMouseDown = false;
        }
        else
        {
            animator.SetBool("holdingMouseDown", false);
        }
    }

    public void HandleMouseClick()
    {
        holdingMouseDown = true;
        // this occurs before UIButton's OnClick occurs
        // so it only sets button to null if we are not pressing on a button
        //currButton = null;
        //selectedAbility = Ability.pet;
    }

    //get ability when mouse clicks button, duh
    public void PressAbilityButton(UIButton button){
        //if (currButton)
        //{
        //    currButton.Deselect();
        //}
        currButton = button;
        selectedAbility = button.ability;
    }
}
