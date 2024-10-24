// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System;
// using Unity.Burst.CompilerServices;
// using UnityEngine.SceneManagement;
// using Photon.Pun;

// public class Server_PlayerCtrl : MonoBehaviour, IPlayerSkill, IPlayerAnim, IPlayerAttack
// {
//     #region 변수 선언
//     //Raycast 관련
//     protected float raycastDistance = 0.75f;
//     protected RaycastHit hit;
//     protected GameObject BossWall1;
//     protected GameObject BossWall2;
//     protected BoxCollider BossWall1Collider;
//     protected BoxCollider BossWall2Collider;

//     // GetAxis 값
//     protected float hAxis;

//     // Player의 transform, YPosition, YRotation 값
//     protected Transform trs;
//     protected float YRot;
//     protected float YPos;

//     //플레이어 스테이터스
//     public float Damage;       //받은 피해량
//     public float moveSpeed;     //이동속도
//     public float moveSpd;      //이동속도
//     public float JumpPower;     //점프력
//     public float fallPower;     //떨어지는 힘

//     // 애니메이션 컨트롤
//     protected Vector3 initPos;
//     protected bool isFloor = false;
//     protected bool isStair = false;
//     protected bool isSkill = false;
//     protected bool isAttack = false;
//     protected bool isJumping = false;
//     protected bool isRun = false;
//     protected bool isForward = true;
//     protected bool isJumpAttack;
//     protected bool isFall = false;
//     protected bool isDodge = false;
//     protected bool isCommonAttack1InProgress = false;
//     protected bool isCommonAttack2InProgress = false;
//     protected bool isCommonAttack3InProgress = false;
    
//     //점프 관련(08.28)
//     protected float JumpCoolTime;

//     //애니메이션 상태 컨트롤 (GetCurrentAnimatorStateInfo(0).IsName 을 체크)
//     protected bool stateIdle = false;
//     protected bool stateWait = false;
//     protected bool stateJump = false;
//     protected bool stateFall = false;
//     protected bool stateRun = false;
//     protected bool stateDodge = false;
//     protected bool stateAttack1 = false;
//     protected bool stateAttack1_Wait = false;
//     protected bool stateAttack2 = false;
//     protected bool stateAttack2_Wait = false;
//     protected bool stateAttack3 = false;
//     protected bool stateJumpAttack1 = false;
//     protected bool stateJumpAttack2 = false;
//     protected bool stateJumpAttack3 = false;
//     protected bool stateSkillQ = false;
//     protected bool stateSkillW = false;
//     protected bool stateSkillE = false;
//     protected bool stateSkillE_Wait = false;
//     protected bool stateDamage = false;
//     protected bool stateDash = false;
//     protected bool stateDashAttack = false; // 대쉬공격 사운드땜에 추가 (08.31)
//     protected bool stateDie = false;
//     protected bool stateShop = false; //상점 문제땜에 추가 (09.05)

//     // 코루틴 컨트롤
//     protected bool coroutineMove = false;

//     // 애니메이터, Rigidbody
//     public Animator anim;
//     protected Rigidbody rd;

//     // 이펙트
//     public GameObject commonAttack_Ice1_Effect;
//     public GameObject commonAttack_Ice2_Effect;
//     public GameObject commonAttack_Ice3_Effect;
//     public GameObject commonAttack_Fire1_Effect;
//     public GameObject commonAttack_Fire2_Effect;
//     public GameObject commonAttack_Fire3_Effect;
//     public GameObject Skill_FireQ_Effect;
//     public GameObject Skill_IceQ_Effect;
//     public GameObject Skill_FireW_Effect;
//     public GameObject Skill_IceW_Effect;
//     public GameObject Skill_FireE1_Effect;
//     public GameObject Skill_FireE2_Effect;
//     public GameObject Skill_FireE3_Effect;
//     public GameObject Skill_FireE4_Effect;
//     public GameObject Skill_IceE1_Effect;
//     public GameObject Skill_IceE2_Effect;
//     public GameObject Skill_IceE3_Effect;
//     public GameObject Skill_IceE4_Effect;
//     public GameObject Attack1_Effect;
//     public GameObject Attack2_Effect;
//     public GameObject Attack3_Effect;
//     public GameObject SkillQ_Effect;
//     public GameObject SkillW_Effect;
//     public GameObject SkillE1_Effect;
//     public GameObject SkillE2_Effect;
//     public GameObject SkillE3_Effect;
//     public GameObject SkillE4_Effect;
//     public float SkillYRot;
//     public float LocalSkillYRot;
//     public GameObject EffectGen;
//     public GameObject SkillEffect;
//     public GameObject DamageText; //텍스트
//     public GameObject Damage_Effect;
//     public GameObject Heal_Effect;
//     public GameObject Item_Weapon_Effect;
//     public GameObject Item_Weapon_Ice_Effect;
//     public GameObject Item_Weapon_Fire_Effect;
//     public GameObject Item_Aura_Effect;
//     public GameObject Item_Aura_Ice_Effect;
//     public GameObject Item_Aura_Fire_Effect;
//     public GameObject Item_Sheild_Effect;
//     public GameObject Item_Time_Effect;
//     public GameObject PlayerCanvas;

//     // 카메라, 사운드
//     protected GameObject mainCamera;
//     protected GameObject cameraEffect;
//     protected AudioClip[] effectAudio;
//     protected bool isSound = false;
//     protected AudioSource[] audioSources;

//     // 벽 충돌체크
//     protected bool WallCollision;

//     // HP Bar
//     protected Slider HpBar;
//     public TMP_Text HpText;

//     //포션
//     public InventoryCtrl InvenCtrl;
//     public TMP_Text hpPotionValue;
//     public TMP_Text ADPotionValue;
//     public TMP_Text ArmorPotionValue;
//     protected float buffTime;
//     protected float buffPer;
//     protected string buffType;
//     protected float buffStat;
//     protected bool ADBuff_On;
//     protected bool ArmorBuff_On;

//     //스탯 UI 관련
//     protected TMP_Text[] StateText; 

//     public static bool isShop = false;

//     //보스 관련
//     public GameObject Druid;
//     public GameObject DruidGen;

//     // 쿨타임 관련
//     protected bool QSkillReady;

//     protected float QSkillCoolTime; //쿨 돌았는지 체크용
//     protected float TotalQSkillCoolTime = 0;  // 실제 쿨타임 도훈 2024-08-27

//     protected bool WSkillReady; 
//     protected float WSkillCoolTime; //쿨 돌았는지 체크용
//     protected float TotalWSkillCoolTime = 0; // 실제 쿨타임  도훈 2024-08-27

