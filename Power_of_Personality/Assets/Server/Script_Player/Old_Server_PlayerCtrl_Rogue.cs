// using System.Collections;
// using System.Collections.Generic;
// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using Photon.Pun;
// public class Server_PlayerCtrl_Rogue : Server_PlayerCtrl
// {
//     private bool isDash = false;
//     private bool isDashAttack = false;

//     //캐릭터 공격 콜라이더
//     private GameObject Attack_Collider_All;
//     public GameObject QSkill_Collider;
//     public GameObject QSkill_Last_Collider;
//     private GameObject WSkill_Collider;
//     private GameObject WSkill_Last_Collider;
//     private GameObject ESkill_Collider1;
//     private GameObject ESkill_Collider2;
//     private GameObject ESkill_Collider3;
//     private GameObject ESkill_Collider4;
//     private GameObject Attack_1_Collider;
//     private GameObject Attack_2_Collider;
//     private GameObject Attack_3_Collider;

//     //도적은 양손 검이라 무기 이펙트 하나 더 추가해야 함
//     public GameObject Item_Weapon2_Effect;
//     public GameObject Item_Weapon2_Ice_Effect;
//     public GameObject Item_Weapon2_Fire_Effect;
//     private string CurProperty;

//     protected override void Start()
//     {
//         base.Start();   // PlayerCtrl의 Start문을 상속 받아서 실행

//         // 이동속도, 점프 힘, 떨어지는 힘을 도적에 맞게 설정
//         moveSpeed = 7;
//         JumpPower = 14;
//         fallPower = 4;

//         // 도적의 Dash는 따로 실행
//         StartCoroutine(DashListener());
//         isDashAttack = false;
//     }
//     protected override void FixedUpdate()
//     {
//         base.FixedUpdate();
//     }
//     protected override void Update()
//     {
//         base.Update(); // PlayerCtrl의 Update문을 상속 받아서 실행
//         if(stateJumpAttack2 == true)
//         {
//             isJumpAttack = true;
//         }

//         //대쉬일 때
//         if (stateDash == true)
//         {
//             moveSpd = moveSpeed * 1.25f;
//         }
//         else
//         {
//             moveSpd = moveSpeed;
//         }
//         if(photonview.IsMine){
//             CurProperty = PlayerPrefs.GetString("property");
//             photonview.RPC("SetProperty",RpcTarget.All, CurProperty);
//         }
//         //상점에서 대쉬 안 되게(09.04)
//         if(Status.IsShop == true)
//         {
//             isDash = false;
//             StopAnim("isDash");
//         }
        
//     }
//     #region HP 설정
//     public override void SetHp(float amount)
//     {
//         base.SetHp(amount);
//     }
//     public override void CheckHp()
//     {
//         base.CheckHp();
//     }
//     public override void HealHp()
//     {
//         base.HealHp();
//     }

//     protected override IEnumerator TakeDamage()
//     {
//         //애니메이션 초기화(09.04)
//         if (Status.MaxHP != 0 || Status.HP > 0)
//         {
//             isDash = false;
//             StopAnim("isDash");
//         }
//         yield return base.TakeDamage();
//     }

//     protected override IEnumerator DamageTextAlpha()
//     {
//         yield return base.DamageTextAlpha();
//     }
//     protected override IEnumerator Immune(float seconds)
//     {
//         Debug.Log(seconds + "만큼 무적");
//         yield return base.Immune(seconds);
//     }
//     #endregion

//     #region 이동 관련 함수
//     protected override void WallCheck()
//     {
//         base.WallCheck();
//     }

//     protected override void GetInput()
//     {
//         base.GetInput();
//     }

//     public override void Move()
//     {
//         base.Move();
//     }

//     protected override void Turn()
//     {
//         base.Turn();
//     }

//     [PunRPC]
//     protected override void Jump()
//     {
//         base.Jump();
//     }

//     protected override void Fall()
//     {
//         base.Fall();
//     }
//     protected override void Stay()
//     {
//         base.Stay();
//     }
//     #endregion

