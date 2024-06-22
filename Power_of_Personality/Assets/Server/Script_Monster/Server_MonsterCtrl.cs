using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Server_MonsterCtrl : MonoBehaviour
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

    public float TickCoolTime;

    public GameObject DamageText; //맞았을 때 나오는 데미지 텍스트
    public GameObject MonsterCanvas;
    public Vector3 hpBarPosition;

    public SkinnedMeshRenderer matObj;
    public GameObject targetObj;
    public GameObject[] Targets;    // 모든 플레이어 오브젝트 배열
    public Transform PlayerTr;     // 플레이어 Transform
    public float PlayerDistance;        // 타겟 플레이어와 몬스터 간의 거리
    public float TempDistance;         // 타겟 플레이어와 몬스터 간의 거리 (저장용)
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
    private PhotonView photonview; //포톤뷰 (멀티)
    private string CurProperty;     

    #endregion

    public virtual void Awake()
    {

        photonview = GetComponent<PhotonView>();
        // 몬스터 기본 설정
        if (this.tag == "Monster_Melee")     // 이 몬스터가 근접 몬스터일때
        {
            AttackCollider.SetActive(false);    // 몬스터의 공격 콜라이더를 비활성화
        }
        SetHP(1000000);                         // 몬스터의 기본 HP를 설정
        CheckHP();                          // 몬스터 HP바 설정
        anim = GetComponent<Animator>();    // 몬스터 애니메이터를 가져옴
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        PlayerTr = this.transform;          // 플레이어 Transform을 설정하기 전에 임시로 몬스터(스크립트가 들어있는 게임 오브젝트)의 Transform을 담아놓음.
        TempDistance = -4f;                 // 플레이어 거리 측정 전 임시 수치
        if (PhotonNetwork.IsMasterClient){
            StartCoroutine(FindPlayer());       // 플레이어를 찾는 코루틴 함수 실행
        }
        
    }

    public virtual void Update()
    {
        HpBar.transform.position = hpBarPosition;
        hpBarPosition = GetHPBarPosition();
        CurProperty = PlayerPrefs.GetString("property");
        if (PhotonNetwork.IsMasterClient){
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
            TickCoolTime += Time.deltaTime;
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
        Targets = GameObject.FindGameObjectsWithTag("Target");
        Settarget();
        //PlayerTr = GameObject.FindWithTag("Player").transform;
    }

    public virtual void DistanceCheck()
    {
        Distance = Vector3.Distance(transform.position, PlayerTr.position);

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit)
        {
            StartCoroutine(Trace());
        }
        else if (Distance > TraceRadius){ 
            Settarget();
        }

        if (Distance <= attackRadius && AttackCoolTime >= 3.0f && !isDie && !isHit)
        {
            photonview.RPC("Server_Attack", RpcTarget.All);
            //StartCoroutine(Attack());
        }
    }
    public virtual void Settarget()
    {
        TempDistance = -4f;

        foreach (GameObject Player in Targets){
            Transform TargetTr = Player.transform.parent.gameObject.transform;
            PlayerDistance = Vector3.Distance(transform.position, TargetTr.position);
            if (PlayerDistance < TempDistance || TempDistance < 1f){
                TempDistance = PlayerDistance;
                PlayerTr = TargetTr;
            }   
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
    //photonview.RPC("Attack", RpcTarget.All);
    [PunRPC]
    public virtual void Server_Attack(){
        StartCoroutine(Attack());
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
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 2f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "WarriorSkillE")
        {
            isHit = true;
            Damage = Status.TotalAP * 4f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        #endregion
        #region 도적
        if (col.tag == "RougeAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillQ_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillW_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 1.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        if (col.tag == "RougeSkillE_4")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "ArcherAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "ArcherSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 0.1f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillW")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP * 2.5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 5f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
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
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
        }
        #endregion
        #region 도적
        if (col.tag == "RougeSkillQ_1" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.3f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }

        if (col.tag == "RougeSkillW_1" && TickCoolTime >= 0.3f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.4f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        if (col.tag == "RougeSkillE_3" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            //StartCoroutine(TakeDamage());
            TickCoolTime = 0;
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherSkillW" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        if (col.tag == "ArcherSkillE" && TickCoolTime >= 0.2f)
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardSkillQ" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
    }

    public virtual IEnumerator TakeDamage(float CurDamage, string Property)
    {
        if (maxHP != 0 || curHP > 0)
        {
            if (Property == "Ice")
            {
                Instantiate(IceHit, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            if (Property == "Fire")
            {
                Instantiate(FireHit, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            if(Property == WeakProperty)
            {
                CurDamage = CurDamage * 1.5f;
            }
            Material[] materials = matObj.materials;
            curHP -= CurDamage * (1 / (1 + DEF * 0.01f));
            CheckHP(); // ü�� ����
            anim.SetBool("TakeDamage", true);
            foreach (Material material in materials)
            {
                material.color = Color.red;
            }

            StartCoroutine(DamageTextAlpha(CurDamage));

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
            //PhotonNetwork.Destroy(this.gameObject);
            this.gameObject.transform.parent.tag="Untagged";
        }
    }
    #endregion

    #region 몬스터 피격 텍스트
    public virtual IEnumerator DamageTextAlpha(float CurDamage)
    {
        if(anim.GetBool("Die") == false)
        {   
            //데미지 텍스트 출력 부분(05.31)
            GameObject instText = Instantiate(DamageText);
            instText.transform.SetParent(MonsterCanvas.transform, false);
            instText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + 0.5f, HpBar.transform.position.z); 
            instText.GetComponent<TMP_Text>().text = (CurDamage * (1 / (1 + DEF * 0.01f))).ToString("F0"); //소수점 날리고 데미지 표현
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

    [PunRPC]
    public virtual void RPCTakeDamage(float CurDamage, string Property){
        StartCoroutine(TakeDamage(CurDamage,Property));
    }
}