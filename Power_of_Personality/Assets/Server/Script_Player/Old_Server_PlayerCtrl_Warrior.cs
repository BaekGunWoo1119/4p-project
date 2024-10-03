// using System.Collections;
// using System.Collections.Generic;
// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using Photon.Pun;

// public class Server_PlayerCtrl_Warrior : Server_PlayerCtrl
// {
//     #region 변수 선언
//     //애니메이션 컨트롤
//     private bool isSkillQ;
    
//     //점프공격 float
//     private float YOrgPos;
//     private float upperUpTime = 0;

//     //캐릭터 공격 콜라이더
//     private GameObject Attack_Collider_All;
//     public GameObject QSkill_Collider;
//     private GameObject WSkill_Collider;
//     public GameObject ESkill_Collider;
//     private GameObject Attack_1_Collider;
//     private GameObject Attack_2_Collider;
//     private GameObject Attack_3_Collider;

//     //이펙트
//     public GameObject Skill_FireE_Effect;
//     public GameObject Skill_IceE_Effect;
//     public GameObject SKill_FireQ_Rev_Effect;
//     public GameObject SKill_IceQ_Rev_Effect;
//     private GameObject SkillE_Effect;
//     private GameObject SkillQ_Rev_Effect;

//     private ParticleSystem setParticles1;
//     private ParticleSystem setParticles2;
//     private ParticleSystem setParticles3;
//     private ParticleSystem setParticles4;
//     private ParticleSystem setParticles5;
//     private ParticleSystem setParticles6;

//     private string CurProperty;
//     #endregion

//     #region Start, FixedUpdate, Update
//     protected override void Start()
//     {
//         base.Start();

//         //플레이어 어택 콜라이더 인식 방식 변경 (서버에 맞게)
//         /*
//         Attack_Collider_All = transform.Find("AttackColliders").gameObject;
//         WSkill_Collider = Attack_Collider_All.transform.Find("WSkill_Collider").gameObject;
//         WSkill_Collider.SetActive(false);
//         Attack_1_Collider = Attack_Collider_All.transform.Find("Attack_1_Collider").gameObject;
//         Attack_2_Collider = Attack_Collider_All.transform.Find("Attack_2_Collider").gameObject;
//         Attack_3_Collider = Attack_Collider_All.transform.Find("Attack_3_Collider").gameObject;
//         Attack_1_Collider.SetActive(false);
//         Attack_2_Collider.SetActive(false);
//         Attack_3_Collider.SetActive(false);

//         isSkillQ = false;
//         */
//     }

//     protected override void FixedUpdate()
//     {
//         base.FixedUpdate();
//     }

//     protected override void Update()
//     {
//         base.Update();
//         if(photonview.IsMine){
//             CurProperty = PlayerPrefs.GetString("property");
//             photonview.RPC("SetProperty",RpcTarget.All, CurProperty);
//         }

//         //점프공격 시 Y 포지션 고정
//         if (stateJumpAttack1 == true && !isJumpAttack)
//         {
//             Vector3 OriginPos;
//             if (upperUpTime == 0)
//             {
//                 OriginPos = transform.position;
//                 YOrgPos = OriginPos.y;
//                 upperUpTime += 1;
//                 isJumpAttack = true;
//                 Debug.Log(YOrgPos);
//             }
//             if(transform.position.y != YOrgPos)
//                 transform.position = new Vector3(transform.position.x, YOrgPos, transform.position.z);
//         }
//         else if (stateJumpAttack1 == true)
//         {
//             rd.velocity = Vector3.zero;
//         }
//         else if (stateJumpAttack2 == true || stateJumpAttack3 == true)
//         {
//             Vector3 OriginPos;
//             if (upperUpTime == 0)
//             {
//                 //공중에서 고정되어 때리다가 떨어짐
//                 OriginPos = transform.position;
//                 YOrgPos = OriginPos.y;
//                 upperUpTime += 1;
//             }
//             if(transform.position.y != YOrgPos)
//                 transform.position = new Vector3(transform.position.x, YOrgPos, transform.position.z);
//             Debug.Log(YOrgPos);
//             //isAttack = false;
//             rd.velocity = Vector3.zero;
//         }
//         else if(stateJumpAttack1 == false && stateJumpAttack2 == false && stateJumpAttack3 == false)
//         {
//             upperUpTime = 0;
//         }
//     }
//     #endregion

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
//         yield return base.TakeDamage();
//     }
//     //데미지 텍스트(06.01)
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
//     protected override void OnTriggerExit(Collider col)
//     {
//         base.OnTriggerExit(col);
//     }

//     protected override void OnTriggerStay(Collider col)
//     {
//         base.OnTriggerStay(col);
//     }
//     #endregion