//     protected bool ESkillReady;
//     protected float ESkillCoolTime; //쿨 돌았는지 체크용
//     protected float TotalESkillCoolTime = 0; // 실제 쿨타임 도훈 2024-08-27
//     protected Image Qcool;
//     protected Image Wcool;
//     protected Image Ecool;
//     protected bool canTakeDamage = true; // 데미지를 가져올 수 있는지
//     protected float damageCooldown = 1.0f; // 1초마다 틱데미지를 가져오기 위함

//     // 무적 관련
//     protected int ImmuneCount = 0;
//     protected bool isImmune;

//     // 회전 관련
//     protected GameObject CurrentFloor;
//     protected Vector3 moveVec;

//     //아이템 세트효과 관련
//     protected int attackCount = 0;
//     protected int currentStack = 0;
//     protected float clockEffectTime = 0;
//     Stack prevADC = new Stack();
//     protected TextMeshProUGUI CoinText;

//     #endregion

    
//     #region 서버 관련
//     private string RPCproperty;
//     protected PhotonView photonview;
//     #endregion

//     protected virtual void Start()
//     {
//         // 8번 세트 3번 이펙트 효과 관리
//         GetComponent<SkinnedMeshAfterImage>().enabled = false;
//         Item_Time_Effect.SetActive(false);

//         //상점 씬 등에 거쳐왔을 시 플레이어 위치 초기화(06.13)
//         if(PlayerPrefs.GetFloat("PlayerXPos") != null && PlayerPrefs.GetString("Hidden_Shop_Spawn_Scene") == SceneManager.GetActiveScene().name)
//         {
//             transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerXPos"), PlayerPrefs.GetFloat("PlayerYPos"), PlayerPrefs.GetFloat("PlayerZPos"));
//         }

//         // 플레이어 스테이터스 초기화
//         SetIce();

//         // 보스 문 할당
//         // BossWall1 = GameObject.Find("BossWall1").gameObject;
//         // BossWall1Collider = BossWall1.GetComponent<BoxCollider>();
//         // BossWall2 = GameObject.Find("BossWall2").gameObject;
//         // BossWall2Collider = BossWall2.GetComponent<BoxCollider>();

//         // HP Bar 설정
//         HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();
//         HpText = GameObject.Find("StatPoint - Hp").GetComponent<TMP_Text>();
//         HpText.text = "HP" + Status.HP + "/" + Status.MaxHP;
//         CheckHp();

//         //코인 설정(09.05)
//         CoinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();

//         //포션 설정(06.15)
//         InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
//         hpPotionValue = GameObject.Find("HP_Potion - Text").GetComponent<TMP_Text>();
//         ADPotionValue = GameObject.Find("AD_Potion - Text").GetComponent<TMP_Text>();
//         ArmorPotionValue = GameObject.Find("Armor_Potion - Text").GetComponent<TMP_Text>();

//         //스텟 UI 변동 설정(06.14)
//         StateText = new TMP_Text[8];
//         for(int i = 0; i <= 7; i++)
//         {
//             string statname = "StatText-" + i;
//             //Debug.Log(statname);
//             StateText[i] = GameObject.Find(statname).GetComponent<TMP_Text>();
//         }
        
//         //데미지 텍스트 설정(06.01)
//         PlayerCanvas = this.transform.Find("Canvas - Player").gameObject;     //잠시
        
//         //쿨타임 UI(03.18)
//         Qcool = GameObject.Find("CoolTime-Q").GetComponent<Image>();
//         Wcool = GameObject.Find("CoolTime-W").GetComponent<Image>();
//         Ecool = GameObject.Find("CoolTime-E").GetComponent<Image>();

//         // 애니메이션, Rigidbody, Transform 컴포넌트 지정
//         anim = this.GetComponent<Animator>();
//         rd = GetComponent<Rigidbody>();
//         trs = GetComponentInChildren<Transform>();

//         initPos = trs.position; // initPos에 Transform.position 할당
//         mainCamera = GameObject.FindWithTag("MainCamera");  // 메인 카메라 지정
//         cameraEffect = GameObject.FindWithTag("CameraEffect"); // 카메라 이펙트 볼륨 설정
//         PlayAnim("isIdle");   // isIdle을 True로 설정해서 Idle 상태 지정
//         //EffectGen = transform.Find("EffectGen - Player").gameObject; // EffectGen 지정

//         // 애니메이션, 스킬 관리하는 bool값을 false로 초기화
//         isSkill = false;
//         isAttack = false;
//         isJumping = false;
//         isRun = false;

//         //사운드
//         audioSources = GetComponents<AudioSource>();
//         for (int i = 0; i < audioSources.Length; i++)
//         {
//             audioSources[i].Stop();
//         }
//         photonview = GetComponent<PhotonView>();
//         if(photonview.IsMine){
//             this.gameObject.tag = "Player";
//         }
//         else{
//             this.gameObject.tag = "OtherPlayer";
//         }

//     }
//     protected virtual void FixedUpdate()
//     {
//         if(!photonview.IsMine){
//                 moveVec = Vector3.forward;
//             }
//         //처음 시작 시에만 피 초기화
//         if(PlayerPrefs.GetInt("GameSet") == 1)
//         {
//             SetHp(100);
//             PlayerPrefs.SetInt("GameSet", 0);
//         }
            
//         if(photonview.IsMine){
//             CheckState();
//             // Move 함수 실행
//             if (!isSkill && !isAttack && !stateAttack3 && !anim.GetBool("isDie"))
//             {
//                 Move();
//                 Turn();
//             }

//             // Turn 함수 실행
//             if (!isSkill && !isAttack && !stateIdle && !anim.GetBool("isDie"))
//             {
//                 Turn();
//             }
//         }
//         //코인 업데이트(09.05)
//         if(PlayerPrefs.GetFloat("Coin").ToString() != CoinText.text)
//             CoinText.text = PlayerPrefs.GetFloat("Coin").ToString();
//         CheckHp();
//     }

//     protected virtual void Update()
//     {
        
//         if(photonview.IsMine){
//             #region 7번 세트 3셋 이펙트 효과 (등 뒤에 잔상 이펙트)
//             if (Status.set7_3_Activated && GetComponent<SkinnedMeshAfterImage>().enabled == false)
//             {
//                 photonview.RPC("SetEffect",RpcTarget.All, 6);
//             }
//             #endregion
//             #region 8번 세트 3셋 이펙트 효과 (시계)
//             if (Item_Time_Effect.activeSelf)
//             {
//                 clockEffectTime -= Time.deltaTime;
//                 if(clockEffectTime < 0)
//                 {
//                     photonview.RPC("SetEffect",RpcTarget.All, 7);
//                 }
//             }
//             #endregion
//         }
    
//         // 해당 bool값 실행 시 모든 행동 멈춤
//         if(!Status.IsShop && !anim.GetBool("isDie"))
//         {
//             ////////////////////////////////////내꺼 아닐때도 호출
//             // 땅에 닿아있는지 체크
//             isGrounded();
//             //애니메이션 상태 확인
//             if(photonview.IsMine){
//             photonview.RPC("AnimState",RpcTarget.All);
//             }

