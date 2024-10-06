using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DruidCtrl : BossCtrl
{
    #region 변수 선언
    public GameObject LScratch_Effect;
    public GameObject RScratch_Effect;
    private GameObject Scratch_Collider;

    public GameObject GroundStrike_Effect;
    private GameObject GroundStrike_Collider_S;
    private GameObject GroundStrike_Collider_M;
    private GameObject GroundStrike_Collider_L;
    public GameObject Projectile_Effect;
    public GameObject Projectile_Collider;

    public GameObject Vine_Effect;
    private GameObject Vine_Collider;
    private float Vine_xGrowthRate = 8f;
    private bool isVine = false;

    public GameObject ToxicPortal_Effect;
    private GameObject ToxicPortal_Collider;

    // 보스 공격 컨트롤
    private Coroutine currentCoroutine;
    private bool canTraceAttack;
    private float TraceTime = 0;
    private bool TraceOn;
    #endregion

    #region Awake, Start, Update문
    protected override void Awake()
    {
        base.Awake();
        Scratch_Collider = GameObject.Find("Scratch");
        GroundStrike_Collider_S = GameObject.Find("GroundStrike_S");
        GroundStrike_Collider_M = GameObject.Find("GroundStrike_M");
        GroundStrike_Collider_L = GameObject.Find("GroundStrike_L");
        Vine_Collider = GameObject.Find("Vine");
        ToxicPortal_Collider = GameObject.Find("ToxicPortal");
    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed = 7f;
        Scratch_Collider.SetActive(false);
        GroundStrike_Collider_S.SetActive(false);
        GroundStrike_Collider_M.SetActive(false);
        GroundStrike_Collider_L.SetActive(false);
        Vine_Collider.SetActive(false);
        ToxicPortal_Collider.SetActive(false);
        shopPortal.SetActive(false);
        StartCoroutine(Think());
        SoundsManager.Change_Sounds("Forest_Boss"); //소리 추가(08.31)
    }

    protected override void Update()
    {
        base.Update();
        SkillYRot = transform.localEulerAngles.y;
        //캔버스 뒤집어지는 오류 해결(08.29)
        MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot - 180f, 0);
        /*
        if(GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y > 0 && GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y < 180)
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot + 90f, 0);
        else
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot - 90f, 0);
        */
        DistanceCheck();
        if (isVine == true && Vine_Collider.transform.localScale.x <= 12.0)
        {
            Vector3 newScale = Vine_Collider.transform.localScale;
            newScale.x += Vine_xGrowthRate * Time.deltaTime;
            Vine_Collider.transform.localScale = newScale;
        }
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
        if(Distance <= 12f)
        {
            int ranAction = Random.Range(0, 1);
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
            int ranAction = Random.Range(0, 2);
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
    protected override IEnumerator MeleeWeakAttack() //첫번째 공격 애니메이션 1.4초 두번째 1.2초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeWeakAttack");   // 애니메이션
        yield return new WaitForSeconds(0.7f);   //애니메이션 지속 시간(10.03 수정)
        atkAudio[0].PlayOneShot(atkAudio[0].clip); //약 공격(할퀴기) 사운드(10.03)
        yield return new WaitForSeconds(1.9f);   //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);   //다음 행동까지 걸리는 시간 
        StartCoroutine(Think());
    }

    protected override IEnumerator MeleeStrongAttack() //공격 애니메이션 1.7초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션
        yield return new WaitForSeconds(0.7f);     //애니메이션 지속 시간(10.03 수정)
        atkAudio[1].PlayOneShot(atkAudio[1].clip); //강 공격(바닥 1번 찍기) 사운드(10.03)
        yield return new WaitForSeconds(1.0f);     //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(3f);    //다음 행동까지 걸리는 시간 
        StartCoroutine(Think());
    }

    protected override IEnumerator RangedWeakAttack() //공격 애니메이션 1.2초
    {
        isAttacking = true;
        anim.SetTrigger("doRangedWeakAttack");
        yield return new WaitForSeconds(0.4f);      //애니메이션 지속 시간(10.03 수정)
        atkAudio[2].PlayOneShot(atkAudio[2].clip); //약 공격(에너지파 날리기) 사운드(10.03)
        yield return new WaitForSeconds(0.8f);      //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);      //다음 행동까지 걸리는 시간 

        StartCoroutine(Think());
    }

    protected override IEnumerator RangedStrongAttack() //공격 애니메이션 3.4초
    {
        isAttacking = true;
        anim.SetTrigger("doRangedStrongAttack");    // 애니메이션
        yield return new WaitForSeconds(0.7f);      //애니메이션 지속 시간(10.03 수정)
        atkAudio[3].PlayOneShot(atkAudio[3].clip); //강 공격(쿵쿵따) 사운드(10.03)
        yield return new WaitForSeconds(2.7f);      //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(3f);      //다음 행동까지 걸리는 시간 

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_1() //첫번째 공격 애니메이션 1.6초 두번째 1.4초 세번째 2초
    {
        isAttacking = true;
        anim.SetTrigger("doSkill1");
        yield return new WaitForSeconds(0.8f);     //애니메이션 지속 시간(10.03 수정)
        atkAudio[4].PlayOneShot(atkAudio[4].clip); //스킬 1(바닥 3번찍기) 사운드(10.03)
        yield return new WaitForSeconds(4.2f);     //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(3f);     //다음 행동까지 걸리는 시간 

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_2()
    {
        isAttacking = true;
        anim.SetTrigger("doSkill2");
        atkAudio[5].PlayOneShot(atkAudio[5].clip); //스킬 2(독구름) 사운드(10.03)
        yield return new WaitForSeconds(1.3f);      //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);      //다음 행동까지 걸리는 시간
        StartCoroutine(Think());
    }
    #endregion

    #region 공격 이펙트 스크립트
    public void Scratch_1()
    {
        if (SkillYRot == 90)
        {
            SkillEffect = Instantiate(RScratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot + 90f, 0));
        }
        else
        {
            Debug.Log("이건가요??");
            SkillEffect = Instantiate(RScratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0 , SkillYRot - 180f, 0));
        }
    }

    public void Scratch_2()
    {
        if (SkillYRot == 90)
        {
            SkillEffect = Instantiate(LScratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot + 90f, 0));
        }
        else
        {
            Debug.Log("이건가요22??");
            SkillEffect = Instantiate(LScratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, SkillYRot - 180f, 0));
        }
    }
    public void GroundStrike()
    {
        SkillEffect = Instantiate(GroundStrike_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 1f, EffectGen.transform.position.z), Quaternion.Euler(0, 0, 0));
    }

    public void Projectile()
    {
        SkillEffect = Instantiate(Projectile_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot-90, 0));
        //SkillCollider = Instantiate(Projectile_Collider, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot-180, 0));
    }

    public void Vine()
    {
        SkillEffect = Instantiate(Vine_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(90, SkillYRot-90, 0));
    }

    public void ToxicPortal()
    {
        SkillEffect = Instantiate(ToxicPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(90, 0, 0));
        //GameObject.FindWithTag("CameraEffect").GetComponent<CameraEffectCtrl>().poisonEffectCamera();
    }
    #endregion
}
