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
    public float hpMul;     // 체력 배율
    public float ATK;  // 공격력
    public float atkMul;    // 공격력 배율
    public float DEF;       // 방어력
    public float MoveSpeed;  // 이동 속도
    public float Damage;   // 받은 피해량
    public string ownWeakProperty; // 자신의 원래 약점 속성
    public string WeakProperty; // 약점 속성

    public float TickCoolTime;  // 틱 피해량 쿨타임

    public GameObject DamageText; //맞았을 때 나오는 데미지 텍스트
    public GameObject MonsterCanvas;
    public float CanvasYRot = 0f;
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
    public bool isStun;  //스턴상태인지
    public bool isSpawn; //스폰상태인지
    public float hitCount;  //몬스터가 맞았을 때, 시간을 늘려서 몬스터가 피격 상태인지 체크하기 위한 변수

    public GameObject IceHit; //몬스터 피격 이펙트(얼음)
    public GameObject FireHit; //몬스터 피격 이펙트(불)
    public GameObject AttackEffect; //몬스터 공격 이펙트
    public GameObject EffectGen; //몬스터 공격 이펙트 소환 장소
    public float AnimSpeed = 1.0f; //보조스킬 시간 감속용 변수 (09.18 정도훈)

    public GameObject Stun_Effect_1; //스턴 이펙트 생성용
    public GameObject Stun_Effect_2; //스턴 이펙트 On/Off용

    protected AudioSource hitAudio; //몬스터 히트 사운드 추가(09.30)
    protected AudioSource atkAudio; //몬스터 공격 사운드 추가(09.30)

    public Transform desiredParent;

    protected Rigidbody rd; // 리지드바디 
    public int bonusstat;
    #endregion

    public virtual void Awake()
    {
        hpMul = PlayerPrefs.GetFloat("hpMul", 100);
        atkMul = PlayerPrefs.GetFloat("atkMul", 100);
        ATK = ATK*atkMul/100;
        Debug.Log(ATK);
        rd = GetComponent<Rigidbody>();
        // 몬스터 기본 설정
        if (this.tag == "Monster_Melee")     // 이 몬스터가 근접 몬스터일때
        {
            AttackCollider.SetActive(false);    // 몬스터의 공격 콜라이더를 비활성화
        }
        
        SetHP(maxHP);                         // 몬스터의 기본 HP를 설정
        CheckHP();                          // 몬스터 HP바 설정
        anim = GetComponent<Animator>();    // 몬스터 애니메이터를 가져옴
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(FindPlayer());       // 플레이어를 찾는 코루틴 함수 실행
        SetWeakProperty();  //약점 속성 설정

        StartCoroutine(Spawn_State());

        bonusstat = PlayerPrefs.GetInt("Drop");
    }
    
    public virtual void Start()
    {
        PlayerTr = this.transform;
        StartCoroutine(FindPlayer());
        hitAudio = transform.parent.Find("Monster_Hitsound").GetComponent<AudioSource>(); //히트 시 재생 오디오 지정(09.30)
        hitAudio.Stop(); //처음에는 소리 나오지 않게(09.30)
        atkAudio = transform.parent.Find("Monster_Atksound").GetComponent<AudioSource>(); //공격 시 재생 오디오 지정(09.30)
        atkAudio.Stop(); //처음에는 소리 나오지 않게(09.30)
    }

    public virtual void Update()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0f);
        if(Status.Spell_TimeSlowdown_ON){
            AnimSpeed = 0.3f;
            anim.SetFloat("AnimSpeed", AnimSpeed);
        }
        else{
            AnimSpeed = 1.0f;
            anim.SetFloat("AnimSpeed", AnimSpeed);
        }
        hpBarPosition = GetHPBarPosition(); // 몬스터의 상단으로 설정
        HpBar.transform.position = hpBarPosition;
        rd.AddForce(Vector3.down * 4, ForceMode.VelocityChange);
        if (!isDie &&!isStun)     // 죽어있는 상태가 아니면
        {
            DistanceCheck();    // 플레이어와의 거리를 계산
        }
        AttackCoolTime += Time.deltaTime;
        TickCoolTime += Time.deltaTime;
        hitCount -= Time.deltaTime;
        if(PlayerTr != null && !isSpawn && !anim.GetBool("isAttack"))
        {
            Turn();
        }
        #region 3/4세트 4세트 효과
        if (Status.set3_4_Activated && WeakProperty != "Fire")
        {
            WeakProperty = "Fire";
            //Debug.Log("약점속성이 화속성이 됨.");
        }

        if (Status.set4_4_Activated && WeakProperty != "Ice")
        {
            WeakProperty = "Ice";
            //Debug.Log("약점속성이 빙속성이 됨.");
        }
        #endregion

        //캔버스 뒤집어지는 오류 해결(08.29)
        if(GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y > 0 && GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y < 180)
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, CanvasYRot, 0);
        else
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, CanvasYRot + 180f, 0);
    }

    #region 몬스터 초기 값 설정
    public virtual void SetHP(float amount) // HP 설정
    {
        Debug.Log("체력 배율 곱하기 전 몬스터의 체력 = " + amount);
        maxHP = amount * (hpMul / 100);
        Debug.Log("현재 몬스터의 체력 배율 = " + hpMul);
        Debug.Log("체력 배율 곱한 후의 몬스터의 체력 = " + maxHP);
        curHP = maxHP;
    }

    public virtual void CheckHP() // HP 체크
    {
        if (HpBar != null)
            HpBar.value = curHP / maxHP;
    }

    public virtual void SetWeakProperty()
    {
        if(Status.set3_4_Activated == true)
        {
            Debug.Log("화속성 약점으로 설정");
            WeakProperty = "Fire";
        }
        else if(Status.set4_4_Activated == true)
        {
            Debug.Log("빙속성 약점으로 설정");
            WeakProperty = "Ice";
        }
        else
        {
            WeakProperty = ownWeakProperty;
        }
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

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit && !isSpawn && !anim.GetBool("isAttack"))
        {
            StartCoroutine(Trace());
        }

        if (Distance <= attackRadius && AttackCoolTime >= 3.0f*(1f/AnimSpeed) && !isDie && hitCount <= 0 && !isSpawn)
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
        desiredParent = this.transform.parent.parent;
        Vector3 localPosition = desiredParent.InverseTransformPoint(transform.position);
        Vector3 playerLocalPosition = desiredParent.InverseTransformPoint(PlayerTr.position);
        //Debug.Log(localPosition.z - playerLocalPosition.z);

        // 플레이어를 향해 이동하는 로직
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement;
        if(localPosition.z - playerLocalPosition.z > -1 && localPosition.z - playerLocalPosition.z < 1){
            movement = new Vector3(directionToPlayer.x, 0, directionToPlayer.z) * MoveSpeed * Time.deltaTime * AnimSpeed;
            transform.parent.Translate(movement, Space.World);
        }
        yield return null;
    }

    public virtual Vector3 GetHPBarPosition()
    {
        return transform.position + Vector3.up * 3.5f;
    }

    public virtual IEnumerator Spawn_State()
    {
        this.gameObject.layer = 10;
        isSpawn =true;
        yield return new WaitForSeconds(2f);
        this.gameObject.layer = 6;
        isSpawn =false;
    }

    #endregion

    #region 몬스터의 공격

    public virtual IEnumerator Attack()
    {
        AttackCoolTime = 0;
        anim.SetBool("isAttack", true);
        yield return null;
    }

    public virtual void Attack_On()
    {
        if(EffectGen != null && AttackEffect != null)
        {
            AttackCollider.SetActive(false);
            AttackCollider.SetActive(true);
            atkAudio.PlayOneShot(atkAudio.clip); //공격 시 재생 오디오 재생(09.30)
            GameObject effect_on = Instantiate(AttackEffect, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
            StartCoroutine(Attack_On_2());
        }
    }

    public virtual IEnumerator Attack_On_2(){
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);
        AttackCollider.SetActive(false);
        AttackCoolTime = 0;
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
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "WarriorAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "WarriorAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
        }
        #endregion
        #region 도적
        if (col.tag == "RougeDashAttack")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeSkillQ_2")
        {
            isHit = true;
            Damage = Status.TotalAP*2f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeSkillW_2")
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
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "RougeSkillE_4")
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
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "ArcherAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "ArcherSkillQ")
        {
            isHit = true;
            Damage = Status.TotalAP * 0.25f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardAttack1")
        {
            isHit = true;
            Damage = Status.TotalADC;
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "WizardAttack2")
        {
            isHit = true;
            Damage = Status.TotalADC;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "WizardAttack3")
        {
            isHit = true;
            Damage = Status.TotalADC * 1.5f;
            if (Status.set5_3_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
        }
        if (col.tag == "WizardSkillW")
        {
            isHit = true;
            Damage = Status.TotalAP * 4f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
        }
        #endregion
        #region 보조스킬
        if(col.tag == "Spell_Stun")
        {
            StartCoroutine(Stun());
        }
        #endregion
        //Debug.Log("충돌한 객체 태그 : "+col.tag);
    }

    public virtual void OnTriggerStay(Collider col)
    {
        #region 전사
        if (col.tag == "WarriorSkillW" && TickCoolTime >= 0.5f)
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
            StartCoroutine(TakeDamage(Damage));
            TickCoolTime = 0;
        }
        #endregion
        #region 도적
        if (col.tag == "RougeSkillQ_1" && TickCoolTime >= 0.25f)
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
            StartCoroutine(TakeDamage(Damage));
            TickCoolTime = 0;
        }

        if (col.tag == "RougeSkillW_1" && TickCoolTime >= 0.3f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.5f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
            TickCoolTime = 0;
        }
        #endregion
        #region 궁수
        if (col.tag == "ArcherSkillW" && TickCoolTime >= 0.25f)
        {
            isHit = true;
            Damage = Status.TotalAP * 0.35f;
            if (Status.set6_3_Activated)
            {
                Damage *= 1.2f;
            }
            if (Status.set7_4_Activated)
            {
                Damage *= 1.2f;
            }
            StartCoroutine(TakeDamage(Damage));
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
            StartCoroutine(TakeDamage(Damage));
            TickCoolTime = 0;
        }
        #endregion
        #region 마법사
        if (col.tag == "WizardSkillQ" && TickCoolTime >= 0.3f)
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
            StartCoroutine(TakeDamage(Damage));
            TickCoolTime = 0;
        }
        #endregion
    }

    public virtual IEnumerator TakeDamage(float Damage)
    {
        if (maxHP != 0 || curHP > 0)
        {
            hitCondition();
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

                if(Status.HP + (Damage * 0.05f) > Status.MaxHP)
                {
                    Status.HP = Status.MaxHP;
                }
                else
                {
                    Status.HP += (Damage * 0.05f);
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
            curHP -= Damage;
            hitAudio.PlayOneShot(hitAudio.clip); //히트 시 재생 오디오 재생(09.30)
            CheckHP(); // HP 체크
            anim.SetBool("TakeDamage", true);
            foreach (Material material in materials)
            {
                material.color = Color.red;
            }
            StartCoroutine(DamageTextAlpha(Damage));
           
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
            this.gameObject.layer = 10;
            this.gameObject.tag = "Untagged";
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            if(Random.Range(0, 10)> 5-bonusstat){
                Vector3 CoinPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f, gameObject.transform.position.z);
                Instantiate(Coin, CoinPosition, gameObject.transform.rotation);
            }
            Destroy(HpBar.gameObject);
            Destroy(this.gameObject); // 개체 파괴
        }
    }

    public virtual void hitCondition()
    {
        hitCount = 1f;
    }
    #endregion

    #region 몬스터 피격 텍스트
    public virtual IEnumerator DamageTextAlpha(float Damage)
    {
        if(anim.GetBool("Die") == false)
        {   
            //데미지 텍스트 출력 부분(05.31)
            GameObject instText = Instantiate(DamageText);
            instText.transform.SetParent(MonsterCanvas.transform, false);
            instText.transform.position = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y + 0.5f, HpBar.transform.position.z); 
            instText.GetComponent<TMP_Text>().text = Damage.ToString("F0"); //소수점 날리고 데미지 표현
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
    #region 보조스킬 스턴
    public virtual IEnumerator Stun()
    {
        isStun = true;
        GameObject StunEffect = Instantiate(Stun_Effect_1, this.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(StunEffect,2.0f);
        Stun_Effect_2.SetActive(true);
        yield return new WaitForSeconds(10f); //총 스턴 시간
        Stun_Effect_2.SetActive(false);
        isStun = false;
    }
    #endregion
}