//             ////////////////////////////////////내꺼 일때만 호출
//             if (!canTakeDamage)
//             {
//                 damageCooldown -= Time.deltaTime;
//                 if (damageCooldown < 0)
//                 {
//                     canTakeDamage = true;
//                     damageCooldown = 1.0f;
//                 }
//             }
//             if(photonview.IsMine){
//                //스킬 쿨타임 UI(03.18)
//                 if (Qcool.fillAmount != 0)
//                 {
//                     Qcool.fillAmount -= 1 * Time.smoothDeltaTime / TotalQSkillCoolTime;
//                 }
//                 if (Wcool.fillAmount != 0)
//                 {
//                     Wcool.fillAmount -= 1 * Time.smoothDeltaTime / TotalWSkillCoolTime;
//                 }
//                 if (Ecool.fillAmount != 0)
//                 {
//                     Ecool.fillAmount -= 1 * Time.smoothDeltaTime / TotalESkillCoolTime;
//                 }
//             }

//             // 벽 충돌체크 함수 실행
//             WallCheck();

            
//             if(photonview.IsMine){
//                 // 애니메이션 업데이트
//                 GetInput();
//                 //스킬 쿨타임 충전
//                 SkillCoolTimeCharge();
//             }

//             //로테이션 고정 코드(04.10 백건우 수정, 굴절구간 문제 생길 시 아래 코드 대신 사용)
//             YRot = transform.eulerAngles.y;

//             //Z 포지션 고정
//             transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

//             // char 오브젝트 위치 고정
//             transform.GetChild(0).localPosition = Vector3.zero;

//             //데미지 캔버스 Y값 고정
//             if(mainCamera.transform.eulerAngles.y > 0 && mainCamera.transform.eulerAngles.y < 180)
//                 PlayerCanvas.transform.localRotation = Quaternion.Euler(0, LocalSkillYRot - 180f, 0);
//             else
//                 PlayerCanvas.transform.localRotation = Quaternion.Euler(0, LocalSkillYRot + 180f, 0);

//             // Attack 함수 실행
//             if (Input.GetKeyDown(KeyCode.A))
//             {   
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack_anim",RpcTarget.All);
//                 }
//             }

//             //기본공격1 & 기본공격3 시 전진 애니메이션
//             if (stateAttack1 == true && !isCommonAttack1InProgress)
//             {   
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,0);
//                 }
//                 isCommonAttack1InProgress = true;
//             }
//             else if (stateAttack2 == true && !isCommonAttack2InProgress && !isSound)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,1);
//                 }
//                 isCommonAttack2InProgress = true;
//             }
//             else if (stateAttack3 == true && !isCommonAttack3InProgress)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,2);
//                 }
//                 isCommonAttack3InProgress = true;
//             }

//             //지상공격 2타, 3타 시 방향전환 되도록
//             if(stateAttack1_Wait == true ||
//             stateAttack2_Wait == true)
//             {
//                 isAttack = false;
//             }
//             else if(stateAttack1 == true ||
//                     stateAttack2 == true ||
//                     stateAttack3 == true)
//             {
//                 isAttack = true;
//             }

//             //점프공격 카메라 && 사운드
//             else if (stateJumpAttack1 == true && !coroutineMove)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,3);
//                 }
//                 PlayAnim("isFall");
//             }
//             else if (stateJumpAttack2 == true && !isSound)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,4);
//                 }
//                 PlayAnim("isFall");
//             }
//             else if (stateJumpAttack3 == true && !coroutineMove)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("Attack",RpcTarget.All,5);
//                 }
//                 PlayAnim("isFall");
//             }

//             UpdateCoroutineMoveState();

//             if (stateFall == true && isJumpAttack == true)
//             {
//                 StopAnim("CommonAttack");
//             }
//             //한 번 점프 시 한 번의 점프공격 콤보만 되게
//             else if (stateWait == true && isJumpAttack == true)
//             {
//                 StopAnim("CommonAttack");
//                 isJumpAttack = false;
//                 isAttack = false;
//             }
//             else if (stateRun == true && isJumpAttack == true)
//             {
//                 StopAnim("CommonAttack");
//                 isJumpAttack = false;
//                 isAttack = false;
//             }

//             if (stateJump == true && isJumpAttack == true)
//             {
//                 StopAnim("CommonAttack");
//                 isAttack = false;
//             }

//             //Skill_Q
//             if (Input.GetKeyDown(KeyCode.Q)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && QSkillCoolTime >= TotalQSkillCoolTime //도훈 2024-08-27
//             && !isAttack
//             && !isDodge)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("UseSkill",RpcTarget.All,"Q");
//                 TotalQSkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
//                 }
//                 // Debug.Log("TotalQSkillCoolTime :" + TotalQSkillCoolTime);
//                 // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
//                 // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
//             }
            
//             //Skill_W
//             if (Input.GetKeyDown(KeyCode.W)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && WSkillCoolTime >= TotalWSkillCoolTime //도훈 2024-08-27
//             && !isAttack
//             && !isDodge)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("UseSkill",RpcTarget.All,"W");
//                 TotalWSkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
//                 }
//                 // Debug.Log("TotalWSkillCoolTime :" + TotalWSkillCoolTime);
//                 // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
//                 // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
//             }

//             //Skill_E
//             if (Input.GetKeyDown(KeyCode.E)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && ESkillCoolTime >= TotalESkillCoolTime //도훈 2024-08-27
//             && !isAttack
//             && !isDodge)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("UseSkill",RpcTarget.All,"E");
//                 TotalESkillCoolTime = ((10f-Status.FixedCooltime)*(100f/Status.PercentCooltime)); //도훈 2024-08-27
//                 }
//                 // Debug.Log("TotalESkillCoolTime :" + TotalESkillCoolTime);
//                 // Debug.Log("Status.FixedCooltime :" + Status.FixedCooltime);
//                 // Debug.Log("Status.PercentCooltime :" + Status.PercentCooltime);
//             }

