using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    // Update is called once per frame
    public void Popup(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
    }
}
