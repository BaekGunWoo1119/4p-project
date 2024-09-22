using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Golem : BossCtrl
{
    #region 변수 선언
    public GameObject ArmSwing_Effect;
    public GameObject GroundStamp_Effect;
    public GameObject FallingRock_Effect;
    public GameObject GroundSmash_Effect;
    public GameObject PowerSmash_Effect;
    public GameObject JumpSmash_Effect;

    // 보스 공격 컨트롤
    private bool canTraceAttack;
    private float TraceTime = 0;
    private bool TraceOn;
    private Vector3 PrevPosition;
    #endregion

    #region Awake, Start, Update문
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed = 7f;

        shopPortal.SetActive(false);
        StartCoroutine(Think());
        SoundsManager.Change_Sounds("Forest_Boss"); //소리 추가(08.31)
    }

    protected override void Update()
    {
        base.Update();
        SkillYRot = transform.localEulerAngles.y;
        //캔버스 뒤집어지는 오류 해결(08.29)
        if(GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y > 0 && GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y < 180)
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot + 90f, 0);
        else
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
        DistanceCheck();
        if(TraceOn == true)
        {
            TraceTime += Time.deltaTime;
        }
        if (TraceTime >= 3f)
        {
            TraceTime = 0;
            TraceOn = false;
        }
        if(isDie == true)
        {
            SoundsManager.Change_Sounds("Forest"); //소리 추가(08.31)
        }
    }
    #endregion

    #region HP 관련
    protected override void SetHP(float amount) // Hp����
    {
        maxHP = amount;
        curHP = maxHP;
    }

    public override void CheckHP() // HP ����
    {
        base.CheckHP();
    }

    #endregion

    #region 보스 피격, 피해량 공식
    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
    }
    public override IEnumerator TakeDamage(float Damage)
    {
        yield return base.TakeDamage(Damage);
    }
    #endregion

    #region 보스 패턴 관련
    protected override IEnumerator FindPlayer()     // 플레이어를 찾아서 할당해주는 함수
    {
        yield return base.FindPlayer();
    }
    protected override void DistanceCheck()
    {
        if(PlayerTr != null)
            Distance = Vector3.Distance(transform.position, PlayerTr.position);
        //여기서부턴 세부 구현, 각 스크립트에서 보스 패턴에 맞게 구현
        if(TraceOn == true)
        {
            StartCoroutine(Trace());
        }

    }
    public IEnumerator Trace()
    {
        // 플레이어를 향해 이동하는 로직
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement = new Vector3(directionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        anim.SetBool("doMove", true);
        if (Distance <= 3)
        {
            int ranAction = Random.Range(0, 3);
            anim.SetBool("doMove", false);
            switch (ranAction)
            {
                case 0:
                    Debug.Log("근접 약공 선택됨");
                    StartCoroutine(MeleeWeakAttack());
                    TraceOn = false;
                    break;
                case 1:
                    Debug.Log("근접 강공 선택됨");
                    StartCoroutine(MeleeStrongAttack());
                    TraceOn = false;
                    break;
                case 2:
                    Debug.Log("스킬 1 선택됨");
                    StartCoroutine(Skill_1());
                    TraceOn = false;
                    break;
            }
        }
        else
        {
            transform.Translate(movement, Space.World);
            anim.SetBool("doMove", true);
        }
        if(TraceTime > 3)
        {
            TraceOn = false;
        }
        yield return null;
    }

    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Distance = " + Distance);
        Debug.Log("여기까지는 잘 돼요");
        if(Distance <= 12f)
        {
            Debug.Log("1번 들어옴");
            int ranAction = Random.Range(0, 2);
            Debug.Log("선택된 번호는 ? = " + ranAction);
            switch (ranAction)
            {
                case 0:
                    Debug.Log("Trace 선택됨");
                    TraceOn = true;
                    break;
            }
        }
        else
        {
            Debug.Log("2번 들어옴");
            int ranAction = Random.Range(0, 2);
            Debug.Log("선택된 번호는 ? = " + ranAction);
            switch (ranAction)
            {
                case 0:
                    int chooseAttack = Random.Range(0, 2);
                    switch (chooseAttack)
                    {
                        case 0:
                            Debug.Log("원거리 약공 선택됨");
                            StartCoroutine(RangedWeakAttack());
                            break;
                        case 1:
                            Debug.Log("원거리 강공 선택됨");
                            StartCoroutine(RangedStrongAttack());
                            break;
                    }
                    break;
                case 1:
                    Debug.Log("스킬 2 선택됨");
                    StartCoroutine(Skill_2());
                    break;
            }
        }
    }

    // 공격 애니메이션 && 콜라이더 스크립트
    protected override IEnumerator MeleeWeakAttack()
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeWeakAttack");   // 애니메이션

        yield return new WaitForSeconds(0.75f); // 스킬 콜라이더 ~~
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        yield return new WaitForSeconds(4f);    // ~~ 스킬 콜라이더
        StartCoroutine(Think());
    }

    protected override IEnumerator MeleeStrongAttack()
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션

        yield return new WaitForSeconds(1f);        // 스킬 콜라이더 ~~

        yield return new WaitForSeconds(0.25f);

        yield return new WaitForSeconds(0.25f);
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        yield return new WaitForSeconds(3f);        // ~~ 스킬 콜라이더
        StartCoroutine(Think());
    }

    protected override IEnumerator RangedWeakAttack()
    {
        isAttacking = true;
        anim.SetTrigger("doRangedWeakAttack");      // 애니메이션
        yield return new WaitForSeconds(4f);

        StartCoroutine(Think());
    }

    protected override IEnumerator RangedStrongAttack()
    {
        isAttacking = true;
        anim.SetTrigger("doRangedStrongAttack");    // 애니메이션

        yield return new WaitForSeconds(2.5f);      // 스킬 콜라이더 ~~
        yield return new WaitForSeconds(4f);
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);      // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_1()
    {
        isAttacking = true;
        anim.SetTrigger("doSkill1");

        yield return new WaitForSeconds(1f);          // 스킬 콜라이더 ~~
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기

        yield return new WaitForSeconds(0.1f);          // 0.1초 대기

        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기

        yield return new WaitForSeconds(0.75f);          // 0.1초 대기

        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        isAttacking = false;
        yield return new WaitForSeconds(3f);            // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_2()
    {
        isAttacking = true;
        anim.SetTrigger("doSkill2");
        yield return new WaitForSeconds(0.75f);
        yield return new WaitForSeconds(2.75f);
        isAttacking = false;
        StartCoroutine(Think());
    }
    #endregion

    #region 공격 이펙트 스크립트
    public void ArmSwing_1()
    {
        if (SkillYRot == 180 || (SkillYRot > 130 && SkillYRot < 230))
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(45, -90, 0));
        }
        else
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(45, -90, 0));
        }
    }

    public void ArmSwing_2()
    {
        if (SkillYRot == 180 || (SkillYRot > 130 && SkillYRot < 230))
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(-70, -40, 0));
        }
        else
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(-70, -40, 0));
        }
    }
    public void GroundStamp()
    {
        SkillEffect = Instantiate(GroundStamp_Effect, new Vector3(EffectGen.transform.position.x - 0.5f, EffectGen.transform.position.y, EffectGen.transform.position.z + 1), Quaternion.Euler(0, 0, 0));
    }
    public void FallingRock()
    {
        SkillEffect = Instantiate(FallingRock_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 7f, EffectGen.transform.position.z + 5f), Quaternion.Euler(0, 0, 0));
    }
    public void GroundSmash()
    {
        SkillEffect = Instantiate(GroundSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }
    public void PowerSmash()
    {
        SkillEffect = Instantiate(PowerSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }
    public void JumpSmash()
    {
        SkillEffect = Instantiate(JumpSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }

    #endregion
}