//             //Jump
//             if (Input.GetKeyDown(KeyCode.Space) && !isSkill && !isAttack && !isJumping
//                 && !stateJump && !stateFall && !anim.GetBool("isFall") && !isDodge && JumpCoolTime == 0)
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("RPCJump",RpcTarget.All);
//                 }
//             }
//             else
//             {
//                 //isJumping = false;
//             }
//             //점프 모션이 실행되야만 점프가 실행되게(애니메이션 딜레이 및 더블점프 강제 제거)
//             if (isJumping == true)
//             {
//                 if(photonview.IsMine){
//                     photonview.RPC("Jump",RpcTarget.All);
//                 }
//                 isJumping = false;
//             }
//             // Dodge 함수 실행
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 if(photonview.IsMine){
//                 photonview.RPC("RPCDodge",RpcTarget.All);
//                 }
//             }
//             if (stateDodge == true)
//             {
//                 StopAnim("isJump");
//             }
//             //다중 점프 방지 코드
//             if(stateFall == true && isFloor == false && isStair == false)
//             {
//                 PlayAnim("isFall");
//             }

//             //Idle일때 스킬 및 공격 false 판정
//             if (stateIdle == true && isDodge == false)
//             {
//                 PlayAnim("isIdle");
//                 isAttack = false;
//                 isSkill = false;
//                 StopAnim("CommonAttack");
//             }

//             //다른 모션일 때, 혹시라도 Move가 실행되도 달리지 못하게
//             if (stateWait == true || stateIdle == true || stateAttack1 == true ||
//             stateAttack1_Wait == true || stateAttack2 == true || stateAttack2_Wait == true ||
//             stateAttack3 == true || stateSkillQ == true || stateSkillW == true ||
//             stateSkillE == true || stateJumpAttack2 == true ||
//             (stateJump == true && !anim.GetBool("isRun")) ||
//             (stateFall == true && !anim.GetBool("isRun")))
//             {
//                 moveSpd = 0;
//             }

//             //대쉬일 때
//             else if (stateDash == true)
//             {
//                 moveSpd = moveSpeed * 1.25f;
//             }
//             else
//             {
//                 moveSpd = moveSpeed;
//             }

//             //캐릭터 스킬 이펙트
//             LocalSkillYRot = transform.localEulerAngles.y;
//             SkillYRot = transform.eulerAngles.y;

//             if(photonview.IsMine){
//                 RPCproperty = PlayerPrefs.GetString("property");
//                 photonview.RPC("ApplyProperty",RpcTarget.All, RPCproperty);
//                 //2번 세트 3세트 효과 
//                 if (Status.set2_3_Effect_Activated)
//                 {
//                     photonview.RPC("SetEffect",RpcTarget.All, 4);
//                 }
//             }


//             // 플레이어 피가 30보다 작으면 지속적으로 화면이 깜빡임
//             if(Status.HP <= 30)
//             {
//                 cameraEffect.GetComponent<CameraEffectCtrl>().DangerousCamera();
//             }

//             //포션 버프
//             if(buffType == "AD")
//             {
//                 if(ADBuff_On == false)
//                 {
//                     ADBuff_On = true;
//                     StartCoroutine(PowerUp_On());
//                 }
//                 buffTime -= Time.deltaTime;
//                 if(buffTime < 0)
//                 {
//                     buffTime = 0;
//                     Status.TotalAD = buffStat;
//                     buffStat = 0;
//                     buffType = "None";
//                     ADBuff_On = false;
//                 }
//             }
//             else if(buffType == "Armor")
//             {
//                 if(ArmorBuff_On == false)
//                 {
//                     ArmorBuff_On = true;
//                     StartCoroutine(ArmorUp_On());
//                 }
//                 buffTime -= Time.deltaTime;
//                 if(buffTime < 0)
//                 {
//                     buffTime = 0;
//                     Status.TotalArmor = buffStat;
//                     buffType = "None";
//                     ArmorBuff_On = false;
//                 }
//             }
//             //포션 눌렀을 때
//             // 힐 Potion
//             hpPotionValue.text = InvenCtrl.PotionCount.ToString();
//             if(Input.GetKeyDown(KeyCode.Alpha1) && !anim.GetBool("isDie") && InvenCtrl.PotionCount > 0)
//             {
//                 if(photonview.IsMine){
//                     HealHp();   
//                 }
//             }
//             // 공격 Potion
//             ADPotionValue.text = InvenCtrl.ADPotionCount.ToString();
//             if(Input.GetKeyDown(KeyCode.Alpha2) && !anim.GetBool("isDie") && InvenCtrl.ADPotionCount > 0)
//             {
//                 if(photonview.IsMine){
//                    PowerUp();
//                 }
//             }
//             // 방어 Potion
//             ArmorPotionValue.text = InvenCtrl.ArmorPotionCount.ToString();
//             if(Input.GetKeyDown(KeyCode.Alpha3) && !anim.GetBool("isDie") && InvenCtrl.ArmorPotionCount > 0)
//             {
//                 if(photonview.IsMine){
//                     ArmorUp();
//                 }
//             }

//         }
//         else if(Status.IsShop == true) //(09.05)
//         {
//             moveSpd = 0;
//             stateShop = true;
//         }
//         else
//         {
//             moveSpd = 0;
//         }
//     }

//     #region 포션 효과 설정
//     //(08.29 백건우)
//     public virtual void PowerUp()
//     {
//         InvenCtrl.ADPotionCount -= 1; 
//         buffType = "AD";
//     }

//     public virtual void ArmorUp()
//     {
//         InvenCtrl.ArmorPotionCount -= 1; 
//         buffType = "Armor";
//     }

//     public virtual IEnumerator PowerUp_On()
//     {
//         buffTime = 60f;
//         buffPer = 0.2f;
//         buffStat = Status.TotalAD;
//         Status.TotalAD += (Status.TotalAD * buffPer);
//         Debug.Log("힘 버프");
//         yield break;
//     }

//     public virtual IEnumerator ArmorUp_On()
//     {
//         buffTime = 60f;
//         buffPer = 0.2f;
//         buffStat = Status.TotalArmor;
//         Status.TotalArmor += (Status.TotalArmor * buffPer);
//         Debug.Log("방패 버프");
//         yield break;
//     }

//     #endregion

//     //도적용 무기 세트효과(08.31)
//     protected virtual void Rogue_Sec_Weapon(string weaponType)
//     {
//     }

//     #region HP 설정
//     protected virtual IEnumerator TakeDamage()
//     {
//         if (Status.MaxHP != 0 || Status.HP > 0)
//         {
//             Status.HP -= Damage;
//             CheckHp();
//             PlayAnim("TakeDamage");
//             StartCoroutine(DamageTextAlpha());
//             cameraEffect.GetComponent<CameraEffectCtrl>().DamageCamera();
//             StartCoroutine(Immune(0.5f));   //무적 함수 실행
//             yield return new WaitForSeconds(0.2f);
//             StopAnim("TakeDamage");

//             AnimReset(); //애니메이션 리셋(09.05)
//             cameraEffect.GetComponent<CameraEffectCtrl>().ResetCameraEffect();
//         }

//         if (Status.HP <= 0) // 플레이어가 죽으면 게임오버 창 띄움
//         {
//             PlayAnim("isDie");

//             //InvenCtrl.ResetInven();

//             yield return new WaitForSeconds(2.0f);
//             photonview.RPC("DestroyPlayer",RpcTarget.All);//플레이어 삭제
//             //GameObject.Find("EventSystem").GetComponent<GameEnd>().GameOver(true);
//         }
//     }
//     public virtual void SetHp(float amount) // Hp 세팅
//     {
//         Status.MaxHP = amount;
//         Status.HP = Status.MaxHP;
//     }
//     public virtual void CheckHp() // HP 체크
//     {
//         string inputText = "HP " + Status.HP.ToString("F0") + "/" + Status.MaxHP.ToString("F0");
//         if (HpBar != null)
//             HpBar.value = Status.HP / Status.MaxHP;
//         if (HpText != null)
//             HpText.text = inputText;
//     }
//     public virtual void HealHp()
//     {
//         InvenCtrl.PotionCount -= 1; 
//         Status.HP = Status.MaxHP;
//         CheckHp();
//         StartCoroutine(Heal_on());
//     }
//     //(06.01)
//     protected virtual IEnumerator DamageTextAlpha()
//     {
//         if(anim.GetBool("Die") == false)
//         {   
//             //데미지 텍스트 출력 부분(05.31)
//             GameObject instText = Instantiate(DamageText);
//             instText.transform.SetParent(PlayerCanvas.transform, false);
//             instText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); 
//             instText.GetComponent<TMP_Text>().text = (-Damage).ToString("F0"); //소수점 날리고 데미지 표현
//             float time = 0f;
//             instText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
//             Color fadecolor = instText.GetComponent<TMP_Text>().color;
//             yield return new WaitForSeconds(0.15f);
//             while(fadecolor.a >= 0)
//             {
//                 time += Time.deltaTime;
//                 fadecolor.a = Mathf.Lerp(1, 0, time * 2f);
//                 instText.GetComponent<TMP_Text>().color = fadecolor; // 페이드 되면서 사라짐
//                 instText.transform.position = new Vector3(transform.position.x, transform.position.y + time * 3f + 0.5f, transform.position.z); // 서서히 올라감
//                 //데미지 텍스트 사라지게 해둠(08.30)
//                 if(fadecolor.a == 0)
//                 {
//                     Destroy(instText);
//                     yield break;
//                 }
//                 yield return null;
//             }
//         }
//     }
//     protected virtual IEnumerator Immune(float seconds)
//     {
//         ImmuneCount++;
//         isImmune = true;
//         yield return new WaitForSeconds(seconds);
//         ImmuneCount--;
//         if(ImmuneCount <= 0)
//         {
//             isImmune = false;
//         }
//     }
//     #endregion

//     #region 스탯 UI 관련 함수

//     protected virtual void CheckState()
//     {
//         StateText[0].text = Status.TotalAD.ToString();
//         StateText[1].text = Status.TotalArmor.ToString();
//         StateText[2].text = Status.DisplayADC.ToString();
//         StateText[3].text = Status.DisplayAP.ToString();
//         StateText[4].text = Status.TotalFire.ToString();
//         StateText[5].text = Status.TotalIce.ToString();
//         StateText[6].text = Status.TotalSpeed.ToString();
//         StateText[7].text = (Status.FixedCooltime*-1).ToString();
//     }

//     #endregion

//     #region 이동 관련 함수
//     protected virtual void WallCheck()
//     {
//         WallCollision = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), transform.forward, 0.6f, LayerMask.GetMask("Wall"));
//     }

//     protected virtual void GetInput()
//     {
//         hAxis = Input.GetAxisRaw("Horizontal");
//     }

//     public virtual void Move()
//     {
        
//         if (hAxis != 0)
//         {
//             if(photonview.IsMine){
//             photonview.RPC("PlayAnim",RpcTarget.All,"isRun");
//             moveVec = AdjustDirectionToSlope(transform.forward);
//             }
            
//         }
//         else
//         {
//             if(photonview.IsMine){
//             photonview.RPC("StopAnim",RpcTarget.All,"isRun");
//             moveVec = Vector3.zero;
//             }
            
//         }
//         if (!WallCollision)
//         {
//             if(photonview.IsMine){
//             transform.position += moveVec * moveSpd * Time.fixedDeltaTime;
//             }
//         }
//         StartCoroutine(Delay(0.2f));
//     }
//     public virtual IEnumerator Dodge()
//     {
//         StartCoroutine(Immune(0.5f));
//         PlayAnim("isDodge");
//         isDodge = true;
//         yield return new WaitForSeconds(0.5f);
//         StopAnim("isDodge");
//         isDodge = false;
//     }

//     protected virtual Vector3 AdjustDirectionToSlope(Vector3 direction)
//     {
//         if (!anim.GetBool("isFall"))
//         {
//             return Vector3.ProjectOnPlane(direction, hit.normal).normalized;
//         }
//         else
//         {
//             return direction;
//         }
//     }
//     protected virtual bool isGrounded()
//     {
//         if (Physics.Raycast(transform.position - new Vector3(0, -0.1f, 0), -Vector3.up, out hit, raycastDistance))
//         {
//             if (hit.collider.CompareTag("Floor") && !hit.collider.CompareTag("Stair"))
//             {
//                 isFloor = true;
//                 if (Vector3.Distance(hit.point, transform.position) <= 0.01)
//                 {
//                     isJumping = false; //isJump, isFall을 다시 false로
//                     StopAnim("isJump");
//                     StopAnim("isFall");
//                     JumpCoolTime = 0; //착지 후 점프 되게 쿨타임 걸어 둠
//                 }
//                 return true;
//             }
//             else if (hit.collider.CompareTag("Stair"))
//             {
//                 isStair = true;
//                 isJumping = false; //isJump, isFall을 다시 false로
//                 StopAnim("isJump");
//                 StopAnim("isFall");
//                 JumpCoolTime = 0; //착지 후 점프 되게 쿨타임 걸어 둠
//                 return true;
//             }
//         }
//         else
//         {
//             PlayAnim("isFall");
//         }
//         isFloor = false;
//         isStair = false;
//         Debug.DrawRay(transform.position - new Vector3(0, -0.1f, 0), -Vector3.up * raycastDistance, Color.red);
//         return false;
//     }
//     protected virtual void Turn()
//     {
//         if (hAxis > 0)
//         {
//             transform.localRotation = Quaternion.Euler(0, 90, 0);
//         }
//         else if (hAxis < 0)
//         {
//             transform.localRotation = Quaternion.Euler(0, -90, 0);
//         }
//     }
//     [PunRPC]
//     protected virtual void Jump()
//     {
//         PlayAnim("isJump");
//         rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
//     }
//     protected virtual void Fall()
//     {
//         PlayAnim("isFall"); //떨어지는것으로 감지
//         rd.AddForce(Vector3.down * fallPower, ForceMode.VelocityChange);
//     }
//     protected virtual void Stay()
//     {
//         isJumping = false; //isJump, isFall을 다시 false로
//         StopAnim("isJump");
//         StopAnim("isFall");
//     }
//     #endregion

//     #region 충돌 관련 함수
//     protected virtual void OnTriggerEnter(Collider col)
//     {
//         if (col.gameObject.tag == "Monster_Melee" &&!isImmune )
//         {
//             // 특정 이름을 가진 부모 객체를 찾습니다.
//             string targetParentName = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
//             Transform parent = col.transform;
//             Server_MonsterCtrl monsterCtrl = null;
            

//             while (parent != null)
//             {
//                 if (parent.name == targetParentName)
//                 {
//                     monsterCtrl = parent.GetComponent<Server_MonsterCtrl>();
//                     break;
//                 }
//                 parent = parent.parent;
//             }

//             if(monsterCtrl != null && Status.set2_3_Activated)
//             {
//                 float reflectDamage = Status.TotalArmor;
//                 StartCoroutine(monsterCtrl.TakeDamage(reflectDamage,RPCproperty));
//             }

//             // 가져온 몬스터 스크립트가 유효한지 확인합니다.
//             if (monsterCtrl != null)
//             {
//                 // 스크립트의 이름을 가져옵니다.
//                 string monsterScriptName = monsterCtrl.GetType().Name;

//                 // "Ctrl"을 제거하여 몬스터의 이름을 가져옵니다.
//                 string monsterName = monsterScriptName.Replace("Ctrl", "");

//                 // 몬스터 이름을 사용하여 해당 몬스터의 스크립트 타입을 가져옵니다.
//                 System.Type monsterScriptType = System.Type.GetType(monsterScriptName);

//                 // 가져온 스크립트를 동적으로 몬스터 스크립트 타입으로 캐스팅합니다.
//                 object specificMonsterCtrl = Convert.ChangeType(monsterCtrl, monsterScriptType);

//                 // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
//                 float atkValue = (float)((specificMonsterCtrl as MonoBehaviour).GetType().GetField("ATK").GetValue(specificMonsterCtrl));
//                 Debug.Log("몬스터의 ATK 값: " + atkValue);
//                 Damage = atkValue;
//                 if(photonview.IsMine){
//                 StartCoroutine(TakeDamage());
//                 }
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }

