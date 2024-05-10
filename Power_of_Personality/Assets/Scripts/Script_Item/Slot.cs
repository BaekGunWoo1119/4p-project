using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    public Item slotsItem;

    Sprite defaultSprite;
    Text amountText;
    void Start()
    {
        defaultSprite = GetComponent<Image>().sprite;
        amountText = transform.GetChild(0).GetComponent<Text>();
        amountText.text = "";
    }

    void Update()
    {
        CheckForItem();
    }
    
    public void CheckForItem()
    {
        if(transform.childCount > 3)
        {
            slotsItem = transform.GetChild(3).GetComponent<Item>();
            GetComponent<Image>().sprite = slotsItem.itemSprite;
        }
        else
        {
            slotsItem = null;
            GetComponent<Image>().sprite = defaultSprite;
            amountText.text = "";
        }
    }
}
