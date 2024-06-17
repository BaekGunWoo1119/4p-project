// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System;
// using Photon.Pun;

// public class Server_PlayerCtrl : MonoBehaviourPun
// {
//     #region 변수 선언
//     // GetAxis 값
//     protected float hAxis;
//     protected float vAxis;

//     // Player의 transform, YPosition, YRotation 값
//     protected Transform trs;
//     protected float YRot;
//     protected float YPos;

//     //플레이어 스테이터스
//     public float PlayerHP;     //HP
//     public float maxHP;        //최대 체력
//     public float Damage;       //받은 피해량
//     public float PlayerATK;    //공격력
//     public float PlayerDEF;    //방어력
//     public float FireATT;      //불 속성 데미지 배율
//     public float IceATT;       //얼음 속성 데미지 배율
//     public float moveSpeed;     //이동속도
//     public float moveSpd;      //이동속도
//     public float JumpPower;     //점프력
//     public float fallPower;     //떨어지는 힘

//     // 애니메이션 컨트롤
//     protected Vector3 initPos;
//     protected bool isSkill = false;
//     protected bool isAttack = false;
//     protected bool isJumping = false;
//     protected bool isRun = false;
//     protected bool isForward = true;
//     protected bool isJumpAttack;
//     protected bool isCommonAttack1InProgress = false;
//     protected bool isCommonAttack2InProgress = false;
//     protected bool isCommonAttack3InProgress = false;

//     // 코루틴 컨트롤
//     protected bool coroutineMove = false;

//     // 애니메이터, Rigidbody
//     protected Animator anim;
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

//     // 카메라, 사운드
//     protected GameObject mainCamera;
//     protected AudioClip[] effectAudio;
//     protected bool isSound = false;
//     protected AudioSource[] audioSources;

//     // 벽 충돌체크
//     protected bool WallCollision;

//     // HP Bar
//     protected Slider HpBar;
//     protected TMP_Text HpText;

//     // 쿨타임 관련
//     protected float QSkillCoolTime;
//     protected float WSkillCoolTime;
//     protected float ESkillCoolTime;
//     protected Image Qcool;
//     protected Image Wcool;
//     protected Image Ecool;
//     protected bool canTakeDamage = true; // 데미지를 가져올 수 있는지
//     protected float damageCooldown = 1.0f; // 1초마다 틱데미지를 가져오기 위함

//     // 회전 관련
//     protected GameObject CurrentFloor;
//     protected Vector3 moveVec;
//     #endregion

//     #region 서버 관련
//     private string RPCproperty;
//     private PhotonView photonview;

//     #endregion
//     protected virtual void Start()
//     {
//         // 플레이어 스테이터스 초기화
//         SetIce();
//         SetHp(100);
//         PlayerATK = 100;
//         PlayerDEF = 10;
//         FireATT = 1.0f;
//         IceATT = 1.0f;

//         // HP Bar 설정
//         // HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();
//         // HpText = GameObject.Find("StatPoint - Hp").GetComponent<TMP_Text>();
//         // HpText.text = "HP 100/100";

//         //쿨타임 UI(03.18)
//         Qcool = GameObject.Find("CoolTime-Q").GetComponent<Image>();
//         Wcool = GameObject.Find("CoolTime-W").GetComponent<Image>();
//         Ecool = GameObject.Find("CoolTime-E").GetComponent<Image>();

//         // 애니메이션, Rigidbody, Transform 컴포넌트 지정
//         anim = GetComponent<Animator>();
//         rd = GetComponent<Rigidbody>();
//         trs = GetComponentInChildren<Transform>();

//         initPos = trs.position; // initPos에 Transform.position 할당
//         mainCamera = GameObject.FindWithTag("MainCamera");  // 메인 카메라 지정
//         anim.SetBool("isIdle", true);   // isIdle을 True로 설정해서 Idle 상태 지정
//         EffectGen = transform.Find("EffectGen").gameObject; // EffectGen 지정

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
//     }

//     protected virtual void Update()
//     {
//         if(photonview.IsMine){
//             if (!canTakeDamage)
//             {
//                 damageCooldown -= Time.deltaTime;
//                 if (damageCooldown < 0)
//                 {
//                     canTakeDamage = true;
//                     damageCooldown = 1.0f;
//                 }
//             }

//             //스킬 쿨타임 UI(03.18)
//             if (Qcool.fillAmount != 0)
//             {
//                 Qcool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
//             }
//             if (Wcool.fillAmount != 0)
//             {
//                 Wcool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
//             }
//             if (Ecool.fillAmount != 0)
//             {
//                 Ecool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
//             }
//             // 벽 충돌체크 함수 실행
//             WallCheck();

