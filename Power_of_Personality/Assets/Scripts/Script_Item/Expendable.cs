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
    public float PercentHP; //증가 체력
    public float PercentAD; //공격력 비율
    public float PercentArmor; //방어력 비율
    public float PercentSpeed; //행동속도 비율
}
