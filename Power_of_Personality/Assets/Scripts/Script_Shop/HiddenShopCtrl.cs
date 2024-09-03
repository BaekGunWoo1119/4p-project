using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HiddenShopCtrl : MonoBehaviour
{
    public InventoryCtrl InvenCtrl;

    public HiddenShop_Slot[] HS_Slots;
    public Item[] Items;
    public GameObject[] newItem;

    bool isSave = false;

    int C_Slots;
    int C_Items;

    public int[] L_ItemID;

    void Awake()
    {
        C_Slots = HS_Slots.Length;
        C_Items = Items.Length;
        newItem = new GameObject[C_Slots];

        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
    }

    void Start()
    {
        StartCoroutine(OpenHiddenShop());
    }

    void Update()
    {
        if(Shop_PortalCtrl.isShopOpen == true)
        {
            //상점이 열었을 때 다시 Start에 들어있는 코드 실행
            StartCoroutine(OpenHiddenShop());
        }
    }

    IEnumerator OpenHiddenShop()
    {
        GetRandomItemCode();
        // 레전더리 하나 랜덤으로 띄워주는 코드
        int legendary_int = Random.Range(32, 36);
        newItem[7] = Instantiate(Items[legendary_int].gameObject, HS_Slots[0].transform.position, Quaternion.identity);
        newItem[7].transform.parent = HS_Slots[7].transform;
        newItem[7].SetActive(false);
        Shop_PortalCtrl.isShopOpen = false;
        yield return null;
    }

    public void GetRandomItemCode()
    {
        int[] randItemCode = new int[C_Slots];
        bool isSame;

        if(isSave == false)
        {
            for (int i = 0; i < C_Slots - 1; ++i)
            {
                while (true)
                {
                    randItemCode[i] = Random.Range(0, C_Items);

                    // 배제된 인덱스인지 확인
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

            for (int i = 0; i < C_Slots - 1; ++i)
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
            for (int i = 0; i < C_Slots - 1; ++i)
            {
                if (newItem[i] != null)
                {
                    Destroy(newItem[i]);
                }
            }
            for (int i = 0; i < C_Slots - 1; ++i)
            {
                while (true)
                {
                    randItemCode[i] = Random.Range(0, C_Items);

                    // 배제된 인덱스인지 확인
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

            for (int i = 0; i < C_Slots - 1; ++i)
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