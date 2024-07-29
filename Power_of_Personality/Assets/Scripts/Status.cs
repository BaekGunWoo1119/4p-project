using System;
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
    public static float TotalAD = 1f; // 총 공격력
    public static float TotalDamage = 1f; // 총 데미지 (속성뎀 포함)
    public static float TotalAP = 1f; // 총 스킬뎀
    public static float TotalADC = 1f; //총 평타뎀
    public static float TotalArmor = 1f; //총 방어력
    public static float TotalFire = 1f; //총 화속성 데미지
    public static float TotalIce = 1f; //총 빙속성 데미지
    public static float TotalCooltime = 1f; //총 쿨타임
    public static float TotalSpeed = 1f; //총 행동속도
    public static float EXP = 0f; // 현재 경험치
    public string CurProperty; //현재 플레이어 속성
    public static int[] itemIds = new int[32];
    public static int set1Count = 0;
    public static int set2Count = 0;
    public static int set3Count = 0;
    public static int set4Count = 0;
    public static int set5Count = 0;
    public static int set6Count = 0;
    public static int set7Count = 0;
    public static int set8Count = 0;
    public static bool set1_Activate = false;
    public static bool set2_Activate = false;
    public static bool set3_Activate = false;
    public static bool set4_Activate = false;
    public static bool set5_Activate = false;
    public static bool set6_Activate = false;
    public static bool set7_Activate = false;
    public static bool set8_Activate = false;
    static int a = 0;
    void Start()
    {
        StatUpdate();
        Array.Fill(itemIds, -1);
    }

    void Update()
    {
        SetDamage();
    }

    //속성 설정
    void SetDamage()
    {
        CurProperty = PlayerPrefs.GetString("property");
        if (CurProperty == "Ice")
        {
            TotalDamage = TotalAD * (FixedIce * (PercentIce * 0.01f));
        }
        else if (CurProperty == "Fire")
        {
            TotalDamage = TotalAD * (FixedFire * (PercentFire * 0.01f));
        }
    }
    //데미지 업데이트(아이템,레벨업 후에 사용)
    public static void StatUpdate()
    {
        TotalAD = (FixedAD * (PercentAD * 0.01f));
        TotalDamage = TotalAD * (FixedIce * (PercentIce * 0.01f));
        TotalAP = TotalDamage * (FixedAP * (PercentAP * 0.01f));
        TotalADC = TotalDamage * (FixedADC * (PercentADC * 0.01f));
    }
    public static void SetUpdate(int itemID)
    {
        itemIds[itemID] = itemID;
        #region 1번 세트효과
        if (itemID >= 0 && itemID <= 3 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set1Count++;
            }
            switch (set1Count)
            {
                case 1: Debug.Log("1번 세트 - " + set1Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set1Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set1Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set1Count + "개 활성화."); break;
            }
            if (set1Count == 4)
            {
                set1_Activate = true;
            }
        }
        #endregion
        #region 2번 세트효과
        if (itemID >= 4 && itemID <= 7 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set2Count++;
            }
            switch (set2Count)
            {
                case 1: Debug.Log("1번 세트 - " + set2Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set2Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set2Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set2Count + "개 활성화."); break;
            }
            if (set2Count == 4)
            {
                set2_Activate = true;
            }
        }
        #endregion
        #region 3번 세트효과
        if (itemID >= 8 && itemID <= 11 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set3Count++;
            }
            switch (set3Count)
            {
                case 1: Debug.Log("1번 세트 - " + set3Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set3Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set3Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set3Count + "개 활성화."); break;
            }
            if (set3Count == 4)
            {
                set3_Activate = true;
            }
        }
        #endregion
        #region 4번 세트효과
        if (itemID >= 12 && itemID <= 15 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set4Count++;
            }
            switch (set1Count)
            {
                case 1: Debug.Log("1번 세트 - " + set4Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set4Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set4Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set4Count + "개 활성화."); break;
            }
            if (set4Count == 4)
            {
                set4_Activate = true;
            }
        }
        #endregion
        #region 5번 세트효과
        if (itemID >= 16 && itemID <= 19 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set5Count++;
            }
            switch (set5Count)
            {
                case 1: Debug.Log("1번 세트 - " + set5Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set5Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set5Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set5Count + "개 활성화."); break;
            }
            if (set5Count == 4)
            {
                set5_Activate = true;
            }
        }
        #endregion
        #region 6번 세트효과
        if (itemID >= 20 && itemID <= 23 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set6Count++;
            }
            switch (set6Count)
            {
                case 1: Debug.Log("1번 세트 - " + set6Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set6Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set6Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set6Count + "개 활성화."); break;
            }
            if (set6Count == 4)
            {
                set6_Activate = true;
            }
        }
        #endregion
        #region 7번 세트효과
        if (itemID >= 24 && itemID <= 27 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set7Count++;
            }
            switch (set7Count)
            {
                case 1: Debug.Log("1번 세트 - " + set7Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set7Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set7Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set7Count + "개 활성화."); break;
            }
            if (set7Count == 4)
            {
                set7_Activate = true;
            }
        }
        #endregion
        #region 8번 세트효과
        if (itemID >= 28 && itemID <= 31 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                set8Count++;
            }
            switch (set8Count)
            {
                case 1: Debug.Log("1번 세트 - " + set8Count + "개 활성화."); break;
                case 2: Debug.Log("1번 세트 - " + set8Count + "개 활성화."); break;
                case 3: Debug.Log("1번 세트 - " + set8Count + "개 활성화."); break;
                case 4: Debug.Log("1번 세트 - " + set8Count + "개 활성화."); break;
            }
            if (set8Count == 4)
            {
                set8_Activate = true;
            }
        }
        #endregion
    }

    //스크롤 먹을시 초기화
    void LevelUP()
    {
        EXP = 0f;
    }
    //몹 처치시마다 사용
    void EXPUP()
    {
        EXP += 1f;
    }
}
