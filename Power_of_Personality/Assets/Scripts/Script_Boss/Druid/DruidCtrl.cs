using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DruidCtrl : BossCtrl
{
    #region 변수 선언
    public GameObject Scratch_Effect;
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
    private bool canTraceAttack;
    private float TraceTime = 0;
    private bool TraceOn;
    private bool BackDashOn;
    private Vector3 PrevPosition;
    private GameObject LeftTPpoint;
    private GameObject RightTPpoint;
    private GameObject shopPortal;
    #endregion

    #region Awake, Start, Update문
    protected override void Awake()
    {
        base.Awake();
        SkillYRot = transform.eulerAngles.y;
        Scratch_Collider = GameObject.Find("Scratch");
        GroundStrike_Collider_S = GameObject.Find("GroundStrike_S");
        GroundStrike_Collider_M = GameObject.Find("GroundStrike_M");
        GroundStrike_Collider_L = GameObject.Find("GroundStrike_L");
        Vine_Collider = GameObject.Find("Vine");
        ToxicPortal_Collider = GameObject.Find("ToxicPortal");
        LeftTPpoint = GameObject.Find("LeftTPpoint");
        RightTPpoint = GameObject.Find("RightTPpoint");
        shopPortal = GameObject.Find("Normal_Shop_Portal");
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
    }

    protected override void Update()
    {
        base.Update();
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
        if(BackDashOn == true)
        {
            StartCoroutine(BackDash());
        }
    }
    #endregion

    #region HP 관련
    protected override void SetHP(float amount) // Hp����
    {
        maxHP = amount;
        curHP = maxHP;
    }

    protected override void CheckHP() // HP ����
    {
        if (HpBar != null)
            HpBar.value = curHP / maxHP;
    }
    #endregion

    #region 보스 피격, 피해량 공식
    public override void OnTriggerEnter(Collider col)
    {
        #region 전사
        if (col.tag == "WarriorAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 2f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorSkillE")
        {
            isHit = true;
            Damage = Status.TotalAP * 4f;
            StartCoroutine(TakeDamage());
        }
        #endregion
        #region 도적
        if (col.tag == "RougeAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillQ_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillW_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 1.5f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_4")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            StartCoroutine(TakeDamage());
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "ArcherAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "ArcherSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 0.1f;
            StartCoroutine(TakeDamage());
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WizardAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WizardSkillW")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WizardSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP * 2.5f;
            StartCoroutine(TakeDamage());
        }
        if (col.tag == "WizardSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 5f;
            StartCoroutine(TakeDamage());
        }
        #endregion
    }

    public override void OnTriggerStay(Collider col)
    {
        #region 전사
        if (col.tag == "WarriorSkillW" && TickCoolTime >= 0.75f)
        {
            isHit = true;
            Damage = Status.TotalAP * 1.5f;
            StartCoroutine(TakeDamage());
        }
        #endregion
        #region 도적
        if (col.tag == "RougeSkillQ_1" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.3f;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }

        if (col.tag == "RougeSkillW_1" && TickCoolTime >= 0.3f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.4f;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        if (col.tag == "RougeSkillE_3" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherSkillW" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        if (col.tag == "ArcherSkillE" && TickCoolTime >= 0.2f)
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardSkillQ" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        #endregion
    }
    protected override IEnumerator TakeDamage()
    {
        if (maxHP != 0 || curHP > 0)
        {
            if (PlayerPrefs.GetString("property") == "Ice")
            {
                Instantiate(IceHit, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            if (PlayerPrefs.GetString("property") == "Fire")
            {
                Instantiate(FireHit, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            if (PlayerPrefs.GetString("property") == WeakProperty)
            {
                Damage = Damage * 1.5f;
            }
            Material[] materials = matObj.materials;
            curHP -= Damage * (1 / (1 + DEF * 0.01f));
            CheckHP(); // ü�� ����
            anim.SetBool("TakeDamage", true);
            foreach (Material material in materials)
            {
                material.color = Color.red;
            }

            //StartCoroutine(DamageTextAlpha());

            yield return new WaitForSeconds(0.1f);
            anim.SetBool("TakeDamage", false);
            yield return new WaitForSeconds(0.2f);
            if (anim.GetBool("TakeDamage") == false)
            {
                isHit = false;
            }
            foreach (Material material in materials)
            {
                material.color = Color.white;
            }
        }

        if (curHP <= 0) // ü���� 0�϶�
        {
            isDie = true;
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            Vector3 CoinPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            Instantiate(Coin, CoinPosition, gameObject.transform.rotation);
            Destroy(BossWall1);
            Destroy(BossWall2);
            //상점 생성
            shopPortal.SetActive(true);
            shopPortal.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
            //오브젝트 및 체력 바 파괴
            Destroy(this.gameObject); // ü���� 0 ���϶� ����
            Destroy(HpBar.gameObject);
        }
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



    public IEnumerator BackDash()
    {
        if(canTeleport == false)
        {
            Vector3 ReversedirectionToPlayer = -(PlayerTr.position - transform.position).normalized;
            Vector3 movement = new Vector3(ReversedirectionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
            anim.SetBool("doBack", true);
            if (Vector3.Distance(PrevPosition, transform.position) >= 3)
            {
                BackDashOn = false;
                anim.SetBool("doBack", false);
                StartCoroutine(Think());
            }
            yield return null;
        }
        else
        {
            BackDashOn = false;
            Teleport();
        }
    }
    public void Teleport()
    {
        if (Vector3.Distance(LeftTPpoint.transform.position, transform.position) < Vector3.Distance(RightTPpoint.transform.position, transform.position)) 
        { 
            transform.position = RightTPpoint.transform.position;
            StartCoroutine(Think());
        }
        else
        {
            transform.position = LeftTPpoint.transform.position;
            StartCoroutine(Think());
        }
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
                case 1:
                    Debug.Log("BackDash 선택됨");
                    BackDashOn = true;
                    PrevPosition = transform.position;
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
        Scratch_Collider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Scratch_Collider.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        Scratch_Collider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Scratch_Collider.SetActive(false);
        isAttacking = false;
        yield return new WaitForSeconds(4f);    // ~~ 스킬 콜라이더
        StartCoroutine(Think());
    }

    protected override IEnumerator MeleeStrongAttack()
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션

        yield return new WaitForSeconds(1f);        // 스킬 콜라이더 ~~
        GroundStrike_Collider_S.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        GroundStrike_Collider_S.SetActive(false);
        GroundStrike_Collider_M.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        GroundStrike_Collider_M.SetActive(false);
        GroundStrike_Collider_L.SetActive(true);
        yield return new WaitForSeconds(1f);
        GroundStrike_Collider_L.SetActive(false);
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
        Vine_Collider.SetActive(true);
        isVine = true;
        yield return new WaitForSeconds(4f);
        isVine = false;
        Vine_Collider.SetActive(false);
        Vine_Collider.transform.localScale = new Vector3(1, 1, 1);
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);      // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_1()
    {
        isAttacking = true;
        anim.SetTrigger("doSkill1");

        yield return new WaitForSeconds(1f);          // 스킬 콜라이더 ~~
        GroundStrike_Collider_S.SetActive(true);        // 1-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 1-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 1-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 1-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 1-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 1-3 Off

        yield return new WaitForSeconds(0.1f);          // 0.1초 대기

        GroundStrike_Collider_S.SetActive(true);        // 2-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 2-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 2-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 2-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 2-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 2-3 Off

        yield return new WaitForSeconds(0.75f);          // 0.1초 대기

        GroundStrike_Collider_S.SetActive(true);        // 2-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 2-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 2-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 2-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 2-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 2-3 Off
        isAttacking = false;
        yield return new WaitForSeconds(3f);            // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    protected override IEnumerator Skill_2()
    {
        isAttacking = true;
        anim.SetTrigger("doSkill2");
        yield return new WaitForSeconds(0.75f);
        ToxicPortal_Collider.SetActive(true);
        yield return new WaitForSeconds(2.75f);
        ToxicPortal_Collider.SetActive(false);
        isAttacking = false;
        StartCoroutine(Think());
    }
    #endregion

    #region 공격 이펙트 스크립트
    public void Scratch_1()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30, -30, 90));
        }
        else
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30, 150, 90));
        }
    }

    public void Scratch_2()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(-30, -30, 90));
        }
        else
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(-30, 150, 90));
        }
    }
    public void GroundStrike()
    {
        SkillEffect = Instantiate(GroundStrike_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 1f, EffectGen.transform.position.z), Quaternion.Euler(0, 0, 0));
    }

    public void Projectile()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Projectile_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillCollider = Instantiate(Projectile_Collider, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            SkillEffect = Instantiate(Projectile_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillCollider = Instantiate(Projectile_Collider, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    public void Vine()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Vine_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(90, 0, 0));
        }
        else
        {
            SkillEffect = Instantiate(Vine_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(90, 180, 0));
        }
    }

    public void ToxicPortal()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(ToxicPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(90, 90, 0));
        }
        else
        {
            SkillEffect = Instantiate(ToxicPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(90, -90, 0));
        }
        GameObject.FindWithTag("CameraEffect").GetComponent<CameraEffectCtrl>().poisonEffectCamera();
    }
    #endregion
}
