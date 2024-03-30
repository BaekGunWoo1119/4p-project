using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//전사 애니메이션
public class PlayerCtrl_Warrior : MonoBehaviour
{
    //GetAxis 값
    float hAxis;
    float vAxis;

    //Player의 Y Rotation 값
    float YRot;
    //Player의 Y Position 값
    float YPos;

    //Player의 transform 값
    private Transform trs;

    //inspecter 창에서 이동속도, 점프 세기, 중력 조절


    //애니메이션 컨트롤
    private Vector3 initPos;
    bool isSkill;
    bool isSkillQ;
    bool isAttack;
    bool isJumping;
    bool isJumpAttack;
    
    //코루틴 컨트롤
    bool coroutineMove = false;

    //제어 이상한 사운드 컨트롤
    bool isSound = false;

    Vector3 moveVec;
    Animator anim;
    Rigidbody rd;

    //캐릭터 공격 콜라이더
    private GameObject Attack_Collider_All;
    public GameObject QSkill_Collider;
    private GameObject EffectGen;
    private GameObject WSkill_Collider;
    private GameObject Attack_1_Collider;

    //사운드
    private AudioSource[] audioSources;

    //벽 충돌체크
    private bool WallCollision;

    //스킬 쿨타임
    public float QSkillCoolTime;
    public float WSkillCoolTime;
    public float ESkillCoolTime;

    //캐릭터 공격 이펙트
    public GameObject commonAttack_Ice1_Effect;
    public GameObject commonAttack_Ice2_Effect;
    public GameObject commonAttack_Fire1_Effect;
    public GameObject commonAttack_Fire2_Effect;
    public GameObject Skill_FireQ_Effect;
    public GameObject Skill_IceQ_Effect;
    public GameObject Skill_FireW_Effect;
    public GameObject Skill_IceW_Effect;
    private GameObject Attack1_Effect;
    private GameObject Attack2_Effect;
    private GameObject SkillQ_Effect;
    private GameObject SkillW_Effect;
    private GameObject SkillEffect;
    private float SkillYRot;

    //플레이어 스테이터스
    private float PlayerHP;     //HP
    private float maxHP;        //최대 체력
    private float Damage;       //받은 피해량
    private float PlayerATK;    //공격력
    private float PlayerDEF;    //방어력
    private float FireATT;      //불 속성 데미지 배율
    private float IceATT;       //얼음 속성 데미지 배율
    public float moveSpeed;     //이동속도
    private float moveSpd;      //이동속도
    public float JumpPower;     //점프력
    public float fallPower;     //떨어지는 힘

    //카메라
    private GameObject mainCamera;

    //Hp bar
    private Slider HpBar;
    private TMP_Text HpText;

    //CoolTime(03.18)
    private Image Qcool;
    private Image Wcool;
    private Image Ecool;

    private bool canTakeDamage = true; // 데미지를 가져올 수 있는지
    private float damageCooldown = 1.0f; // 1초마다 틱데미지를 가져오기 위함

    void Start()
    {
        //플레이어 스테이터스 초기화
        SetHp(100);
        PlayerATK = 100;
        PlayerDEF = 10;
        FireATT = 1.0f;
        IceATT = 1.0f;

        HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();
        HpText = GameObject.Find("StatPoint - Hp").GetComponent<TMP_Text>();
        HpText.text = "HP 100/100";

        //쿨타임 UI(03.18)
        Qcool = GameObject.Find("CoolTime-Q").GetComponent<Image>();
        Wcool = GameObject.Find("CoolTime-W").GetComponent<Image>();
        Ecool = GameObject.Find("CoolTime-E").GetComponent<Image>();

        anim = GetComponentInChildren<Animator>();
        rd = GetComponent<Rigidbody>();
        trs = GetComponentInChildren<Transform>();
        initPos = trs.position;
        mainCamera = GameObject.FindWithTag("MainCamera");
        anim.SetBool("isIdle", true);
        EffectGen = transform.Find("EffectGen").gameObject;
        //플레이어 어택 콜라이더 인식 방식 변경 (서버에 맞게) (03.16)
        Attack_Collider_All = transform.Find("AttackColliders").gameObject;
        WSkill_Collider = Attack_Collider_All.transform.Find("WSkill_Collider").gameObject;
        WSkill_Collider.SetActive(false);
        Attack_1_Collider = Attack_Collider_All.transform.Find("Attack_1_Collider").gameObject;
        Attack_1_Collider.SetActive(false);
        isSkill = false;
        isSkillQ = false;
        isAttack = false;
        isJumping = false;

        //사운드
        audioSources = GetComponents<AudioSource>();
        for(int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }

    void Update()
    {
        if (!canTakeDamage)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                canTakeDamage = true;
                damageCooldown = 1.0f;
            }
        }
        //스킬 쿨타임 충전
        QSkillCoolTime += Time.deltaTime;
        WSkillCoolTime += Time.deltaTime;
        ESkillCoolTime += Time.deltaTime;

