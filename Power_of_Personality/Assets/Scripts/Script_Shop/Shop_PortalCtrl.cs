using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_PortalCtrl : MonoBehaviour
{
    
    public static Inventory[] inventory;

    public Transform shopPos;
    public GameObject shopWindow;
    public GameObject orgWindow;
    public ShopCtrl shopctrl; //상점 시작 시 리롤을 위해 불러옴
    private GameObject playerObj;
    private Vector3 playerPos;
    private GameObject thisObj;
    private GameObject exitshop;
    private float orgSpd;
    public static bool isShopOpen = false;

    void Start()
    {
        thisObj = this.gameObject;
        StartCoroutine(FindInventory());
        exitshop = GameObject.Find("Exit_Shop");
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            //Debug.Log("S키를 눌러 상점 진입");
            if(Input.GetKeyDown(KeyCode.S) && !Status.IsDie)
            {
                Open_Shop(col);
                GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().PotionCount++;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            //Debug.Log("상점 콜라이더 지나침");
        }
    }

    IEnumerator FindInventory()
    {
        yield return new WaitForSeconds(3f);
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        inventory = new Inventory[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            inventory[i] = player[i].GetComponent<Inventory>();
        }
    }

    public void Open_Shop(Collider col)
    {
        isShopOpen = true;
        Status.IsShop = true;
        //현재 플레이어 위치 및 현재 스테이지 저장 후 상점으로 넘기기
        playerObj = col.gameObject;
        playerPos = col.gameObject.transform.position;
        Transform col_trs = col.gameObject.transform;
        col_trs.position = shopPos.position;
        shopWindow.transform.localScale = new Vector3(1, 1, 1);
        exitshop.SetActive(false);
        orgWindow.transform.localScale = new Vector3(0, 0, 0);

        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().SetHadItem();

        if(shopctrl != null)
        {
            shopctrl.Reroll_Item();
            float currentCoin = PlayerPrefs.GetFloat("Coin");
            PlayerPrefs.SetFloat("Coin", currentCoin + 3);
        }
    }

    public void Exit_Shop()
    {
        Debug.Log("상점나가기");
        playerObj.transform.position = playerPos;
        Status.IsShop = false;
        shopWindow.transform.localScale = new Vector3(0, 0, 0);
        orgWindow.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().CheckInven();
        inventory[0].ItemPlus();
        Destroy(GameObject.Find("Shop_Info_Canvas"));
        Destroy(thisObj);
    }

    public void Exit_Shop_Multi()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        inventory = new Inventory[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            inventory[i] = player[i].GetComponent<Inventory>();
        }
        playerObj.transform.position = GameObject.FindGameObjectWithTag("CurrentSpawnPotint").transform.position;
        //playerObj.transform.position = GameObject.FindGameObjectsWithTag("CurrentSpawnPotint").transform.position;
        Status.IsShop = false;
        shopWindow.transform.localScale = new Vector3(0, 0, 0);
        orgWindow.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().CheckInven();
        inventory[0].ItemPlus();
        Destroy(GameObject.Find("Shop_Info_Canvas"));
    }
}