//     #region 충돌 관련 함수
//     protected override void OnCollisionExit(Collision collision)
//     {
//         base.OnCollisionExit(collision);
//     }
//     protected override void OnTriggerEnter(Collider col)
//     {
//         base.OnTriggerEnter(col);
//     }
//     protected override void OnTriggerStay(Collider col)
//     {
//         base.OnTriggerStay(col);
//     }
//     protected override void OnTriggerExit(Collider col)
//     {
//         base.OnTriggerExit(col);
//     }
//     #endregion 

//     #region 공격 관련 함수
//     [PunRPC]
//     public override void Attack(int AttackNumber)
//     {
//         if (AttackNumber == 0)
//         {
//             StartCoroutine(Attack_Sound(0, 0.5f)); //소리 추가(08.31)
//         }

//         if (AttackNumber == 1)
//         {
//             StartCoroutine(Attack_Sound(1, 0.5f)); //소리 추가(08.31)
//         }

//         if (AttackNumber == 2)
//         {
//         }

//         if (AttackNumber == 3)
//         {
//             StartCoroutine(Attack_Sound(1, 0.5f)); //소리 추가(08.31)
//         }

//         if (AttackNumber == 4)
//         {
//             StartCoroutine(Attack_Sound(2, 0.5f)); //소리 추가(08.31)
//             photonview.RPC("StopAnim",RpcTarget.All,"CommonAttack");
//         }
//     }
    
//     [PunRPC]
//     protected override void Attack_anim()
//     {
//         PlayAnim("CommonAttack");
//         isAttack = true;
//     }

//     //도적 스킬 E 카메라 무브 및 스킬 공격
//     IEnumerator Skill_E_Move()
//     {
//         if(photonview.IsMine){
//         mainCamera.GetComponent<CameraCtrl>().UltimateCamera_Rogue(LocalSkillYRot);
//         }
//         yield return new WaitForSeconds(1.2f);
//         StartCoroutine(Attack_Sound(5, 5.0f)); //소리 추가(08.31)
//         //ESkill_Collider1.SetActive(true);
//         yield return new WaitForSeconds(0.25f);
//         //ESkill_Collider1.SetActive(false);
//         //ESkill_Collider2.SetActive(true);
//         yield return new WaitForSeconds(0.3f);
//         //ESkill_Collider2.SetActive(false);
//         yield return new WaitForSeconds(0.3f);
//         //ESkill_Collider3.SetActive(true);
//         yield return new WaitForSeconds(0.75f);
//         //ESkill_Collider3.SetActive(false);
//         yield return new WaitForSeconds(0.3f);
//         //ESkill_Collider4.SetActive(true);
//         yield return new WaitForSeconds(0.5f);
//         //ESkill_Collider4.SetActive(false);
//         //스킬 나갈 시 사운드 및 콜라이더(추가 예정)
//         yield return new WaitForSeconds(1.2f);
//         ESkillCoolTime = 0;
//         Ecool.fillAmount = 1;
//     }
//     public void comboAttack_1_on()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void comboAttack_2_on()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void comboAttack_3_on()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 0));
//             SkillEffect.transform.parent = EffectGen.transform;
//             StartCoroutine(Attack_Sound(2, 0.5f)); //소리 추가(08.31)
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 0));
//             SkillEffect.transform.parent = EffectGen.transform;
//             StartCoroutine(Attack_Sound(2, 0.5f)); //소리 추가(08.31)
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void comboAttack_off()
//     {
//         Destroy(SkillEffect);
//     }

//     public void skill_Q_on()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void skill_W_on()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             SkillEffect.transform.parent = EffectGen.transform;
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void skill_E1_0n()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void skill_E2_0n()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void skill_E3_0n()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot, 90f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }

//     public void skill_E4_0n()
//     {
//         if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
//         {
//             SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//         else
//         {
//             SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
//             if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//             }
//         }
//     }
//     protected override void SkillCoolTimeCharge()
//     {
//         base.SkillCoolTimeCharge();
//     }

//      public override IEnumerator Heal_on()
//     {
//         yield return base.Heal_on();
//     }

