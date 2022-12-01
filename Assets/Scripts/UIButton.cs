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
    private Button button;
    private Image buttonImage;
    private Text counterText;

    private void Start()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
        buttonImage = GetComponent<Image>();
        counterText = gameObject.transform.Find("Count").gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        int counter = CatManager.singleton.GetAbilityCount(ability);
        if (counterText)
        {
            counterText.text = (counter).ToString();
        }
        if (button.interactable && counter < 1)
        {
            button.interactable = false;
            UIManager.singleton.PressAbilityButton(this); // Deselect button
        }
    }

    public void Press(){
        //Debug.Log("onClick");
        animator.SetBool("selected", true);
        if (counterText)
            counterText.color = new Color(238f/255, 96f/255, 85f/255);
        buttonImage.sprite = selectedSprite;
        UIManager.singleton.PressAbilityButton(this);
    }

    public void Deselect()
    {
        if (!button.interactable)
        {
            if (counterText)
                counterText.enabled = false;
        }
        Debug.Log("deselect");
        animator.SetBool("selected", false);
        if (counterText)
            counterText.color = new Color(97f/255, 164f/255, 127f/255);
        buttonImage.sprite = defaultSprite;
    }
}
