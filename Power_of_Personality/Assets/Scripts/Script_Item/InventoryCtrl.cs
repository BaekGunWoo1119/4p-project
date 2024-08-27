using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryCtrl : MonoBehaviour
{
    public static InventoryCtrl instance = null;
    public static Inventory[] inventory;

    public GameObject[] itemList;
    public Item[] collectedItems;
    public int[] collectedItemsID;

    public GameObject HiddenShopSlots;
    public GameObject NormalShopSlots;

    public int itemCount;

    public int PotionCount;
    public int StatPoint;
    
    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 생성 방지
        }

    }

    void Start()
    {
        //플레이어 확인(인벤토리 연결)
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        inventory = new Inventory[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            inventory[i] = player[i].GetComponent<Inventory>();
        }

        collectedItems = new Item[itemList.Length];
        collectedItemsID = new int[itemList.Length];

        PlayerPrefs.SetFloat("Coin", 0);

        for(int i = 0; i < collectedItemsID.Length; i++)
        {
            collectedItemsID[i] = -1;
        }

        PotionCount = 0;
        StatPoint = 0;

        HiddenShopSlots = GameObject.Find("HiddenShop_Slots");
       
        //NormalShopSlots = GameObject.Find("Slot");
        
        
    }   

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckInven();

        CheckPlayer();

        if(scene.name == "Hidden_Shop")
        {
            HiddenShopSlots = GameObject.Find("Slots");
        }
        else if(scene.name == "Normal_Shop")
        {
            NormalShopSlots = GameObject.Find("Slot");
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        CheckInven();
    }

    public void CheckInven()
    {

        for(int i = 0; i < itemCount; i++)
        {
            collectedItems[i] = itemList[collectedItemsID[i]].GetComponent<Item>();
        }
    }
    
    public void CheckPlayer()
    {
        //플레이어 확인(인벤토리 연결)
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        inventory = new Inventory[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            inventory[i] = player[i].GetComponent<Inventory>();
        }
    }

    public void ResetInven()
    {
        collectedItems = new Item[itemList.Length];
        collectedItemsID = new int[itemList.Length];

        PlayerPrefs.SetFloat("Coin", 0);

        for(int i = 0; i < collectedItemsID.Length; i++)
        {
            collectedItemsID[i] = -1;
        }

        PotionCount = 0;
        StatPoint = 0;
    }
}