//     public override void Damaged_on()
//     {
//         base.Damaged_on();
//     }

//     public override void Destroyed_Effect()
//     {
//         base.Destroyed_Effect();
//     }
//     #endregion

//     #region 도적 Dash 함수
//     IEnumerator DashListener()
//     {
//         if(photonview.IsMine){
//         while (true)
//             {
//                 if (Input.GetKeyDown(KeyCode.RightArrow))
//                 {
//                     yield return Dash("Right");
//                 }
//                 if (Input.GetKeyDown(KeyCode.LeftArrow))
//                 {
//                     yield return Dash("Left");
//                 }

//                 //Dash 종료
//                 if (anim.GetBool("isDash") && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
//                 {
//                     photonview.RPC("RPCDashListener",RpcTarget.All);
//                 }
//                 else if (anim.GetBool("isDash"))
//                 {
//                     photonview.RPC("RPCDashListener2",RpcTarget.All);
                    
//                 }

//                 yield return null;
//             }
//         }
//     }

//     IEnumerator Dash(string direct)
//     {
//         yield return new WaitForFixedUpdate(); // 이준경 : 대쉬 빌드시 문제 해결(08.31)

//         float count = 0f;

//         while (count < 0.5f)
//         {
//             if (direct == "Right" &&
//                 Input.GetKeyDown(KeyCode.RightArrow) &&      //방향키 버튼 눌렀을 때
//                 !isDash)                                    //isDash가 false라면
//             {
//                 photonview.RPC("RPCDash",RpcTarget.All);

//                 yield break;
//             }
//             else if (direct == "Left" &&
//                 Input.GetKeyDown(KeyCode.LeftArrow) &&      //방향키 버튼 눌렀을 때
//                 !isDash)                                    //isDash가 false라면
//             {
//                 photonview.RPC("RPCDash",RpcTarget.All);

//                 yield break;
//             }

//             count += Time.deltaTime;
//             yield return null;
//         }
//     }

//     protected override void Rogue_Sec_Weapon(string weaponType)
//     {
//         if(weaponType == "Fire")
//         {
//             Item_Weapon2_Fire_Effect.SetActive(true);
//             Item_Weapon2_Ice_Effect.SetActive(false);
//         }
//         else if(weaponType == "Ice")
//         {
//             Item_Weapon2_Fire_Effect.SetActive(false);
//             Item_Weapon2_Ice_Effect.SetActive(true);
//         }
//         else if(weaponType == "None")
//         {
//             Item_Weapon2_Fire_Effect.SetActive(false);
//             Item_Weapon2_Ice_Effect.SetActive(false);
//         }
//         else
//         {
//             Item_Weapon2_Fire_Effect.SetActive(false);
//             Item_Weapon2_Ice_Effect.SetActive(false);
//         }

//     }

//     #endregion

//     #region 스킬이나 공격 움직임, Delay 등 세부 조정 함수
//     [PunRPC]
//     public override void UseSkill(string skillName)
//     {
//         base.UseSkill(skillName);
//         isSkill = true;
//         if(skillName == "Q")
//         {
//             PlayAnim("Skill_Q");
//             StartCoroutine(Attack_Sound(3, 3.0f)); //소리 추가(08.31)
//             StartCoroutine(MoveForwardForSeconds(1.0f));
//             StartCoroutine(Immune(2.5f));
//             QSkillCoolTime = 0;
//             Qcool.fillAmount = 1;
//         }

//         if(skillName == "W")
//         {
//             PlayAnim("Skill_W");
//             StartCoroutine(Attack_Sound(4, 3.0f)); //소리 추가(08.31)
//             StartCoroutine(Skill_W());
//             StartCoroutine(Immune(3f));
//         }

//         if(skillName == "E")
//         {
//             PlayAnim("Skill_E");
//             StartCoroutine(Skill_E_Move());
//             StartCoroutine(Immune(5.5f));
//         }
//     }

//     IEnumerator MoveForwardForSeconds(float seconds)
//     {
//         yield return new WaitForSeconds(0.3f);
//         float elapsedTime = 0;

