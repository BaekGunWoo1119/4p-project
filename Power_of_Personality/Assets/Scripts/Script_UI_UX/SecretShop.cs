using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretShop : MonoBehaviour
{
    public InventoryCtrl InvenCtrl;

    public GameObject[] brightImages;
    public GameObject[] strokeImages;
    public float rotationSpeed = 0.1f;
    private int enableIndex = 0;
    private bool stopRotate;

    public HiddenShop_Slot[] HS_Slots;
    private Item slotsItem;

    private GameObject exitBtn;

    void Awake()
    {
        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
    }

    void Start()
    {
        exitBtn = GameObject.Find("Exit_Shop");
        for (int i = 0; i < brightImages.Length; i++)
        {
            brightImages[i].SetActive(false);
            strokeImages[i].SetActive(false);
        }

        // 회전 진행
        //StartCoroutine(RotateImages());
    }

    public void StartAtRandom()
    {
        StartCoroutine(RotateImages());
    }

    public void StopAtRandom()
    {
        StartCoroutine(StopImages());
    }


    IEnumerator StopImages()
    {
        int currentIndex = 7;//Random.Range(0, brightImages.Length);
        Debug.Log("멈췄다");
        stopRotate = true;
        StopCoroutine(RotateImages());
        int i = enableIndex;
        Debug.Log(currentIndex);
        while (i < brightImages.Length)
        {
            if(i == currentIndex)
            {
                yield return new WaitForSeconds(0.6f);
                brightImages[i].SetActive(true);
                strokeImages[i].SetActive(true);
                strokeImages[i].GetComponent<Image>().color = new Color(255f, 0f, 0f, 1f);
                exitBtn.SetActive(true);
                if(InvenCtrl.collectedItems[InvenCtrl.itemCount] == null)
                {
                    slotsItem = HS_Slots[currentIndex].transform.GetChild(5).GetComponent<Item>();
                    InvenCtrl.collectedItemsID[InvenCtrl.itemCount] = slotsItem.itemID;
                    Debug.Log(currentIndex);
                    Debug.Log(slotsItem.Name);
                    InvenCtrl.itemCount++;
                    yield break;
                }
            }
            else if(i == 7)
            {
                yield return new WaitForSeconds(0.6f);
                brightImages[i].SetActive(false);
                strokeImages[i].SetActive(false);
                i = 0;
                brightImages[i].SetActive(true);
                strokeImages[i].SetActive(true);
            }
            else
            {
                yield return new WaitForSeconds(0.6f);
                brightImages[i].SetActive(false);
                strokeImages[i].SetActive(false);
                i = i + 1;
                brightImages[i].SetActive(true);
                strokeImages[i].SetActive(true);
            }
        }
    }

    IEnumerator RotateImages()
    {
        while (stopRotate == false)
        {
            brightImages[enableIndex].SetActive(true);
            strokeImages[enableIndex].SetActive(true);

            yield return new WaitForSeconds(rotationSpeed);

            brightImages[enableIndex].SetActive(false);
            strokeImages[enableIndex].SetActive(false);

            enableIndex = (enableIndex + 1) % brightImages.Length;
        }
    }

    public void SlowRotate(float slowlySpeed)
    {
        rotationSpeed = rotationSpeed + slowlySpeed;
    }
}
