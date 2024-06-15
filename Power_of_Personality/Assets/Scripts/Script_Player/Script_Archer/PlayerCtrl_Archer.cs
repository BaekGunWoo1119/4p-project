using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;

//궁수 애니메이션
public class PlayerCtrl_Archer : PlayerCtrl
{
    #region 변수 선언
    //캐릭터 공격 콜라이더
    public GameObject Attack_Collider_All;
    public GameObject CommonAttack1_Collider;
    public GameObject CommonAttack2_Collider;
    public GameObject WSkill_Collider;
    public GameObject ESkill_Collider;

    //캐릭터 공격 이펙트
    public GameObject Skill_SmokeEffect;

    //스킬 컨트롤 bool
    private bool isSkillWE;

    //스킬 컨트롤 변수
    private float fixedY;
    #endregion

    #region Start, FixedUpdate, Update
    protected override void Start()
    {
        base.Start();
        Attack_Collider_All = transform.Find("AttackColliders").gameObject;
        WSkill_Collider = Attack_Collider_All.transform.Find("WSkill_Collider").gameObject;
        WSkill_Collider.SetActive(false);
        ESkill_Collider = Attack_Collider_All.transform.Find("ESkill_Collider").gameObject;
        ESkill_Collider.SetActive(false);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        base.Update();
        if (isSkillWE)      //W, E 스킬 사용 시 공중 고정
        {
            transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
        }   
    }
    #endregion

    #region HP 설정
    public override void SetHp(float amount)
    {
        base.SetHp(amount);
    }
    public override void CheckHp()
    {
        base.CheckHp();
    }
    public override void HealHp()
    {
        base.HealHp();
    }
    protected override IEnumerator TakeDamage()
    {
        yield return base.TakeDamage();
    }
    protected override IEnumerator DamageTextAlpha()
    {
        yield return base.DamageTextAlpha();
    }
    #endregion

    #region 이동 관련 함수
    protected override void WallCheck()
    {
        base.WallCheck();
    }
    protected override void GetInput()
    {
        base.GetInput();
    }
    public override void Move()
    {
        base.Move();
    }
    protected override void Turn()
    {
        base.Turn();
    }
    protected override void Jump()
    {
        base.Jump();
    }
    protected override void Fall()
    {
        base.Fall();
    }
    protected override void Stay()
    {
        base.Stay();
    }
    #endregion

    #region 충돌 관련 함수
    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
    }
    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    protected override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
    }
    #endregion

    #region 공격 관련 함수
    protected override void Attack_anim()
    {
        PlayAnim("CommonAttack");
        isAttack = true;
    }
    public override void Attack(int AttackNumber)
    {
        if (AttackNumber == 0)
        {
            StartCoroutine(Spawn_CommonAttack1());
        }

        if (AttackNumber == 1)
        {
            StartCoroutine(Spawn_CommonAttack2());
        }

        if (AttackNumber == 2)
        {
            StartCoroutine(Spawn_CommonAttack3());
        }

        if (AttackNumber == 3)
        {

        }

        if (AttackNumber == 4)
        {

        }

        if (AttackNumber == 5)
        {

        }
    }
    public IEnumerator Spawn_CommonAttack1()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject CommonAttack = Instantiate(CommonAttack1_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
    }
    public IEnumerator Spawn_CommonAttack2()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject CommonAttack = Instantiate(CommonAttack2_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
    }
    public IEnumerator Spawn_CommonAttack3()
    {
        GameObject CommonAttack = Instantiate(CommonAttack2_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return null;
    }
    public void Skill_Q()
    {
        isSkill = true;
        anim.SetTrigger("Skill_Q");
    }
    IEnumerator Skill_W()
    {
        anim.SetTrigger("Skill_W");
        isSkill = true;
        StartCoroutine(SKill_Up_Move(10.0f, 0.5f, 1f, 0.0f));
        yield return new WaitForSeconds(0.5f);
        WSkill_Collider.SetActive(true);
        yield return new WaitForSeconds(1f);
        WSkill_Collider.SetActive(false);
    }
    IEnumerator Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(Skill_E_Deal());
        StartCoroutine(SKill_Up_Move(20.0f, 0.5f, 2.5f, 1.2f));
        yield return new WaitForSeconds(3.1f);
        ESkill_Collider.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ESkill_Collider.SetActive(false);
    }
    IEnumerator SKill_Up_Move(float upScale, float waitTime1, float waitTime2, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        rd.useGravity = false;
        rd.velocity = Vector3.zero;
        rd.AddForce(Vector3.up * upScale, ForceMode.Impulse);
        yield return new WaitForSeconds(waitTime1);

        // 현재 y 좌표를 고정하고 스킬 상태로 설정
        fixedY = transform.position.y;
        isSkillWE = true;

        yield return new WaitForSeconds(waitTime2);

        rd.useGravity = true;
        rd.velocity = Vector3.zero;
        isSkillWE = false;
    }
    IEnumerator Skill_E_Deal()
    {
        mainCamera.GetComponent<CameraCtrl>().UltimateCamera_Archer(SkillYRot);
        yield return new WaitForSeconds(1.0f);
        //GameObject ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
        //ArrowInstant = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.05f);
    }
    public void comboAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, -90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }
    public void comboAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, -90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }
    public void comboAttack_off()
    {
        Destroy(SkillEffect);
    }
    public void jumpAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, -90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void jumpAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(60, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(60, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void skill_Q_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
        }
    }

    public void skill_W_on()
    {
        SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(SkillW_Effect.transform.eulerAngles));
        SkillEffect.transform.parent = EffectGen.transform;
    }
    public void skill_E1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x + 1.5f, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(SkillE1_Effect.transform.eulerAngles));
        }
        else
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x - 1.5f, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(SkillE1_Effect.transform.eulerAngles));
        }
        SkillEffect.transform.parent = EffectGen.transform;
    }

    public void skill_E2_on()
    {
        SkillEffect = Instantiate(SkillE2_Effect, EffectGen.transform.position, Quaternion.Euler(EffectGen.transform.eulerAngles));
        SkillEffect.transform.parent = EffectGen.transform;
    }

    public void skill_Jump_on()
    {
        SkillEffect = Instantiate(Skill_SmokeEffect, EffectGen.transform.position, Quaternion.Euler(-90f, 0f, 0f));

    }

    public override IEnumerator Heal_on()
    {
        yield return base.Heal_on();
    }

    public override void Damaged_on()
    {
        base.Damaged_on();
    }

    public override void Destroyed_Effect()
    {
        base.Destroyed_Effect();
    }
    
    #endregion

    #region 스킬이나 공격 움직임, Delay 등 세부 조정 함수
    public override void UseSkill(string skillName)
    {
        isSkill = true;
        if (skillName == "Q")
        {
            isSkill = true;
            PlayAnim("Skill_Q");
            Skill_Q();
        }

        if (skillName == "W")
        {
            isSkill = true;
            PlayAnim("Skill_W");
            StartCoroutine(Skill_W());
        }

        if (skillName == "E")
        {
            isSkill = true;
            PlayAnim("Skill_E");
            StartCoroutine(Skill_E());
        }
    }
    protected override IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayAnim("isIdle");
            isAttack = false;
            isSkill = false;
            isSound = false;
        }
    }

    #endregion

    #region 애니메이션 
    public override void PlayAnim(string AnimationName)
    {
        base.PlayAnim(AnimationName);
    }

    public override void StopAnim(string AnimationName)
    {
        base.StopAnim(AnimationName);
    }
    #endregion
}