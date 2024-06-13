using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCtrl : MonoBehaviour
{
    public HiddenShop_Slot[] HS_Slots;
    public Item[] Items;
    private GameObject[] newItem;

    bool isSave = false;

    int C_Slots;
    int C_Items;

    //아이템 가격
    private float itemCost;

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
        int[] randItemCode = new int[C_Slots];
        bool isSame;
        
        if(isSave == false)
        {
            for (int i = 0; i < C_Slots; ++i)
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
        
            for (int i = 0; i < C_Slots; ++i)
            {
                if (Items[randItemCode[i]] != null)
                {
                    newItem[i] = Instantiate(Items[randItemCode[i]].gameObject, HS_Slots[i].transform.position, Quaternion.identity);
                    newItem[i].transform.parent = HS_Slots[i].transform;
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(newItem[i].GetComponent<Item>().Name, 0); 
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(newItem[i].GetComponent<Item>().Description, 1); 
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(itemCost.ToString(), 2); 

                    //색깔 변경
                    if(newItem[i].GetComponent<Item>().Grade == "레어")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(74, 134, 232, 0);
                    else if(newItem[i].GetComponent<Item>().Grade == "에픽")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(153, 0, 255, 0);
                    else if(newItem[i].GetComponent<Item>().Grade == "유니크")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(255, 255, 0, 0);

                    newItem[i].SetActive(false);
                }
            }

            isSave = true;
        }
        else if(isSave == true)
        {
            for (int i = 0; i < C_Slots; ++i)
            {
                if (newItem[i] != null)
                {
                    Destroy(newItem[i]);
                }
            }
            for (int i = 0; i < C_Slots; ++i)
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
        
            for (int i = 0; i < C_Slots; ++i)
            {
                if (Items[randItemCode[i]] != null)
                {
                    newItem[i] = Instantiate(Items[randItemCode[i]].gameObject, HS_Slots[i].transform.position, Quaternion.identity);
                    newItem[i].transform.parent = HS_Slots[i].transform;
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(newItem[i].GetComponent<Item>().Name, 0); 
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(newItem[i].GetComponent<Item>().Description, 1); 
                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(itemCost.ToString(), 2); 

                    //색깔 변경
                    if(newItem[i].GetComponent<Item>().Grade == "레어")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(74, 134, 232, 0);
                    else if(newItem[i].GetComponent<Item>().Grade == "에픽")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(153, 0, 255, 0);
                    else if(newItem[i].GetComponent<Item>().Grade == "유니크")
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(255, 255, 0, 0);

                    newItem[i].SetActive(false);
                }
            }
        }
    }
}
