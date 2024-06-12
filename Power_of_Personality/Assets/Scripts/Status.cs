using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public static float HP = 100f;
    public static float MaxHP = 100f;
    public static float FixedAD = 50f; //공격력 고정
    public static float PercentAD = 100f; //공격력 비율
    public static float FixedArmor = 50f; //방어력 고정
    public static float PercentArmor = 100f; //방어력 비율
    public static float FixedSpeed = 50f; //행동속도   고정
    public static float PercentSpeed = 100f; //행동속도 비율
    public static float FixedADC = 50f; //평타 데미지 고정
    public static float PercentADC = 100f; // 평타 데미지 비율
    public static float FixedAP = 50f; // 스킬 데미지 고정
    public static float PercentAP = 100f; // 스킬 데미지 비율
    public static float FixedCooltime = 0f; // 스킬 고정 쿨감
    public static float PercentCooltime = 100f; // 스킬 퍼센트 쿨감
    public static float FixedFire = 50f; // 화속성 데미지 고정
    public static float PercentFire = 100f; // 화속성 데미지 비율
    public static float FixedIce = 50f; // 빙속성 데미지 고정
    public static float PercentIce = 100f; // 빙속성 데미지 비율
    public static float TotalAD = 0f; // 총 공격력
    public static float TotalDamage = 0f; // 총 데미지 (속성뎀 포함)
    public static float TotalAP = 0f; // 총 스킬뎀
    public static float TotalADC = 0f; //총 평타뎀
    public static float EXP = 0f; // 현재 경험치
    public string CurProperty; //현재 플레이어 속성


    void Start()    
    {
        StatUpdate();
    }

    void Update()
    {
        SetDamage();
        //Debug.Log(TotalADC);
    }

    //속성 설정
    void SetDamage(){
        CurProperty = PlayerPrefs.GetString("property");
        if(CurProperty == "Ice"){
            TotalDamage = TotalAD*(FixedIce*(PercentIce*0.01f));
        }
        else if(CurProperty == "Fire"){
            TotalDamage = TotalAD*(FixedFire*(PercentFire*0.01f));
        }
    }
    //데미지 업데이트(아이템,레벨업 후에 사용)
    public static void StatUpdate(){
        TotalAD = (FixedAD*(PercentAD*0.01f));
        TotalDamage = TotalAD*(FixedIce*(PercentIce*0.01f));
        TotalAP = TotalDamage*(FixedAP*(PercentAP*0.01f));
        TotalADC = TotalDamage*(FixedADC*(PercentADC*0.01f));
    }

    //스크롤 먹을시 초기화
    void LevelUP(){
        EXP = 0f;
    }
    //몹 처치시마다 사용
    void EXPUP(){
        EXP += 1f;
    }
}
