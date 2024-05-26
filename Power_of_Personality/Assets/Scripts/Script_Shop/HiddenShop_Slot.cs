using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HiddenShop_Slot : MonoBehaviour
{
    public Item slotsItem;

    Sprite defaultSprite;

    void Start()
    {
        defaultSprite = GetComponent<Image>().sprite;
    }

    void Update()
    {
        CheckForItem();
    }
    
    public void CheckForItem()
    {
        if(transform.childCount > 4)
        {
            slotsItem = transform.GetChild(4).GetComponent<Item>();
            GetComponent<Image>().sprite = slotsItem.itemSprite;
        }
        else
        {
            slotsItem = null;
            GetComponent<Image>().sprite = defaultSprite;
            //amountText.text = "";
        }
    }
}
