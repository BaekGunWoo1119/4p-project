using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_PortalCtrl : MonoBehaviour
{
    
    public static Inventory[] inventory;

    public Transform shopPos;
    public GameObject shopWindow;
    public GameObject orgWindow;
    private GameObject playerObj;
    private Vector3 playerPos;
    private GameObject thisObj;
    private float orgSpd;

    void Start()
    {
        thisObj = this.gameObject;
        StartCoroutine(FindInventory());
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            //Debug.Log("S키를 눌러 상점 진입");
            if(Input.GetKeyDown(KeyCode.S))
            {            
                Open_Shop(col);
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
        yield return new WaitForSeconds(0.03f);
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        inventory = new Inventory[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            inventory[i] = player[i].GetComponent<Inventory>();
        }
    }

    public void Open_Shop(Collider col)
    {
        //현재 플레이어 위치 및 현재 스테이지 저장 후 상점으로 넘기기
        playerObj = col.gameObject;
        playerPos = col.gameObject.transform.position;
        Transform col_trs = col.gameObject.transform;
        col_trs.position = shopPos.position;
        shopWindow.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Exit_Shop").SetActive(false);
        orgWindow.transform.localScale = new Vector3(0, 0, 0);
        Status.IsShop = true;
    }

    public void Exit_Shop()
    {
        playerObj.transform.position = playerPos;
        Status.IsShop = false;
        shopWindow.transform.localScale = new Vector3(0, 0, 0);
        orgWindow.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().CheckInven();
        inventory[0].ItemPlus();
        Destroy(GameObject.Find("Shop_Info_Canvas"));
        Destroy(thisObj);
    }
}
