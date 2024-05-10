using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//전사 애니메이션
public class PlayerCtrl_Warrior : PlayerCtrl
{
    #region 변수 선언
    //애니메이션 컨트롤
    private bool isSkillQ;

    //캐릭터 공격 콜라이더
    private GameObject Attack_Collider_All;
    public GameObject QSkill_Collider;
    private GameObject WSkill_Collider;
    private GameObject Attack_1_Collider;

    //이펙트
    public GameObject Skill_FireE_Effect;
    public GameObject Skill_IceE_Effect;
    private GameObject SkillE_Effect;

    private ParticleSystem setParticles1;
    private ParticleSystem setParticles2;
    private ParticleSystem setParticles3;
    private ParticleSystem setParticles4;
    #endregion

    protected override void Start()
    {
        base.Start();

        //플레이어 어택 콜라이더 인식 방식 변경 (서버에 맞게)
        Attack_Collider_All = transform.Find("AttackColliders").gameObject;
        WSkill_Collider = Attack_Collider_All.transform.Find("WSkill_Collider").gameObject;
        WSkill_Collider.SetActive(false);
        Attack_1_Collider = Attack_Collider_All.transform.Find("Attack_1_Collider").gameObject;
        Attack_1_Collider.SetActive(false);

        isSkillQ = false;
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
            SkillE_Effect = Skill_FireE_Effect;
        }
        else if (PlayerPrefs.GetString("property") == "Ice")
        {
            SkillE_Effect = Skill_IceE_Effect;
        }
    }

    #region HP 설정
    public override void SetHp(float amount)
    {
        base.SetHp(amount);
    }
    public override void CheckHp()
    {
        base.CheckHp();
    }

    protected override IEnumerator TakeDamage()
    {
        yield return base.TakeDamage();
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

    protected override void Move()
    {
        base.Move();
    }

    protected override void Move_anim()
    {
        base.Move_anim();
    }

    protected override void Turn()
    {
        base.Turn();
    }

    protected override void Dodge()
    {
        base.Dodge();
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
    protected override void OnCollisionStay(Collision collision) // 충돌 감지
    {
        base.OnCollisionStay(collision);
    }
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
        anim.SetTrigger("CommonAttack");
        isAttack = true;
    }

    protected override void Skill_Q()
    {
        isSkill = true;
        isSkillQ = true;
        anim.SetTrigger("Skill_Q");
        StartCoroutine(Spawn_SwordAura());
    }
    protected override void Skill_W()
    {
        WSkill_Collider.SetActive(true);
        anim.SetTrigger("Skill_W");
        isSkill = true;
        StartCoroutine(MoveForwardForSeconds(1.35f));
    }
    protected override void Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(SKill_E_Move());
        StartCoroutine(WarriorSkill_E());
    }
    protected override void CommonAttack1()
    {
        Debug.Log("기본공격 실행!!");
        isSound = false;
        StartCoroutine(Attack1_Collider());
        StartCoroutine(Delay(0.4f));
        StartCoroutine(MoveForwardForSeconds(0.3f));
        StartCoroutine(Delay(0.2f));
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.03f, null);
    }
    protected override void CommonAttack2()
    {
        StartCoroutine(Attack1_Collider());
        StartCoroutine(Attack_Sound(1, 0.8f));
        StartCoroutine(Delay(0.2f));
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.01f, null);
    }
    protected override void CommonAttack3()
    {
        StartCoroutine(Delay(0.2f));
        StartCoroutine(MoveForwardForSeconds(0.3f));
        StartCoroutine(Attack1_Collider());
        StartCoroutine(Attack_Sound(2, 0.8f));
        StartCoroutine(Delay(5.0f));
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.3f, 0.1f, null);
    }
    protected override void JumpAttack1()
    {
        isSound = false;
        mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.5f, "null");
        StartCoroutine(Attack1_Collider());
    }
    protected override void JumpAttack2()
    {
        mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.6f, "null");
        StartCoroutine(Attack1_Collider());
        StartCoroutine(Delay(0.2f));
    }
    protected override void JumpAttack3()
    {
        mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.5f, "null");
        StartCoroutine(Attack1_Collider());
        anim.ResetTrigger("CommonAttack");
    }
    IEnumerator Attack1_Collider()
    {
        yield return new WaitForSeconds(0.15f);
        Attack_1_Collider.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (Attack_1_Collider == true)
        {
            Attack_1_Collider.SetActive(false);
        }
    }
    IEnumerator Attack_Sound(int AttackValue, float playsec)
    {
        if (AttackValue == 1)
        {
            isSound = true;
        }
        audioSources[AttackValue].Play();
        yield return new WaitForSeconds(playsec);
        audioSources[AttackValue].Stop();
        yield return null;
    }


    IEnumerator Spawn_SwordAura()
    {
        isSkillQ = false;
        yield return new WaitForSeconds(0.2f);
        GameObject SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        yield return new WaitForSeconds(0.1f);
        audioSources[3].Play();
        yield return new WaitForSeconds(0.3f);
        //쿨타임
        QSkillCoolTime = 0;
        Qcool.fillAmount = 1;
        yield return new WaitForSeconds(0.2f);
        audioSources[3].Stop();
    }

    IEnumerator SKill_E_Move()
    {
        float elapsedTime = 0;
        yield return new WaitForSeconds(1.8f);
        //스킬 사용 시 카메라 무빙(등 포커스)
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 5, transform.position.y + 2.5f, transform.position.z, 60, 5.3f, "round");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 2.5f, transform.position.y + 2.5f, transform.position.z, -30, 5.3f, "round");
        }
        yield return new WaitForSeconds(1.0f);
        while (elapsedTime < 0.3)
        {
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        elapsedTime = 0;
        while (elapsedTime < 0.5)
        {
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0;
        while (elapsedTime < 0.5)
        {
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator WarriorSkill_E()
    {
        yield return new WaitForSeconds(1.8f);
        GameObject SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        //스킬 나갈 시 카메라 무빙(흔들림)
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.6f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.5f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.8f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.2f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(1.2f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.8f);
        audioSources[3].Stop();
        SwordAuraInstance = Instantiate(QSkill_Collider, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        audioSources[3].Play();
        mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(1f);
        audioSources[3].Stop();
        ESkillCoolTime = 0;
        Ecool.fillAmount = 1;
        mainCamera.GetComponent<CameraCtrl>().moveStop(0.1f);
    }

    public void comboAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            audioSources[0].Play();
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            audioSources[0].Play();
        }
    }
    public void comboAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
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
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            audioSources[1].Play();
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            audioSources[1].Play();
        }
    }

    public void skill_Q_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            //Find로 Slash 찾아서 파티클시스템의 3d start 직접 제어
            setParticles1 = SkillQ_Effect.transform.Find("Slashes").GetComponent<ParticleSystem>();
            setParticles2 = SkillQ_Effect.transform.Find("Slashes").transform.Find("Slashes-1").GetComponent<ParticleSystem>();
            StartCoroutine(RotateEffect(0f, 0f, 0f, setParticles1));
            StartCoroutine(RotateEffect(0f, 0f, 0f, setParticles2));
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        }
        else
        {
            setParticles1 = SkillQ_Effect.transform.Find("Slashes").GetComponent<ParticleSystem>();
            setParticles2 = SkillQ_Effect.transform.Find("Slashes").transform.Find("Slashes-1").GetComponent<ParticleSystem>();
            StartCoroutine(RotateEffect(0f, 179.0f, 0f, setParticles1));
            StartCoroutine(RotateEffect(0f, 179.0f, 0f, setParticles2));
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
        }
    }

    public void skill_W_on()
    {
        SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(SkillW_Effect.transform.eulerAngles));
        SkillEffect.transform.parent = EffectGen.transform;
    }

    public void skill_E_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            setParticles1 = SkillE_Effect.transform.Find("Slashes").GetComponent<ParticleSystem>();
            setParticles2 = SkillE_Effect.transform.Find("Slashes-1").GetComponent<ParticleSystem>();
            setParticles3 = SkillE_Effect.transform.Find("Slashes").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
            setParticles4 = SkillE_Effect.transform.Find("Slashes-1").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
            StartCoroutine(RotateEffect(0.6f, 0f, 0f, setParticles1));
            StartCoroutine(RotateEffect(-0.6f, 0f, 0f, setParticles2));
            StartCoroutine(RotateEffect(0.8f, 0f, 0f, setParticles3));
            StartCoroutine(RotateEffect(-0.8f, 0f, 0f, setParticles4));
            SkillEffect = Instantiate(SkillE_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        }
        else
        {
            setParticles1 = SkillE_Effect.transform.Find("Slashes").GetComponent<ParticleSystem>();
            setParticles2 = SkillE_Effect.transform.Find("Slashes-1").GetComponent<ParticleSystem>();
            setParticles3 = SkillE_Effect.transform.Find("Slashes").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
            setParticles4 = SkillE_Effect.transform.Find("Slashes-1").transform.Find("Slashes (1)").GetComponent<ParticleSystem>();
            StartCoroutine(RotateEffect(0.6f, 179.0f, 0f, setParticles1));
            StartCoroutine(RotateEffect(-0.6f, 179.0f, 0f, setParticles2));
            StartCoroutine(RotateEffect(0.8f, 179.0f, 0f, setParticles3));
            StartCoroutine(RotateEffect(-0.8f, 179.0f, 0f, setParticles4));
            SkillEffect = Instantiate(SkillE_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
        }
    }
    IEnumerator RotateEffect(float xR, float yR, float zR, ParticleSystem particle)
    {
        var mainModule = particle.main;
        mainModule.startRotationX = xR;
        mainModule.startRotationY = yR;
        mainModule.startRotationZ = zR;
        yield return null;
    }
    #endregion

    #region 스킬이나 공격 움직임, Delay 등 세부 조정 함수

    IEnumerator MoveForwardForSeconds(float seconds)
    {
        coroutineMove = true;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //앞으로 가는 애니메이션 실행 시가 아닐 때만 false 반환
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E"))
        {

        }
        else
        {
            coroutineMove = false;
        }

        if (WSkill_Collider.activeSelf == true)
        {
            WSkill_Collider.SetActive(false);
            WSkillCoolTime = 0;
            Wcool.fillAmount = 1;
        }
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetBool("isIdle", true);
            isAttack = false;
            isSkill = false;
            isSound = false;
        }
    }
    #endregion

}
