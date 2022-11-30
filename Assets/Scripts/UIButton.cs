using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CatManager;

public class UIButton : MonoBehaviour
{
    public Ability ability;

    //private Animator animator;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    public void Press(){
        //Debug.Log("onClick");
        //animator.SetBool("selected", true);
        UIManager.singleton.PressAbilityButton(this);
    }

    //public void Deselect()
    //{
    //    Debug.Log("deselect");
    //    animator.SetBool("selected", false);
    //}
}
