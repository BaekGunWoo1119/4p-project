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
    public static float FixedSpeed = 100f; //행동속도   고정
    public static float PercentSpeed = 100f; //행동속도 비율
    public static float FixedADC = 50f; //평타 데미지 고정
    public static float PercentADC = 100f; // 평타 데미지 비율
    public static float FixedAP = 25f; // 스킬 데미지 고정
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
    public static float DisplayAP = 1f; // 보여주기용 AP
    public static float DisplayADC = 1f; // 보여주기용 ADC
    public static float TotalADC = 1f; //총 평타뎀
    public static float TotalArmor = 1f; //총 방어력
    public static float TotalFire = 1f; //총 화속성 데미지
    public static float TotalIce = 1f; //총 빙속성 데미지
    public static float TotalCooltime = 1f; //총 쿨타임
    public static float TotalSpeed = 1f; //총 행동속도
    public static float EXP = 0f; // 현재 경험치
    public string CurProperty; //현재 플레이어 속성
    public static int extraLife = 0;    // 플레이어 추가 목숨
    public static int[] itemIds = new int[60];
    public static int set1Count = 0;
    public static int set2Count = 0;
    public static int set3Count = 0;
    public static int set4Count = 0;
    public static int set5Count = 0;
    public static int set6Count = 0;
    public static int set7Count = 0;
    public static int set8Count = 0;

    
    // 1번 세트
    public static bool set1_Activate = false;
    public static bool set1_3_Activated = false;
    public static bool set1_4_Activated = false;

    //2번 세트
    public static bool set2_Activate = false;
    public static bool set2_3_Activated = false;
    public static bool set2_4_Activated = false;
    public static bool set2_3_Effect_Activated = false;

    //3번 세트
    public static bool set3_Activate = false;
    public static bool set3_3_Activated = false;
    public static bool set3_4_Activated = false;

    //4번 세트
    public static bool set4_Activate = false;
    public static bool set4_3_Activated = false;
    public static bool set4_4_Activated = false;

    //5번 세트
    public static bool set5_Activate = false;
    public static bool set5_3_Activated = false;
    public static bool set5_4_Activated = false;

    //6번 세트
    public static bool set6_Activate = false;
    public static bool set6_3_Activated = false;
    public static bool set6_4_Activated = false;

    //7번 세트
    public static bool set7_Activate = false;
    public static bool set7_3_Activated = false;
    public static bool set7_4_Activated = false;

    //8번 세트
    public static bool set8_Activate = false;
    public static bool set8_3_Activated = false;

    public static bool IsShop = false; //현재 상점인지 확인
    public static bool Spell_TimeSlowdown_ON = false; //보조스킬 시간 감속 (09.18 정도훈)
    static int a = 0;
    public static int BonusStat;
    public static string MBTI; 
    void Start()
    {
        isDie();
        Array.Fill(itemIds, -1);
        IsShop = false;
        BonusStat = PlayerPrefs.GetInt("Stat") * 5;
        MBTI = PlayerPrefs.GetString("PlayerMBTI");
        ApplyBonusStat();
        ApplyMBTI(MBTI);
        StatUpdate();
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
            TotalDamage = TotalAD * (FixedIce * (PercentIce * 0.01f)*0.0001f);
        }
        else if (CurProperty == "Fire")
        {
            TotalDamage = TotalAD * (FixedFire * (PercentFire * 0.01f)*0.0001f);
        }
    }
    //데미지 업데이트(아이템,레벨업 후에 사용)
    public static void StatUpdate()
    {
        TotalAD = (FixedAD * (PercentAD * 0.01f));
        TotalFire = FixedFire * (PercentFire * 0.01f);
        TotalIce = FixedIce * (PercentIce * 0.01f);
        TotalDamage = TotalAD * TotalIce * 0.0001f;
        TotalAP = TotalDamage * (FixedAP * (PercentAP * 0.01f));
        TotalADC = TotalDamage * (FixedADC * (PercentADC * 0.01f));
        TotalArmor = FixedArmor * (PercentArmor * 0.01f);
        TotalSpeed = FixedSpeed * (PercentSpeed * 0.01f) *0.01f;
        TotalSpeed = (float)Math.Round(TotalSpeed, 2); //소수 둘째자리까지만 반올림 (09.18 정도훈)
        DisplayAP = FixedAP * (PercentAP * 0.01f);
        DisplayADC = FixedADC * (PercentADC * 0.01f);
    }
    public static void SetUpdate(int itemID)
    {
        //Debug.Log("Debug.Log(itemToBeAdded.itemID);"+itemID);
        itemIds[itemID] = itemID;
        #region 1번 세트효과 ATK
        if (itemID >= 0 && itemID <= 3 && set1_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set1Count++;
            }
            switch (set1Count)
            {
                // case 1: 
                //     Debug.Log("1번 세트 - " + set1Count + "개 활성화.");
                //     Debug.Log("플레이어 공격력 20 증가. 증가 전 = " + FixedAD + "증가 후 = " + (FixedAD + 20f));
                //     FixedAD += 20f;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("1번 세트 - " + set1Count + "개 활성화.");
                    Debug.Log("플레이어 공격력 20% 증가. 증가 전 = " + PercentAD + "증가 후 = " + PercentAD * 1.2f);
                    PercentAD *= 1.2f;
                    StatUpdate();
                    break;
                case 3: // 3세트 N% 확률로 N% 추가 데미지, 플레이어 몸 주변 아우라
                    Debug.Log("1번 세트 - " + set1Count + "개 활성화.");
                    set1_3_Activated = true;
                    Debug.Log("플레이어 공격 시 일정 확률로 추가 데미지 활성화. 현재 상태 = " + set1_3_Activated);
                    break;
                case 4: 
                    Debug.Log("1번 세트 - " + set1Count + "개 활성화.");
                    set1_4_Activated = true;
                    Debug.Log("피해량 비례 체력 회복 +N% 활성화. 현재 상태 = " + set1_4_Activated);
                    break;
            }
            if (set1Count == 4)
            {
                set1_Activate = true;
            }
        }
        #endregion
        #region 2번 세트효과 DEF
        if (itemID >= 4 && itemID <= 7 && set2_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set2Count++;
            }
            switch (set2Count)
            {
                // case 1: 
                //     Debug.Log("2번 세트 - " + set2Count + "개 활성화.");
                //     Debug.Log("플레이어 방여력 20 증가. 증가 전 = " + FixedArmor + "증가 후 = " + (FixedArmor + 20f));
                //     FixedArmor += 20f;
                //     break;
                case 2: 
                    Debug.Log("2번 세트 - " + set2Count + "개 활성화.");
                    Debug.Log("플레이어 방여력 20% 증가. 증가 전 = " + PercentArmor + "증가 후 = " + PercentArmor * 1.2f);
                    PercentArmor *= 1.2f;
                    break;
                case 3: 
                    Debug.Log("2번 세트 - " + set2Count + "개 활성화.");
                    set2_3_Activated = true;
                    set2_3_Effect_Activated = true;
                    Debug.Log("피격 시 반격 데미지 활성화. 현재 상태 = " + set2_3_Activated);
                    break;
                case 4: 
                    Debug.Log("2번 세트 - " + set2Count + "개 활성화.");
                    set2_4_Activated = true;
                    Debug.Log("플레이어 추가 목숨 활성화. 현재 상태 = " + set2_3_Activated);
                    extraLife += 1;
                    break;
            }
            if (set2Count == 4)
            {
                set2_Activate = true;
            }
        }
        #endregion
        #region 3번 세트효과 FIRE
        if (itemID >= 8 && itemID <= 11 && set3_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set3Count++;
            }
            switch (set3Count)
            {
                // case 1: 
                //     Debug.Log("3번 세트 - " + set3Count + "개 활성화.");
                //     Debug.Log("플레이어 화속성 데미지 20 증가. 증가 전 = " + FixedFire + "증가 후 = " + (FixedFire + 20f));
                //     FixedFire += 20f;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("3번 세트 - " + set3Count + "개 활성화.");
                    Debug.Log("플레이어 화속성 데미지 20% 증가. 증가 전 = " + PercentFire + "증가 후 = " + PercentFire * 1.2f);
                    PercentFire *= 1.2f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("3번 세트 - " + set3Count + "개 활성화.");
                    set3_3_Activated = true;
                    Debug.Log("플레이어 무기 화속성 이펙트, 화속성 약점 추가데미지 + N% 활성화. 현재 상태 = " + set3_3_Activated);
                    break;
                case 4: 
                    Debug.Log("3번 세트 - " + set3Count + "개 활성화.");
                    set3_4_Activated = true;
                    Debug.Log("모든 몬스터 화속성 약점으로 설정 활성화. 현재 상태 =" + set3_4_Activated);
                    break;
            }
            if (set3Count == 4)
            {
                set3_Activate = true;
            }
        }
        #endregion
        #region 4번 세트효과 ICE
        if (itemID >= 12 && itemID <= 15 && set4_Activate == false)
        {
            Debug.Log("얼음세트먹음");
            if (itemIds[itemID] == itemID)
            {
                //set4Count++;
            }
            switch (set4Count)
            {
                // case 1: 
                //     Debug.Log("4번 세트 - " + set4Count + "개 활성화.");
                //     Debug.Log("플레이어 빙속성 데미지 20 증가. 증가 전 = " + FixedIce + "증가 후 = " + (FixedIce + 20f));
                //     FixedIce += 20f;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("4번 세트 - " + set4Count + "개 활성화.");
                    Debug.Log("플레이어 빙속성 데미지 20% 증가. 증가 전 = " + PercentIce + "증가 후 = " + PercentIce * 1.2f);
                    PercentIce *= 1.2f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("4번 세트 - " + set4Count + "개 활성화.");
                    set4_3_Activated = true;
                    Debug.Log("플레이어 무기 빙속성 이펙트, 빙속성 약점 추가데미지 + N% 활성화. 현재 상태 = " + set4_3_Activated);
                    // 플레이어가 빙속성일 시 무기에 빙속성 이펙트
                    break;
                case 4: 
                    Debug.Log("4번 세트 - " + set4Count + "개 활성화.");
                    set4_4_Activated = true;
                    Debug.Log("모든 몬스터 빙속성 약점으로 설정 활성화. 현재 상태 =" + set4_4_Activated);
                    break;
            }
            if (set4Count == 4)
            {
                set4_Activate = true;
            }
        }
        #endregion
        #region 5번 세트효과 ADC
        if (itemID >= 16 && itemID <= 19 && set5_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set5Count++;
            }
            switch (set5Count)
            {
                // case 1: 
                //     Debug.Log("5번 세트 - " + set5Count + "개 활성화.");
                //     Debug.Log("플레이어 평타 데미지 20 증가. 증가 전 = " + FixedADC + "증가 후 = " + (FixedADC + 20f));
                //     FixedADC += 20f;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("5번 세트 - " + set5Count + "개 활성화.");
                    Debug.Log("플레이어 평타 데미지 20% 증가. 증가 전 = " + PercentADC + "증가 후 = " + PercentADC * 1.2f);
                    PercentADC *= 1.2f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("5번 세트 - " + set5Count + "개 활성화.");
                    set5_3_Activated = true;
                    Debug.Log("플레이어 2타 3타 추가 데미지 활성화. 현재 상태 = " + set5_3_Activated);
                    break;
                case 4: 
                    Debug.Log("5번 세트 - " + set5Count + "개 활성화.");
                    set5_4_Activated = true;
                    Debug.Log("평타 3타 칠때 마다 7초간 공격력 +N% (최대 3회 누적) 활성화. 현재 상태 = " + set5_4_Activated);
                    break;
            }
            if (set5Count == 4)
            {
                set5_Activate = true;
            }
        }
        #endregion
        #region 6번 세트효과 AP
        if (itemID >= 20 && itemID <= 23 && set6_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set6Count++;
            }
            switch (set6Count)
            {
                // case 1: 
                //     Debug.Log("6번 세트 - " + set6Count + "개 활성화.");
                //     Debug.Log("플레이어 스킬 데미지 20 증가. 증가 전 = " + FixedAP + "증가 후 = " + (FixedAP + 20f));
                //     FixedAP += 20f;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("6번 세트 - " + set6Count + "개 활성화.");
                    Debug.Log("플레이어 스킬 데미지 20% 증가. 증가 전 = " + PercentAP + "증가 후 = " + PercentAP * 1.2f);
                    PercentAP *= 1.2f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("6번 세트 - " + set6Count + "개 활성화.");
                    set6_3_Activated = true;
                    Debug.Log("플레이어 스킬 Q, W 추가 데미지 활성화. 현재 상태 = " + set6_3_Activated);
                    break;
                case 4: 
                    Debug.Log("6번 세트 - " + set6Count + "개 활성화.");
                    set6_4_Activated = true;
                    Debug.Log("플레이어 스킬 E 추가 데미지 활성화. 현재 상태 = " + set6_4_Activated);
                    break;
            }
            if (set6Count == 4)
            {
                set6_Activate = true;
            }
        }
        #endregion
        #region 7번 세트효과 SPEED
        if (itemID >= 24 && itemID <= 27 && set7_Activate == false)
        {
            if (itemIds[itemID] == itemID)
            {
                //set7Count++;
            }
            switch (set7Count)
            {
                // case 1: 
                //     Debug.Log("7번 세트 - " + set7Count + "개 활성화.");
                //     Debug.Log("플레이어 행동속도 20 증가. 증가 전 = " + FixedSpeed + "증가 후 = " + (FixedSpeed + 20f));
                //     FixedSpeed += 20;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("7번 세트 - " + set7Count + "개 활성화.");
                    Debug.Log("플레이어 행동속도 10% 증가. 증가 전 = " + PercentSpeed + "증가 후 = " + PercentSpeed * 1.1f);
                    PercentSpeed *= 1.1f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("7번 세트 - " + set7Count + "개 활성화.");
                    set7_3_Activated = true;
                    Debug.Log("플레이어 공격 시 추가 데미지, 플레이어 등 뒤에 잔상 활성화. 현재 상태 = " + set7_3_Activated);
                    break;
                case 4: 
                    Debug.Log("7번 세트 - " + set7Count + "개 활성화.");
                    set7_4_Activated = true;
                    Debug.Log("플레이어 스킬 추가 데미지 활성화. 현재 상태 = " + set7_4_Activated);
                    break;
            }
            if (set7Count == 4)
            {
                set7_Activate = true;
            }
        }
        #endregion
        #region 8번 세트효과 COOLTIME
        if (itemID >= 28 && itemID <= 31 && set8_Activate == false)
        {
            Debug.Log("8번 세트머금");
            if (itemIds[itemID] == itemID)
            {
                //set8Count++;
            }
            switch (set8Count)
            {
                // case 1: 
                //     Debug.Log("8번 세트 - " + set8Count + "개 활성화.");
                //     Debug.Log("플레이어 쿨타임 감소 1 증가. 증가 전 = " + FixedCooltime + "증가 후 = " + (FixedCooltime + 1f));
                //     FixedCooltime += 1;
                //     StatUpdate();
                //     break;
                case 2: 
                    Debug.Log("8번 세트 - " + set8Count + "개 활성화.");
                    Debug.Log("플레이어 쿨타임 감소 20% 증가. 증가 전 = " + PercentCooltime + "증가 후 = " + PercentCooltime * 1.2f);
                    PercentCooltime *= 1.2f;
                    StatUpdate();
                    break;
                case 3: 
                    Debug.Log("8번 세트 - " + set8Count + "개 활성화.");
                    set8_3_Activated = true;
                    Debug.Log("스킬 사용 시 플레이어 주변 시계 이펙트 활성화. 현재 상태 = " + set8_3_Activated);
                    break;
                case 4: Debug.Log("8번 세트 - " + set8Count + "개 활성화."); break;
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

    public static void isDie()
    {
        set1_Activate = false;
        set1_3_Activated = false;
        set1_4_Activated = false;

        set2_Activate = false;
        set2_3_Activated = false;
        set2_4_Activated = false;
        set2_3_Effect_Activated = false;

        set3_Activate = false;
        set3_3_Activated = false;
        set3_4_Activated = false;

        set4_Activate = false;
        set4_3_Activated = false;
        set4_4_Activated = false;

        set5_Activate = false;
        set5_3_Activated = false;
        set5_4_Activated = false;

        set6_Activate = false;
        set6_3_Activated = false;
        set6_4_Activated = false;

        set7_Activate = false;
        set7_3_Activated = false;
        set7_4_Activated = false;

        set8_Activate = false;
        set8_3_Activated = false;

        set1Count = 0;
        set2Count = 0;
        set3Count = 0;
        set4Count = 0;
        set5Count = 0;
        set6Count = 0;
        set7Count = 0;
        set8Count = 0;



        MaxHP = 100f;
        FixedAD = 50f; //공격력 고정
        PercentAD = 100f; //공격력 비율
        FixedArmor = 50f; //방어력 고정
        PercentArmor = 100f; //방어력 비율
        FixedSpeed = 100f; //행동속도   고정
        PercentSpeed = 100f; //행동속도 비율
        FixedADC = 50f; //평타 데미지 고정
        PercentADC = 100f; // 평타 데미지 비율
        FixedAP = 50f; // 스킬 데미지 고정
        PercentAP = 100f; // 스킬 데미지 비율
        FixedCooltime = 0f; // 스킬 고정 쿨감
        PercentCooltime = 100f; // 스킬 퍼센트 쿨감
        FixedFire = 50f; // 화속성 데미지 고정
        PercentFire = 100f; // 화속성 데미지 비율
        FixedIce = 50f; // 빙속성 데미지 고정
        PercentIce = 100f; // 빙속성 데미지 비율
        TotalAD = 1f; // 총 공격력
        TotalDamage = 1f; // 총 데미지 (속성뎀 포함)
        TotalAP = 1f; // 총 스킬뎀
        DisplayAP = 1f; // 보여주기용 AP
        DisplayADC = 1f; // 보여주기용 ADC
        TotalADC = 1f; //총 평타뎀
        TotalArmor = 1f; //총 방어력
        TotalFire = 1f; //총 화속성 데미지
        TotalIce = 1f; //총 빙속성 데미지
        TotalCooltime = 1f; //총 쿨타임
        TotalSpeed = 1f; //총 행동속도
        EXP = 0f; // 현재 경험치
    }

    #region 보너스 스탯 적용
    void ApplyBonusStat(){
        FixedAD+=BonusStat;
        FixedArmor+=BonusStat;
        FixedSpeed+=BonusStat;
        FixedADC+=BonusStat;
        FixedAP+=BonusStat;
        FixedFire+=BonusStat;
        FixedIce+=BonusStat;
    }
    #endregion
    #region MBTI 스탯 적용
    void ApplyMBTI(string MBTI){
        switch(MBTI){
            case "INFP":
                FixedArmor+=10;
                FixedSpeed+=10;
                FixedAP+=10;
                FixedFire+=10;
                break;
            case "INFJ":
                FixedArmor+=10;
                FixedAP+=10;
                FixedFire+=10;
                FixedCooltime+=1;
                break;
            case "INTP":
                FixedArmor+=10;
                FixedSpeed+=10;
                FixedAP+=10;
                FixedIce+=10;
                break;
            case "INTJ":
                FixedArmor+=10;
                FixedAP+=10;
                FixedIce+=10;
                FixedCooltime+=1;
                break;
            case "ISFP":
                FixedArmor+=10;
                FixedSpeed+=10;
                FixedADC+=10;
                FixedFire+=10;
                break;
            case "ISFJ":
                FixedArmor+=10;
                FixedADC+=10;
                FixedFire+=10;
                FixedCooltime+=1;
                break;
            case "ISTP":
                FixedArmor+=10;
                FixedSpeed+=10;
                FixedADC+=10;
                FixedIce+=10;
                break;
            case "ISTJ":
                FixedArmor+=10;
                FixedADC+=10;
                FixedIce+=10;
                FixedCooltime+=1;
                break;
            case "ENFP":
                FixedAD+=10;
                FixedSpeed+=10;
                FixedAP+=10;
                FixedFire+=10;
                break;
            case "ENFJ":
                FixedAD+=10;
                FixedAP+=10;
                FixedFire+=10;
                FixedCooltime+=1;
                break;
            case "ENTP":
                FixedAD+=10;
                FixedSpeed+=10;
                FixedAP+=10;
                FixedIce+=10;
                break;
            case "ENTJ":
                FixedAD+=10;
                FixedAP+=10;
                FixedIce+=10;
                FixedCooltime+=1;
                break;
            case "ESFP":
                FixedAD+=10;
                FixedSpeed+=10;
                FixedADC+=10;
                FixedFire+=10;
                break;
            case "ESFJ":
                FixedAD+=10;
                FixedADC+=10;
                FixedFire+=10;
                FixedCooltime+=1;
                break;
            case "ESTP":
                FixedAD+=10;
                FixedSpeed+=10;
                FixedADC+=10;
                FixedIce+=10;
                break;
            case "ESTJ":
                FixedAD+=10;
                FixedADC+=10;
                FixedIce+=10;
                FixedCooltime+=1;
                break;
        }
    }
    #endregion
}