//             // 애니메이션 업데이트
//             GetInput();

//             //스킬 쿨타임 충전
//             SkillCoolTimeCharge();

//             //로테이션 고정 코드(04.10 백건우 수정, 굴절구간 문제 생길 시 아래 코드 대신 사용)
//             YRot = transform.eulerAngles.y;
//             //transform.localRotation = Quaternion.Euler(0, YRot, 0);
            
//             /*
//                 XRot = transform.eulerAngles.x;
//                 YRot = transform.eulerAngles.y;
//                 transform.localRotation = Quaternion.Euler(XRot, YRot, 0);
//             */

//             //Z 포지션 고정
//             transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

//             // char 오브젝트 위치 고정
//             transform.GetChild(0).localPosition = Vector3.zero;

//             // Move 함수 실행
//             if (!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3"))
//             {
//                 Move();
//                 Move_anim();
//                 Turn();
//             }

//             // Turn 함수 실행
//             if (!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//             {
//                 Turn();
//             }

//             // Dodge 함수 실행
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 photonview.RPC("Dodge",RpcTarget.All);
//             }
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
//             {
//                 anim.ResetTrigger("isDodge");
//             }
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
//             {
//                 anim.SetBool("isJump", false);
//             }

//             // Attack 함수 실행
//             if (Input.GetKeyDown(KeyCode.A))
//             {
//                 photonview.RPC("Attack_anim",RpcTarget.All);
//             }

//             //기본공격1 & 기본공격3 시 전진 애니메이션
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1") && !isCommonAttack1InProgress)
//             {
//                 CommonAttack1();
//                 isCommonAttack1InProgress = true;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2") && !isCommonAttack2InProgress && !isSound)
//             {
//                 CommonAttack2();
//                 isCommonAttack2InProgress = true;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3") && !isCommonAttack3InProgress)
//             {
//                 CommonAttack3();
//                 isCommonAttack3InProgress = true;
//             }

//             //지상공격 2타, 3타 시 방향전환 되도록
//             if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1_Wait") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2_Wait"))
//             {
//                 isAttack = false;
//             }
//             else if(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1") ||
//                     anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2") ||
//                     anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3"))
//             {
//                 isAttack = true;
//             }

//             //점프공격 카메라 && 사운드
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !coroutineMove)
//             {
//                 photonview.RPC("JumpAttack1",RpcTarget.All);
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") && !isSound)
//             {
//                 photonview.RPC("JumpAttack2",RpcTarget.All);
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3") && !coroutineMove)
//             {
//                 photonview.RPC("JumpAttack3",RpcTarget.All);
//             }

//             UpdateCoroutineMoveState();
//             //Debug.Log(isAttack);

//             //점프공격 시 Y 포지션 고정
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !isJumpAttack)
//             {
//                 Vector3 OriginPos = transform.position;
//                 YPos = OriginPos.y;
//                 isJumpAttack = true;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1"))
//             {
//                 rd.velocity = Vector3.zero;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
//             {
//                 float upperUpTime = 0;
//                 if (upperUpTime == 0)
//                 {
//                     //공중에서 고정되어 때리다가 떨어짐
//                     Vector3 OriginPos = transform.position;
//                     YPos = OriginPos.y;
//                     upperUpTime += 1;
//                 }
//                 Vector3 newPos = transform.position;
//                 newPos.y = YPos;
//                 isAttack = false;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isJumpAttack == true)
//             {
//                 anim.ResetTrigger("CommonAttack");
//             }
//             //한 번 점프 시 한 번의 점프공격 콤보만 되게
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait") && isJumpAttack == true)
//             {
//                 anim.ResetTrigger("CommonAttack");
//                 isJumpAttack = false;
//                 isAttack = false;
//             }
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && isJumpAttack == true)
//             {
//                 anim.ResetTrigger("CommonAttack");
//                 isJumpAttack = false;
//                 isAttack = false;
//             }

//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && isJumpAttack == true)
//             {
//                 anim.ResetTrigger("CommonAttack");
//                 isAttack = false;
//             }

//             //Skill_Q
//             if (Input.GetKeyDown(KeyCode.Q)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && QSkillCoolTime >= 3.0f
//             && !isAttack)
//             {
//                 photonview.RPC("Skill_Q", RpcTarget.All);
//             }

