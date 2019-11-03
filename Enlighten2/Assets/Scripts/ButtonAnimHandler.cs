using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimHandler : MonoBehaviour
{
    //public Text menuText;

    //Color initialTextColor = new Color();
    //Color glowColor = new Color(.2941f, .8549f, .7529f);

    [HideInInspector]
    public bool animDone;

    Animator[] anims;

    bool hovering;
    bool canHover;

    // Start is called before the first frame update
    void Start()
    {
        anims = GetComponentsInChildren<Animator>();
        canHover = true;
        animDone = false;
        //initialTextColor = menuText.color;
    }

    private void Update()
    {
        if (hovering && canHover)
        {
            foreach (Animator anim in anims)
            {
                if (anim.transform.name == "Right")
                {
                    anim.SetBool("Right", true);
                }
                else if (anim.transform.name == "Left")
                {
                    anim.SetBool("Left", true);
                }
                else if (anim.transform.name == "Middle")
                {
                    anim.SetBool("Middle", true);
                }
            }
        }

        foreach (Animator anim in anims)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Left Select") || anim.GetCurrentAnimatorStateInfo(0).IsName("Mid Select") || 
                anim.GetCurrentAnimatorStateInfo(0).IsName("Right Select"))
            {
                //menuText.color = glowColor;
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) { animDone = true; }
            }
        }

        if (transform.name == "Exit Button Image" && animDone)
        {
            Application.Quit();
        }
        else if (transform.name == "Play Button Image" && animDone)
        {
            UIManager.instance.onSelect.Invoke();
        }
    }

    public void OnMouseOver()
    {
        hovering = true;
        //menuText.color = glowColor;
    }

    public void OnMouseExit()
    {
        hovering = false;

        //menuText.color = initialTextColor;

        foreach (Animator anim in anims)
        {
            if (anim.transform.name == "Right")
            {
                anim.SetBool("Right", false);
            }
            else if (anim.transform.name == "Left")
            {
                anim.SetBool("Left", false);
            }
            else if (anim.transform.name == "Middle")
            {
                anim.SetBool("Middle", false);
            }
        }
    }

    public void OnMouseDown()
    {
        //menuText.color = glowColor;

        foreach (Animator anim in anims)
        {
            if (anim.transform.name == "Right")
            {
                anim.SetBool("Right", false);
                anim.Play("Right Select");

                if (anim.GetCurrentAnimatorStateInfo(0).loop)
                {
                    canHover = false;
                }
                else
                {
                    canHover = true;
                }
            }
            else if (anim.transform.name == "Left")
            {
                anim.SetBool("Left", false);
                anim.Play("Left Select");

                if (anim.GetCurrentAnimatorStateInfo(0).loop)
                {
                    canHover = false;
                }
                else
                {
                    canHover = true;
                }
            }
            else if (anim.transform.name == "Middle")
            {
                anim.SetBool("Middle", false);
                anim.Play("Mid Select");

                if (anim.GetCurrentAnimatorStateInfo(0).loop)
                {
                    canHover = false;
                }
                else
                {
                    canHover = true;
                }
            }
        }
    }
}