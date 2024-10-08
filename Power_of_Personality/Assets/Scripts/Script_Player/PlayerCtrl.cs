using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Burst.CompilerServices;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour, IPlayerSkill, IPlayerAnim, IPlayerAttack
{
    #region 변수 선언
    //Raycast 관련
    protected float raycastDistance = 0.75f;
    protected RaycastHit hit;
    protected GameObject BossWall1;
    protected GameObject BossWall2;
    protected BoxCollider BossWall1Collider;
    protected BoxCollider BossWall2Collider;

    // GetAxis 값
    protected float hAxis;

    // Player의 transform, YPosition, YRotation 값
    protected Transform trs;
    protected float YRot;
    protected float YPos;

    //플레이어 스테이터스
    public float Damage;       //받은 피해량
    public float moveSpeed;     //이동속도
    public float moveSpd;      //이동속도
    public float JumpPower;     //점프력
    public float fallPower;     //떨어지는 힘

    // 애니메이션 컨트롤
    protected Vector3 initPos;
    protected bool isFloor = false;
    protected bool isStair = false;
    protected bool isSkill = false;
    protected bool isAttack = false;
    protected bool isJumping = false;
    protected bool isRun = false;
    protected bool isForward = true;
    protected bool isJumpAttack;
    protected bool isFall = false;
    protected bool isDodge = false;
    protected bool isCommonAttack1InProgress = false;
    protected bool isCommonAttack2InProgress = false;
    protected bool isCommonAttack3InProgress = false;
    
    //점프 관련(08.28)
    protected float JumpCoolTime;

    //애니메이션 상태 컨트롤 (GetCurrentAnimatorStateInfo(0).IsName 을 체크)
    protected bool stateIdle = false;
    protected bool stateWait = false;
    protected bool stateJump = false;
    protected bool stateFall = false;
    protected bool stateRun = false;
    protected bool stateDodge = false;
    protected bool stateAttack1 = false;
    protected bool stateAttack1_Wait = false;
    protected bool stateAttack2 = false;
    protected bool stateAttack2_Wait = false;
    protected bool stateAttack3 = false;
    protected bool stateJumpAttack1 = false;
    protected bool stateJumpAttack2 = false;
    protected bool stateJumpAttack3 = false;
    protected bool stateSkillQ = false;
    protected bool stateSkillW = false;
    protected bool stateSkillE = false;
    protected bool stateSkillE_Wait = false;
    protected bool stateDamage = false;
    protected bool stateDash = false;
    protected bool stateDashAttack = false; // 대쉬공격 사운드땜에 추가 (08.31)
    protected bool stateDie = false;
    protected bool stateShop = false; //상점 문제땜에 추가 (09.05)

    // 코루틴 컨트롤
    protected bool coroutineMove = false;

    // 애니메이터, Rigidbody
    public Animator anim;
    protected Rigidbody rd;

    // 이펙트
    public GameObject commonAttack_Ice1_Effect;
    public GameObject commonAttack_Ice2_Effect;
    public GameObject commonAttack_Ice3_Effect;
    public GameObject commonAttack_Fire1_Effect;
    public GameObject commonAttack_Fire2_Effect;
    public GameObject commonAttack_Fire3_Effect;
    public GameObject Skill_FireQ_Effect;
    public GameObject Skill_IceQ_Effect;
    public GameObject Skill_FireW_Effect;
    public GameObject Skill_IceW_Effect;
    public GameObject Skill_FireE1_Effect;
    public GameObject Skill_FireE2_Effect;
    public GameObject Skill_FireE3_Effect;
    public GameObject Skill_FireE4_Effect;
    public GameObject Skill_IceE1_Effect;
    public GameObject Skill_IceE2_Effect;
    public GameObject Skill_IceE3_Effect;
    public GameObject Skill_IceE4_Effect;
    public GameObject Attack1_Effect;
    public GameObject Attack2_Effect;
    public GameObject Attack3_Effect;
    public GameObject SkillQ_Effect;
    public GameObject SkillW_Effect;
    public GameObject SkillE1_Effect;
    public GameObject SkillE2_Effect;
    public GameObject SkillE3_Effect;
    public GameObject SkillE4_Effect;
    public float SkillYRot;
    public float LocalSkillYRot;
    public GameObject EffectGen;
    public GameObject SkillEffect;
    public GameObject DamageText; //텍스트
    public GameObject Damage_Effect;
    public GameObject HPPotion_Effect;
    public GameObject ATKPotion_Effect;
    public GameObject DEFPotion_Effect;
    public GameObject Item_Weapon_Effect;
    public GameObject Item_Weapon_Ice_Effect;
    public GameObject Item_Weapon_Fire_Effect;
    public GameObject Item_Aura_Effect;
    public GameObject Item_Aura_Ice_Effect;
    public GameObject Item_Aura_Fire_Effect;
    public GameObject Item_Sheild_Effect;
    public GameObject Item_Time_Effect;
    public GameObject PlayerCanvas;

    // 카메라, 사운드
    protected GameObject mainCamera;
    protected GameObject cameraEffect;
    protected AudioClip[] effectAudio;
    protected bool isSound = false;
    protected AudioSource[] audioSources;
    protected AudioSource hitAudio; //히트 사운드 추가(09.30)
    protected AudioSource[] spellSounds; //스펠 사운드 추가(10.02)

    // 벽 충돌체크
    protected bool WallCollision;

    // HP Bar
    protected Slider HpBar;
    public TMP_Text HpText;

    //코인
    protected TextMeshProUGUI CoinText;

    //포션
    public InventoryCtrl InvenCtrl;
    public TMP_Text hpPotionValue;
    public TMP_Text ADPotionValue;
    public TMP_Text ArmorPotionValue;
    protected float buffTime;
    protected float buffPer;
    protected string buffType;
    protected float buffStat;
    protected bool ADBuff_On;
    protected bool ArmorBuff_On;

    //보조스킬 관련
    protected bool Swiftness_Buff_ON; //신속
    protected bool Unstoppable_Buff_ON; //저지불가
    protected bool RoarOfAnger_Buff_ON; //분노의 포효
    protected float Swiftness_buffTime;
    protected float Unstoppable_buffTime;
    protected float RoarOfAnger_buffTime;
    protected float Swiftness_Stat;
    protected float RoarOfAnger_Stat;
    protected float TimeSlowdown_buffTime; //시간 감속
    public GameObject Spell_Swiftness_Effect;//도훈 2024-09-20
    public GameObject Spell_Unstoppable_Effect;
    public GameObject Spell_RoarOfAnger_Effect;
    public GameObject Spell_TimeSlowdown_Effect;
    public GameObject Spell_Immune_Effect;
    public GameObject Spell_Stun_Effect;
    public GameObject Spell_Heal_Effect;
    protected float Spell_Swiftness_CoolTime;
    protected float Spell_Unstoppable_CoolTime;
    protected float Spell_RoarOfAnger_CoolTime;
    protected float Spell_TimeSlowdown_CoolTime;
    protected float Spell_Immune_CoolTime;
    protected float Spell_Stun_CoolTime;
    protected float Spell_Heal_CoolTime;
    protected bool Spell_Swiftness_Ready = true;
    protected bool Spell_Unstoppable_Ready = true;
    protected bool Spell_RoarOfAnger_Ready = true;
    protected bool Spell_TimeSlowdown_Ready = true;
    protected bool Spell_Immune_Ready = true;
    protected bool Spell_Stun_Ready = true;
    protected bool Spell_Heal_Ready = true;//도훈 2024-09-20
    protected string Spell_1; //보조스킬 받아오기
    protected string Spell_2;
    protected float Spell_1_CoolTime = 0f; //쿨타임 UI용 변수
    protected float Spell_2_CoolTime = 0f; //쿨타임 UI용 변수
    protected float Swiftness_CoolTime = 180f;
    protected float Unstoppable_CoolTime = 120f;
    protected float RoarOfAnger_CoolTime = 120f;
    protected float TimeSlowdown_CoolTime = 180f;
    protected float Immune_CoolTime = 180f;
    protected float Stun_CoolTime = 180f;
    protected float Heal_CoolTime = 120f;

    //구르기 (10.01 정도훈)
    public TMP_Text DodgeValue;
    protected Image Rcool; 
    protected float Dodge_CoolTime; //남은 구르기 쿨
    protected float TotalDodge_CoolTime; //구르기 쿨타임
    protected int DodgeAmount= 1; //남은 구르기 횟수





    //스탯 UI 관련
    protected TMP_Text[] StateText; 

    public static bool isShop = false;

    //보스 관련
    public GameObject Druid;
    public GameObject DruidGen;

    public GameObject StoneGolem;
    public GameObject StoneGolemGen;

    public GameObject Ogre;
    public GameObject OgreGen;

    public GameObject DemonKing;
    public GameObject DemonKingGen;



    // 쿨타임 관련
    protected bool QSkillReady;

    protected float QSkillCoolTime; //쿨 돌았는지 체크용
    protected float TotalQSkillCoolTime = 0;  // 실제 쿨타임 도훈 2024-08-27

    protected bool WSkillReady; 
    protected float WSkillCoolTime; //쿨 돌았는지 체크용
    protected float TotalWSkillCoolTime = 0; // 실제 쿨타임  도훈 2024-08-27

    protected bool ESkillReady;
    protected float ESkillCoolTime; //쿨 돌았는지 체크용
    protected float TotalESkillCoolTime = 0; // 실제 쿨타임 도훈 2024-08-27
    protected Image Qcool;
    protected Image Wcool;
    protected Image Ecool;
    protected Image Dcool;
    protected Image Fcool;
    protected bool canTakeDamage = true; // 데미지를 가져올 수 있는지
    protected float damageCooldown = 1.0f; // 1초마다 틱데미지를 가져오기 위함

    // 무적 관련
    protected int ImmuneCount = 0;
    protected bool isImmune;

    // 회전 관련
    protected GameObject CurrentFloor;
    protected Vector3 moveVec;

    //아이템 세트효과 관련
    protected int attackCount = 0;
    protected int currentStack = 0;
    protected float clockEffectTime = 0;
    Stack prevADC = new Stack();

    #endregion

    protected virtual void Start()
    {
        // 8번 세트 3번 이펙트 효과 관리
        GetComponent<SkinnedMeshAfterImage>().enabled = false;
        Item_Time_Effect.SetActive(false);

        //상점 씬 등에 거쳐왔을 시 플레이어 위치 초기화(06.13)
        if(PlayerPrefs.GetFloat("PlayerXPos") != null && PlayerPrefs.GetString("Hidden_Shop_Spawn_Scene") == SceneManager.GetActiveScene().name)
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerXPos"), PlayerPrefs.GetFloat("PlayerYPos"), PlayerPrefs.GetFloat("PlayerZPos"));
        }

        // 플레이어 스테이터스 초기화
        SetIce();

        // 보스 문 할당
        BossWall1 = GameObject.Find("BossWall1").gameObject;
        BossWall1Collider = BossWall1.GetComponent<BoxCollider>();
        BossWall2 = GameObject.Find("BossWall2").gameObject;
        BossWall2Collider = BossWall2.GetComponent<BoxCollider>();
        //보스 소환 방식 변경(08.29)
        DruidGen = GameObject.Find("DruidGen");
        StoneGolemGen = GameObject.Find("StoneGolemGen");
        OgreGen = GameObject.Find("OgreGen");
        DemonKingGen = GameObject.Find("DemonKingGen");
        //Null 오류 날 시 코드 추가(09.30)
        TotalDodge_CoolTime = 10f; //구르기 쿨타임 (10.01)

        // HP Bar 설정
        HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();
        HpText = GameObject.Find("StatPoint - Hp").GetComponent<TMP_Text>();
        HpText.text = "HP" + Status.HP + "/" + Status.MaxHP;
        CheckHp();

        //코인 설정(09.05)
        CoinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();

        //포션 설정(06.15)
        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
        hpPotionValue = GameObject.Find("HP_Potion - Text").GetComponent<TMP_Text>();
        ADPotionValue = GameObject.Find("AD_Potion - Text").GetComponent<TMP_Text>();
        ArmorPotionValue = GameObject.Find("Armor_Potion - Text").GetComponent<TMP_Text>();
        DodgeValue = GameObject.Find("Dodge_Amount - Text").GetComponent<TMP_Text>();

        //스텟 UI 변동 설정(06.14)
        StateText = new TMP_Text[8];
        for(int i = 0; i <= 7; i++)
        {
            string statname = "StatText-" + i;
            //Debug.Log(statname);
            StateText[i] = GameObject.Find(statname).GetComponent<TMP_Text>();
        }
        
        //데미지 텍스트 설정(06.01)
        PlayerCanvas = this.transform.Find("Canvas - Player").gameObject;     //잠시
        
        //쿨타임 UI(03.18)
        Qcool = GameObject.Find("CoolTime-Q").GetComponent<Image>();
        Wcool = GameObject.Find("CoolTime-W").GetComponent<Image>();
        Ecool = GameObject.Find("CoolTime-E").GetComponent<Image>();
        Dcool = GameObject.Find("CoolTime-D").GetComponent<Image>();
        Fcool = GameObject.Find("CoolTime-F").GetComponent<Image>();
        Rcool = GameObject.Find("CoolTime-R").GetComponent<Image>(); //(10.01 정도훈)
        Rcool.fillAmount = 1;

        // 애니메이션, Rigidbody, Transform 컴포넌트 지정
        anim = this.GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        trs = GetComponentInChildren<Transform>();

        initPos = trs.position; // initPos에 Transform.position 할당
        mainCamera = GameObject.FindWithTag("MainCamera");  // 메인 카메라 지정
        cameraEffect = GameObject.FindWithTag("CameraEffect"); // 카메라 이펙트 볼륨 설정
        PlayAnim("isIdle");   // isIdle을 True로 설정해서 Idle 상태 지정
        EffectGen = transform.Find("EffectGen - Player").gameObject; // EffectGen 지정

        // 애니메이션, 스킬 관리하는 bool값을 false로 초기화
        isSkill = false;
        isAttack = false;
        isJumping = false;
        isRun = false;

        //사운드
        audioSources = GetComponents<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
        hitAudio = transform.Find("Player_HitSound").GetComponent<AudioSource>(); //히트 시 재생 오디오 지정(09.30)
        hitAudio.Stop(); //처음에는 소리 나오지 않게(09.30)

        //보조스킬 받아오기
        Spell_1 = PlayerPrefs.GetString("Spell_1");
        Spell_2 = PlayerPrefs.GetString("Spell_2");

        //보조스킬 사운드(10.02)
        spellSounds = transform.Find("Spell_Sounds").GetComponents<AudioSource>();
        for (int i = 0; i < spellSounds.Length; i++)
        {
            spellSounds[i].Stop();
        }
        
    }
    protected virtual void FixedUpdate()
    {
        //처음 시작 시에만 피 초기화
        if(PlayerPrefs.GetInt("GameSet") == 1)
        {
            SetHp(100);
            PlayerPrefs.SetInt("GameSet", 0);
        }

        CheckState();
        // Move 함수 실행
        if(!Status.IsShop)
        {
            if (!isSkill && !isAttack && !stateAttack3 && !anim.GetBool("isDie") && !stateDamage)
            {
                Move();
                Turn();
            }
            if (hAxis == 0)
            {
                StopAnim("isRun");
            }

            // Turn 함수 실행
            if (!isSkill && !isAttack && !stateIdle && !anim.GetBool("isDie") && !stateDamage)
            {
                Turn();
            }

            //코인 업데이트(09.05)
            if(PlayerPrefs.GetFloat("Coin").ToString() != CoinText.text)
                CoinText.text = PlayerPrefs.GetFloat("Coin").ToString();
        }
        ChargeDodge(); //구르기 충전 
    }

    protected virtual void Update()
    {
        
        if(Status.IsShop)
        {   
            StopAnim("isRun");
        }

        #region 7번 세트 3셋 이펙트 효과 (등 뒤에 잔상 이펙트)
        if (Status.set7_3_Activated && GetComponent<SkinnedMeshAfterImage>().enabled == false)
        {
            GetComponent<SkinnedMeshAfterImage>().enabled = true;
        }
        #endregion
        #region 8번 세트 3셋 이펙트 효과 (시계)
        if (Item_Time_Effect.activeSelf)
        {
            clockEffectTime -= Time.deltaTime;
            if(clockEffectTime < 0)
            {
                Item_Time_Effect.SetActive(false);
            }
        }
        #endregion
        // 해당 bool값 실행 시 모든 행동 멈춤
        if(!Status.IsShop && !anim.GetBool("isDie"))
        {
            if(stateShop == true)
            {
                stateShop = false;
                AnimReset();
            }

            CheckHp(); //Hp 체크(08.30)

            if (!canTakeDamage)
            {
                damageCooldown -= Time.deltaTime;
                if (damageCooldown < 0)
                {
                    canTakeDamage = true;
                    damageCooldown = 1.0f;
                }
            }

            //스킬 쿨타임 UI(03.18)
            if (Qcool.fillAmount != 0)
            {
                Qcool.fillAmount -= 1 * Time.smoothDeltaTime / TotalQSkillCoolTime;
            }
            if (Wcool.fillAmount != 0)
            {
                Wcool.fillAmount -= 1 * Time.smoothDeltaTime / TotalWSkillCoolTime;
            }
            if (Ecool.fillAmount != 0)
            {
                Ecool.fillAmount -= 1 * Time.smoothDeltaTime / TotalESkillCoolTime;
            }
            if (Dcool.fillAmount != 0)
            {
                Dcool.fillAmount -= 1 * Time.smoothDeltaTime / Spell_1_CoolTime;
            }
            if (Fcool.fillAmount != 0)
            {
                Fcool.fillAmount -= 1 * Time.smoothDeltaTime / Spell_2_CoolTime;
            }
            if (Rcool.fillAmount != 0 && DodgeAmount < 2) //(10.01 정도훈)
            {
                Rcool.fillAmount -= 1 * Time.smoothDeltaTime /TotalDodge_CoolTime;
            } 
            else
            {
                Rcool.fillAmount = 0;
            }
            // 땅에 닿아있는지 체크
            isGrounded();

            // 벽 충돌체크 함수 실행
            WallCheck();

            // 애니메이션 업데이트
            GetInput();

            //스킬 쿨타임 충전
            SkillCoolTimeCharge();

            //애니메이션 상태 확인
            AnimState();

            //로테이션 고정 코드(04.10 백건우 수정, 굴절구간 문제 생길 시 아래 코드 대신 사용)
            YRot = transform.eulerAngles.y;

            //Z 포지션 고정
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

            // char 오브젝트 위치 고정
            transform.GetChild(0).localPosition = Vector3.zero;

            //데미지 캔버스 Y값 고정(08.29)
            if(mainCamera.transform.eulerAngles.y > 0 && mainCamera.transform.eulerAngles.y < 180)
                PlayerCanvas.transform.localRotation = Quaternion.Euler(0, LocalSkillYRot - 180f, 0);
            else
                PlayerCanvas.transform.localRotation = Quaternion.Euler(0, LocalSkillYRot + 180f, 0);

            //애니메이션 속도 조절 (평타, 대쉬, 걷기)(09.10 정도훈)
            anim.SetFloat("AnimSpeed", Status.TotalSpeed);

            // Attack 함수 실행
            if (Input.GetKeyDown(KeyCode.A) && !stateDamage)
            {
                Attack_anim();
            }

            //기본공격1 & 기본공격3 시 전진 애니메이션
            if (stateAttack1 == true && !isCommonAttack1InProgress && !stateDamage)
            {
                Attack(0);
                isCommonAttack1InProgress = true;
            }
            else if (stateAttack2 == true && !isCommonAttack2InProgress && !isSound && !stateDamage)
            {
                Attack(1);
                isCommonAttack2InProgress = true;
            }
            else if (stateAttack3 == true && !isCommonAttack3InProgress && !stateDamage)
            {
                Attack(2);
                isCommonAttack3InProgress = true;
            }

            //지상공격 2타, 3타 시 방향전환 되도록
            if(stateAttack1_Wait == true ||
            stateAttack2_Wait == true)
            {
                isAttack = false;
            }
            else if(stateAttack1 == true ||
                    stateAttack2 == true ||
                    stateAttack3 == true)
            {
                isAttack = true;
            }

            //점프공격 카메라 && 사운드
            else if (stateJumpAttack1 == true && !coroutineMove  && !stateDamage)
            {
                Attack(3);
                PlayAnim("isFall");
            }
            else if (stateJumpAttack2 == true && !isSound  && !stateDamage)
            {
                Attack(4);
                PlayAnim("isFall");
            }
            else if (stateJumpAttack3 == true && !coroutineMove  && !stateDamage)
            {
                Attack(5);
                PlayAnim("isFall");
            }

            UpdateCoroutineMoveState();

            if (stateFall == true && isJumpAttack == true)
            {
                StopAnim("CommonAttack");
            }
            //한 번 점프 시 한 번의 점프공격 콤보만 되게
            else if (stateWait == true && isJumpAttack == true)
            {
                StopAnim("CommonAttack");
                isJumpAttack = false;
                isAttack = false;
            }
            else if (stateRun == true && isJumpAttack == true)
            {
                StopAnim("CommonAttack");
                isJumpAttack = false;
                isAttack = false;
            }

            if (stateJump == true && isJumpAttack == true)
            {
                StopAnim("CommonAttack");
                isAttack = false;
            }

            //Skill_Q
            if (Input.GetKeyDown(KeyCode.Q)
            && !isSkill
            && !isJumping
            && !anim.GetBool("isFall")
            && QSkillCoolTime >= TotalQSkillCoolTime //도훈 2024-08-27
            && !isAttack
            && !isDodge
            && !stateDamage)
            {
                UseSkill("Q");
                TotalQSkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
                // Debug.Log("TotalQSkillCoolTime :" + TotalQSkillCoolTime);
                // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
                // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
            }
            
            //Skill_W
            if (Input.GetKeyDown(KeyCode.W)
            && !isSkill
            && !isJumping
            && !anim.GetBool("isFall")
            && WSkillCoolTime >= TotalWSkillCoolTime //도훈 2024-08-27
            && !isAttack
            && !isDodge
            && !stateDamage)
            {
                UseSkill("W");
                TotalWSkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
                // Debug.Log("TotalWSkillCoolTime :" + TotalWSkillCoolTime);
                // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
                // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
            }

            //Skill_E
            if (Input.GetKeyDown(KeyCode.E)
            && !isSkill
            && !isJumping
            && !anim.GetBool("isFall")
            && ESkillCoolTime >= TotalESkillCoolTime //도훈 2024-08-27
            && !isAttack
            && !isDodge
            && !stateDamage)
            {
                UseSkill("E");
                TotalESkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
                // Debug.Log("TotalESkillCoolTime :" + TotalESkillCoolTime);
                // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
                // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
            }

            //Skill_D (09.18 정도훈)
            if (Input.GetKeyDown(KeyCode.D)
            && !isSkill
            && !isJumping
            && !anim.GetBool("isFall")
            //&& DSkillCoolTime >= TotalDSkillCoolTime
            && !isAttack
            && !isDodge)
            {
                //Spell_Swiftness(); //(이펙트 추가해야됨)
                //Spell_Immune(); //완료
                //Spell_TimeSlowdown();
                UseSpell(Spell_1, 1);
            }

            //Skill_F (09.18 정도훈)
            if (Input.GetKeyDown(KeyCode.F)
            && !isSkill
            && !isJumping
            && !anim.GetBool("isFall")
            //&& DSkillCoolTime >= TotalDSkillCoolTime
            && !isAttack
            && !isDodge)
            {
                //Spell_RoarOfAnger(); //완료
                //Spell_Unstoppable(); //(이펙트 추가해야됨)
                //Spell_Heal(); //완료
                UseSpell(Spell_2, 2); 
            }


            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && !isSkill && !isAttack && !isJumping
                && !stateJump && !stateFall && !anim.GetBool("isFall") && !isDodge && JumpCoolTime == 0 && !stateDamage)
            {
                isJumping = true;
                JumpCoolTime += Time.deltaTime;
            }
            else
            {
                //isJumping = false;
            }
            //점프 모션이 실행되야만 점프가 실행되게(애니메이션 딜레이 및 더블점프 강제 제거)
            if (isJumping == true)
            {
                Jump();
                isJumping = false;
            }
            // Dodge 함수 실행
            if (Input.GetKeyDown(KeyCode.R) && DodgeAmount>0 && !stateDamage)
            {
                StartCoroutine(Dodge());
            }
            if (stateDodge == true)
            {
                StopAnim("isJump");
            }
            //다중 점프 방지 코드
            if(stateFall == true && isFloor == false && isStair == false)
            {
                PlayAnim("isFall");
            }

            //Idle일때 스킬 및 공격 false 판정
            if (stateIdle == true && isDodge == false)
            {
                PlayAnim("isIdle");
                isAttack = false;
                isSkill = false;
                StopAnim("CommonAttack");
            }

            //다른 모션일 때, 혹시라도 Move가 실행되도 달리지 못하게
            if (stateWait == true || stateIdle == true || stateAttack1 == true ||
            stateAttack1_Wait == true || stateAttack2 == true || stateAttack2_Wait == true ||
            stateAttack3 == true || stateSkillQ == true || stateSkillW == true ||
            stateSkillE == true || stateJumpAttack2 == true ||
            (stateJump == true && !anim.GetBool("isRun")) ||
            (stateFall == true && !anim.GetBool("isRun")))
            {
                moveSpd = 0;
            }

            //대쉬일 때
            else if (stateDash == true)
            {
                moveSpd = moveSpeed * 1.25f *Status.TotalSpeed; // 행동속도 조절 추가(09.10 정도훈)
            }
            else
            {
                moveSpd = moveSpeed * Status.TotalSpeed; // 행동속도 조절 추가(09.10 정도훈)
            }

            //캐릭터 스킬 이펙트
            LocalSkillYRot = transform.localEulerAngles.y;
            SkillYRot = transform.eulerAngles.y;
            if (Status.set2_3_Effect_Activated)
            {
                Item_Sheild_Effect.SetActive(true);
            }
            if (PlayerPrefs.GetString("property") == "Fire")
            {
                Attack1_Effect = commonAttack_Fire1_Effect;
                Attack2_Effect = commonAttack_Fire2_Effect;
                Attack3_Effect = commonAttack_Fire3_Effect;
                SkillQ_Effect = Skill_FireQ_Effect;
                SkillW_Effect = Skill_FireW_Effect;
                SkillE1_Effect = Skill_FireE1_Effect;
                SkillE2_Effect = Skill_FireE2_Effect;
                SkillE3_Effect = Skill_FireE3_Effect;
                SkillE4_Effect = Skill_FireE4_Effect;
                //아이템 이펙트 추가(07.29 백건우)
                //세트효과 추가(08.26 이준경) 
                #region 3번 세트 4셋 이펙트 효과
                if (Status.set3_3_Activated)
                {
                    Item_Weapon_Effect = Item_Weapon_Fire_Effect;
                    if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
                    {
                        Item_Weapon_Fire_Effect.SetActive(true);
                        Item_Weapon_Ice_Effect.SetActive(false);
                        Rogue_Sec_Weapon("Fire"); //도적 두번째 단검 코드, 도적한테만 override 예정(08.31)
                    }
                } else if(!Status.set3_3_Activated)
                {
                    Item_Weapon_Effect = Item_Weapon_Ice_Effect;
                    if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
                    {
                        Item_Weapon_Fire_Effect.SetActive(false);
                        Item_Weapon_Ice_Effect.SetActive(false);
                        Rogue_Sec_Weapon("None"); //도적 두번째 단검 코드, 도적한테만 override 예정(08.31)
                    }
                }
                #endregion
                #region 1번 세트 3셋 이펙트 효과
                if (Status.set1_3_Activated)
                {
                    Item_Aura_Effect = Item_Aura_Fire_Effect;
                    if (Item_Aura_Fire_Effect != null && Item_Aura_Ice_Effect != null)
                    {
                        Item_Aura_Fire_Effect.SetActive(true);
                        Item_Aura_Ice_Effect.SetActive(false);
                    }
                }
                #endregion
            }
            else if (PlayerPrefs.GetString("property") == "Ice")
            {
                Attack1_Effect = commonAttack_Ice1_Effect;
                Attack2_Effect = commonAttack_Ice2_Effect;
                Attack3_Effect = commonAttack_Ice3_Effect;
                SkillQ_Effect = Skill_IceQ_Effect;
                SkillW_Effect = Skill_IceW_Effect;
                SkillE1_Effect = Skill_IceE1_Effect;
                SkillE2_Effect = Skill_IceE2_Effect;
                SkillE3_Effect = Skill_IceE3_Effect;
                SkillE4_Effect = Skill_IceE4_Effect;
                //아이템 이펙트 추가(07.29 백건우)
                //세트효과 추가(08.26 이준경) 
                #region 4번 세트 4셋 이펙트 효과
                if (Status.set4_3_Activated == true) 
                {
                    Item_Weapon_Effect = Item_Weapon_Ice_Effect;
                    if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
                    {
                        Item_Weapon_Fire_Effect.SetActive(false);
                        Item_Weapon_Ice_Effect.SetActive(true);
                        Rogue_Sec_Weapon("Ice"); //도적 두번째 단검 코드, 도적한테만 override 예정(08.31)
                    }
                } else if(!Status.set4_3_Activated)
                {
                    Item_Weapon_Effect = Item_Weapon_Ice_Effect;
                    if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
                    {
                        Item_Weapon_Fire_Effect.SetActive(false);
                        Item_Weapon_Ice_Effect.SetActive(false);
                        Rogue_Sec_Weapon("None"); //도적 두번째 단검 코드, 도적한테만 override 예정(08.31)
                    }
                }
                #endregion

                #region 1번 세트 3셋 이펙트 효과
                if (Status.set1_3_Activated == true)
                {
                    Item_Aura_Effect = Item_Aura_Ice_Effect;
                    if (Item_Aura_Fire_Effect != null && Item_Aura_Ice_Effect != null)
                    {
                        Item_Aura_Fire_Effect.SetActive(false);
                        Item_Aura_Ice_Effect.SetActive(true);
                    }
                }
                #endregion
            }
            else
            {
                Attack1_Effect = commonAttack_Ice1_Effect;
                Attack2_Effect = commonAttack_Ice2_Effect;
                Attack3_Effect = commonAttack_Ice3_Effect;
                SkillQ_Effect = Skill_IceQ_Effect;
                SkillW_Effect = Skill_IceW_Effect;
                SkillE1_Effect = Skill_IceE1_Effect;
                SkillE2_Effect = Skill_IceE2_Effect;
                SkillE3_Effect = Skill_IceE3_Effect;
                SkillE4_Effect = Skill_IceE4_Effect;
            }

            // 플레이어 피가 30보다 작으면 지속적으로 화면이 깜빡임
            if(Status.HP <= 30)
            {
                cameraEffect.GetComponent<CameraEffectCtrl>().DangerousCamera();
            }

            //포션 버프
            if(buffType == "AD")
            {
                if(ADBuff_On == false)
                {
                    ADBuff_On = true;
                    StartCoroutine(PowerUp_On());
                }
                buffTime -= Time.deltaTime;
                if(buffTime < 0)
                {
                    buffTime = 0;
                    Status.PercentAD = buffStat;
                    Status.StatUpdate();
                    buffStat = 0;
                    buffType = "None";
                    ADBuff_On = false;
                }
            }
            else if(buffType == "Armor")
            {
                if(ArmorBuff_On == false)
                {
                    ArmorBuff_On = true;
                    StartCoroutine(ArmorUp_On());
                }
                buffTime -= Time.deltaTime;
                if(buffTime < 0)
                {
                    buffTime = 0;
                    Status.TotalArmor = buffStat;
                    Status.StatUpdate();
                    buffType = "None";
                    ArmorBuff_On = false;
                }
            }
            //포션 눌렀을 때
            //포션 음수되는 오류 수정(08.29)
            // 힐 Potion
            hpPotionValue.text = InvenCtrl.PotionCount.ToString();
            if(Input.GetKeyDown(KeyCode.Alpha1) && !anim.GetBool("isDie") && InvenCtrl.PotionCount > 0)
            {
                HealHp();
            }
            // 공격 Potion
            ADPotionValue.text = InvenCtrl.ADPotionCount.ToString();
            if(Input.GetKeyDown(KeyCode.Alpha2) && !anim.GetBool("isDie") && InvenCtrl.ADPotionCount > 0)
            {
                PowerUp();
            }
            // 방어 Potion
            ArmorPotionValue.text = InvenCtrl.ArmorPotionCount.ToString();
            if(Input.GetKeyDown(KeyCode.Alpha3) && !anim.GetBool("isDie") && InvenCtrl.ArmorPotionCount > 0)
            {
                ArmorUp();
            }

            //보조스킬 (09.14)
            if(Swiftness_Buff_ON){
                Swiftness_buffTime -= Time.deltaTime;
                if(Swiftness_buffTime < 0)
                {
                    Swiftness_buffTime = 0;
                    Status.PercentSpeed = Swiftness_Stat;
                    Status.StatUpdate();
                    Swiftness_Buff_ON = false;
                    Spell_Swiftness_Effect.SetActive(false);
                    Debug.Log("신속 OFF");
                }
            }
            if(RoarOfAnger_Buff_ON){
                RoarOfAnger_buffTime -= Time.deltaTime;
                if(RoarOfAnger_buffTime < 0)
                {
                    RoarOfAnger_buffTime = 0;
                    Status.PercentAD = RoarOfAnger_Stat;
                    Status.StatUpdate();
                    RoarOfAnger_Buff_ON = false;
                    Spell_RoarOfAnger_Effect.SetActive(false);
                    Debug.Log("분노의 포효 OFF");
                }
            }
            if(Unstoppable_Buff_ON){
                Unstoppable_buffTime -= Time.deltaTime;
                if(Unstoppable_buffTime < 0)
                {
                    Unstoppable_buffTime = 0;
                    Unstoppable_Buff_ON = false;
                    Debug.Log("저지불가 OFF");
                }
            }

            if(Status.Spell_TimeSlowdown_ON){
                TimeSlowdown_buffTime -= Time.deltaTime;
                if(TimeSlowdown_buffTime < 0)
                {
                    TimeSlowdown_buffTime = 0;
                    Status.Spell_TimeSlowdown_ON = false;
                    Spell_TimeSlowdown_Effect.SetActive(false);
                    Debug.Log("시간감속 OFF");
                }
            }

        }
        else if(Status.IsShop == true) //(09.05)
        {
            moveSpd = 0;
            stateShop = true;
        }
        else
        {
            moveSpd = 0;
        }
    }

    #region 포션 효과 설정
    //(08.29 백건우)
    public virtual void PowerUp()
    {
        InvenCtrl.ADPotionCount -= 1; 
        buffType = "AD";
        StartCoroutine(ATKPotion_on());
    }

    public virtual void ArmorUp()
    {
        InvenCtrl.ArmorPotionCount -= 1; 
        buffType = "Armor";
        StartCoroutine(DEFPotion_on());
    }

    public virtual IEnumerator PowerUp_On()
    {
        buffTime = 60f;
        buffPer = 0.2f;
        buffStat = Status.PercentAD;
        Status.PercentAD += (Status.PercentAD * buffPer);
        Status.StatUpdate();
        Debug.Log("힘 버프");
        yield break;
    }

    public virtual IEnumerator ArmorUp_On()
    {
        buffTime = 60f;
        buffPer = 0.2f;
        buffStat = Status.TotalArmor;
        Status.TotalArmor += (Status.TotalArmor * buffPer);
        Status.StatUpdate();
        Debug.Log("방패 버프");
        yield break;
    }
    #endregion

    //도적용 무기 세트효과(08.31)
    protected virtual void Rogue_Sec_Weapon(string weaponType)
    {
    }

    #region HP 설정
    protected virtual IEnumerator TakeDamage()
    {
        if (Status.MaxHP != 0 || Status.HP > 0)
        {
            int hitCount = PlayerPrefs.GetInt("hitCount", 0);
            PlayerPrefs.SetInt("hitCount", hitCount + 1);
            Debug.Log(PlayerPrefs.GetInt("hitCount"));
            Status.HP -= Damage;
            CheckHp();
            //저지불가 (09.14 정도훈)
            if(Unstoppable_Buff_ON==false)
            {
                PlayAnim("TakeDamage"); 
            }
            StartCoroutine(DamageTextAlpha());
            Damaged_on(); //맞았을 때 이펙트 애니메이션에서 코드로 옮김(08.30)
            hitAudio.PlayOneShot(hitAudio.clip); //히트 시 재생 오디오 재생(09.30)
            cameraEffect.GetComponent<CameraEffectCtrl>().DamageCamera();
            StartCoroutine(Immune(0.5f));   //무적 함수 실행
            yield return new WaitForSeconds(0.2f);
            StopAnim("TakeDamage");
            AnimReset(); //애니메이션 리셋(09.05)
            cameraEffect.GetComponent<CameraEffectCtrl>().ResetCameraEffect();
        }

        if (Status.HP <= 0) // 플레이어가 죽으면 게임오버 창 띄움
        {
            PlayAnim("isDie");

            InvenCtrl.ResetInven();
            Status.isDie();

            yield return new WaitForSeconds(2.0f);
            GameObject.Find("EventSystem").GetComponent<GameEnd>().GameOver(true);
        }
    }
    public virtual void SetHp(float amount) // Hp 세팅
    {
        Status.MaxHP = amount;
        Status.HP = Status.MaxHP;
    }
    public virtual void CheckHp() // HP 체크
    {
        string inputText = "HP " + Status.HP.ToString("F0") + "/" + Status.MaxHP.ToString("F0");
        if (HpBar != null)
            HpBar.value = Status.HP / Status.MaxHP;
        if (HpText != null)
            HpText.text = inputText;
    }
    public virtual void HealHp()
    {
        InvenCtrl.PotionCount -= 1; 
        Status.HP = Status.MaxHP;
        CheckHp();
        StartCoroutine(HPPotion_on());
    }
    //(06.01)
    protected virtual IEnumerator DamageTextAlpha()
    {
        if(anim.GetBool("isDie") == false)
        {   
            //데미지 텍스트 출력 부분(05.31)
            GameObject instText = Instantiate(DamageText);
            instText.transform.SetParent(PlayerCanvas.transform, false);
            instText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); 
            instText.GetComponent<TMP_Text>().text = (-Damage).ToString("F0"); //소수점 날리고 데미지 표현
            float time = 0f;
            instText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
            Color fadecolor = instText.GetComponent<TMP_Text>().color;
            yield return new WaitForSeconds(0.15f);
            while(fadecolor.a >= 0)
            {
                time += Time.deltaTime;
                fadecolor.a = Mathf.Lerp(1, 0, time * 2f);
                instText.GetComponent<TMP_Text>().color = fadecolor; // 페이드 되면서 사라짐
                instText.transform.position = new Vector3(transform.position.x, transform.position.y + time * 3f + 0.5f, transform.position.z); // 서서히 올라감
                //데미지 텍스트 사라지게 해둠(08.30)
                if(fadecolor.a == 0)
                {
                    Destroy(instText);
                    yield break;
                }
                yield return null;
            }
        }
    }
    protected virtual IEnumerator Immune(float seconds)
    {
        ImmuneCount++;
        isImmune = true;
        yield return new WaitForSeconds(seconds);
        ImmuneCount--;
        if(ImmuneCount <= 0)
        {
            isImmune = false;
        }
    }
    #endregion

    #region 스탯 UI 관련 함수

    protected virtual void CheckState()
    {
        StateText[0].text = Status.TotalAD.ToString();
        StateText[1].text = Status.TotalArmor.ToString();
        StateText[2].text = Status.DisplayADC.ToString();
        StateText[3].text = Status.DisplayAP.ToString();
        StateText[4].text = Status.TotalFire.ToString();
        StateText[5].text = Status.TotalIce.ToString();
        StateText[6].text = Status.TotalSpeed.ToString();
        StateText[7].text = (Status.FixedCooltime*-1).ToString();
    }

    #endregion

    #region 이동 관련 함수
    protected virtual void WallCheck()
    {
        WallCollision = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), transform.forward, 0.6f, LayerMask.GetMask("Wall"));
    }

    protected virtual void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
    }

    public virtual void Move()
    {
        if (hAxis != 0)
        {
            PlayAnim("isRun");
            moveVec = AdjustDirectionToSlope(transform.forward);
        }
        else
        {
            StopAnim("isRun");
            moveVec = Vector3.zero;
        }
        if (!WallCollision)
        {
            transform.position += moveVec * moveSpd * Time.fixedDeltaTime;
        }
        StartCoroutine(Delay(0.2f));
    }
    public virtual IEnumerator Dodge()
    {
        DodgeAmount-=1;
        StartCoroutine(Immune(0.5f));
        PlayAnim("isDodge");
        isDodge = true;

        if(Rcool.fillAmount == 0)
        {
            Rcool.fillAmount = 1;
        }

        yield return new WaitForSeconds(0.5f);
        StopAnim("isDodge");
        //구르기 연타하고 안 움직이는 버그 해결(09.04)
        StopAnim("CommonAttack");
        StopAnim("isRun");
        isDodge = false;
    }

    protected virtual Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        if (!anim.GetBool("isFall"))
        {
            return Vector3.ProjectOnPlane(direction, hit.normal).normalized;
        }
        else
        {
            return direction;
        }
    }
    protected virtual bool isGrounded()
    {
        if (Physics.Raycast(transform.position - new Vector3(0, -0.1f, 0), -Vector3.up, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Floor") && !hit.collider.CompareTag("Stair"))
            {
                isFloor = true;
                if (Vector3.Distance(hit.point, transform.position) <= 0.01)
                {
                    isJumping = false; //isJump, isFall을 다시 false로
                    StopAnim("isJump");
                    StopAnim("isFall");
                    JumpCoolTime = 0; //착지 후 점프 되게 쿨타임 걸어 둠
                }
                return true;
            }
            else if (hit.collider.CompareTag("Stair"))
            {
                isStair = true;
                isJumping = false; //isJump, isFall을 다시 false로
                StopAnim("isJump");
                StopAnim("isFall");
                JumpCoolTime = 0; //착지 후 점프 되게 쿨타임 걸어 둠
                return true;
            }
        }
        else
        {
            PlayAnim("isFall");
        }
        isFloor = false;
        isStair = false;
        Debug.DrawRay(transform.position - new Vector3(0, -0.1f, 0), -Vector3.up * raycastDistance, Color.red);
        return false;
    }
    protected virtual void Turn()
    {
        if (hAxis > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (hAxis < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
    }
    protected virtual void Jump()
    {
        PlayAnim("isJump");
        rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
    }
    protected virtual void Fall()
    {
        PlayAnim("isFall"); //떨어지는것으로 감지
        rd.AddForce(Vector3.down * fallPower, ForceMode.VelocityChange);
    }
    protected virtual void Stay()
    {
        isJumping = false; //isJump, isFall을 다시 false로
        StopAnim("isJump");
        StopAnim("isFall");
    }
    #endregion

    #region 충돌 관련 함수
    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Monster_Melee" && !isImmune)
        {
            // 특정 이름을 가진 부모 객체를 찾습니다.
            string targetParentName = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
            Transform parent = col.transform;
            MonsterCtrl monsterCtrl = null;
            

            while (parent != null)
            {
                if (parent.name == targetParentName)
                {
                    monsterCtrl = parent.GetComponent<MonsterCtrl>();
                    break;
                }
                parent = parent.parent;
            }

            if(monsterCtrl != null && Status.set2_3_Activated)
            {
                float reflectDamage = Status.TotalArmor;
                StartCoroutine(monsterCtrl.TakeDamage(reflectDamage));
            }

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (monsterCtrl != null)
            {
                // 스크립트의 이름을 가져옵니다.
                string monsterScriptName = monsterCtrl.GetType().Name;

                // "Ctrl"을 제거하여 몬스터의 이름을 가져옵니다.
                string monsterName = monsterScriptName.Replace("Ctrl", "");

                // 몬스터 이름을 사용하여 해당 몬스터의 스크립트 타입을 가져옵니다.
                System.Type monsterScriptType = System.Type.GetType(monsterScriptName);

                // 가져온 스크립트를 동적으로 몬스터 스크립트 타입으로 캐스팅합니다.
                object specificMonsterCtrl = Convert.ChangeType(monsterCtrl, monsterScriptType);

                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)((specificMonsterCtrl as MonoBehaviour).GetType().GetField("ATK").GetValue(specificMonsterCtrl));
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }

        else if (col.gameObject.tag == "Monster_Ranged" && !isImmune)
        {
            // 특정 이름을 가진 부모 객체를 찾습니다.
            string targetParentName = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
            Transform parent = col.transform;
            MonsterCtrl monsterCtrl = null;

            while (parent != null)
            {
                if (parent.name == targetParentName)
                {
                    monsterCtrl = parent.GetComponent<MonsterCtrl>();
                    break;
                }
                parent = parent.parent;
            }

            if (monsterCtrl != null && Status.set2_3_Activated)
            {
                float reflectDamage = Status.TotalArmor;
                StartCoroutine(monsterCtrl.TakeDamage(reflectDamage));
            }

            // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
            MonoBehaviour attackCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (attackCtrl != null)
            {
                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)attackCtrl.GetType().GetField("ATK").GetValue(attackCtrl);
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }
    }

    protected virtual void OnTriggerStay(Collider col)
    {
        if (canTakeDamage == true && col.gameObject.tag == "Druid_Poison" && !isImmune)
        {
            // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
            MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (monsterCtrl != null)
            {
                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                canTakeDamage = false;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && !stateSkillE && !stateSkillE && !isStair && !isFloor)    // Tag가 Floor인 오브젝트와 충돌이 끝났을 때
        {
            Fall();
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "BossWall1" && BossWall1.transform.localPosition.x - transform.localPosition.x < 0)
        {
            BossWall1.layer = 3;
            BossWall2.layer = 3;
            BossWall1Collider.isTrigger = false;
            BossWall2Collider.isTrigger = false;
            //스테이지마다 생성되는 보스 다르게 수정(09.30)
            if(DruidGen != null)
                Instantiate(Druid, DruidGen.transform.position, Quaternion.Euler(0, 0, 0));
            else if(StoneGolemGen != null)
                Instantiate(StoneGolem, StoneGolemGen.transform.position, Quaternion.Euler(0, -90f, 0));
            else if(OgreGen != null)
                Instantiate(Ogre, OgreGen.transform.position, Quaternion.Euler(0, 0f, 0));
            else if(DemonKingGen!= null)
                Instantiate(DemonKing, DemonKingGen.transform.position, Quaternion.Euler(0, -90f, 0));
        }
    }
    #endregion

    #region 스킬 / 공격 관련 함수. 자식 스크립트에 상속은 시키되 함수를 비워서 각각에 맞는 스크립트를 사용할 수 있도록 함.
    protected virtual void SetIce()
    {
    }
    protected virtual void SetFire()
    {
    }
    protected virtual void Attack_anim()
    {
    }

    public virtual void Attack(int AttackNumber)
    {
        Debug.Log("Attack 메서드 실행, AttackNumber는 = " + AttackNumber);
        switch (AttackNumber) {
            case 0:
            case 1:
            case 2:
                {
                    if(attackCount < 3 && Status.set5_4_Activated)
                    {
                        attackCount++;
                    }
                    if(attackCount == 3 && currentStack < 3 && Status.set5_4_Activated)
                    {
                        Debug.Log("평타 " + attackCount + "번 침");
                        StartCoroutine(set5_4());
                    }
                    if(attackCount >= 3 && Status.set5_4_Activated)
                    {
                        attackCount = 0;
                    }
                    break;
                }
        }
    }

    public virtual IEnumerator set5_4()
    {
        currentStack++;
        prevADC.Push(Status.TotalADC);
        Status.TotalADC = Status.TotalADC * 1.2f;
        Debug.Log("강화된 공격력 : " + Status.TotalADC);
        Debug.Log("현재 " + currentStack + "스택");
        yield return new WaitForSeconds(7);
        currentStack--;
        Status.TotalADC = (float)prevADC.Pop();
        Debug.Log("돌아간 공격력 : " + Status.TotalADC);
        Debug.Log("현재 " + currentStack + "스택");
    }

    protected virtual void SkillCoolTimeCharge()
    {
        QSkillCoolTime += Time.deltaTime;
        WSkillCoolTime += Time.deltaTime;
        ESkillCoolTime += Time.deltaTime;
        Spell_Heal_CoolTime += Time.deltaTime; //도훈 2024-09-20
        Spell_Immune_CoolTime += Time.deltaTime;//도훈 2024-09-20
        Spell_RoarOfAnger_CoolTime += Time.deltaTime;//도훈 2024-09-20
        Spell_Stun_CoolTime += Time.deltaTime;//도훈 2024-09-20
        Spell_Swiftness_CoolTime += Time.deltaTime;//도훈 2024-09-20
        Spell_TimeSlowdown_CoolTime += Time.deltaTime;//도훈 2024-09-20
        Spell_Unstoppable_CoolTime += Time.deltaTime;//도훈 2024-09-20


        if(QSkillCoolTime > TotalQSkillCoolTime) //도훈 2024-08-27
        {
            QSkillReady = true;
        }
        if (WSkillCoolTime > TotalWSkillCoolTime)  //도훈 2024-08-27
        { 
            WSkillReady = true;
        }
        if (ESkillCoolTime > TotalESkillCoolTime) //도훈 2024-08-27
        {
            ESkillReady = true;
        }
        if(Spell_Heal_CoolTime> Heal_CoolTime) //도훈 2024-09-20
        {
            Spell_Heal_Ready =true;
        }
        if(Spell_Immune_CoolTime> Immune_CoolTime)//도훈 2024-09-20
        {
            Spell_Immune_Ready =true;
        }
        if(Spell_RoarOfAnger_CoolTime> RoarOfAnger_CoolTime)//도훈 2024-09-20
        {
            Spell_RoarOfAnger_Ready =true;
        }
        if(Spell_Stun_CoolTime> Stun_CoolTime)//도훈 2024-09-20
        {
            Spell_Stun_Ready =true;
        }
        if(Spell_Swiftness_CoolTime> Swiftness_CoolTime)//도훈 2024-09-20
        {
            Spell_Swiftness_Ready =true;
        }
        if(Spell_TimeSlowdown_CoolTime> TimeSlowdown_CoolTime)//도훈 2024-09-20
        {
            Spell_TimeSlowdown_Ready =true;
        }
        if(Spell_Unstoppable_CoolTime> Unstoppable_CoolTime)//도훈 2024-09-20
        {
            Spell_Unstoppable_Ready =true;
        }
    }
    
    protected virtual void ResetAttackInProgressStates()
    {
        isCommonAttack1InProgress = false;
        isCommonAttack2InProgress = false;
        isCommonAttack3InProgress = false;
    }
    protected virtual void UpdateCoroutineMoveState()
    {
        if (!(stateAttack1 ||
            stateAttack2 ||
            stateAttack3))
        {
            ResetAttackInProgressStates();
        }
    }

    public virtual void UseSkill(string skillName)
    {
        if (Status.set8_3_Activated)
        {
            clockEffectTime += 3;
            Item_Time_Effect.SetActive(true);
        }
    }
    #endregion

    #region Delay 함수
    protected virtual IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (stateIdle == true)
        {
            moveVec = new Vector3(0, 0, 0);
            isAttack = false;
            isSkill = false;
        }
        yield return null;
    }
    #endregion

    #region 이펙트 함수

    public virtual IEnumerator HPPotion_on()
    {
        SkillEffect = Instantiate(HPPotion_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
        SkillEffect.transform.parent = EffectGen.transform;
        yield return new WaitForSeconds(1.0f);
        Destroy(SkillEffect);
    }

    public virtual IEnumerator ATKPotion_on()
    {
        SkillEffect = Instantiate(ATKPotion_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
        SkillEffect.transform.parent = EffectGen.transform;
        yield return new WaitForSeconds(1.0f);
        Destroy(SkillEffect);
    }

    public virtual IEnumerator DEFPotion_on()
    {
        SkillEffect = Instantiate(DEFPotion_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
        SkillEffect.transform.parent = EffectGen.transform;
        yield return new WaitForSeconds(1.0f);
        Destroy(SkillEffect);
    }

    public virtual void Damaged_on()
    {
        SkillEffect = Instantiate(Damage_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
        SkillEffect.transform.parent = EffectGen.transform;
    }

    public virtual void Destroyed_Effect()
    {
        Destroy(SkillEffect);
    }

    #endregion

    #region 사운드 함수
    //사운드 함수 추가 (08.31)
    public virtual IEnumerator Attack_Sound(int AttackValue, float playsec)
    {
        audioSources[AttackValue].Play();
        yield return new WaitForSeconds(playsec);
        audioSources[AttackValue].Stop();
        yield return null;
    }

    #endregion

    #region 애니메이션 
    public virtual void PlayAnim(string AnimationName)
    {
        //Debug.Log(AnimationName + " 실행");
        if(AnimationName == "CommonAttack" || AnimationName == "Skill_Q" || AnimationName == "Skill_W" || AnimationName == "Skill_E" || AnimationName == "isDodge")
        {
            anim.SetTrigger(AnimationName);
        }
        else if(AnimationName == "isRun")
        {
            anim.SetBool(AnimationName, moveVec != Vector3.zero);
        }
        else
        {
            anim.SetBool(AnimationName, true);
        }
    }

    public virtual void StopAnim(string AnimationName)
    {
        if (AnimationName == "CommonAttack" || AnimationName == "Skill_Q" || AnimationName == "Skill_W" || AnimationName == "Skill_E" || AnimationName == "isDodge")
        {
            anim.ResetTrigger(AnimationName);
        }
        else
        {
            anim.SetBool(AnimationName, false);
        }
    }

    public virtual void AnimState()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            stateIdle = true;
        else
            stateIdle = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
            stateWait = true;
        else
            stateWait = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            stateJump = true;
        else
            stateJump = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            stateFall = true;
        else
            stateFall = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            stateRun = true;
        else
            stateRun = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
            stateDodge = true;
        else
            stateDodge = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1"))
            stateAttack1 = true;
        else
            stateAttack1 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1_Wait"))
            stateAttack1_Wait = true;
        else
            stateAttack1_Wait = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2"))
            stateAttack2 = true;
        else
            stateAttack2 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2_Wait"))
            stateAttack2_Wait = true;
        else
            stateAttack2_Wait = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3"))
            stateAttack3 = true;
        else
            stateAttack3 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1"))
            stateJumpAttack1 = true;
        else
            stateJumpAttack1 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
            stateJumpAttack2 = true;
        else
            stateJumpAttack2 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3"))
            stateJumpAttack3 = true;
        else
            stateJumpAttack3 = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q"))
            stateSkillQ = true;
        else
            stateSkillQ = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W"))
            stateSkillW = true;
        else
            stateSkillW = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E"))
            stateSkillE = true;
        else
            stateSkillE = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E_Wait"))
            stateSkillE_Wait = true;
        else
            stateSkillE_Wait = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            stateDash = true;
        else
            stateDash = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            stateDamage = true;
        else
            stateDamage = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            stateDie = true;
        else
            stateDie = false;
    }

    public virtual void AnimReset()
    {
        //애니메이션 초기화(09.05)
        isSkill = false;
        isAttack = false;
        isJumping = false;
        isRun = false;
        isDodge = false;
        StopAnim("Skill_Q");
        StopAnim("Skill_W");
        StopAnim("Skill_E");
        StopAnim("isRun");
        StopAnim("isDodge");
    }
    #endregion

    #region 보조스킬
    //신속(유체화)
    public virtual void Spell_Swiftness(int Spell_num){
        if(Spell_Swiftness_Ready){
            StartCoroutine(Spell_Swiftness_On());
            Swiftness_Buff_ON = true;
            Spell_Swiftness_CoolTime = 0f; //도훈 2024-09-20
            Spell_Swiftness_Ready = false;
            Set_Spell_Cooltime("Spell_Swiftness", Spell_num);
        }
    }

    public virtual IEnumerator Spell_Swiftness_On()
    {
        Swiftness_buffTime = 15f;
        buffPer = 0.2f;
        Swiftness_Stat = Status.PercentSpeed;
        Status.PercentSpeed += (Status.PercentSpeed * buffPer);
        Status.StatUpdate();
        spellSounds[0].PlayOneShot(spellSounds[0].clip); //스펠 사운드 추가(10.02)
        Spell_Swiftness_Effect.SetActive(true);
        Debug.Log("신속 ON");
        yield break;
    }

    //저지불가

    public virtual void Spell_Unstoppable(int Spell_num){
        if(Spell_Unstoppable_Ready){
            Unstoppable_Buff_ON = true;
            StartCoroutine(Spell_Unstoppable_On());
            Debug.Log("저지불가 ON");
            Spell_Unstoppable_CoolTime = 0f;    //도훈 2024-09-20
            Spell_Unstoppable_Ready = false;
            Set_Spell_Cooltime("Spell_Unstoppable", Spell_num);
        }
    }

    public virtual IEnumerator Spell_Unstoppable_On()
    {
        Unstoppable_buffTime = 20f;
        spellSounds[4].PlayOneShot(spellSounds[4].clip); //스펠 사운드 추가(10.02)
        cameraEffect.GetComponent<CameraEffectCtrl>().unStoppableCamera(); //저지불가 이펙트 추가(09.30)
        yield break;
    }

    //회복
    public virtual void Spell_Heal(int Spell_num){
        if(Spell_Heal_Ready){
            Status.HP = Status.MaxHP;
            CheckHp();
            StartCoroutine(Spell_Heal_on());
            Debug.Log("힐 ON");
            Spell_Heal_CoolTime = 0f;   //도훈 2024-09-20
            Spell_Heal_Ready = false;
            Set_Spell_Cooltime("Spell_Heal", Spell_num);
        }
    }

    public virtual IEnumerator Spell_Heal_on()
    {
        SkillEffect = Instantiate(Spell_Heal_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
        SkillEffect.transform.parent = EffectGen.transform;
        SkillEffect.transform.localPosition = new Vector3(0,-0.9f,0);
        spellSounds[2].PlayOneShot(spellSounds[2].clip); //스펠 사운드 추가(10.02)
        yield return new WaitForSeconds(3.0f);
        Destroy(SkillEffect);
    }

    //기절 미완
    public virtual void Spell_Stun(int Spell_num){
        if(Spell_Stun_Ready){
            StartCoroutine(Spell_Stun_on());
            Debug.Log("기절 ON");
            Spell_Stun_CoolTime = 0f;   //도훈 2024-09-20
            Spell_Stun_Ready = false;
            Set_Spell_Cooltime("Spell_Stun", Spell_num);
        }
    }
    public virtual IEnumerator Spell_Stun_on()
    {
        SkillEffect = Instantiate(Spell_Stun_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot + 90f, 0));
        spellSounds[1].PlayOneShot(spellSounds[1].clip); //스펠 사운드 추가(10.02)
        yield return new WaitForSeconds(3.0f);
        Destroy(SkillEffect);
    }


    //무적
    public virtual void Spell_Immune(int Spell_num){
        if(Spell_Immune_Ready){
            StartCoroutine(Immune(10.0f));
            StartCoroutine(Spell_Immune_on());
            Debug.Log("무적 ON");
            Spell_Immune_CoolTime = 0f; //도훈 2024-09-20
            Spell_Immune_Ready = false;
            Set_Spell_Cooltime("Spell_Immune", Spell_num);
        }
    }
    public virtual IEnumerator Spell_Immune_on()
    {
        Spell_Immune_Effect.SetActive(true);
        spellSounds[6].PlayOneShot(spellSounds[6].clip); //스펠 사운드 추가(10.02)
        yield return new WaitForSeconds(10.0f);
        Spell_Immune_Effect.SetActive(false);
    }

    //시간 감속

    public virtual void Spell_TimeSlowdown(int Spell_num){
        Debug.Log(Spell_TimeSlowdown_Ready);
        Debug.Log(Spell_TimeSlowdown_CoolTime);
        if(Spell_TimeSlowdown_Ready){
        StartCoroutine(Spell_TimeSlowdown_On());
        Status.Spell_TimeSlowdown_ON = true;
        Spell_TimeSlowdown_CoolTime = 0f;   //도훈 2024-09-20
        Spell_TimeSlowdown_Ready = false;
        Set_Spell_Cooltime("Spell_TimeSlowdown", Spell_num);
        }
    }
    public virtual IEnumerator Spell_TimeSlowdown_On()
    {
        TimeSlowdown_buffTime = 180f;
        Debug.Log("시감감속 ON");
        Spell_TimeSlowdown_Effect.SetActive(true);
        spellSounds[5].PlayOneShot(spellSounds[5].clip); //스펠 사운드 추가(10.02)
        yield break;
    }

    //부활
    //이거 진행해줘야함(10.02)
    //spellSounds[7].PlayOneShot(spellSounds[7].clip); //스펠 사운드도 추가해야함(10.02)

    //분노의 포효
    public virtual void Spell_RoarOfAnger(int Spell_num){
        if(Spell_RoarOfAnger_Ready){
            StartCoroutine(Spell_RoarOfAnger_On());
            RoarOfAnger_Buff_ON = true;
            Spell_RoarOfAnger_CoolTime = 0f;    //도훈 2024-09-20
            Spell_RoarOfAnger_Ready = false;
            Set_Spell_Cooltime("Spell_RoarOfAnger", Spell_num);
        }
    }

    public virtual IEnumerator Spell_RoarOfAnger_On()
    {
        RoarOfAnger_buffTime = 15f;
        buffPer = 0.2f;
        RoarOfAnger_Stat = Status.PercentAD;
        Status.PercentAD += (Status.PercentAD * buffPer);
        Status.StatUpdate();
        Debug.Log("분노의 포효 ON");
        Spell_RoarOfAnger_Effect.SetActive(true);
        spellSounds[3].PlayOneShot(spellSounds[3].clip); //스펠 사운드 추가(10.02)
        yield break;
    }

    //전체적으로 보조스킬 사용하도록 하는 함수
    public virtual void UseSpell(string Spell, int Spell_num){
        Debug.Log(Spell);
        switch(Spell){
            case "Spell_Swiftness":
                Spell_Swiftness(Spell_num);
                break;
            case "Spell_Unstoppable":
                Spell_Unstoppable(Spell_num);
                break;
            case "Spell_Heal":
                Spell_Heal(Spell_num);
                break;
            case "Spell_Stun":
                Spell_Stun(Spell_num);
                break;
            case "Spell_Immune":
                Spell_Immune(Spell_num);
                break;
            case "Spell_TimeSlowdown":
                Spell_TimeSlowdown(Spell_num);
                break;
            case "Spell_RoarOfAnger":
                Spell_RoarOfAnger(Spell_num);
                break;
        }
    }
    //보조스킬 쿨타임 적용하기
    public virtual void Set_Spell_Cooltime(string Spell, int Spell_num){
        switch(Spell_num){
            case 1:
                switch(Spell){
                    case "Spell_Swiftness":
                        Spell_1_CoolTime = Swiftness_CoolTime;
                        break;
                    case "Spell_Unstoppable":
                        Spell_1_CoolTime = Unstoppable_CoolTime;
                        break;
                    case "Spell_Heal":
                        Spell_1_CoolTime = Heal_CoolTime;
                        break;
                    case "Spell_Stun":
                        Spell_1_CoolTime = Stun_CoolTime;
                        break;
                    case "Spell_Immune":
                        Spell_1_CoolTime = Immune_CoolTime;
                        break;
                    case "Spell_TimeSlowdown":
                        Spell_1_CoolTime = TimeSlowdown_CoolTime;
                        break;
                    case "Spell_RoarOfAnger":
                        Spell_1_CoolTime = RoarOfAnger_CoolTime;
                        break;
                }
            Dcool.fillAmount = 1;
            Debug.Log(Spell_1_CoolTime);
            break;
            case 2:
                switch(Spell){
                    case "Spell_Swiftness":
                        Spell_2_CoolTime = Swiftness_CoolTime;
                        break;
                    case "Spell_Unstoppable":
                        Spell_2_CoolTime = Unstoppable_CoolTime;
                        break;
                    case "Spell_Heal":
                        Spell_2_CoolTime = Heal_CoolTime;
                        break;
                    case "Spell_Stun":
                        Spell_2_CoolTime = Stun_CoolTime;
                        break;
                    case "Spell_Immune":
                        Spell_2_CoolTime = Immune_CoolTime;
                        break;
                    case "Spell_TimeSlowdown":
                        Spell_2_CoolTime = TimeSlowdown_CoolTime;
                        break;
                    case "Spell_RoarOfAnger":
                        Spell_2_CoolTime = RoarOfAnger_CoolTime;
                        break;
                }
            Fcool.fillAmount = 1;
            Debug.Log(Spell_2_CoolTime);
            break;
        }
        
    }
    #endregion

    #region 구르기
    public virtual void ChargeDodge()
    {
        if(DodgeAmount < 2)
        {    
            Dodge_CoolTime += Time.deltaTime;
            if(Dodge_CoolTime > TotalDodge_CoolTime){
                DodgeAmount += 1;
                Dodge_CoolTime = 0f;
                Rcool.fillAmount = 1;
            }
            DodgeValue.text = DodgeAmount.ToString();
        }
    }
    #endregion
    
}   