//         else if (col.gameObject.tag == "Monster_Ranged" && !isImmune)
//         {
//             // 특정 이름을 가진 부모 객체를 찾습니다.
//             string targetParentName = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
//             Transform parent = col.transform;
//             MonsterCtrl monsterCtrl = null;

//             while (parent != null)
//             {
//                 if (parent.name == targetParentName)
//                 {
//                     monsterCtrl = parent.GetComponent<MonsterCtrl>();
//                     break;
//                 }
//                 parent = parent.parent;
//             }

//             if (monsterCtrl != null && Status.set2_3_Activated)
//             {
//                 float reflectDamage = Status.TotalArmor;
//                 if(photonview.IsMine){
//                 StartCoroutine(monsterCtrl.TakeDamage(reflectDamage));
//                 }
//             }

//             // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
//             MonoBehaviour attackCtrl = col.gameObject.GetComponent<MonoBehaviour>();

//             // 가져온 몬스터 스크립트가 유효한지 확인합니다.
//             if (attackCtrl != null)
//             {
//                 // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
//                 float atkValue = (float)attackCtrl.GetType().GetField("ATK").GetValue(attackCtrl);
//                 Debug.Log("몬스터의 ATK 값: " + atkValue);
//                 Damage = atkValue;
//                 if(photonview.IsMine){
//                 StartCoroutine(TakeDamage());
//                 }
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }
//     }

