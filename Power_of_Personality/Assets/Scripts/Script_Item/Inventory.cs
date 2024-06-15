using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryObject;
    public InventoryCtrl InvenCtrl;

    public Slot[] slots;

    private Vector3 originalScale;
    private Vector3 hiddenScale;

    void Start()
    {
        originalScale = new Vector3(1, 1, 1);
        hiddenScale = new Vector3(0, 0, 0);

        GameObject[] TraitBoxes = GameObject.FindGameObjectsWithTag("TraitBox");
        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
        //(06.03) 슬롯 추가
        /*
        for(int i = 0; i < TraitBoxes.Length; i++) 
        {
            slots[i] = TraitBoxes[i].GetComponent<Slot>();
        }
        */

        for(int i = 0; i < InvenCtrl.itemCount; i++)
        {
            if(InvenCtrl.collectedItems[i] != null)
            {
                Debug.Log(InvenCtrl.collectedItemsID[i]);
                Instantiate(InvenCtrl.itemList[InvenCtrl.collectedItemsID[i]], this.transform.position, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryObject.transform.localScale != hiddenScale)
                inventoryObject.transform.localScale = hiddenScale;
            else
                inventoryObject.transform.localScale = originalScale;
        }
    }

    public void AddItem(Item itemToBeAdded, Item startingItem = null)
    {
        List<Slot> emptySlots = new List<Slot>();

        foreach(Slot i in slots)
        {
            emptySlots.Add(i);
        }
        if(emptySlots.Count > 0)
        {
            itemToBeAdded.transform.parent = emptySlots[itemToBeAdded.SlotIndex].transform;
            itemToBeAdded.gameObject.SetActive(false);
            Status.FixedAD += itemToBeAdded.FixedAD;
            Status.PercentAD += itemToBeAdded.PercentAD;
            Status.FixedArmor += itemToBeAdded.FixedArmor;
            Status.PercentArmor += itemToBeAdded.PercentArmor;
            Status.FixedSpeed += itemToBeAdded.FixedSpeed;
            Status.PercentSpeed += itemToBeAdded.PercentSpeed;
            Status.FixedADC += itemToBeAdded.FixedADC;
            Status.PercentADC += itemToBeAdded.PercentADC;
            Status.FixedAP += itemToBeAdded.FixedAP;
            Status.PercentAP += itemToBeAdded.PercentAP;
            Status.FixedCooltime += itemToBeAdded.FixedCooltime;
            Status.PercentCooltime += itemToBeAdded.PercentCooltime;
            Status.FixedFire += itemToBeAdded.FixedFire;
            Status.PercentFire += itemToBeAdded.PercentFire;
            Status.FixedIce += itemToBeAdded.FixedIce;
            Status.PercentIce += itemToBeAdded.PercentIce;
            Status.StatUpdate();
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Item>())
        {
            Debug.Log("삽입 실행실행");
            AddItem(col.GetComponent<Item>());


            /*
            if(InvenCtrl.collectedItems[InvenCtrl.itemCount] == null)
            {
                InvenCtrl.collectedItemsID[InvenCtrl.itemCount] = col.GetComponent<Item>().itemID;
            }
            */        
            //InvenCtrl.itemCount++;
        }
    }
    
}
