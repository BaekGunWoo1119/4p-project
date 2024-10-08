using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    private bool isDuplicated;
    public List<int> invenlist;
    public GameObject inventoryObject;
    public InventoryCtrl InvenCtrl;

    public GameObject[] TraitBoxes;
    public Slot[] slots;

    private Vector3 originalScale;
    private Vector3 hiddenScale;

    void Start()
    {
        originalScale = new Vector3(1, 1, 1);
        hiddenScale = new Vector3(0, 0, 0);
        
        TraitBoxes = GameObject.FindGameObjectsWithTag("TraitBox").OrderBy(go => go.name).ToArray();
        slots = new Slot[TraitBoxes.Length];
        
    }

    void Update()
    {
        if(InvenCtrl == null){
            InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
            //(06.03) 슬롯 추가

            for(int i = 0; i < TraitBoxes.Length; i++) 
            {
                slots[i] = TraitBoxes[i].GetComponent<Slot>();
            }

            ItemPlus();

        }
        if(inventoryObject == null){
            inventoryObject = GameObject.Find("StatWindow");
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryObject.transform.localScale != hiddenScale)
                inventoryObject.transform.localScale = hiddenScale;
            else
                inventoryObject.transform.localScale = originalScale;
        }
    } 

    public void AddItem(Item itemToBeAdded, Item startingItem = null)
    {
        isDuplicated = false;
        List<Slot> emptySlots = new List<Slot>();

        foreach(Slot i in slots)
        {
            emptySlots.Add(i);
        }
        if(emptySlots.Count > 0)
        {
            foreach(int id in invenlist){
                if(id == itemToBeAdded.itemID){
                    isDuplicated = true;
                }
            }
            if(isDuplicated != true){
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
                    invenlist.Add(itemToBeAdded.itemID);
                }
            Debug.Log(itemToBeAdded.itemID);
            Status.SetUpdate(itemToBeAdded.itemID);
        }
    }


    public void ItemPlus()
    {
        for(int i = 0; i < InvenCtrl.itemCount; i++)
        {
            if(InvenCtrl.collectedItems[i] != null)
            {
                if(InvenCtrl.collectedItemsID[i] != InvenCtrl.HadItemsID[i])
                {
                    Debug.Log(InvenCtrl.collectedItemsID[i]);
                    Instantiate(InvenCtrl.itemList[InvenCtrl.collectedItemsID[i]], this.transform.position, Quaternion.identity);
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Item>())
        {
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