//     protected virtual void OnTriggerStay(Collider col)
//     {
//         if (canTakeDamage == true && col.gameObject.tag == "Druid_Poison" && !isImmune)
//         {
//             // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
//             MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

//             // 가져온 몬스터 스크립트가 유효한지 확인합니다.
//             if (monsterCtrl != null)
//             {
//                 // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
//                 float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
//                 Debug.Log("몬스터의 ATK 값: " + atkValue);
//                 Damage = atkValue;
//                 canTakeDamage = false;
//                 if(photonview.IsMine){
//                 StartCoroutine(TakeDamage());
//                 }
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }
//     }
//     protected virtual void OnCollisionExit(Collision collision)
//     {
//         if (collision.gameObject.tag == "Floor" && !stateSkillE && !stateSkillE && !isStair && !isFloor)    // Tag가 Floor인 오브젝트와 충돌이 끝났을 때
//         {
//             Fall();
//         }
//     }

//     protected virtual void OnTriggerExit(Collider col)
//     {
//         if(col.gameObject.tag == "BossWall1" && BossWall1.transform.localPosition.x - transform.localPosition.x < 0)
//         {
//             BossWall1.layer = 3;
//             BossWall2.layer = 3;
//             BossWall1Collider.isTrigger = false;
//             BossWall2Collider.isTrigger = false;
//             Instantiate(Druid, DruidGen.transform.position, Quaternion.Euler(0, -90f, 0));
//         }
//     }
//     #endregion

//     #region 스킬 / 공격 관련 함수. 자식 스크립트에 상속은 시키되 함수를 비워서 각각에 맞는 스크립트를 사용할 수 있도록 함.
//     protected virtual void SetIce()
//     {
//     }
//     protected virtual void SetFire()
//     {
//     }
//     [PunRPC]
//     protected virtual void Attack_anim()
//     {
//     }
//     [PunRPC]
//     public virtual void Attack(int AttackNumber)
//     {
//         Debug.Log("Attack 메서드 실행, AttackNumber는 = " + AttackNumber);
//         switch (AttackNumber) {
//             case 0:
//             case 1:
//             case 2:
//                 {
//                     if(attackCount < 3 && Status.set5_4_Activated)
//                     {
//                         attackCount++;
//                     }
//                     if(attackCount == 3 && currentStack < 3 && Status.set5_4_Activated)
//                     {
//                         Debug.Log("평타 " + attackCount + "번 침");
//                         if(photonview.IsMine){
//                         StartCoroutine(set5_4());
//                         }
//                     }
//                     if(attackCount >= 3 && Status.set5_4_Activated)
//                     {
//                         attackCount = 0;
//                     }
//                     break;
//                 }
//         }
//     }

//     public virtual IEnumerator set5_4()
//     {
//         currentStack++;
//         prevADC.Push(Status.TotalADC);
//         Status.TotalADC = Status.TotalADC * 1.2f;
//         Debug.Log("강화된 공격력 : " + Status.TotalADC);
//         Debug.Log("현재 " + currentStack + "스택");
//         yield return new WaitForSeconds(7);
//         currentStack--;
//         Status.TotalADC = (float)prevADC.Pop();
//         Debug.Log("돌아간 공격력 : " + Status.TotalADC);
//         Debug.Log("현재 " + currentStack + "스택");
//     }

