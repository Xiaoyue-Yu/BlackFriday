using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image original;
    public Sprite newSprite;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NewImage()
    {
        original.sprite = newSprite;
    }
}
