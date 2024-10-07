using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    public Item slotsItem;
    public int itemID;

    Sprite defaultSprite;
    //Text amountText;
    void Start()
    {
        defaultSprite = transform.GetChild(0).GetComponent<Image>().sprite;
        //amountText = transform.GetChild(0).GetComponent<Text>();
        //amountText.text = "";

        itemID = - 1;
    }

    void Update()
    {
        CheckForItem();
    }
    
    public void CheckForItem()
    {
        if(transform.childCount > 2)
        {
            slotsItem = transform.GetChild(2).GetComponent<Item>();
            itemID = transform.GetChild(2).GetComponent<Item>().itemID;
            transform.GetChild(0).GetComponent<Image>().sprite = slotsItem.itemSprite;
        }
        else
        {
            slotsItem = null;
            transform.GetChild(0).GetComponent<Image>().sprite = defaultSprite;
            //amountText.text = "";
        }
    }
}