//     protected virtual void SkillCoolTimeCharge()
//     {
//         QSkillCoolTime += Time.deltaTime;
//         WSkillCoolTime += Time.deltaTime;
//         ESkillCoolTime += Time.deltaTime;

//         if(QSkillCoolTime > TotalQSkillCoolTime) //도훈 2024-08-27
//         {
//             QSkillReady = true;
//         }
//         if (WSkillCoolTime > TotalWSkillCoolTime)  //도훈 2024-08-27
//         { 
//             WSkillReady = true;
//         }
//         if (ESkillCoolTime > TotalESkillCoolTime) //도훈 2024-08-27
//         {
//             ESkillReady = true;
//         }
//     }
    
//     protected virtual void ResetAttackInProgressStates()
//     {
//         isCommonAttack1InProgress = false;
//         isCommonAttack2InProgress = false;
//         isCommonAttack3InProgress = false;
//     }
//     protected virtual void UpdateCoroutineMoveState()
//     {
//         if (!(stateAttack1 ||
//             stateAttack2 ||
//             stateAttack3))
//         {
//             ResetAttackInProgressStates();
//         }
//     }

//     [PunRPC]
//     public virtual void UseSkill(string skillName)
//     {
//         if(photonview.IsMine){
//             if (Status.set8_3_Activated)
//             {
//                 photonview.RPC("SetEffect",RpcTarget.All, 5);
//             }
//         }
//     }
//     #endregion

//     #region Delay 함수
//     protected virtual IEnumerator Delay(float seconds)
//     {
//         yield return new WaitForSeconds(seconds);
//         if (stateIdle == true)
//         {
//             moveVec = new Vector3(0, 0, 0);
//             isAttack = false;
//             isSkill = false;
//         }
//         yield return null;
//     }
//     #endregion

//     #region 이펙트 함수

//     public virtual IEnumerator Heal_on()
//     {
//         SkillEffect = Instantiate(Heal_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
//         SkillEffect.transform.parent = EffectGen.transform;
//         yield return new WaitForSeconds(1.0f);
//         Destroy(SkillEffect);
//     }

//     public virtual void Damaged_on()
//     {
//         SkillEffect = Instantiate(Damage_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
//         SkillEffect.transform.parent = EffectGen.transform;
//     }

//     public virtual void Destroyed_Effect()
//     {
//         Destroy(SkillEffect);
//     }

//     #endregion

//     #region 사운드 함수
//     //사운드 함수 추가 (08.31)
//     public virtual IEnumerator Attack_Sound(int AttackValue, float playsec)
//     {
//         audioSources[AttackValue].Play();
//         yield return new WaitForSeconds(playsec);
//         audioSources[AttackValue].Stop();
//         yield return null;
//     }

//     #endregion

//     #region 애니메이션 
//     [PunRPC]
//     public virtual void PlayAnim(string AnimationName)
//     {
//         //Debug.Log(AnimationName + " 실행");
//         if(AnimationName == "CommonAttack" || AnimationName == "Skill_Q" || AnimationName == "Skill_W" || AnimationName == "Skill_E" || AnimationName == "isDodge")
//         {
//             anim.SetTrigger(AnimationName);
//         }
//         else if(AnimationName == "isRun")
//         {
//             anim.SetBool(AnimationName, moveVec != Vector3.zero);
//         }
//         else
//         {
//             anim.SetBool(AnimationName, true);
//         }
//     }

//     [PunRPC]
//     public virtual void StopAnim(string AnimationName)
//     {
//         if (AnimationName == "CommonAttack" || AnimationName == "Skill_Q" || AnimationName == "Skill_W" || AnimationName == "Skill_E" || AnimationName == "isDodge")
//         {
//             anim.ResetTrigger(AnimationName);
//         }
//         else
//         {
//             anim.SetBool(AnimationName, false);
//         }
//     }

