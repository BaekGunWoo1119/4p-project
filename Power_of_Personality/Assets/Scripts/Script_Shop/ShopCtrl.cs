using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ShopCtrl : MonoBehaviour
{
    public InventoryCtrl InvenCtrl;

    public HiddenShop_Slot[] HS_Slots;
    public Item[] Items;
    public Button[] BuyButton;
    public int buttonIndex;
    private GameObject[] newItem;
    public float[] itemCost;

    bool isSave = false;

    int C_Slots;
    int C_Items;

    //아이템 가격

    void Awake()
    {
        C_Slots = HS_Slots.Length;
        C_Items = Items.Length;
        newItem = new GameObject[C_Slots];
        itemCost = new float[C_Slots];

        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
    }

    void Start()
    {
        for(buttonIndex = 0; buttonIndex == BuyButton.Length; buttonIndex++)
        {
            BuyButton[buttonIndex].onClick.AddListener(BuyItem);
        }

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

                    if (InvenCtrl.collectedItemsID.Contains(randItemCode[i]))
                    {
                        continue;
                    }

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
                    if(newItem[i].GetComponent<Item>().Grade == "커먼")
                    {
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(127, 127, 127, 0);
                        itemCost[i] = 5f;
                    }
                    else if(newItem[i].GetComponent<Item>().Grade == "레어")
                    {
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(74, 134, 232, 0);
                        itemCost[i] = 10f;
                    }
                    else if(newItem[i].GetComponent<Item>().Grade == "에픽")
                    {    
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(153, 0, 255, 0);
                        itemCost[i] = 15f;
                    }
                    else if(newItem[i].GetComponent<Item>().Grade == "유니크")
                    {
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(255, 255, 0, 0);
                        itemCost[i] = 20f;
                    }
                    newItem[i].SetActive(false);
                }
            }

            isSave = true;
        }
        else if(isSave == true)
        {
            if(PlayerPrefs.GetFloat("Coin") >= 3)
            {
                float currentCoin = PlayerPrefs.GetFloat("Coin", 0);
                PlayerPrefs.SetFloat("Coin", currentCoin -3);

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

                        if (InvenCtrl.collectedItemsID.Contains(randItemCode[i]))
                        {
                            continue;
                        }

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
                        if(newItem[i].GetComponent<Item>().Grade == "커먼")
                        {
                            HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(127, 127, 127, 0);
                            itemCost[i] = 5f;
                        }
                        else if(newItem[i].GetComponent<Item>().Grade == "레어")
                        {
                            HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(74, 134, 232, 0);
                            itemCost[i] = 10f;
                        }
                        else if(newItem[i].GetComponent<Item>().Grade == "에픽")
                        {    
                            HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(153, 0, 255, 0);
                            itemCost[i] = 15f;
                        }
                        else if(newItem[i].GetComponent<Item>().Grade == "유니크")
                        {
                            HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeTextColor(255, 255, 0, 0);
                            itemCost[i] = 20f;
                        }
                        newItem[i].SetActive(false);
                    }
                }
            } else
            {
                Debug.Log("돈 부족");
            }
        }
    }

        void BuyItem()
        {

            if(PlayerPrefs.GetFloat("Coin") >= itemCost[buttonIndex])
            {
                float currentCoin = PlayerPrefs.GetFloat("Coin", 0);
                PlayerPrefs.SetFloat("Coin", currentCoin -3);
                if(InvenCtrl.collectedItems[InvenCtrl.itemCount] == null)
                {
                    InvenCtrl.collectedItemsID[InvenCtrl.itemCount] = newItem[buttonIndex].GetComponent<Item>().itemID;
                }
                    
                    InvenCtrl.itemCount++;
            }
        }
}