        //스킬 쿨타임 UI(03.18)
        if(Qcool.fillAmount == 0)
        {
            
        }
        else
        {
            Qcool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
        }

        if(Wcool.fillAmount == 0)
        {
            
        }
        else
        {
            Wcool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
        }

        if(Ecool.fillAmount == 0)
        {
            
        }
        else
        {
            Ecool.fillAmount -= 1 * Time.smoothDeltaTime / 3;
        }

        //캐릭터 스킬 이펙트
        SkillYRot = transform.eulerAngles.y;
        if (PlayerPrefs.GetString("property") == "Fire")
        {
            Attack1_Effect = commonAttack_Fire1_Effect;
            Attack2_Effect = commonAttack_Fire2_Effect;
            SkillQ_Effect = Skill_FireQ_Effect;
            SkillW_Effect = Skill_FireW_Effect;
        }
        else if (PlayerPrefs.GetString("property") == "Ice")
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
        }
        else
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
        }

        //벽 충돌체크
        WallCheck();

        //애니메이션 업데이트
        GetInput();

        //Y 로테이션 고정 코드 & 캐릭터가 회전했는가 확인
        YRot = transform.eulerAngles.y;
        if((YRot < 100 && YRot > 80) || (YRot > -300 && YRot < -250))
        {
            YRot = 90;
        }
        if ((YRot < -80 && YRot > -100) || (YRot < 300 && YRot > 250))
        {
            YRot = -90;
        }
        Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);

        //Z 포지션 고정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // 오브젝트에 회전 적용
        transform.rotation = desiredRotation;

        //Move
        if (!isSkill && !isAttack && !anim.GetBool("isDie"))
        {
            Move();
            Move_anim();
        }

        //Turn
        if(!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.GetBool("isDie"))
        {
            Turn();
        }

        //Dodge
        if(Input.GetKeyDown(KeyCode.R))
        {
            Dodge();
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
        {
            anim.ResetTrigger("isDodge");
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            anim.SetBool("isJump", false);
        }

        //Attack
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isJumpAttack == true){
                anim.ResetTrigger("CommonAttack");
            }
            else{
                Attack_anim();
            }
        }

        //기본공격1 & 기본공격3 시 전진 애니메이션 & 공격사운드
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B") && !coroutineMove)
        {
            isSound = false;
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.4f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            //StartCoroutine(Attack_Sound(0, 0.8f));
            StartCoroutine(Delay(0.2f));
            mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.03f, null);
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2") && !coroutineMove && !isSound)
        {
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack_Sound(1, 0.8f));
            StartCoroutine(Delay(0.2f));
            mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.1f, 0.01f, null);
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4") && !coroutineMove)
        {
            StartCoroutine(Delay(0.2f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack_Sound(2, 0.8f));
            StartCoroutine(Delay(5.0f));
            mainCamera.GetComponent<CameraCtrl>().ShakeCamera(0.3f, 0.1f, null);
        }
        //점프공격 카메라 && 사운드
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !coroutineMove)
        {
            isSound = false;
            //audioSources[0].Stop();
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.5f, "null");
            StartCoroutine(Attack1_Collider());
            //StartCoroutine(Delay(0.4f));
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") && !isSound)
        {
            //audioSources[1].Play();
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.6f, "null");
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.2f));
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3") && !coroutineMove)
        {
            //audioSources[1].Stop();
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x, transform.position.y + 2, transform.position.z - 9, 0, 0.5f, "null");
            //audioSources[1].Play();
            //StartCoroutine(Delay(0.2f));
            StartCoroutine(Attack1_Collider());
            anim.ResetTrigger("CommonAttack");
        }
        //애니메이션이 끝나면 coroutine을 강제로 종료
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_3") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_7Combo_ALL")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3"))
        {
            
        }
        else
        {
            coroutineMove = false;
        }
        //점프공격 시 Y 포지션 고정
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !isJumpAttack)
        {
            Vector3 OriginPos = transform.position;
            YPos = OriginPos.y;
            isJumpAttack = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3"))
        {
            rd.velocity = Vector3.zero;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            //떨어지는 코드 추후 수정
            //rd.AddForce(Vector3.down * fallPower/3, ForceMode.VelocityChange);
        }
        //한 번 점프 시 한 번의 점프공격 콤보만 되게
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
        }

        //지상공격 2타, 3타 시 방향전환 되도록
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B_Wait") ||
           anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2_Wait"))
        {
            isAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4"))
        {
            isAttack = true;
        }

        //Skill_Q
        if(Input.GetKeyDown(KeyCode.Q) 
        && !isSkill 
        && !isJumping 
        && !anim.GetBool("isFall")
        && !isSkillQ
        && QSkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_Q();
        }

        //Skill_W
        if (Input.GetKeyDown(KeyCode.W)
        && !isSkill
        && !isJumping
        && !anim.GetBool("isFall")
        && WSkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_W();
        }

        if(Input.GetKeyDown(KeyCode.E) 
        && !isSkill 
        && !isJumping 
        && !anim.GetBool("isFall")
        && ESkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_E();
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isSkill && !isAttack && !isJumping
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fall")
            && !anim.GetBool("isFall")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
        //점프 모션이 실행되야만 점프가 실행되게(애니메이션 딜레이 및 더블점프 강제 제거)
        if (isJumping == true)
        {
            Jump();
        }

        //Idle일때 스킬 및 공격 false 판정
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetBool("isIdle", true);
            isAttack = false;
            isSkill = false;
            anim.ResetTrigger("CommonAttack");
        }

        //다른 모션일 때, 혹시라도 Move가 실행되도 달리지 못하게
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack3") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            moveSpd = 0;
        }
        else
        {
            moveSpd = moveSpeed;
        }
        //Run 애니메이션이 작동 되었는데 Move가 실행되지 않을 때
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && isAttack == true)
        {
            isAttack = false;
        }
    }

    public void SetHp(float amount) // Hp 세팅
    {
        maxHP = amount;
        PlayerHP = maxHP;
    }
    public void CheckHp() // HP 체크
    {
        string inputText = "HP " + PlayerHP.ToString("F0") + "/" + maxHP.ToString("F0");
        if (HpBar != null)
            HpBar.value = PlayerHP / maxHP;
        if (HpText != null)
            HpText.text = inputText;
    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (!WallCollision)
        {
            transform.position += moveVec * moveSpd * Time.deltaTime;
        }
        StartCoroutine(Delay(0.2f));
    }

    void Move_anim()
    {
        anim.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Dodge()
    {
        anim.SetTrigger("isDodge");
    }

    void Attack_anim()
    {
        anim.SetTrigger("CommonAttack");
        isAttack = true;
    }
    private void WallCheck()
    {
        WallCollision = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), transform.forward, 0.6f, LayerMask.GetMask("Wall", "Monster"));
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
        if(AttackValue == 1)
        {
            isSound = true;
        }
        audioSources[AttackValue].Play();
        yield return new WaitForSeconds(playsec); 
        audioSources[AttackValue].Stop();
        yield return null;
    }
    void Skill_Q()
    {
        isSkill = true;
        isSkillQ = true;
        anim.SetTrigger("Skill_Q");
        StartCoroutine(Spawn_SwordAura());
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

    void Skill_W()
    {
        WSkill_Collider.SetActive(true);
        anim.SetTrigger("Skill_W");
        isSkill = true;
        StartCoroutine(MoveForwardForSeconds(1.35f));
    }

    void Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(SKill_E_Move());
        StartCoroutine(WarriorSkill_E());
    }

    IEnumerator SKill_E_Move()
    {
        float elapsedTime = 0;
        yield return new WaitForSeconds(0.5f);
        //스킬 사용 시 카메라 무빙(등 포커스)
        if(SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
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
        //스킬 나갈 시 카메라 무빙(얼굴 포커스, 멈춤)
        if(SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x + 0.7f, transform.position.y + 0.6f, transform.position.z - 1.5f, -50, 1.0f, "stop");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 1.5f, transform.position.y + 0.6f, transform.position.z + 1.5f, 170, 1.0f, "stop");
        }
        yield return new WaitForSeconds(0.5f);
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

    void Jump()
    {
        anim.SetBool("isJump", true);
        rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
    }

    void OnCollisionStay(Collision collision) //충돌 감지
    {
        if (collision.gameObject.tag == "Floor")
        {   
            //tag가 Floor인 오브젝트와 충돌했을 때
            Stay();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //tag가 Floor인 오브젝트와 충돌이 끝났을 때
            Fall();
        }
    }

    void Fall()
    {
        anim.SetBool("isFall", true); //떨어지는것으로 감지
        rd.AddForce(Vector3.down * fallPower, ForceMode.VelocityChange);
    }

    void Stay()
    {
        isJumping = false; //isJump, isFall을 다시 false로
        anim.SetBool("isJump", false);
        anim.SetBool("isFall", false);
    }

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

        //검사 애니메이션 실행 시가 아닐 때만 false 반환
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_1B") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_2") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_4") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_7Combo_ALL"))
        {

        }
        else
        {
            coroutineMove = false;
        }

        if(WSkill_Collider.activeSelf == true)
        {
            WSkill_Collider.SetActive(false);
            WSkillCoolTime = 0;
            Wcool.fillAmount = 1;
        }
        /*
        if (Attack_1_Collider == true)
        {
            Attack_1_Collider.SetActive(false);
        }
        */
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
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
        }
    }

    public void skill_W_on()
    {
        SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(SkillW_Effect.transform.eulerAngles));
        SkillEffect.transform.parent = EffectGen.transform;
    }

    IEnumerator TakeDamage()
    {
        if (maxHP != 0 || PlayerHP > 0)
        {
            PlayerHP -= Damage;
            Debug.Log(PlayerHP);
            CheckHp();
            anim.SetBool("TakeDamage", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("TakeDamage", false);
        }

        if (PlayerHP <= 0) // 플레이어가 죽으면 게임오버 창 띄움
        {
            anim.SetBool("isDie", true);
            yield return new WaitForSeconds(2.0f);
            GameObject.Find("EventSystem").GetComponent<GameEnd>().GameOver(true);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Monster_Melee")
        {
            // 충돌한 몬스터 오브젝트에서 해당 스크립트를 가져옵니다.
            MonoBehaviour monsterCtrl = col.gameObject.transform.root.GetComponentInChildren<MonoBehaviour>();

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (monsterCtrl != null)
            {
                // 스크립트의 이름을 가져옵니다.
                string monsterScriptName = monsterCtrl.GetType().Name;

                // "Ctrl"을 제거하여 몬스터의 이름을 가져옵니다.
                string monsterName = monsterScriptName.Replace("Ctrl", "");

                // 몬스터 이름을 사용하여 해당 몬스터의 스크립트 타입을 가져옵니다.
                System.Type monsterScriptType = System.Type.GetType(monsterScriptName);

                // 가져온 스크립트를 동적으로 몬스터 스크립트 타입으로 캐스팅합니다.
                object specificMonsterCtrl = Convert.ChangeType(monsterCtrl, monsterScriptType);

                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)((specificMonsterCtrl as MonoBehaviour).GetType().GetField("ATK").GetValue(specificMonsterCtrl));
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }

        else if (col.gameObject.tag == "Monster_Ranged")
        {
            // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
            MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (monsterCtrl != null)
            {
                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(canTakeDamage == true && col.gameObject.tag == "Druid_Poison")
        {
            // 충돌한 몬스터 공격에서 해당 스크립트를 가져옵니다.
            MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // 가져온 몬스터 스크립트가 유효한지 확인합니다.
            if (monsterCtrl != null)
            {
                // 몬스터 스크립트로 캐스팅된 객체에서 ATK 값을 가져옵니다.
                float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
                Debug.Log("몬스터의 ATK 값: " + atkValue);
                Damage = atkValue;
                canTakeDamage = false;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("해당 몬스터에 대한 스크립트를 찾을 수 없습니다.");
            }
        }
    }
}