//             //Skill_W
//             if (Input.GetKeyDown(KeyCode.W)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && WSkillCoolTime >= 3.0f
//             && !isAttack)
//             {
//                 photonview.RPC("Skill_W", RpcTarget.All);
//             }

//             //Skill_E
//             if (Input.GetKeyDown(KeyCode.E)
//             && !isSkill
//             && !isJumping
//             && !anim.GetBool("isFall")
//             && ESkillCoolTime >= 3.0f
//             && !isAttack)
//             {
//                 photonview.RPC("Skill_E", RpcTarget.All);
//             }

//             //Jump
//             if (Input.GetKeyDown(KeyCode.Space) && !isSkill && !isAttack && !isJumping
//                 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")
//                 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fall")
//                 && !anim.GetBool("isFall"))
//             {
//                 isJumping = true;
//             }
//             else
//             {
//                 isJumping = false;
//             }
//             //점프 모션이 실행되야만 점프가 실행되게(애니메이션 딜레이 및 더블점프 강제 제거)
//             if (isJumping == true)
//             {
//                 Jump();
//             }

//             //Idle일때 스킬 및 공격 false 판정
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//             {
//                 anim.SetBool("isIdle", true);
//                 isAttack = false;
//                 isSkill = false;
//                 anim.ResetTrigger("CommonAttack");
//             }

//             //다른 모션일 때, 혹시라도 Move가 실행되도 달리지 못하게
//             if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1_Wait") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2_Wait") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") ||
//             (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetBool("isRun")) ||
//             (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && !anim.GetBool("isRun")))
//             {
//                 moveSpd = 0;
//             }

//             //대쉬일 때
//             else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
//             {
//                 moveSpd = moveSpeed * 1.25f;
//             }
//             else
//             {
//                 moveSpd = moveSpeed;
//             }

//             //캐릭터 스킬 이펙트
//             RPCproperty = PlayerPrefs.GetString("property");
//             photonview.RPC("ApplyProperty",RpcTarget.All, RPCproperty);
//         }
//     }

//     #region HP 설정
//     protected virtual IEnumerator TakeDamage()
//     {
//         if (maxHP != 0 || PlayerHP > 0)
//         {
//             PlayerHP -= Damage;
//             Debug.Log(PlayerHP);
//             CheckHp();
//             anim.SetBool("TakeDamage", true);
//             yield return new WaitForSeconds(0.5f);
//             anim.SetBool("TakeDamage", false);
//         }

//         if (PlayerHP <= 0) // 플레이어가 죽으면 게임오버 창 띄움
//         {
//             anim.SetBool("isDie", true);
//             yield return new WaitForSeconds(2.0f);
//             GameObject.Find("EventSystem").GetComponent<GameEnd>().GameOver(true);
//         }
//     }
//     public virtual void SetHp(float amount) // Hp 세팅
//     {
//         maxHP = amount;
//         PlayerHP = maxHP;
//     }
//     public virtual void CheckHp() // HP 체크
//     {
//         string inputText = "HP " + PlayerHP.ToString("F0") + "/" + maxHP.ToString("F0");
//         if (HpBar != null)
//             HpBar.value = PlayerHP / maxHP;
//         if (HpText != null)
//             HpText.text = inputText;
//     }
//     #endregion

//     #region 이동 관련 함수
//     protected virtual void WallCheck()
//     {
//         WallCollision = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), transform.forward, 0.6f, LayerMask.GetMask("Wall", "Monster"));
//     }

//     protected virtual void GetInput()
//     {
//         hAxis = Input.GetAxisRaw("Horizontal");
//     }

//     protected virtual void Move()
//     {
//         moveVec = new Vector3(hAxis, 0, vAxis).normalized;
//         if (!WallCollision)
//         {
//             transform.localPosition += moveVec * moveSpd * Time.deltaTime;
//         }
//         StartCoroutine(Delay(0.2f));
//     }
//     protected virtual void Move_anim()
//     {
//         anim.SetBool("isRun", moveVec != Vector3.zero);
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
//     protected virtual void Dodge()
//     {
//         anim.SetTrigger("isDodge");
//     }
//     protected virtual void Jump()
//     {
//         anim.SetBool("isJump", true);
//         rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
//     }
//     protected virtual void Fall()
//     {
//         anim.SetBool("isFall", true); //떨어지는것으로 감지
//         rd.AddForce(Vector3.down * fallPower, ForceMode.VelocityChange);
//     }
//     protected virtual void Stay()
//     {
//         isJumping = false; //isJump, isFall을 다시 false로
//         anim.SetBool("isJump", false);
//         anim.SetBool("isFall", false);
//     }
//     #endregion

