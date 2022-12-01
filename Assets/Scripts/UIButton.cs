using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CatManager;

public class UIButton : MonoBehaviour
{
    public Ability ability;
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    private Animator animator;
    private Image buttonImage;

    private void Start()
    {
        animator = GetComponent<Animator>();
        buttonImage = GetComponent<Image>();
    }

    public void Press(){
        //Debug.Log("onClick");
        animator.SetBool("selected", true);
        buttonImage.sprite = selectedSprite;
        UIManager.singleton.PressAbilityButton(this);
    }

    public void Deselect()
    {
        Debug.Log("deselect");
        animator.SetBool("selected", false);
        buttonImage.sprite = defaultSprite;
    }
}
