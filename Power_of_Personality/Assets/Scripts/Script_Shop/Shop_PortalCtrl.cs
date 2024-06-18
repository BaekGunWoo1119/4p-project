using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_PortalCtrl : MonoBehaviour
{
    public Transform shopPos;
    public GameObject shopWindow;
    public GameObject orgWindow;
    private GameObject playerObj;
    private Vector3 playerPos;
    private float orgSpd;
    public static bool isShop = false;

    void OnTriggerStay(Collider col)
    {
        Debug.Log("플레이어가 아니라는데요~");
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("S키를 눌러 상점 진입");
            if(Input.GetKeyDown(KeyCode.S))
            {            
                //현재 플레이어 위치 및 현재 스테이지 저장 후 씬 넘기기
                playerObj = col.gameObject;
                playerPos = col.gameObject.transform.position;
                col.gameObject.transform.position = shopPos.position;
                shopWindow.transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("Exit_Shop").SetActive(false);
                orgWindow.transform.localScale = new Vector3(0, 0, 0);
                isShop = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("플레이어가 아니라는데요~");
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("꺼져");
        }
    }

    public void Exit_Shop()
    {
        playerObj.transform.position = playerPos;
        isShop = false;
        shopWindow.transform.localScale = new Vector3(0, 0, 0);
        orgWindow.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().CheckInven();
        GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>().InsertItem();
        GameObject.Find("InventoryCtrl").GetComponent<Inventory>().ItemPlus();
        //shopWindow.transform.localScale = new Vector3(0, 0, 0);
        //orgWindow.transform.localScale = new Vector3(1, 1, 1);
    }
}