//         while (elapsedTime < seconds)
//         {
//             transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }
//         if(elapsedTime > seconds)
//         {
//             yield return new WaitForSeconds(0.3f);
//         }
        
//     }
//     IEnumerator Skill_W()
//     {
//         yield return new WaitForSeconds(0.15f);
//         //WSkill_Collider.SetActive(true);
//         yield return new WaitForSeconds(1.5f);
//         //WSkill_Collider.SetActive(false);
//         yield return new WaitForSeconds(0.2f);
//         //WSkill_Last_Collider.SetActive(true);
//         yield return new WaitForSeconds(0.5f);
//         WSkillCoolTime = 0;
//         Wcool.fillAmount = 1;
//         //WSkill_Last_Collider.SetActive(false);
//     }
//     protected override IEnumerator Delay(float seconds)
//     {
//         yield return base.Delay(seconds);
//     }
//     protected override void SetIce()
//     {
//         Attack1_Effect = commonAttack_Ice1_Effect;
//         Attack2_Effect = commonAttack_Ice2_Effect;
//         Attack3_Effect = commonAttack_Ice3_Effect;
//         SkillQ_Effect = Skill_IceQ_Effect;
//         SkillW_Effect = Skill_IceW_Effect;
//         SkillE1_Effect = Skill_IceE1_Effect;
//         SkillE2_Effect = Skill_IceE2_Effect;
//         SkillE3_Effect = Skill_IceE3_Effect;
//         SkillE4_Effect = Skill_IceE4_Effect;
//     }
//     protected override void SetFire()
//     {
//         Attack1_Effect = commonAttack_Fire1_Effect;
//         Attack2_Effect = commonAttack_Fire2_Effect;
//         Attack3_Effect = commonAttack_Fire3_Effect;
//         SkillQ_Effect = Skill_FireQ_Effect;
//         SkillW_Effect = Skill_FireW_Effect;
//         SkillE1_Effect = Skill_FireE1_Effect;
//         SkillE2_Effect = Skill_FireE2_Effect;
//         SkillE3_Effect = Skill_FireE3_Effect;
//         SkillE4_Effect = Skill_FireE4_Effect;
//     }
//     #endregion

//     #region 애니메이션
//     [PunRPC]
//     public override void PlayAnim(string AnimationName)
//     {
//         base.PlayAnim(AnimationName);
//     }
//     [PunRPC]
//     public override void StopAnim(string AnimationName)
//     {
//         base.StopAnim(AnimationName);
//     }

//     [PunRPC]
//     public override void AnimState()
//     {
//         base.AnimState();
//     }
//     #endregion
//     [PunRPC]
//     public void SetProperty(string CurProperty){
//         //도적 두번째 무기 이펙트 변경
//         if (CurProperty == "Fire")
//         {
//             Item_Weapon2_Effect = Item_Weapon_Fire_Effect;
//         }
//         else if (CurProperty == "Ice")
//         {
//             Item_Weapon2_Effect = Item_Weapon_Ice_Effect;
//         }
//         else
//         {
//             Item_Weapon2_Effect = Item_Weapon_Ice_Effect;
//         }
//     }
//     [PunRPC]
//     public override void RPCDodge()
//     {
//         base.RPCDodge();
//     }

//     [PunRPC]
//     public override void ApplyProperty(string RPCproperty)
//     {
//         base.ApplyProperty(RPCproperty);
//     }
//     [PunRPC]
//     public void RPCDash()
//     {
//         isDash = true;
//         anim.SetBool("isDash", true);
//     }
//     [PunRPC]
//     public void RPCDashListener()
//     {
//         isDash = false;
//         anim.SetBool("isDash", false);
//     }

//     [PunRPC]
//     public void RPCDashListener2()
//     {
//         isDash = true;
//         isAttack = false;
//     }
//     public override void ToggleGameObjects(GameObject go)
//     {
//         base.ToggleGameObjects(go);
//     }
//     public override void AnimReset()
//     {
//         base.AnimReset();
//     }

// }
