using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expendable : MonoBehaviour
{
    public Sprite itemSprite;

    public int itemID;
    public int SlotIndex;
    public string Name;
    public string Set; //세트
    public string Grade; // 등급
    public string Description; //설명
    public float AddHP; //증가 체력
    public float PercentAD; //공격력 비율
    public float PercentArmor; //방어력 비율
    public float PercentSpeed; //행동속도 비율
    public float PercentADC; // 평타 데미지 비율
    public float PercentAP; // 스킬 데미지 비율
    public float PercentCooltime; // 스킬 퍼센트 쿨감
    public float PercentFire; // 화속성 데미지 비율
    public float PercentIce; // 빙속성 데미지 비율
}
