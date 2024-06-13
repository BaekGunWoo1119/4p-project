using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class ShopCtrl : MonoBehaviour
{
    public InventoryCtrl InvenCtrl;

    public HiddenShop_Slot[] HS_Slots;
    public Item[] Items;
    private Button BuyButton1;
    private Button BuyButton2;
    private Button BuyButton3;
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
        BuyButton1 = GameObject.Find("Pick(1)").GetComponent<Button>();
        BuyButton2 = GameObject.Find("Pick(2)").GetComponent<Button>();
        BuyButton3 = GameObject.Find("Pick(3)").GetComponent<Button>();

        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
    }

    void Start()
    {
        BuyButton1.onClick.AddListener(() => BuyItem(0));
        BuyButton2.onClick.AddListener(() => BuyItem(1));
        BuyButton3.onClick.AddListener(() => BuyItem(2));

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

                    HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(itemCost[i].ToString(), 2); 
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
                        //가격 적용
                        HS_Slots[i].transform.gameObject.GetComponent<TextChange>().ChangeText(itemCost[i].ToString(), 2); 
                        Debug.Log(itemCost[i] + "로 가격 변경 완료");
                        newItem[i].SetActive(false);
                    }
                }
            } else
            {
                Debug.Log("돈 부족");
            }
        }
    }

    void BuyItem(int index)
    {
        Debug.Log("구매 완료");
        if(PlayerPrefs.GetFloat("Coin") >= itemCost[index])
        {
            float currentCoin = PlayerPrefs.GetFloat("Coin");
            PlayerPrefs.SetFloat("Coin", currentCoin - itemCost[index]);
            GameObject.Find("CoinText").GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("Coin").ToString();
            if(InvenCtrl.collectedItems[InvenCtrl.itemCount] == null)
            {
                InvenCtrl.collectedItemsID[InvenCtrl.itemCount] = newItem[index].GetComponent<Item>().itemID;
            }
                    
            InvenCtrl.itemCount++;
        }
    }
}
