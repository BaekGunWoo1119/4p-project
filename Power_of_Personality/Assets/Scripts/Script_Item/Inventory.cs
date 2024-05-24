using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryObject;

    public Slot[] slots;

    private Vector3 originalScale;
    private Vector3 hiddenScale;

    void Start()
    {
        originalScale = new Vector3(1, 1, 1);
        hiddenScale = new Vector3(0, 0, 0);
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
