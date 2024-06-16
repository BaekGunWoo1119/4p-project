using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

public class PlayerCtrl_Wizard : PlayerCtrl
{
    #region 변수 선언
    //스킬 컨트롤 bool
    private bool isSkillQ;

    //캐릭터 공격 콜라이더
    private GameObject Attack_Collider_All;
    public GameObject CommonAttack1_Collider;
    public GameObject CommonAttack3_Collider;
    private GameObject QSkill_Collider;
    private float QSkill_zGrowthRate = 25f;
    private GameObject WSkill_Collider;


    //이펙트
    public GameObject Skill_Fire_Aura_Effect;
    public GameObject Skill_Ice_Aura_Effect;
    public GameObject Skill_FireE_Aura_Effect;
    public GameObject Skill_IceE_Aura_Effect;
    private GameObject Skill_Aura_Effect;
    private GameObject SkillE_Aura_Effect;

    private Vector3 tgPos;
    #endregion

    #region Start, FixedUpdate, Update
    protected override void Start() 
    {
        base.Start();
        Attack_Collider_All = transform.Find("AttackColliders").gameObject;
        QSkill_Collider = Attack_Collider_All.transform.Find("QSkill_Collider").gameObject;
        QSkill_Collider.SetActive(false);
        WSkill_Collider = Attack_Collider_All.transform.Find("WSkill_Collider").gameObject;
        WSkill_Collider.SetActive(false);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        base.Update();
        if (PlayerPrefs.GetString("property") == "Fire")
        {
            Skill_Aura_Effect = Skill_Fire_Aura_Effect;
            SkillE_Aura_Effect = Skill_FireE_Aura_Effect;
        }
        else if (PlayerPrefs.GetString("property") == "Ice")
        {
            Skill_Aura_Effect = Skill_Ice_Aura_Effect;
            SkillE_Aura_Effect = Skill_IceE_Aura_Effect;
        }
        if (isSkillQ == true && QSkill_Collider.transform.localScale.z <= 30.0)     //Q 스킬
        {
            Vector3 newScale = QSkill_Collider.transform.localScale;
            newScale.z += QSkill_zGrowthRate * Time.deltaTime;
            QSkill_Collider.transform.localScale = newScale;
        }
        if (stateSkillE == true)    //E 스킬
        {
            transform.position = Vector3.Lerp(transform.position, tgPos, 0.01f);
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
    protected override IEnumerator Immune(float seconds)
    {
        Debug.Log(seconds + "만큼 무적");
        yield return base.Immune(seconds);
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
    public IEnumerator Skill_Q()
    {
        StartCoroutine(Immune(4f));
        yield return new WaitForSeconds(1.25f);
        QSkill_Collider.SetActive(true);
        isSkillQ = true;
        yield return new WaitForSeconds(2f);
        QSkill_Collider.SetActive(false);
        isSkillQ = false;
        QSkill_Collider.transform.localScale = new Vector3(1, 5, 1);
    }
    public IEnumerator Skill_W()
    {
        StartCoroutine(Immune(3f));
        yield return new WaitForSeconds(1f);
        WSkill_Collider.SetActive(true);
        yield return new WaitForSeconds(1f);
        WSkill_Collider.SetActive(false);
    }
    public IEnumerator Skill_E_Move()
    {
        mainCamera.GetComponent<CameraCtrl>().UltimateCamera_Wizard(SkillYRot);
        StartCoroutine(Immune(6f));
        tgPos = new Vector3(transform.position.x, transform.position.y + 4.0f, transform.position.z);
        rd.useGravity = false;
        yield return new WaitForSeconds(6.0f);
        isSkill = false;
        rd.useGravity = true;
        Fall();
    }
    public IEnumerator Spawn_CommonAttack1()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject CommonAttack = Instantiate(CommonAttack1_Collider, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot + 90f, 0f));
    }
    public IEnumerator Spawn_CommonAttack2()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject CommonAttack = Instantiate(CommonAttack1_Collider, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot + 90f, 0f));
    }
    public IEnumerator Spawn_CommonAttack3()
    {
        GameObject CommonAttack = Instantiate(CommonAttack3_Collider, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot + 90f, 0f));
        yield return null;
    }
    public void comboAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
    }
    public void comboAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
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
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, 0, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, 180, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
    }

    public void jumpAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(60, 0, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(60, 180, 0));
            SkillEffect.transform.position = EffectGen.transform.position;
        }
    }

    public void skill_Aura_on()
    {
        SkillEffect = Instantiate(Skill_Aura_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 1, EffectGen.transform.position.z), Quaternion.Euler(0f, 90, 0f));
    }

    public void skill_Q_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 0, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 180, 0f));
        }
    }

    public void skill_W_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }

    }

    public void skill_E_Aura_on()
    {
        SkillEffect = Instantiate(SkillE_Aura_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 1, EffectGen.transform.position.z), Quaternion.Euler(0f, 90, 0f));
    }

    public void skill_E1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, -90, 0f));
        }
    }

    public void skill_E2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, -90, 0f));
        }
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
            StartCoroutine(Skill_Q());
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
            StartCoroutine(Skill_E_Move());
        }
    }

    IEnumerator MoveForwardForSeconds(float seconds)
    {
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
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
