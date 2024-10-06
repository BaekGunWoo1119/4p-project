using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

public class Server_BossCtrl : MonoBehaviourPun, IPunObservable
{
    #region 변수 선언
    protected Animator anim;   // 애니메이터
    public UnityEngine.UI.Slider HpBar;    // HP바 슬라이더
    public SkinnedMeshRenderer matObj;
    public GameObject targetObj;
    protected GameObject BossWall1;
    protected GameObject BossWall2;
    protected BoxCollider BossWall1Collider;
    protected BoxCollider BossWall2Collider;

    // 보스의 스테이터스
    protected float curHP;     // 현재 체력
    protected float maxHP;     // 최대 체력
    protected float ATK;  // 공격력
    protected float DEF;       // 방어력
    protected float MoveSpeed;  // 이동 속도
    protected float Damage;   // 받은 피해량
    protected string WeakProperty; // 약점 속성

    // 보스 공격 관련
    protected GameObject EffectGen;
    protected GameObject SkillCollider;
    protected GameObject SkillEffect;
    protected float SkillYRot;
    // 공격 사운드(10.03)
    protected AudioSource[] atkAudio;

    // 보스 피격 관련
    public GameObject IceHit; //몬스터 피격 이펙트(얼음)
    public GameObject FireHit; //몬스터 피격 이펙트(불)
    public GameObject DamageText; //맞았을 때 나오는 데미지 텍스트
    public GameObject MonsterCanvas;
    public GameObject Coin;     //몬스터를 죽이면 드랍되는 코인

    // 플레이어 추적 관련
    public Transform PlayerTr;     // 플레이어 Transform
    protected float Distance;         // 플레이어와 몬스터 간의 거리
    protected float TraceRadius;  // 몬스터가 플레이어를 추적(Trace)하기 시작하는 거리
    protected float attackRadius;  // 몬스터가 플레이어를 공격(Attack)하기 시작하는 거리

    // 보스 상태 체크
    protected bool isDie;
    protected bool isHit;
    protected bool isAttacking;
    protected float TickCoolTime;  // 틱 피해량 쿨타임
    protected bool canTeleport;

    //상점 포탈
    protected GameObject shopPortal;
    public Transform desiredParent;
    protected PhotonView photonview;
    protected string CurProperty;
    protected Vector3 receivePos;
    protected Quaternion receiveRot;
    public GameObject[] Targets;    // 모든 플레이어 오브젝트 배열
    public GameObject CurTarget;
    public float TempDistance;         // 타겟 플레이어와 몬스터 간의 거리 (저장용)
    public float PlayerDistance;        // 타겟 플레이어와 몬스터 간의 거리
    protected AudioSource hitAudio; //몬스터 히트 사운드 추가(09.30)


    #endregion
    protected virtual void Awake()
    {
        photonview = GetComponent<PhotonView>();
        PlayerTr = this.transform;
        anim = GetComponent<Animator>();
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        EffectGen = transform.Find("EffectGen-Boss").gameObject;
        SkillYRot = transform.eulerAngles.y;
        HpBar = GameObject.Find("HPBar-Boss").GetComponent<UnityEngine.UI.Slider>();//보스 체력바 코드(06.29)
        MonsterCanvas = GameObject.Find("Canvas_Boss");
        TempDistance = -4f;                 // 플레이어 거리 측정 전 임시 수치
        //shopPortal = GameObject.Find("Normal_Shop_Portal");
        StartCoroutine(FindPlayer());
    }

    protected virtual void Start()
    {
        if(photonview.IsMine){
            StartCoroutine(FindPlayer());
        }
        // 보스 문 할당
        // BossWall1 = GameObject.Find("BossWall1").gameObject;
        // BossWall1Collider = BossWall1.GetComponent<BoxCollider>();
        // BossWall2 = GameObject.Find("BossWall2").gameObject;
        // BossWall2Collider = BossWall2.GetComponent<BoxCollider>();
        SetHP(100);
        CheckHP();
        GameObject.Find("Boss_HP_Bar").transform.localScale = new Vector3(1, 1, 1);

        // 공격 사운드 할당(10.03)
        atkAudio = transform.Find("Boss_AtkSound").GetComponents<AudioSource>();
        hitAudio = transform.Find("Boss_HitSound").GetComponent<AudioSource>();

        for(int i = 0; i < atkAudio.Length; i++)
        {
            atkAudio[i].Stop();
        }
    }

    protected virtual void Update()
    {
        if(photonview == null){
            photonview = GetComponent<PhotonView>();
        }
        CurProperty = PlayerPrefs.GetString("property");
        TickCoolTime += Time.deltaTime;
        TeleportCheck();
        if(photonview.IsMine){
            if (!isAttacking)
            {
                Turn();
            }
        }
        if(PhotonNetwork.IsMasterClient){}
        else{
            Debug.Log("이거 움직이긴 해요"+gameObject.transform.position +"ㅁㅇㅁㅇㅁㅇㅇ"+receivePos);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, receivePos, 2.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation,receiveRot, 2.0f * Time.deltaTime);
        }
        // 레이캐스트의 시작점
        Vector3 rayOrigin = transform.position + new Vector3(0, 1.0f, 0);
        // 레이캐스트의 방향 (여기서는 transform.forward의 반대 방향으로 설정)
        Vector3 rayDirection = -transform.forward;
        // 레이의 최대 길이
        float rayLength = 4f;

