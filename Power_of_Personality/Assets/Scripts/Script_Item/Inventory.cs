using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryObject;

    public Slot[] slots;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryObject.SetActive(!inventoryObject.activeInHierarchy);
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
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Item>())
        {
            Debug.Log("실행");
            AddItem(col.GetComponent<Item>());
        }
    }
}