//     #region 공격 관련 함수
//     [PunRPC]
//     protected override void Attack_anim()
//     {
//         PlayAnim("CommonAttack");
//         isAttack = true;
//     }
    
//     [PunRPC]
//     //Attack 함수 통합/분리 (번호 할당 => 기본공격 1,2,3 = 0,1,2 / 점프공격 1,2,3 = 3,4,5)
//     public override void Attack(int AttackNumber)
//     {
//         if(AttackNumber == 0)
//         {
//             isSound = false;
//             //StartCoroutine(Attack1_Collider());
//             StartCoroutine(Attack_Sound(AttackNumber , 0.8f));
//             StartCoroutine(Delay(0.4f));
//             StartCoroutine(MoveForwardForSeconds(0.3f));
//             StartCoroutine(Delay(0.2f));
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.03f, null);
//             }
//         }

//         if(AttackNumber == 1)
//         {
//             //StartCoroutine(Attack2_Collider());
//             StartCoroutine(Attack_Sound(AttackNumber, 0.8f));
//             StartCoroutine(Delay(0.2f));
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.01f, null);
//             }
//         }

//         if(AttackNumber == 2)
//         {
//             StartCoroutine(Delay(0.2f));
//             StartCoroutine(MoveForwardForSeconds(0.3f));
//             //StartCoroutine(Attack3_Collider());
//             StartCoroutine(Attack_Sound(AttackNumber, 0.8f));
//             StartCoroutine(Delay(5.0f));
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.3f, 0.1f, null);
//             }
//         }

//         if(AttackNumber == 3)
//         {
//             isSound = false;
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().JumpCamera_Warrior();
//             }
//             //StartCoroutine(Attack1_Collider());
//         }

//         if(AttackNumber == 4)
//         {
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().JumpCamera_Warrior();
//             }
//             //StartCoroutine(Attack1_Collider());
//             StartCoroutine(Delay(0.2f));
//         }

//         if(AttackNumber == 5)
//         {
//             if(photonview.IsMine){
//             mainCamera.GetComponent<CameraCtrl>().JumpCamera_Warrior();
//             }
//             //StartCoroutine(Attack1_Collider());
//             StopAnim("CommonAttack");
//         }

//     }

//     /*
//     IEnumerator Attack1_Collider()
//     {
//         yield return new WaitForSeconds(0.125f);
//         Attack_1_Collider.SetActive(true);
//         yield return new WaitForSeconds(0.3f);
//         if (Attack_1_Collider == true)
//         {
//             Attack_1_Collider.SetActive(false);
//         }
//     }
//     IEnumerator Attack2_Collider()
//     {
//         Attack_2_Collider.SetActive(true);
//         yield return new WaitForSeconds(0.3f);
//         if (Attack_2_Collider == true)
//         {
//             Attack_2_Collider.SetActive(false);
//         }
//     }
//     IEnumerator Attack3_Collider()
//     {
//         yield return new WaitForSeconds(0.125f);
//         Attack_3_Collider.SetActive(true);
//         yield return new WaitForSeconds(0.5f);
//         if (Attack_3_Collider == true)
//         {
//             Attack_3_Collider.SetActive(false);
//         }
//     }
//     */

//     public override IEnumerator Attack_Sound(int AttackValue, float playsec)
//     {
//         if (AttackValue == 1)
//         {
//             isSound = true;
//         }
//         yield return base.Attack_Sound(AttackValue, playsec);
//     }


//     IEnumerator Spawn_SwordAura()
//     {
//         //코루틴 내역 전면 수정(08.31)
//         isSkillQ = false;
//         yield return new WaitForSeconds(0.3f);
//         StartCoroutine(Attack_Sound(3, 0.5f));
//         //쿨타임
//         QSkillCoolTime = 0;
//         Qcool.fillAmount = 1;
//     }

