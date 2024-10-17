using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKingCtrl : BossCtrl
{
    #region 변수 선언
    public GameObject SpearSwing_Effect;
    public GameObject SpearSting_Effect;
    public GameObject SpearSwing_Strong_Effect;
    public GameObject FireShot_Effect;
    public GameObject SpearPortal_Effect;
    public GameObject FireThrower_Effect;
    public GameObject FireWall_Effect;


    // 보스 공격 컨트롤
    private bool canTraceAttack;
    private bool isRangedAttack = true;
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
        //SoundsManager.Change_Sounds("Castle_Boss"); //소리 추가(08.31)
    }

    protected override void Update()
    {
        base.Update();
        SkillYRot = transform.localEulerAngles.y;
        //캔버스 뒤집어지는 오류 해결(08.29)
        MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot - 180f, 0);
        DistanceCheck();
        if(isDie == true)
        {
            SoundsManager.Change_Sounds("Castle"); //소리 추가(08.31)
            GameObject.Find("EventSystem").GetComponent<GameEnd>().enabled = false;
            GameObject.Find("EventSystem").GetComponent<GameClear>().Game_Clear(true); //싱글용 클리어 추가(10.18)
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
        Debug.Log("추적");
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement = new Vector3(directionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        anim.SetBool("doMove", true);

        if (Distance <= 3f)
        {
            int ranAction = Random.Range(0, 3);
            anim.SetBool("doMove", false);
            switch (ranAction)
            {
                case 0:
                    Debug.Log("근접 약공 선택됨");
                    StartCoroutine(MeleeWeakAttack());
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
                case 1:
                    Debug.Log("근접 강공 선택됨");
                    StartCoroutine(MeleeStrongAttack());
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
                case 2:
                    Debug.Log("스킬 1 선택됨");
                    StartCoroutine(Skill_1());
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
            }
        }
        else if(Distance <= 12f && isRangedAttack)
        {
            int ranAction = Random.Range(0, 2);
            anim.SetBool("doMove", false);
            switch (ranAction)
            {
                case 0:
                    int chooseAttack = Random.Range(0, 3);
                    switch (chooseAttack)
                    {
                        case 0:
                            Debug.Log("추적 후_원거리 약공 선택됨");
                            StartCoroutine(RangedWeakAttack());
                            TraceOn = false;
                            break;
                        case 1:
                            Debug.Log("추적 후_원거리 강공 선택됨");
                            StartCoroutine(RangedStrongAttack());
                            TraceOn = false;
                            break;
                        case 2:
                            Debug.Log("추적 후_스킬 2 선택됨");
                            StartCoroutine(Skill_2());
                            TraceOn = false;
                            break;
                    }
                    break;
                case 1:
                    isRangedAttack = false;
                    break;
            }
        }
        else
        {
            transform.Translate(movement, Space.World);
            anim.SetBool("doMove", true);
        }
        yield return null;
    }

    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("생각");
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
                case 1:
                    int chooseAttack = Random.Range(0, 3);
                    switch (chooseAttack)
                    {
                        case 0:
                            Debug.Log("원거리 약공 선택됨");
                            StartCoroutine(RangedWeakAttack());
                            TraceOn = false;
                            break;
                        case 1:
                            Debug.Log("원거리 강공 선택됨");
                            StartCoroutine(RangedStrongAttack());
                            TraceOn = false;
                            break;
                        case 2:
                            Debug.Log("스킬 2 선택됨");
                            StartCoroutine(Skill_2());
                            TraceOn = false;
                            break;
                    }
                    break;
            }
        }
        else
        {
            TraceOn = true;
        }
    }

    // 공격 애니메이션 && 콜라이더 스크립트
    protected override IEnumerator MeleeWeakAttack() //첫번째 공격 애니메이션 1.2초 두번째 1.3초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeWeakAttack");   // 애니메이션
        yield return new WaitForSeconds(0.65f); 
        atkAudio[0].PlayOneShot(atkAudio[0].clip); //약 공격(창 3번 스윙) 사운드(10.18)
        yield return new WaitForSeconds(0.6f); 
        atkAudio[6].PlayOneShot(atkAudio[6].clip); //약 공격(창 3번 스윙) 사운드(10.18)
        yield return new WaitForSeconds(1.0f); //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(1f);        //다음 행동까지 걸리는 시간 
        StartCoroutine(Think());
    }

    protected override IEnumerator MeleeStrongAttack() //공격 애니메이션 1.8초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션
        yield return new WaitForSeconds(1.0f); 
        atkAudio[1].PlayOneShot(atkAudio[1].clip); //강 공격(창 강한 스윙) 사운드(10.18)
        yield return new WaitForSeconds(0.8f);    //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);        //다음 행동까지 걸리는 시간
        StartCoroutine(Think());
    }

    protected override IEnumerator RangedWeakAttack() //공격 애니메이션 1.15초
    {
        isAttacking = true;
        anim.SetTrigger("doRangedWeakAttack");      // 애니메이션
        yield return new WaitForSeconds(0.93f); 
        atkAudio[2].PlayOneShot(atkAudio[2].clip); //약 공격(파이어 샷) 사운드(10.18)
        yield return new WaitForSeconds(0.22f);    //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(1f);        //다음 행동까지 걸리는 시간 
        StartCoroutine(Think());
    }

    protected override IEnumerator RangedStrongAttack() //공격 애니메이션 1.3초 
    {
        isAttacking = true;
        anim.SetTrigger("doRangedStrongAttack");    // 애니메이션
        yield return new WaitForSeconds(0.93f); 
        atkAudio[3].PlayOneShot(atkAudio[3].clip); //강 공격(창 포탈) 사운드(10.18)
        yield return new WaitForSeconds(0.37f);      //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);        //다음 행동까지 걸리는 시간      
        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_1() //공격 애니메이션 3.4초
    {
        isAttacking = true;
        anim.SetTrigger("doSkill1");    // 애니메이션
        yield return new WaitForSeconds(2.6f); 
        atkAudio[4].PlayOneShot(atkAudio[4].clip); //스킬 공격(불) 사운드(10.18)
        yield return new WaitForSeconds(0.8f);      //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(2f);        //다음 행동까지 걸리는 시간      
        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_2() //공격 애니메이션 2.1f초
    {
        isAttacking = true;
        anim.SetTrigger("doSkill2");    // 애니메이션
        yield return new WaitForSeconds(1.55f); 
        atkAudio[5].PlayOneShot(atkAudio[5].clip); //스킬 공격(불) 사운드(10.18)
        yield return new WaitForSeconds(0.55f);        //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(2f);        //다음 행동까지 걸리는 시간      
        StartCoroutine(Think());
    }
    #endregion

    #region 공격 이펙트 스크립트
    public void SpearSwing()
    {
        SkillEffect = Instantiate(SpearSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot + 90f, 0));
    }

    public void SpearSting()
    {
        SkillEffect = Instantiate(SpearSting_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot, 0));
    }
    public void SpearSwing_Strong()
    {
        SkillEffect = Instantiate(SpearSwing_Strong_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot + 90f, 0));
    }
    public void FireShot()
    {
        SkillEffect = Instantiate(FireShot_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1.5f, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot, 0));
    }

    public void SpearPortal()
    {
        SkillEffect = Instantiate(SpearPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot, 0));        
    }

    public void FireThrower()
    {
        SkillEffect = Instantiate(FireThrower_Effect, new Vector3(PlayerTr.position.x, PlayerTr.position.y, PlayerTr.position.z), Quaternion.Euler(0, 0, 0));
    }
    public void FireWall()
    {
        SkillEffect = Instantiate(FireWall_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot - 90f, 0));
    }
    #endregion
}