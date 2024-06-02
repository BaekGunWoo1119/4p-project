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
    public float FireATTDEF;     // 화 속성 방어력
    public float IceATTDEF;      // 빙 속성 방어력
    public float Damage;   // 받은 피해량


    public string WeakProperty;

    public GameObject DamageText; //맞았을 때 나오는 데미지 텍스트
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

    public GameObject IceHit;
    public GameObject FireHit;
     

    #endregion

    public virtual void Awake()
    {
        // 몬스터 기본 설정
        if (this.tag == "Monster_Melee")     // 이 몬스터가 근접 몬스터일때
        {
            AttackCollider.SetActive(false);    // 몬스터의 공격 콜라이더를 비활성화
        }
        SetHP(100);                         // 몬스터의 기본 HP를 설정
        CheckHP();                          // 몬스터 HP바 설정
        anim = GetComponent<Animator>();    // 몬스터 애니메이터를 가져옴
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        PlayerTr = this.transform;          // 플레이어 Transform을 설정하기 전에 임시로 몬스터(스크립트가 들어있는 게임 오브젝트)의 Transform을 담아놓음.
        StartCoroutine(FindPlayer());       // 플레이어를 찾는 코루틴 함수 실행
        DamageText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0);
    }

    public virtual void Update()
    {
        if (!isDie)     // 죽어있는 상태가 아니면
        {
            DistanceCheck();    // 플레이어와의 거리를 계산
        }
        AttackCoolTime += Time.deltaTime;
        if (this.transform.position.x - PlayerTr.transform.position.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (this.transform.position.x - PlayerTr.transform.position.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
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

    public virtual Vector3 GetHPBarPosition()
    {
        return transform.position + Vector3.up * 3.5f;
    }
    #endregion

    #region 몬스터 피격, 피해량 공식
    public virtual void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WarriorAttack1")
        {
            isHit = true;
            Damage = 10;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillQ")
        {
            isHit = true;
            Damage = 20;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillW")
        {
            isHit = true;
            Damage = 20;
            StartCoroutine(TakeDamage());
        }
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
            DamageText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + 0.5f, HpBar.transform.position.z); 
            DamageText.GetComponent<TMP_Text>().text = (Damage * (1 / (1 + DEF * 0.01f))).ToString("F0"); //소수점 날리고 데미지 표현
            float time = 0f;
            DamageText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
            Color fadecolor = DamageText.GetComponent<TMP_Text>().color;
            yield return new WaitForSeconds(0.15f);
            while(fadecolor.a >= 0)
            {
                time += Time.deltaTime;
                fadecolor.a = Mathf.Lerp(1, 0, time);
                DamageText.GetComponent<TMP_Text>().color = fadecolor; // 페이드 되면서 사라짐
                DamageText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + time + 0.1f, HpBar.transform.position.z); // 서서히 올라감
                yield return null;
            }
        }
    }
    #endregion
}