//     IEnumerator SKill_E_Move()
//     {
//         float elapsedTime = 0;
//         yield return new WaitForSeconds(2.0f);
//         //스킬 사용 시 카메라 무빙(등 포커스)
//         yield return new WaitForSeconds(1.0f);
//         while (elapsedTime < 0.3)
//         {
//             transform.Translate(Vector3.forward * 3 * Time.deltaTime);

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }
//         yield return new WaitForSeconds(1.5f);
//         elapsedTime = 0;
//         while (elapsedTime < 0.5)
//         {
//             transform.Translate(Vector3.forward * 3 * Time.deltaTime);

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }
//         yield return new WaitForSeconds(0.5f);
//         elapsedTime = 0;
//         while (elapsedTime < 0.5)
//         {
//             transform.Translate(Vector3.forward * 3 * Time.deltaTime);

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }
//     }
//     IEnumerator WarriorSkill_E()
//     {
//         if(photonview.IsMine){
//         mainCamera.GetComponent<CameraCtrl>().UltimateCamera_Warrior(LocalSkillYRot);
//         }
//         //스킬 나갈 시 사운드(08.31)
//         yield return new WaitForSeconds(1.8f);
//         audioSources[3].Play();
//         yield return new WaitForSeconds(0.6f);
//         audioSources[3].Stop();
//         audioSources[3].Play();
//         yield return new WaitForSeconds(0.8f);
//         audioSources[3].Stop();
//         audioSources[3].Play();
//         yield return new WaitForSeconds(0.4f);
//         audioSources[3].Stop();
//         audioSources[3].Play();
//         yield return new WaitForSeconds(0.4f);
//         audioSources[3].Stop();
//         audioSources[3].Play();
//         yield return new WaitForSeconds(1.2f);
//         audioSources[3].Stop();
//         audioSources[3].Play();
//         yield return new WaitForSeconds(0.7f);
//         audioSources[3].Stop();
//         yield return new WaitForSeconds(0.3f);
//         audioSources[3].Play();
//         yield return new WaitForSeconds(1f);
//         audioSources[3].Stop();
//         ESkillCoolTime = 0;
//         Ecool.fillAmount = 1;
//     }

//     #endregion

//     #region 이펙트 함수

//     public void comboAttack_1_on()
//     {
//         SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
//         SkillEffect.transform.parent = EffectGen.transform;
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//         audioSources[0].Play();
//     }
//     public void comboAttack_2_on()
//     {
//         SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90f, 0));
//         SkillEffect.transform.parent = EffectGen.transform;
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//     }
//     public void comboAttack_off()
//     {
//         Destroy(SkillEffect);
//     }
//     public void jumpAttack_1_on()
//     {
//         SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, SkillYRot - 90f, 0));
//         SkillEffect.transform.parent = EffectGen.transform;
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//         audioSources[1].Play();
//     }

//     public void skill_Q_on()
//     {
//         //Find로 Slash 찾아서 파티클시스템의 3d start 직접 제어
        
//         SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot, 0f));
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//         setParticles1 = SkillEffect.transform.Find("Slashes").GetComponent<ParticleSystem>();
//         setParticles2 = SkillEffect.transform.Find("Slashes").transform.Find("Slashes-1").GetComponent<ParticleSystem>();
//         ParticleSystem setParticles5 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").GetComponent<ParticleSystem>();
//         Transform setParticles7 = SkillEffect.transform.Find("Slashes").transform.Find("Flash").transform;
//         Transform setParticles8 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").transform.Find("Sparks").transform;
//         Transform setParticles9 = SkillEffect.transform.Find("Slashes").transform.Find("SparksCore").transform;
//         setParticles7.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles8.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles9.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         StartCoroutine(RotateEffect(0f, (SkillYRot - 90f), 0f, setParticles1));
//         StartCoroutine(RotateEffect(0f, (SkillYRot - 90f), 0f, setParticles2));
//         StartCoroutine(RotateEffect(0f, 0f, (SkillYRot - 90f), setParticles5));
//     }

//     public void skill_Q_Rev_on()
//     {
//         SkillEffect = Instantiate(SkillQ_Rev_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot, 0f));
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//         //Find로 Slash 찾아서 파티클시스템의 3d start 직접 제어
//         setParticles1 = SkillEffect.transform.Find("Slashes").GetComponent<ParticleSystem>();
//         setParticles2 = SkillEffect.transform.Find("Slashes").transform.Find("Slashes-1").GetComponent<ParticleSystem>();
//         ParticleSystem setParticles5 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").GetComponent<ParticleSystem>();
//         Transform setParticles7 = SkillEffect.transform.Find("Slashes").transform.Find("Flash").transform;
//         Transform setParticles8 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").transform.Find("Sparks").transform;
//         Transform setParticles9 = SkillEffect.transform.Find("Slashes").transform.Find("SparksCore").transform;   
//         setParticles7.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles8.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles9.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         StartCoroutine(RotateEffect(0f, (SkillYRot - 90f), 0f, setParticles1));
//         StartCoroutine(RotateEffect(0f, (SkillYRot - 90f), 0f, setParticles2));
//         StartCoroutine(RotateEffect(0f, 0f, (SkillYRot - 90f), setParticles5));     
//     }

//     public void skill_W_on()
//     {
//         SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(SkillW_Effect.transform.eulerAngles));
//         SkillEffect.transform.parent = EffectGen.transform;
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//     }