//     [PunRPC]
//     public virtual void AnimState()
//     {
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//             stateIdle = true;
//         else
//             stateIdle = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
//             stateWait = true;
//         else
//             stateWait = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
//             stateJump = true;
//         else
//             stateJump = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
//             stateFall = true;
//         else
//             stateFall = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
//             stateRun = true;
//         else
//             stateRun = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
//             stateDodge = true;
//         else
//             stateDodge = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1"))
//             stateAttack1 = true;
//         else
//             stateAttack1 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1_Wait"))
//             stateAttack1_Wait = true;
//         else
//             stateAttack1_Wait = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2"))
//             stateAttack2 = true;
//         else
//             stateAttack2 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2_Wait"))
//             stateAttack2_Wait = true;
//         else
//             stateAttack2_Wait = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3"))
//             stateAttack3 = true;
//         else
//             stateAttack3 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1"))
//             stateJumpAttack1 = true;
//         else
//             stateJumpAttack1 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
//             stateJumpAttack2 = true;
//         else
//             stateJumpAttack2 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3"))
//             stateJumpAttack3 = true;
//         else
//             stateJumpAttack3 = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q"))
//             stateSkillQ = true;
//         else
//             stateSkillQ = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W"))
//             stateSkillW = true;
//         else
//             stateSkillW = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E"))
//             stateSkillE = true;
//         else
//             stateSkillE = false;
//         if (anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E_Wait"))
//             stateSkillE_Wait = true;
//         else
//             stateSkillE_Wait = false;
//         if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
//             stateDash = true;
//         else
//             stateDash = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
//             stateDamage = true;
//         else
//             stateDamage = false;
//         if(anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
//             stateDie = true;
//         else
//             stateDie = false;
//     }
//     public virtual void AnimReset()
//     {
//         //애니메이션 초기화(09.05)
//         isSkill = false;
//         isAttack = false;
//         isJumping = false;
//         isRun = false;
//         isDodge = false;
//         StopAnim("Skill_Q");
//         StopAnim("Skill_W");
//         StopAnim("Skill_E");
//         StopAnim("isRun");
//         StopAnim("isDodge");
//     }
//     #endregion
//     [PunRPC]
//     public virtual void ApplyProperty(string RPCproperty){
//         if (RPCproperty == "Fire")
//         {
//             Attack1_Effect = commonAttack_Fire1_Effect;
//             Attack2_Effect = commonAttack_Fire2_Effect;
//             Attack3_Effect = commonAttack_Fire3_Effect;
//             SkillQ_Effect = Skill_FireQ_Effect;
//             SkillW_Effect = Skill_FireW_Effect;
//             SkillE1_Effect = Skill_FireE1_Effect;
//             SkillE2_Effect = Skill_FireE2_Effect;
//             SkillE3_Effect = Skill_FireE3_Effect;
//             SkillE4_Effect = Skill_FireE4_Effect;
//             //아이템 이펙트 추가(07.29 백건우)
//             //세트효과 추가(08.26 이준경) 
//             if(photonview.IsMine){
//             #region 3번 세트 3셋 이펙트 효과
//                 if (Status.set3_3_Activated)
//                 {   
//                     photonview.RPC("SetEffect",RpcTarget.All, 0);
//                 }
//                 else{
//                     photonview.RPC("SetEffect",RpcTarget.All, 8);
//                 }
//                 #endregion
//                 #region 1번 세트 3셋 이펙트 효과
//                 if (Status.set1_3_Activated)
//                 {
//                     photonview.RPC("SetEffect",RpcTarget.All, 1);
//                 }
//             }
//             #endregion
//             }
//             else if (RPCproperty == "Ice")
//             {
//                 Attack1_Effect = commonAttack_Ice1_Effect;
//                 Attack2_Effect = commonAttack_Ice2_Effect;
//                 Attack3_Effect = commonAttack_Ice3_Effect;
//                 SkillQ_Effect = Skill_IceQ_Effect;
//                 SkillW_Effect = Skill_IceW_Effect;
//                 SkillE1_Effect = Skill_IceE1_Effect;
//                 SkillE2_Effect = Skill_IceE2_Effect;
//                 SkillE3_Effect = Skill_IceE3_Effect;
//                 SkillE4_Effect = Skill_IceE4_Effect;
//                 //아이템 이펙트 추가(07.29 백건우)
//                 if(photonview.IsMine){
//                     //세트효과 추가(08.26 이준경) 
//                     #region 4번 세트 3셋 이펙트 효과
//                     if (Status.set4_3_Activated == true) 
//                     {
//                         photonview.RPC("SetEffect",RpcTarget.All, 2);
//                     }
//                     else{
//                         photonview.RPC("SetEffect",RpcTarget.All, 9);
//                     }
//                     #endregion
//                     #region 1번 세트 3셋 이펙트 효과
//                     if (Status.set1_3_Activated == true)
//                     {
//                         photonview.RPC("SetEffect",RpcTarget.All, 3);
//                     }
//                     #endregion
//                 }
//             }
//             else
//             {
//                 Attack1_Effect = commonAttack_Ice1_Effect;
//                 Attack2_Effect = commonAttack_Ice2_Effect;
//                 Attack3_Effect = commonAttack_Ice3_Effect;
//                 SkillQ_Effect = Skill_IceQ_Effect;
//                 SkillW_Effect = Skill_IceW_Effect;
//                 SkillE1_Effect = Skill_IceE1_Effect;
//                 SkillE2_Effect = Skill_IceE2_Effect;
//                 SkillE3_Effect = Skill_IceE3_Effect;
//                 SkillE4_Effect = Skill_IceE4_Effect;
//             }
//     }
//     [PunRPC]
//     public virtual void SetEffect(int SetNumber){
//         switch(SetNumber)
//         {
//             //3번 세트 3셋 이펙트 효과
//             case 0:
//                 Item_Weapon_Effect = Item_Weapon_Fire_Effect;
//                 if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
//                 {
//                     Item_Weapon_Fire_Effect.SetActive(true);
//                     Item_Weapon_Ice_Effect.SetActive(false);
//                 }
//                 break;
//             //1번 세트 3셋 이펙트 효과
//             case 1:
//                 Item_Aura_Effect = Item_Aura_Fire_Effect;
//                 if (Item_Aura_Fire_Effect != null && Item_Aura_Ice_Effect != null)
//                 {
//                     Item_Aura_Fire_Effect.SetActive(true);
//                     Item_Aura_Ice_Effect.SetActive(false);
//                 }
//                 break;
//             //4번 세트 4셋 이펙트 효과
//             case 2:
//                 Item_Weapon_Effect = Item_Weapon_Ice_Effect;
//                 if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
//                 {
//                     Item_Weapon_Fire_Effect.SetActive(false);
//                     Item_Weapon_Ice_Effect.SetActive(true);
//                     Rogue_Sec_Weapon("Fire");
//                 }
//                 break;
//             //1번 세트 3셋 이펙트 효과
//             case 3:
//                 Item_Aura_Effect = Item_Aura_Ice_Effect;
//                 if (Item_Aura_Fire_Effect != null && Item_Aura_Ice_Effect != null)
//                 {
//                     Item_Aura_Fire_Effect.SetActive(false);
//                     Item_Aura_Ice_Effect.SetActive(true);
//                     Rogue_Sec_Weapon("Ice");
//                 }
//                 break;
//             //2번 세트 3셋 이펙트 효과
//             case 4:
//                 Item_Sheild_Effect.SetActive(true);
//                 break;
//             //8번 세트 3셋 이펙트 효과
//             case 5:
//                 clockEffectTime += 3;
//                 Item_Time_Effect.SetActive(true);
//                 break;
//             //7번 세트 3셋 이펙트 효과 (등 뒤에 잔상 이펙트)
//             case 6:
//                 GetComponent<SkinnedMeshAfterImage>().enabled = true;
//                 break;
//             //8번 세트 3셋 이펙트 효과
//             case 7:
//                 Item_Time_Effect.SetActive(false);
//                 break;
//             case 8:
//                 Item_Weapon_Effect = Item_Weapon_Ice_Effect;
//                 if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
//                 {
//                     Item_Weapon_Fire_Effect.SetActive(false);
//                     Item_Weapon_Ice_Effect.SetActive(false);
//                     Rogue_Sec_Weapon("None");
//                 }
//                 break;
//             case 9:
//                 Item_Weapon_Effect = Item_Weapon_Ice_Effect;
//                 if (Item_Weapon_Fire_Effect != null && Item_Weapon_Ice_Effect != null)
//                 {
//                     Item_Weapon_Fire_Effect.SetActive(false);
//                     Item_Weapon_Ice_Effect.SetActive(false);
//                     Rogue_Sec_Weapon("None");
//                 }
//                 break;

        
//         }
//     }

//     [PunRPC]
//     public virtual void RPCDodge(){
//         StartCoroutine(Dodge());
//     }
//     [PunRPC]
//     public virtual void RPCJump(){
//         isJumping = true;
//         JumpCoolTime += Time.deltaTime;
//     }
//     [PunRPC]
//     public virtual void DestroyPlayer(){
//         // 자기가 스스로를 삭제 
//         PhotonNetwork.Destroy(this.gameObject);
//     }

//     public virtual void ToggleGameObjects(GameObject go)
//         {
//             if (go == null) return;
//             var children = go.GetComponentsInChildren<Transform>();
//             foreach (var child in children)
//             {
//                 child.gameObject.tag = "Other";
//                 Debug.Log(child.gameObject.name + "의 태그 :" + child.gameObject.tag);
//             }
//         }

// }