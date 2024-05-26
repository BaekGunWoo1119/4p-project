using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretShop : MonoBehaviour
{

    public GameObject[] brightImages;
    public GameObject[] strokeImages;
    public float rotationSpeed = 0.1f;
    private int enableIndex = 0;
    private bool stopRotate;

    public HiddenShop_Slot[] HS_Slots;
    private Item slotsItem;

    void Start()
    {
        for (int i = 0; i < brightImages.Length; i++)
        {
            brightImages[i].SetActive(false);
            strokeImages[i].SetActive(false);
        }

        if (brightImages.Length > 0)
        {
            brightImages[enableIndex].SetActive(true);
            strokeImages[enableIndex].SetActive(true);
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
        Debug.Log("멈췄다");
        StopCoroutine(RotateImages());
        stopRotate = true;
        int i = enableIndex;
        int currentIndex = Random.Range(0, brightImages.Length);
        while (i < brightImages.Length)
        {
            if(i == currentIndex)
            {
                yield return new WaitForSeconds(0.6f);
                strokeImages[i].GetComponent<Image>().color = new Color(255f, 0f, 0f, 1f);
                yield break;
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

            Debug.Log(currentIndex);
            slotsItem = HS_Slots[currentIndex].transform.GetChild(4).GetComponent<Item>();
            Debug.Log(slotsItem.Name);
        }
    }

    IEnumerator RotateImages()
    {
        while (stopRotate == false)
        {
            yield return new WaitForSeconds(rotationSpeed);

            brightImages[enableIndex].SetActive(false);
            strokeImages[enableIndex].SetActive(false);

            enableIndex = (enableIndex + 1) % brightImages.Length;
            //다음 이미지 활성화
            brightImages[enableIndex].SetActive(true);
            strokeImages[enableIndex].SetActive(true);
        }
    }

    public void SlowRotate(float slowlySpeed)
    {
        rotationSpeed = rotationSpeed + slowlySpeed;
    }
}
