using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenShopCtrl : MonoBehaviour
{
    public HiddenShop_Slot[] HS_Slots;
    public Item[] Items;
    private GameObject[] newItem;

    bool isSave = false;

    int C_Slots;
    int C_Items;

    void Awake()
    {
        C_Slots = HS_Slots.Length;
        C_Items = Items.Length;
        newItem = new GameObject[C_Slots];
    }

    void Start()
    {
        GetRandomItemCode();
    }

    public void GetRandomItemCode()
    {
        int[] randItemCode = new int[8];
        bool isSame;
        
        if(isSave == false)
        {
            for (int i = 0; i < 8; ++i)
            {
                while (true)
                {
                    randItemCode[i] = Random.Range(0, C_Items);
                    isSame = false;

                    for (int j = 0; j < i; ++j)
                    {
                        if (randItemCode[j] == randItemCode[i])
                        {
                            isSame = true;
                            break;
                        }
                    }
                    if (!isSame) break;
                }
            }
        
            for (int i = 0; i < 8; ++i)
            {
                if (Items[randItemCode[i]] != null)
                {
                    newItem[i] = Instantiate(Items[randItemCode[i]].gameObject, HS_Slots[i].transform.position, Quaternion.identity);
                    newItem[i].transform.parent = HS_Slots[i].transform;
                    newItem[i].SetActive(false);
                }
            }

            isSave = true;
        }
        else if(isSave == true)
        {
            for (int i = 0; i < 8; ++i)
            {
                if (newItem[i] != null)
                {
                    Destroy(newItem[i]);
                }
            }
            for (int i = 0; i < 8; ++i)
            {
                while (true)
                {
                    randItemCode[i] = Random.Range(0, C_Items);
                    isSame = false;

                    for (int j = 0; j < i; ++j)
                    {
                        if (randItemCode[j] == randItemCode[i])
                        {
                            isSame = true;
                            break;
                        }
                    }
                    if (!isSame) break;
                }
            }
        
            for (int i = 0; i < 8; ++i)
            {
                if (Items[randItemCode[i]] != null)
                {
                    newItem[i] = Instantiate(Items[randItemCode[i]].gameObject, HS_Slots[i].transform.position, Quaternion.identity);
                    newItem[i].transform.parent = HS_Slots[i].transform;
                    newItem[i].SetActive(false);
                }
            }
        }
    }
}