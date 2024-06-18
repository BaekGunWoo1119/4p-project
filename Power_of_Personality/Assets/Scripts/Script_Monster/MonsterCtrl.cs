using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterCtrl : MonoBehaviour
{
    #region 변수 선언

    public Animator anim;   // 몬스터 애니메이터
    public Slider HpBar;    // HP바 슬라이더

    //몬스터의 스테이터스
    public float curHP;     // 현재 체력
    public float maxHP;     // 최대 체력
    public float ATK;  // 공격력
    public float DEF;       // 방어력
    public float MoveSpeed;  // 이동 속도
    public float Damage;   // 받은 피해량
    public string WeakProperty; // 약점 속성

    public float TickCoolTime;  // 틱 피해량 쿨타임

    public GameObject DamageText; //맞았을 때 나오는 데미지 텍스트
    public GameObject MonsterCanvas;
    public Vector3 hpBarPosition;

    public SkinnedMeshRenderer matObj;
    public GameObject targetObj;

    public Transform PlayerTr;     // 플레이어 Transform
    public float Distance;         // 플레이어와 몬스터 간의 거리
    public float TraceRadius;  // 몬스터가 플레이어를 추적(Trace)하기 시작하는 거리
    public float attackRadius;  // 몬스터가 플레이어를 공격(Attack)하기 시작하는 거리
    public float AttackCoolTime;       // 몬스터의 공격 쿨타임
    public GameObject AttackCollider;  // 몬스터의 공격 콜라이더
    public GameObject Coin;     //몬스터를 죽이면 드랍되는 코인
    public bool isDie;     //몬스터 사망체크
    public bool isHit;     //몬스터 피격체크

    public GameObject IceHit; //몬스터 피격 이펙트(얼음)
    public GameObject FireHit; //몬스터 피격 이펙트(불)
    public GameObject AttackEffect; //몬스터 공격 이펙트
    public GameObject EffectGen; //몬스터 공격 이펙트 소환 장소

    public Transform desiredParent;

    protected Rigidbody rd; // 리지드바디
    #endregion

    public virtual void Awake()
    {
        rd = GetComponent<Rigidbody>();
        // 몬스터 기본 설정
        if (this.tag == "Monster_Melee")     // 이 몬스터가 근접 몬스터일때
        {
            AttackCollider.SetActive(false);    // 몬스터의 공격 콜라이더를 비활성화
        }
        SetHP(100000);                         // 몬스터의 기본 HP를 설정
        CheckHP();                          // 몬스터 HP바 설정
        anim = GetComponent<Animator>();    // 몬스터 애니메이터를 가져옴
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(FindPlayer());       // 플레이어를 찾는 코루틴 함수 실행
    }
    
    public virtual void Start()
    {
        PlayerTr = this.transform;
        StartCoroutine(FindPlayer());
    }

    public virtual void Update()
    {
        rd.AddForce(Vector3.down * 4, ForceMode.VelocityChange);

        if (!isDie)     // 죽어있는 상태가 아니면
        {
            DistanceCheck();    // 플레이어와의 거리를 계산
        }
        AttackCoolTime += Time.deltaTime;
        TickCoolTime += Time.deltaTime;
        if(PlayerTr != null)
        {
            Turn();
        }
    }

    #region 몬스터 HP 설정하는 부분
    public virtual void SetHP(float amount) // HP 설정
    {
        maxHP = amount;
        curHP = maxHP;
    }

    public virtual void CheckHP() // HP 체크
    {
        if (HpBar != null)
            HpBar.value = curHP / maxHP;
    }
    #endregion

    #region 플레이어 찾기, 플레이어와 거리 체크, 추적, 공격 함수, HP바 위치 설정
    public virtual IEnumerator FindPlayer()     // 플레이어를 찾아서 할당해주는 함수
    {
        yield return new WaitForSeconds(0.1f);
        PlayerTr = GameObject.FindWithTag("Player").transform;
    }

    public virtual void DistanceCheck()
    {
        Distance = Vector3.Distance(transform.position, PlayerTr.position);

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit)
        {
            StartCoroutine(Trace());
        }

        if (Distance <= attackRadius && AttackCoolTime >= 3.0f && !isDie && !isHit)
        {
            StartCoroutine(Attack());
        }
    }
    public virtual void Turn()
    {
        desiredParent = this.transform.parent.parent;
        Vector3 localPosition = desiredParent.InverseTransformPoint(transform.position);
        Vector3 playerLocalPosition = desiredParent.InverseTransformPoint(PlayerTr.position);

        if (localPosition.x < playerLocalPosition.x)
        {
            this.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (localPosition.x > playerLocalPosition.x)
        {
            this.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public virtual IEnumerator Trace()
    {
        // 플레이어를 향해 이동하는 로직
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement = new Vector3(directionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        hpBarPosition = GetHPBarPosition(); // 몬스터의 상단으로 설정
        HpBar.transform.position = hpBarPosition;

        yield return null;
    }

    public virtual Vector3 GetHPBarPosition()
    {
        return transform.position + Vector3.up * 3.5f;
    }
    #endregion

    #region 몬스터의 공격

    public virtual IEnumerator Attack()
    {
        AttackCoolTime = 0;
        AttackCollider.SetActive(true);
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);
        AttackCollider.SetActive(false);
        AttackCoolTime = 0;
    }

    public virtual void Attack_On()
    {
        if(EffectGen != null && AttackEffect != null)
        {
            GameObject effect_on = Instantiate(AttackEffect, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
        }
    }

    #endregion

    #region 몬스터 피격, 피해량 공식
    public virtual void OnTriggerEnter(Collider col)
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

    public virtual void OnTriggerStay(Collider col)
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

    public virtual IEnumerator TakeDamage()
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
            if(PlayerPrefs.GetString("property") == WeakProperty)
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

            StartCoroutine(DamageTextAlpha());

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
            Destroy(this.gameObject); // ü���� 0 ���϶� ����
            Destroy(HpBar.gameObject);
        }
    }
    #endregion

    #region 몬스터 피격 텍스트
    public virtual IEnumerator DamageTextAlpha()
    {
        if(anim.GetBool("Die") == false)
        {   
            //데미지 텍스트 출력 부분(05.31)
            GameObject instText = Instantiate(DamageText);
            instText.transform.SetParent(MonsterCanvas.transform, false);
            instText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + 0.5f, HpBar.transform.position.z); 
            instText.GetComponent<TMP_Text>().text = (Damage * (1 / (1 + DEF * 0.01f))).ToString("F0"); //소수점 날리고 데미지 표현
            float time = 0f;
            instText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
            Color fadecolor = instText.GetComponent<TMP_Text>().color;
            yield return new WaitForSeconds(0.15f);
            while(fadecolor.a >= 0)
            {
                time += Time.deltaTime;
                fadecolor.a = Mathf.Lerp(1, 0, time);
                instText.GetComponent<TMP_Text>().color = fadecolor; // 페이드 되면서 사라짐
                instText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + time + 0.1f, HpBar.transform.position.z); // 서서히 올라감
                yield return null;
            }
        }
    }
    #endregion
}
