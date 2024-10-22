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
        hpBarImg.gameObject.transform.parent.localScale = new Vector3(0, 0, 0);
    }
    
    void Update()
    {
        //Debug.Log("HP바 변경 잘 됨");
        bossType = GameObject.FindWithTag("Boss");
        if(bossType != null){
            if(bossType.name == "Druid(Clone)")
            {
                hpBarImg.sprite = bossHpImg[0];
            }

            if(bossType.name == "Stone_Golem(Clone)")
            {
                hpBarImg.sprite = bossHpImg[1];
            }

            if(bossType.name == "Ogre(Clone)")
            {
                hpBarImg.sprite = bossHpImg[2];
            }   

            if(bossType.name == "DemonKing")
            {
                hpBarImg.sprite = bossHpImg[3];
            }   

            if(bossType.name == "Server_Druid(Clone)")
            {
                hpBarImg.sprite = bossHpImg[0];
            }

            if(bossType.name == "Server_Stone_Golem(Clone)")
            {
                hpBarImg.sprite = bossHpImg[1];
            }

            if(bossType.name == "Server_Ogre(Clone)")
            {
                hpBarImg.sprite = bossHpImg[2];
            }

            if(bossType.name == "Server_DemonKing(Clone)")
            {
                hpBarImg.sprite = bossHpImg[3];
            }             
        }
    }
}