//     public void skill_E_on()
//     {
//         SkillEffect = Instantiate(SkillE_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot, 0f));
//         if(!photonview.IsMine){
//             ToggleGameObjects(SkillEffect);
//         }
//         setParticles1 = SkillEffect.transform.Find("Slashes").GetComponent<ParticleSystem>();
//         setParticles2 = SkillEffect.transform.Find("Slashes-1").GetComponent<ParticleSystem>();
//         setParticles3 = SkillEffect.transform.Find("Slashes").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
//         setParticles4 = SkillEffect.transform.Find("Slashes-1").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
//         ParticleSystem setParticles5 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").GetComponent<ParticleSystem>();
//         ParticleSystem setParticles6 = SkillEffect.transform.Find("Slashes-1").transform.Find("GroundCrack").GetComponent<ParticleSystem>();
//         Transform setParticles7 = SkillEffect.transform.Find("Slashes").transform.Find("Flash").transform;
//         Transform setParticles8 = SkillEffect.transform.Find("Slashes").transform.Find("GroundCrack").transform.Find("Sparks").transform;
//         Transform setParticles9 = SkillEffect.transform.Find("Slashes").transform.Find("SparksCore").transform;   
//         Transform setParticles10 = SkillEffect.transform.Find("Slashes-1").transform.Find("Flash").transform;
//         Transform setParticles11 = SkillEffect.transform.Find("Slashes-1").transform.Find("GroundCrack").transform.Find("Sparks").transform;
//         Transform setParticles12 = SkillEffect.transform.Find("Slashes-1").transform.Find("SparksCore").transform;  
//         setParticles7.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles8.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles9.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles10.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles11.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         setParticles12.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
//         StartCoroutine(RotateEffect(30f, (SkillYRot - 90f), 0f, setParticles1));
//         StartCoroutine(RotateEffect(-30f, (SkillYRot - 90f), 0f, setParticles2));
//         StartCoroutine(RotateEffect(30f, (SkillYRot - 90f), 0f, setParticles3));
//         StartCoroutine(RotateEffect(-30f, (SkillYRot - 90f), 0f, setParticles4));
//         StartCoroutine(RotateEffect(0f, 0f, (SkillYRot - 90f), setParticles5));
//         StartCoroutine(RotateEffect(0f, 0f, (SkillYRot - 90f), setParticles6));
//     }
//     IEnumerator RotateEffect(float xR, float yR, float zR, ParticleSystem particle)
//     {
//         var mainModule = particle.main;
//         mainModule.startRotationX = xR * Mathf.Deg2Rad;
//         mainModule.startRotationY = yR * Mathf.Deg2Rad;
//         mainModule.startRotationZ = zR * Mathf.Deg2Rad;
//         yield return null;
//     }

//     public override IEnumerator Heal_on()
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

//     #region 스킬이나 공격 움직임, Delay 등 세부 조정 함수

//     [PunRPC]
//     public override void UseSkill(string skillName)
//     {
//         base.UseSkill(skillName);
//         isSkill = true;
//         if(skillName == "Q")
//         {
//             isSkillQ = true;
//             PlayAnim("Skill_Q");
//             StartCoroutine(Spawn_SwordAura());
//             StartCoroutine(Immune(1f));
//         }

//         if(skillName == "W")
//         {
//             //WSkill_Collider.SetActive(true);
//             PlayAnim("Skill_W");
//             WSkillCoolTime = 0;
//             Wcool.fillAmount = 1;
//             StartCoroutine(Attack_Sound(4, 3.4f));
//             StartCoroutine(Immune(2f));
//             StartCoroutine(MoveForwardForSeconds(1.35f));
//         }

//         if(skillName == "E")
//         {
//             PlayAnim("Skill_E");
//             StartCoroutine(SKill_E_Move());
//             StartCoroutine(WarriorSkill_E());
//             StartCoroutine(Immune(5.5f));
//         }
//     }

//     IEnumerator MoveForwardForSeconds(float seconds)
//     {
//         coroutineMove = true;
//         float elapsedTime = 0;
        
//         while (elapsedTime < seconds)
//         {
//             elapsedTime += Time.deltaTime;
//             if (!WallCollision)
//             {
//                 transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
//             }
//             yield return null;
//         }
//         if(elapsedTime > seconds)
//         {
//             //WSkill_Collider.SetActive(false);
//         }
//     }

//     protected override IEnumerator Delay(float seconds)
//     {
//         yield return new WaitForSeconds(seconds);

//         if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//         {
//             PlayAnim("isIdle");
//             isAttack = false;
//             isSkill = false;
//             isSound = false;
//         }
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
//         if (CurProperty == "Fire")
//         {
//             SkillE_Effect = Skill_FireE_Effect;
//             SkillQ_Rev_Effect = SKill_FireQ_Rev_Effect;
//         }
//         else if (CurProperty == "Ice")
//         {
//             SkillE_Effect = Skill_IceE_Effect;
//             SkillQ_Rev_Effect = SKill_IceQ_Rev_Effect;
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
//     public override void ToggleGameObjects(GameObject go)
//     {
//         base.ToggleGameObjects(go);
//     }
//     public override void AnimReset()
//     {
//         base.AnimReset();
//     }

// }
