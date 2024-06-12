using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl_Wizard : MonoBehaviour
{
    float hAxis;
    float vAxis;

    private float YRot;
    private float YPos;

    //애니메이션 컨트롤
    private Vector3 initPos;
    bool isSkill;
    bool isSkillQ;
    bool isAttack;
    bool isJumping;
    bool isJumpAttack;

    //코루틴 컨트롤
    bool coroutineMove = false;
    
    //벽 충돌체크
    private bool WallCollision;

    //스킬 쿨타임
    private float QSkillCoolTime;
    private float WSkillCoolTime;
    private float ESkillCoolTime;

    //캐릭터 공격 이펙트
    public GameObject commonAttack_Ice1_Effect;
    public GameObject commonAttack_Ice2_Effect;
    public GameObject commonAttack_Fire1_Effect;
    public GameObject commonAttack_Fire2_Effect;
    public GameObject Skill_Fire_Aura_Effect;
    public GameObject Skill_Ice_Aura_Effect;
    public GameObject Skill_FireQ_Effect;
    public GameObject Skill_IceQ_Effect;
    public GameObject Skill_FireW_Effect;
    public GameObject Skill_IceW_Effect;
    public GameObject Skill_FireE_Aura_Effect;
    public GameObject Skill_IceE_Aura_Effect;
    public GameObject Skill_FireE1_Effect;
    public GameObject Skill_IceE1_Effect;
    public GameObject Skill_FireE2_Effect;
    public GameObject Skill_IceE2_Effect;
    private GameObject Attack1_Effect;
    private GameObject Attack2_Effect;
    private GameObject Skill_Aura_Effect;
    private GameObject SkillQ_Effect;
    private GameObject SkillW_Effect;
    private GameObject SkillE_Aura_Effect;
    private GameObject SkillE1_Effect;
    private GameObject SkillE2_Effect;
    private GameObject SkillEffect;
    private float SkillYRot;

    //캐릭터 공격 콜라이더
    private GameObject EffectGen;

    //카메라
    private GameObject mainCamera;

    //Hp bar
    private Slider HpBar;
    
    Vector3 moveVec;

    Animator anim;
    Rigidbody rd;

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
    public float fallPower;     //떨어지는 
    
    private Vector3 tgPos;

    void Start()
    {
        //플레이어 스테이터스 초기화
        SetHp(100);
        PlayerATK = 100;
        PlayerDEF = 10;
        FireATT = 1.0f;
        IceATT = 1.0f;

        //HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();

        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();

        mainCamera = GameObject.FindWithTag("MainCamera");
        anim.SetBool("isIdle", true);
        EffectGen = transform.Find("EffectGen").gameObject;
        isSkill = false;
        isSkillQ = false;
        isAttack = false;
        isJumping = false;
    }

    void Update()
    {
        //스킬 쿨타임 충전
        QSkillCoolTime += Time.deltaTime;
        WSkillCoolTime += Time.deltaTime;
        ESkillCoolTime += Time.deltaTime;

        //캐릭터 스킬 이펙트
        SkillYRot = transform.eulerAngles.y;
        if (PlayerPrefs.GetString("property") == "Fire")
        {
            Attack1_Effect = commonAttack_Fire1_Effect;
            Attack2_Effect = commonAttack_Fire2_Effect;
            Skill_Aura_Effect = Skill_FireE_Aura_Effect;
            SkillQ_Effect = Skill_FireQ_Effect;
            SkillW_Effect = Skill_FireW_Effect;
            SkillE1_Effect = Skill_FireE1_Effect;
            SkillE2_Effect = Skill_FireE2_Effect;
            SkillE_Aura_Effect = Skill_FireE_Aura_Effect;
        }
        else if (PlayerPrefs.GetString("property") == "Ice")
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            Skill_Aura_Effect = Skill_Ice_Aura_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
            SkillE1_Effect = Skill_IceE1_Effect;
            SkillE2_Effect = Skill_IceE2_Effect;
            SkillE_Aura_Effect = Skill_IceE_Aura_Effect;
        }
        else
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            Skill_Aura_Effect = Skill_Ice_Aura_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
            SkillE1_Effect = Skill_IceE1_Effect;
            SkillE2_Effect = Skill_IceE2_Effect;
            SkillE_Aura_Effect = Skill_IceE_Aura_Effect;
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E"))
        {
            transform.position = Vector3.Lerp(transform.position, tgPos, 0.01f);
        }

        //벽 충돌체크
        WallCheck();

        //애니메이션 업데이트
        GetInput();

        //Y 로테이션 고정 코드
        YRot = transform.eulerAngles.y;
        if (YRot < 160 && YRot > 20)
        {
            YRot = 90;
        }

        if (YRot < -20 && YRot > -160)
        {
            YRot = -90;
        }
        Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);
        // 오브젝트에 회전 적용
        transform.rotation = desiredRotation;
        //Z 포지션 고정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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

        //점프공격 시 Y 포지션 고정
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !isJumpAttack)
        {
            Vector3 OriginPos = transform.position;
            YPos = OriginPos.y;
            isJumpAttack = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E"))
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
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack1_Wait") ||
           anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack2_Wait"))
        {
            isAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack2") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack3"))
        {
            isAttack = true;
        }

        //Skill_Q
        if(Input.GetKeyDown(KeyCode.Q) 
        && !isSkill 
        && !isJumping 
        && !anim.GetBool("isFall")
        //&& QSkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_Q();
        }

        //Skill_W
        if (Input.GetKeyDown(KeyCode.W)
        && !isSkill
        && !isJumping
        && !anim.GetBool("isFall")
        //&& WSkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_W();
        }

        //Skill_E
        if(Input.GetKeyDown(KeyCode.E) 
        && !isSkill 
        && !isJumping 
        && !anim.GetBool("isFall")
        //&& ESkillCoolTime >= 3.0f
        && !isAttack)
        {
            Skill_E();
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isSkill && !isAttack && !isJumping
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fall")
            && !anim.GetBool("isFall"))
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
            anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack1_Wait") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack2_Wait") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack3") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E") )
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
        if (HpBar != null)
            HpBar.value = PlayerHP / maxHP;
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
        //StartCoroutine(Dodge_Move());
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


    void Skill_Q()
    {
        isSkill = true;
        isSkillQ = true;
        anim.SetTrigger("Skill_Q");
        //StartCoroutine(Spawn_SwordAura());
    }
    void Skill_W()
    {
        //WSkill_Collider.SetActive(true);
        anim.SetTrigger("Skill_W");
        isSkill = true;
    }
    void Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(SKill_E_Move());
    }
    IEnumerator SKill_E_Move()
    {
        mainCamera.GetComponent<CameraCtrl>().UltimateCamera_Wizard(SkillYRot);
        tgPos = new Vector3(transform.position.x , transform.position.y + 4.0f, transform.position.z);
        //rd.AddForce(Vector3.up * 7, ForceMode.Impulse);
        yield return new WaitForSeconds(4.0f);
        isSkill = false;
    }

    void Jump()
    {
        anim.SetBool("isJump", true);
        rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
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
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //PlayerModel.transform.rotation = Quaternion.Euler(0,90,0);
            isAttack = false;
            isSkill = false;
        }

    }

    public void comboAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
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
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(60, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void skill_Aura_on()
    {
        SkillEffect = Instantiate(Skill_Aura_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y-1, EffectGen.transform.position.z), Quaternion.Euler(0f, 90, 0f));
    }

    public void skill_Q_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 0, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 180, 0f));
        }
    }

    public void skill_W_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, -90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }

    }

    public void skill_E_Aura_on()
    {
        SkillEffect = Instantiate(SkillE_Aura_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y-1, EffectGen.transform.position.z), Quaternion.Euler(0f, 90, 0f));
    }

    public void skill_E1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, -90, 0f));
        }
    }

    public void skill_E2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30f, -90, 0f));
        }
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
            //Destroy(this.gameObject);
            yield return new WaitForSeconds(0f);
        }
    }

}
