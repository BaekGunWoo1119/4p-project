using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NormalShop_Slot : MonoBehaviour
{
    public Item slotsItem;

    Sprite defaultSprite;

    void Start()
    {
        //defaultSprite = transform.GetChild(4).GetComponent<Image>().sprite;
    }

    void Update()
    {
        CheckForItem();
    }
    
    public void CheckForItem()
    {
        if(transform.childCount > 4)
        {
            slotsItem = transform.GetChild(5).GetComponent<Item>();
            transform.GetChild(3).GetComponent<Image>().sprite = slotsItem.itemSprite;
        }
        else
        {
            slotsItem = null;
            transform.GetChild(3).GetComponent<Image>().sprite = defaultSprite;
            //amountText.text = "";
        }
    }
}