//     #region 충돌 관련 함수
//     protected virtual void OnTriggerEnter(Collider col)
//     {
//         if (col.gameObject.tag == "Monster_Melee")
//         {
//             // 충돌한 몬스터 오브젝트에서 해당 스크립트를 가져옵니다.
//             MonoBehaviour monsterCtrl = col.gameObject.transform.root.GetComponentInChildren<MonoBehaviour>();

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
//                 StartCoroutine(TakeDamage());
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }

//         else if (col.gameObject.tag == "Monster_Ranged")
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
//                 StartCoroutine(TakeDamage());
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }
//     }

//     protected virtual void OnTriggerStay(Collider col)
//     {
//         if (canTakeDamage == true && col.gameObject.tag == "Druid_Poison")
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
//                 StartCoroutine(TakeDamage());
//             }
//             else
//             {
//                 Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
//             }
//         }
//     }
//     protected virtual void OnCollisionStay(Collision collision) // 충돌 감지
//     {
//         if (collision.gameObject.tag == "Floor")    // Tag가 Floor인 오브젝트와 충돌했을 때
//         {
//             Stay();
//         }
//     }
//     protected virtual void OnCollisionExit(Collision collision)
//     {
//         if (collision.gameObject.tag == "Floor")    // Tag가 Floor인 오브젝트와 충돌이 끝났을 때
//         {
//             Fall();
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
//     protected virtual void CommonAttack1()
//     {
//     }
//     [PunRPC]
//     protected virtual void CommonAttack2()
//     {
//     }
//     [PunRPC]
//     protected virtual void CommonAttack3()
//     {
//     }
//     [PunRPC]
//     protected virtual void JumpAttack1()
//     {
//     }
//     [PunRPC]
//     protected virtual void JumpAttack2()
//     {
//     }
//     [PunRPC]
//     protected virtual void JumpAttack3()
//     {
//     }
//     [PunRPC]
//     protected virtual void Skill_Q()
//     {
//     }
//     [PunRPC]
//     protected virtual void Skill_W()
//     {
//     }
//     [PunRPC]
//     protected virtual void Skill_E()
//     {
//     }
//     protected virtual void SkillCoolTimeCharge()
//     {
//         QSkillCoolTime += Time.deltaTime;
//         WSkillCoolTime += Time.deltaTime;
//         ESkillCoolTime += Time.deltaTime;
//     }
//     protected virtual void ResetAttackInProgressStates()
//     {
//         isCommonAttack1InProgress = false;
//         isCommonAttack2InProgress = false;
//         isCommonAttack3InProgress = false;
//     }
//     protected virtual void UpdateCoroutineMoveState()
//     {
//         if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack1") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack2") ||
//             anim.GetCurrentAnimatorStateInfo(0).IsName("CommonAttack3")))
//         {
//             ResetAttackInProgressStates();
//         }
//     }
//     #endregion

//     #region Delay 함수
//     protected virtual IEnumerator Delay(float seconds)
//     {
//         yield return new WaitForSeconds(seconds);
//         if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//         {
//             moveVec = new Vector3(0, 0, 0);
//             isAttack = false;
//             isSkill = false;
//         }
//         yield return null;
//     }
//     #endregion

//     [PunRPC]
//     protected virtual void ApplyProperty(string RPCproperty){
//         LocalSkillYRot = transform.localEulerAngles.y;
//         SkillYRot = transform.eulerAngles.y;
//         if (RPCproperty == "Fire")
//             {
//                 Attack1_Effect = commonAttack_Fire1_Effect;
//                 Attack2_Effect = commonAttack_Fire2_Effect;
//                 SkillQ_Effect = Skill_FireQ_Effect;
//                 SkillW_Effect = Skill_FireW_Effect;
//             }
//             else if (RPCproperty == "Ice")
//             {
//                 Attack1_Effect = commonAttack_Ice1_Effect;
//                 Attack2_Effect = commonAttack_Ice2_Effect;
//                 SkillQ_Effect = Skill_IceQ_Effect;
//                 SkillW_Effect = Skill_IceW_Effect;
//             }
//             else
//             {
//                 Attack1_Effect = commonAttack_Ice1_Effect;
//                 Attack2_Effect = commonAttack_Ice2_Effect;
//                 SkillQ_Effect = Skill_IceQ_Effect;
//                 SkillW_Effect = Skill_IceW_Effect;
//             }
//     }
// }