using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

//도적 애니메이션
public class PlayerCtrl_Rogue : MonoBehaviour
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
    public float moveSpeed;
    private float moveSpd;
    public float JumpPower;
    public float fallPower;

    //애니메이션 컨트롤
    private Vector3 initPos;
    bool isSkill = false;
    bool isAttack = false;
    bool isDash = false;
    bool isDashAttack = false;
    bool isJumping = false;
    bool isRun = false;

    bool isForward = true;
    //bool dashCount = false;
    bool isJumpAttack;

    //코루틴 컨트롤
    bool coroutineMove = false;

    //스킬 쿨타임
    private float QSkillCoolTime;
    private float WSkillCoolTime;
    private float ESkillCoolTime;

    Vector3 moveVector;
    Vector3 moveVec;
    Animator anim;
    Rigidbody rd;
    
    //이펙트
    public GameObject commonAttack_Ice1_Effect;
    public GameObject commonAttack_Ice2_Effect;
    public GameObject commonAttack_Ice3_Effect;
    public GameObject commonAttack_Fire1_Effect;
    public GameObject commonAttack_Fire2_Effect;
    public GameObject commonAttack_Fire3_Effect;
    public GameObject Skill_FireQ_Effect;
    public GameObject Skill_IceQ_Effect;
    public GameObject Skill_FireW_Effect;
    public GameObject Skill_IceW_Effect;
    public GameObject Skill_FireE1_Effect;
    public GameObject Skill_FireE2_Effect;
    public GameObject Skill_FireE3_Effect;
    public GameObject Skill_FireE4_Effect;
    public GameObject Skill_IceE1_Effect;
    public GameObject Skill_IceE2_Effect;
    public GameObject Skill_IceE3_Effect;
    public GameObject Skill_IceE4_Effect;
    private GameObject Attack1_Effect;
    private GameObject Attack2_Effect;
    private GameObject Attack3_Effect;
    private GameObject SkillQ_Effect;
    private GameObject SkillW_Effect;
    private GameObject SkillE1_Effect;
    private GameObject SkillE2_Effect;
    private GameObject SkillE3_Effect;
    private GameObject SkillE4_Effect;
    private float SkillYRot;

    private GameObject EffectGen;
    GameObject SkillEffect;

    //카메라
    private GameObject mainCamera;

    //사운드
    public AudioClip[] effectAudio;

    //벽 충돌체크
    private bool WallCollision;

    void Start()
    {
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        trs = GetComponentInChildren<Transform>();
        initPos = trs.position;
        mainCamera = GameObject.FindWithTag("MainCamera");
        anim.SetBool("isIdle", true);
        EffectGen = transform.Find("EffectGen").gameObject;
        isSkill = false;
        isAttack = false;
        isDashAttack = false;
        isJumping = false;
        isRun = false;
        //Dash
        StartCoroutine(DashListener());
    }

    void Update()
    {
        //벽 충돌체크
        WallCheck();
        //애니메이션 업데이트
        GetInput();
        //Y축 확인

        //스킬 쿨타임 충전
        QSkillCoolTime += Time.deltaTime;
        WSkillCoolTime += Time.deltaTime;
        ESkillCoolTime += Time.deltaTime;

        //Y 로테이션 고정 코드
        YRot = transform.eulerAngles.y;
        // if((YRot < 100 && YRot > 80) || (YRot > -300 && YRot < -250))
        // {
        //     YRot = 90;
        // }

        // if ((YRot < -80 && YRot > -100) || (YRot < 300 && YRot > 250))
        // {
        //     YRot = -90;
        // }

        //Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);
        // 오브젝트에 회전 적용
        //transform.rotation = desiredRotation;

        //Z 포지션 고정
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // char 오브젝트 위치 고정
        transform.GetChild(0).localPosition = Vector3.zero;


        //Move
        if (!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_B_3"))
        {
            Turn();
            Move();
            Move_anim();
        }

        //Turn
        if(!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
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
            Attack_anim();
        }

         //기본공격1 & 기본공격3 시 전진 애니메이션 & 공격사운드
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1") && !coroutineMove)
        {
            /*
            isSound = false;
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.4f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            StartCoroutine(Attack1_Sound());
            */
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2") && !coroutineMove)
        {
            /*
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack2_Sound());
            StartCoroutine(Delay(0.2f));
            */
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_3") && !coroutineMove)
        {
            /*
            StartCoroutine(Delay(0.2f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack3_Sound());
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            */
        }
        //점프공격
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !coroutineMove)
        {
            //isSound = false;
            //StartCoroutine(Attack1_Sound());
            //StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.4f));
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") && !coroutineMove)
        {
            //StartCoroutine(Attack2_Sound());
            //StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.2f));
            anim.ResetTrigger("CommonAttack");
        }
        //애니메이션이 끝나면 coroutine을 강제로 종료
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_A_1") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_A_2") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_A_3")
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
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1"))
        {
            rd.velocity = Vector3.zero;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
        {
            float upperUpTime = 0;
            if(upperUpTime == 0)
            {
                //공중에서 고정되어 때리다가 떨어짐
                Vector3 OriginPos = transform.position;
                YPos = OriginPos.y;
                upperUpTime += 1;
            }
            Vector3 newPos = transform.position;
            newPos.y = YPos;
            isAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            //떨어지는 코드 추후 수정
            //rd.AddForce(Vector3.down * fallPower/3, ForceMode.VelocityChange);
        }
        //한 번 점프 시 한 번의 점프공격 콤보만 되게
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Wait") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
            isAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
            isAttack = false;
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isAttack = false;
        }

        //지상공격 2타, 3타 시 방향전환 되도록
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1_Wait") ||
           anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2_Wait"))
        {
            isAttack = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_3"))
        {
            isAttack = true;
        }

        //Skill_Q
        if (Input.GetKeyDown(KeyCode.Q)
        && !isSkill
        && !isJumping
        && !anim.GetBool("isFall")
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

        //Skill_E
        if (Input.GetKeyDown(KeyCode.E)
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
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1_Wait") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2_Wait") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_3") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Q") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_W") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Skill_E") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
        {
            moveSpd = 0;
            //moveVec = new Vector3(0, 0, 0).normalized;
        }
        //대쉬일 때
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            moveSpd = moveSpeed * 1.25f;
        }
        else
        {
            moveSpd = moveSpeed;
        }

        //캐릭터 스킬 이펙트
        SkillYRot = transform.eulerAngles.y;
        if(PlayerPrefs.GetString("property") == "Fire")
        {
            Attack1_Effect = commonAttack_Fire1_Effect;
            Attack2_Effect = commonAttack_Fire2_Effect;
            Attack3_Effect = commonAttack_Fire3_Effect;
            SkillQ_Effect = Skill_FireQ_Effect;
            SkillW_Effect = Skill_FireW_Effect;
            SkillE1_Effect = Skill_FireE1_Effect;
            SkillE2_Effect = Skill_FireE2_Effect;
            SkillE3_Effect = Skill_FireE3_Effect;
            SkillE4_Effect = Skill_FireE4_Effect;
        }
        else if(PlayerPrefs.GetString("property") == "Ice")
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            Attack3_Effect = commonAttack_Ice3_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
            SkillE1_Effect = Skill_IceE1_Effect;
            SkillE2_Effect = Skill_IceE2_Effect;
            SkillE3_Effect = Skill_IceE3_Effect;
            SkillE4_Effect = Skill_IceE4_Effect;
        }
        else
        {
            Attack1_Effect = commonAttack_Ice1_Effect;
            Attack2_Effect = commonAttack_Ice2_Effect;
            Attack3_Effect = commonAttack_Ice3_Effect;
            SkillQ_Effect = Skill_IceQ_Effect;
            SkillW_Effect = Skill_IceW_Effect;
        }

    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        Vector3 forwardDirection = transform.forward;
        Vector3 moveVector = forwardDirection * moveSpd * Time.deltaTime;
        //transform.transform.LookAt(transform.position + moveVector);
        transform.localPosition += moveVector;
        StartCoroutine(Delay(0.2f));
    }

    void Move_anim()
    {
        anim.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Turn()
    {   
        if(hAxis>0 && isForward==false){
            Vector3 forwardDirection = -transform.forward;
            Debug.Log(forwardDirection);
            transform.LookAt(transform.position + forwardDirection);
            isForward = true;
            
        }
        else if(hAxis<0 && isForward==true){
            Vector3 forwardDirection = -transform.forward;
            Debug.Log(forwardDirection);
            transform.LookAt(transform.position + forwardDirection);
            isForward = false;
        }
        

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
        anim.SetTrigger("Skill_Q");
        StartCoroutine(MoveForwardForSeconds(1.0f));
        QSkillCoolTime = 0;
    }
    

    void Skill_W()
    {
        anim.SetTrigger("Skill_W");
        isSkill = true;
        WSkillCoolTime = 0;
    }

    void Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(Skill_E_Move());
    }

    //도적 스킬 E 카메라 무브 및 스킬 공격
    IEnumerator Skill_E_Move()
    {
        //스킬 나갈 시 카메라 무빙(얼굴 포커스, 멈춤)
        if(SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x + 0.7f, transform.position.y + 2.0f, transform.position.z - 1.5f, -50, 1.0f, "stop");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 1.5f, transform.position.y + 2.0f, transform.position.z + 1.5f, 170, 1.0f, "stop");
        }
        yield return new WaitForSeconds(0.5f);
        //스킬 사용 시 카메라 무빙(등 포커스)
        if(SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 3.0f, transform.position.y + 2.5f, transform.position.z, 90, 3.3f, "forward");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x + 3.0f , transform.position.y + 2.5f, transform.position.z, -90, 3.3f, "forward");
        }
        yield return new WaitForSeconds(1.0f);
        ESkillCoolTime = 0;
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

    void Dodge()
    {
        anim.SetTrigger("isDodge");
        //transform.Translate(Vector3.forward * moveSpeed * 2 * Time.deltaTime);
    }

    IEnumerator MoveForwardForSeconds(float seconds)
    {
        yield return new WaitForSeconds(0.3f);
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
            moveVec = new Vector3(0, 0, 0);
            //transform.rotation = Quaternion.Euler(0,YRot,0);
            isAttack = false;
            isSkill = false;
        }
        
        yield return null;
    }

    IEnumerator DashListener()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                yield return Dash("Right");
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                yield return Dash("Left");
            }

            //Dash 종료
            if(anim.GetBool("isDash") && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                isDash = false;
                anim.SetBool("isDash", false);
            }
            else if(anim.GetBool("isDash"))
            {
                isDash = true;
                isAttack = false;
            }

            yield return null;
        }
    }

    IEnumerator Dash(string direct)
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;

        while (count < 0.5f)
        {
            if(direct == "Right" && 
                Input.GetKeyDown(KeyCode.RightArrow) &&      //방향키 버튼 눌렀을 때
                !isDash)                                    //isDash가 false라면
            {
                isDash = true;
                anim.SetBool("isDash", true);

                yield break;
            }
            else if(direct == "Left" && 
                Input.GetKeyDown(KeyCode.LeftArrow) &&      //방향키 버튼 눌렀을 때
                !isDash)                                    //isDash가 false라면
            {
                isDash = true;
                anim.SetBool("isDash", true);

                yield break;
            }

            count += Time.deltaTime;
            yield return null;
        }
    }

    public void comboAttack_1_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        Debug.Log("일반공격");
    }

    public void comboAttack_2_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 120));
            SkillEffect.transform.parent = EffectGen.transform;
            Debug.Log("일반공격");
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 120));
            SkillEffect.transform.parent = EffectGen.transform;
            Debug.Log("일반공격");
        }
    }

     public void comboAttack_3_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            Debug.Log("일반공격");
        }
        else
        {
            SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillEffect.transform.parent = EffectGen.transform;
            Debug.Log("일반공격");
        }
    }

    public void comboAttack_off()
    {
        Destroy(SkillEffect);
    }


    public void skill_Q_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 0, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 180, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }


    public void skill_W_on()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 0, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 180, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void skill_E1_0n()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+1f, EffectGen.transform.position.z), Quaternion.Euler(0f, 0f, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+1f, EffectGen.transform.position.z), Quaternion.Euler(0f, 180f, 90f));
        }
    }

    public void skill_E2_0n()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+2f, EffectGen.transform.position.z), Quaternion.Euler(0f, 0f, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+2f, EffectGen.transform.position.z), Quaternion.Euler(0f, 180f, 90f));
        }
    }

    public void skill_E3_0n()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+1f, EffectGen.transform.position.z), Quaternion.Euler(0f, 90, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y+1f, EffectGen.transform.position.z), Quaternion.Euler(0f, -90, 90f));
        }
    }

    public void skill_E4_0n()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 0, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, 180, 0f));
        }
    }

}
