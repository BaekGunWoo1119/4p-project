using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Sprite[] bossHpImg;
    public Image hpBarImg;
    public GameObject bossType;
    
    void Start()
    {
        if(bossType.name == "Druid")
        {
            hpBarImg.sprite = bossHpImg[0];
        }

        if(bossType.name == "Golem")
        {
            hpBarImg.sprite = bossHpImg[1];
        }

        if(bossType.name == "Abomination")//기억 안나서 가칭
        {
            hpBarImg.sprite = bossHpImg[2];
        }

        hpBarImg.gameObject.transform.parent.localScale = new Vector3(0, 0, 0);
    }
}
