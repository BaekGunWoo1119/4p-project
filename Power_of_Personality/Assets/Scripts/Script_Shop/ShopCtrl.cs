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
    public GameObject[] BuyButton;
    public GameObject[] newItem;
    public float[] itemCost;
    public GameObject[] soldOut;

    private GameObject warningWindow;



    bool isSave = false;
    public int[] L_ItemID;

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
        warningWindow = GameObject.Find("Sold_Warning");
    }

    void Start()
    {
        ShopCtrl currentInstance = this;
        //for문에다 넣으니 자꾸 에러 떠서 따로 빼둠...
        BuyButton[0].GetComponent<Button>().onClick.AddListener(() => currentInstance.BuyItem(0));
        BuyButton[1].GetComponent<Button>().onClick.AddListener(() => currentInstance.BuyItem(1));
        BuyButton[2].GetComponent<Button>().onClick.AddListener(() => currentInstance.BuyItem(2));

        for(int i = 0; i < BuyButton.Length; i++)
        {
            soldOut[i].SetActive(false);
        }

        warningWindow.SetActive(false);

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

                    if (InvenCtrl.collectedItemsID.Contains(randItemCode[i]) || L_ItemID.Contains(randItemCode[i]))
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
            if(PlayerPrefs.GetFloat("Coin") >= 3) //코인 소모 부분 추후 기획 논의 후 재구성
            {
                float currentCoin = PlayerPrefs.GetFloat("Coin", 0);
                PlayerPrefs.SetFloat("Coin", currentCoin - 3);
                GameObject.Find("CoinText").GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("Coin").ToString();

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
                        soldOut[i].SetActive(false);

                        if (InvenCtrl.collectedItemsID.Contains(randItemCode[i])|| L_ItemID.Contains(randItemCode[i]))
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
        float iCost = itemCost[index];
        if(PlayerPrefs.GetFloat("Coin") >= iCost)
        {
            float currentCoin = PlayerPrefs.GetFloat("Coin");
            PlayerPrefs.SetFloat("Coin", currentCoin - iCost);
            GameObject.Find("CoinText").GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("Coin").ToString();
            if(InvenCtrl.collectedItems[InvenCtrl.itemCount] == null)
            {
                if(newItem[index].GetComponent<Item>().itemID == 50)
                {
                    InvenCtrl.PotionCount ++;
                    Debug.Log("체력 포션 :" + InvenCtrl.PotionCount + "개 보유중");
                    InvenCtrl.itemCount--;
                }else if(newItem[index].GetComponent<Item>().itemID == 51)
                {
                    InvenCtrl.StatPoint ++;
                    Debug.Log("스탯포인트 :" + InvenCtrl.StatPoint + "개 보유중");
                    InvenCtrl.itemCount--;
                }else if(newItem[index].GetComponent<Item>().itemID == 52)
                {
                    InvenCtrl.ADPotionCount ++;
                    Debug.Log("공격력 포션 :" + InvenCtrl.ADPotionCount + "개 보유중");
                    InvenCtrl.itemCount--;
                }else if(newItem[index].GetComponent<Item>().itemID == 53)
                {
                    InvenCtrl.ArmorPotionCount ++;
                    Debug.Log("방어력 포션 :" + InvenCtrl.ArmorPotionCount + "개 보유중");
                    InvenCtrl.itemCount--;
                }else
                {                
                    InvenCtrl.collectedItemsID[InvenCtrl.itemCount] = newItem[index].GetComponent<Item>().itemID;
                }
            }
            
            Debug.Log(index + "번 구매 완료");
            soldOut[index].SetActive(true);

            InvenCtrl.itemCount++;
        }
        else
        {
            StartCoroutine(Sold_Warning());
        }
    }

    IEnumerator Sold_Warning()
    {
        if(warningWindow.activeSelf == false)
        {
            warningWindow.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            warningWindow.SetActive(false);
        }
        else
        {
            while(warningWindow.activeSelf == true)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.05f);
            warningWindow.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            warningWindow.SetActive(false);
        }
    }
}