        // 레이캐스트를 실행하여 결과를 저장
        bool canTeleport = Physics.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Wall"));
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);

        // 시각화를 위해 Debug.DrawRay를 사용하여 레이를 그림
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, canTeleport ? Color.green : Color.red);
        CheckHP(); // HP 체크

    }
    #region 이동 관련
    public virtual void Turn()
    {
        if(CurTarget != null)
        {
            if (this.transform.position.x - PlayerTr.transform.position.x < 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (this.transform.position.x - PlayerTr.transform.position.x > 0)
            {
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else{
            photonview.RPC("RPCSetTarget", RpcTarget.All);
        }
    }
    protected virtual void TeleportCheck()
    {
        canTeleport = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), -transform.forward, 4f, LayerMask.GetMask("Wall"));
        //Debug.Log(canTeleport);
    }
    #endregion

    #region HP 관련
    protected virtual void SetHP(float amount) // HP 설정
    {
        maxHP = amount;
        curHP = maxHP;
    }

    public virtual void CheckHP() // HP 바 갱신
    {
        if (HpBar != null)
            HpBar.value = curHP / maxHP;
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
        }
        if (col.tag == "WarriorAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WarriorAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }

        if (col.tag == "WarriorSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 2f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WarriorSkillE")
        {
            isHit = true;
            Damage = Status.TotalAP * 4f;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        #endregion
        #region 도적
        if (col.tag == "RougeDashAttack")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeSkillQ_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeSkillW_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 1.5f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "RougeSkillE_4")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "ArcherAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "ArcherSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 0.1f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
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
        if (col.tag == "WizardAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillW")
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillE_1")
        {
            isHit = true;
            Damage = Status.TotalAP * 2.5f;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
        }
        if (col.tag == "WizardSkillE_2")
        {
            isHit = true;
            Damage = Status.TotalAP * 5f;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
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
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
        #region 도적
        if (col.tag == "RougeSkillQ_1" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.3f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }

        if (col.tag == "RougeSkillW_1" && TickCoolTime >= 0.3f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.4f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        if (col.tag == "RougeSkillE_3" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherSkillW" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        if (col.tag == "ArcherSkillE" && TickCoolTime >= 0.2f)
        {
            isHit = true;
            Damage = Status.TotalAP * 3f;
            if (Status.set6_4_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardSkillQ" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.75f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            photonview.RPC("RPCTakeDamage", RpcTarget.All, Damage ,CurProperty);
            TickCoolTime = 0;
        }
        #endregion
    }

    public virtual IEnumerator TakeDamage(float Damage, string Property)
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
                #region 3/4번 세트 3셋 효과
                if (WeakProperty == "Fire" && Status.set3_3_Activated)
                {
                    Damage = (Damage * 1.5f) * 1.2f;
                    Debug.Log("20% 추가한 약점 데미지는 : " + Damage);
                }
                if (WeakProperty == "Ice" && Status.set4_3_Activated)
                {
                    Damage = (Damage * 1.5f) * 1.2f;
                    Debug.Log("20% 추가한 약점 데미지는 : " + Damage);
                }
                #endregion
                else if(Status.set3_3_Activated == false && Status.set4_3_Activated == false)
                {
                    Debug.Log("약공터짐!!");    
                    Damage = Damage * 1.5f;
                }
            }
            Material[] materials = matObj.materials;
            #region 1번 세트 3셋효과
            int randomInt = Random.Range(1, 6);
            Debug.Log(randomInt);
            if (randomInt == 5 && Status.set1_3_Activated == true)
            {
                Debug.Log("20%!!");
                Damage = Damage * 1.2f;
            }
            #endregion
            #region 1번 세트 4셋 효과
            if(Status.set1_4_Activated == true)
            {
                Debug.Log("힐 하기 전 HP:" + Status.HP);

                if(Status.HP + (Damage * 0.2f) > Status.MaxHP)
                {
                    Status.HP = Status.MaxHP;
                }
                else
                {
                    Status.HP += (Damage * 0.2f);
                }
                Debug.Log("힐 하고 나서 HP:" + Status.HP);
            }
            #endregion
            #region 7번 세트 3셋 효과
            if (Status.set7_3_Activated == true)
            {
                Damage = Damage * 1.2f;
            }
            #endregion
            Damage = Damage * (1 / (1 + DEF * 0.01f));
            Debug.Log("몬스터가 입은 피해량 = " + Damage);
            anim.SetBool("TakeDamage", true);
            foreach (Material material in materials)
            {
                material.color = Color.red;
            }
            if(photonview.IsMine){
            photonview.RPC("RPCDamage", RpcTarget.All, Damage);
            }
            hitAudio.PlayOneShot(hitAudio.clip); //히트 시 재생 오디오 재생(09.30)
            CheckHP(); // HP 체크
            yield return new WaitForSeconds(0.1f);
            anim.SetBool("TakeDamage", false);
            yield return new WaitForSeconds(0.5f);
            if (anim.GetBool("TakeDamage") == false)
            {
                isHit = false;
            }
            foreach (Material material in materials)
            {
                material.color = Color.white;
            }
        }

        if (curHP <= 0) // 개체의 피가 0이 되었을 때 사망처리
        {
            isDie = true;
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            Vector3 CoinPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            Instantiate(Coin, CoinPosition, gameObject.transform.rotation);
            //상점 생성
            this.gameObject.transform.parent.tag="Untagged";
            GameObject.Find("Boss_HP_Bar").transform.localScale = new Vector3(0, 0, 0);
            Destroy(this.transform.parent.gameObject); // 개체 파괴
        }
    }
    #endregion

    #region 보스 패턴 관련
    protected virtual IEnumerator FindPlayer()     // 플레이어를 찾아서 할당해주는 함수
    {
        yield return new WaitForSeconds(0.1f);
        Targets = GameObject.FindGameObjectsWithTag("Target");
        photonview.RPC("RPCSetTarget", RpcTarget.All);
        DistanceCheck();
    }

    public virtual void Settarget()
    {
        Targets = GameObject.FindGameObjectsWithTag("Target");
        TempDistance = -4f;

        foreach (GameObject Player in Targets){
            Transform TargetTr = Player.transform.parent.gameObject.transform;
            PlayerDistance = Vector3.Distance(transform.position, TargetTr.position);
            if (PlayerDistance < TempDistance || TempDistance < 1f){
                TempDistance = PlayerDistance;
                CurTarget = Player;
                PlayerTr = TargetTr;
            }   
        }
    }
    protected virtual void DistanceCheck()
    {
        Distance = Vector3.Distance(transform.position, PlayerTr.position);
        //여기서부턴 세부 구현, 각 스크립트에서 보스 패턴에 맞게 구현
    }

    protected virtual IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0, 6);

        switch (ranAction)
        {
            case 0: // 근접 약공
                StartCoroutine(MeleeWeakAttack());
                break;
            case 1: // 근접 강공
                StartCoroutine(MeleeStrongAttack());
                break;
            case 2: // 원거리 약공
                StartCoroutine(RangedWeakAttack());
                break;
            case 3: // 원거리 강공
                StartCoroutine(RangedStrongAttack());
                break;
            case 4: // 스킬 1
                StartCoroutine(Skill_1());
                break;
            case 5: // 스킬 2
                StartCoroutine(Skill_2());
                break;
        }
    }

    // 공격 애니메이션 && 콜라이더 스크립트
    protected virtual IEnumerator MeleeWeakAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator MeleeStrongAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator RangedWeakAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator RangedStrongAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator Skill_1()
    {
        yield return null;
    }

    protected virtual IEnumerator Skill_2()
    {
        yield return null;
    }
    #endregion

    #region 몬스터 피격 텍스트
    public virtual IEnumerator DamageTextAlpha(float Damage)
    {
        if (anim.GetBool("Die") == false)
        {
            //데미지 텍스트 출력 부분(05.31)
            GameObject instText = Instantiate(DamageText);
            instText.transform.SetParent(MonsterCanvas.transform, false);
            instText.transform.position = new Vector3(MonsterCanvas.transform.position.x, MonsterCanvas.transform.position.y + 0.5f, MonsterCanvas.transform.position.z);
            instText.GetComponent<TMP_Text>().text = Damage.ToString("F0"); //소수점 날리고 데미지 표현
            float time = 0f;
            instText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
            Color fadecolor = instText.GetComponent<TMP_Text>().color;
            yield return new WaitForSeconds(0.15f);
            while (fadecolor.a >= 0)
            {
                time += Time.deltaTime;
                fadecolor.a = Mathf.Lerp(1, 0, time);
                instText.GetComponent<TMP_Text>().color = fadecolor; // 페이드 되면서 사라짐
                instText.transform.position = new Vector3(MonsterCanvas.transform.position.x, MonsterCanvas.transform.position.y + time + 0.1f, MonsterCanvas.transform.position.z); // 서서히 올라감
                //데미지 텍스트 사라지게 해둠(08.30)
                if(fadecolor.a == 0)
                {
                    Destroy(instText);
                    yield break;
                }
                yield return null;
            }
        }
    }
    #endregion

    [PunRPC]
    public virtual void RPCTakeDamage(float CurDamage, string Property){
        StartCoroutine(TakeDamage(CurDamage,Property));
    }

    [PunRPC]
    public virtual void RPCDamage(float CurDamage){
        curHP -= CurDamage;
        StartCoroutine(DamageTextAlpha(CurDamage));
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        Debug.Log("dasdasd");
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public virtual void RPCSetTarget(){
        Settarget();
    }
}
