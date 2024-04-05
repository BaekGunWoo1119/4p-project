using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerCtrl : MonoBehaviour
{
    #region ���� ����
    // GetAxis ��
    protected float hAxis;
    protected float vAxis;

    // Player�� transform, YPosition, YRotation ��
    protected Transform trs;
    protected float YRot;
    protected float YPos;

    //�÷��̾� �������ͽ�
    public float PlayerHP;     //HP
    public float maxHP;        //�ִ� ü��
    public float Damage;       //���� ���ط�
    public float PlayerATK;    //���ݷ�
    public float PlayerDEF;    //����
    public float FireATT;      //�� �Ӽ� ������ ����
    public float IceATT;       //���� �Ӽ� ������ ����
    public float moveSpeed;     //�̵��ӵ�
    public float moveSpd;      //�̵��ӵ�
    public float JumpPower;     //������
    public float fallPower;     //�������� ��

    // �ִϸ��̼� ��Ʈ��
    protected Vector3 initPos;
    protected bool isSkill = false;
    protected bool isAttack = false;
    protected bool isDash = false;
    protected bool isDashAttack = false;
    protected bool isJumping = false;
    protected bool isRun = false;
    protected bool isForward = true;
    //bool dashCount = false;
    protected bool isJumpAttack;

    // �ڷ�ƾ ��Ʈ��
    protected bool coroutineMove = false;

    // �ִϸ�����, Rigidbody
    protected Animator anim;
    protected Rigidbody rd;

    // ����Ʈ
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
    public GameObject Attack1_Effect;
    public GameObject Attack2_Effect;
    public GameObject Attack3_Effect;
    public GameObject SkillQ_Effect;
    public GameObject SkillW_Effect;
    public GameObject SkillE1_Effect;
    public GameObject SkillE2_Effect;
    public GameObject SkillE3_Effect;
    public GameObject SkillE4_Effect;
    public float SkillYRot;
    public float LocalSkillYRot;
    public GameObject EffectGen;
    public GameObject SkillEffect;

    // ī�޶�, ����
    protected GameObject mainCamera;
    protected AudioClip[] effectAudio;

    // �� �浹üũ
    protected bool WallCollision;

    // HP Bar
    protected Slider HpBar;
    protected TMP_Text HpText;

    // ��Ÿ�� ����
    protected float QSkillCoolTime;
    protected float WSkillCoolTime;
    protected float ESkillCoolTime;
    protected Image Qcool;
    protected Image Wcool;
    protected Image Ecool;
    protected bool canTakeDamage = true; // �������� ������ �� �ִ���
    protected float damageCooldown = 1.0f; // 1�ʸ��� ƽ�������� �������� ����

    // ȸ�� ����
    protected GameObject CurrentFloor;
    protected Vector3 moveVec;
    #endregion

    protected virtual void Start()
    {
        // �÷��̾� �������ͽ� �ʱ�ȭ
        SetHp(100);
        PlayerATK = 100;
        PlayerDEF = 10;
        FireATT = 1.0f;
        IceATT = 1.0f;

        // HP Bar ����
        // HpBar = GameObject.Find("HPBar-Player").GetComponent<Slider>();
        // HpText = GameObject.Find("StatPoint - Hp").GetComponent<TMP_Text>();
        // HpText.text = "HP 100/100";

        // �ִϸ��̼�, Rigidbody, Transform ������Ʈ ����
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        trs = GetComponentInChildren<Transform>();

        initPos = trs.position; // initPos�� Transform.position �Ҵ�
        mainCamera = GameObject.FindWithTag("MainCamera");  // ���� ī�޶� ����
        anim.SetBool("isIdle", true);   // isIdle�� True�� �����ؼ� Idle ���� ����
        EffectGen = transform.Find("EffectGen").gameObject; // EffectGen ����

        // �ִϸ��̼�, ��ų �����ϴ� bool���� false�� �ʱ�ȭ
        isSkill = false;
        isAttack = false;
        isDashAttack = false;
        isJumping = false;
        isRun = false;
    }

    protected virtual void Update()
    {
        // �� �浹üũ �Լ� ����
        WallCheck();

        // �ִϸ��̼� ������Ʈ
        GetInput();

        //��ų ��Ÿ�� ����
        QSkillCoolTime += Time.deltaTime;
        WSkillCoolTime += Time.deltaTime;
        ESkillCoolTime += Time.deltaTime;

        //Y �����̼� ���� �ڵ�
        YRot = transform.eulerAngles.y;
        
        //X축 고정
        transform.localRotation = Quaternion.Euler(0, YRot, 0);

        //Z ������ ����
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        // char ������Ʈ ��ġ ����
        transform.GetChild(0).localPosition = Vector3.zero;

        // Move �Լ� ����
        if (!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_B_3"))
        {
            Move();
            Move_anim();
            Turn();
        }

        // Turn �Լ� ����
        if (!isSkill && !isAttack && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Turn();
        }

        // Dodge �Լ� ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            Dodge();
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
        {
            anim.ResetTrigger("isDodge");
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            anim.SetBool("isJump", false);
        }

        // Attack �Լ� ����
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack_anim();
        }

        //�⺻����1 & �⺻����3 �� ���� �ִϸ��̼� & ���ݻ���
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1") && !coroutineMove)
        {
            /*
            isSound = false;
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.4f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            StartCoroutine(Attack1_Sound());
            */
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2") && !coroutineMove)
        {
            /*
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack2_Sound());
            StartCoroutine(Delay(0.2f));
            */
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_3") && !coroutineMove)
        {
            /*
            StartCoroutine(Delay(0.2f));
            StartCoroutine(MoveForwardForSeconds(0.3f));
            StartCoroutine(Attack1_Collider());
            StartCoroutine(Attack3_Sound());
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            */
        }
        //��������
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !coroutineMove)
        {
            //isSound = false;
            //StartCoroutine(Attack1_Sound());
            //StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.4f));
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") && !coroutineMove)
        {
            //StartCoroutine(Attack2_Sound());
            //StartCoroutine(Attack1_Collider());
            StartCoroutine(Delay(0.2f));
            anim.ResetTrigger("CommonAttack");
        }

        //�ִϸ��̼��� ������ coroutine�� ������ ����
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_4Combo_A_1")
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
        //�������� �� Y ������ ����
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1") && !isJumpAttack)
        {
            Vector3 OriginPos = transform.position;
            YPos = OriginPos.y;
            isJumpAttack = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack1"))
        {
            rd.velocity = Vector3.zero;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2"))
        {
            float upperUpTime = 0;
            if (upperUpTime == 0)
            {
                //���߿��� �����Ǿ� �����ٰ� ������
                Vector3 OriginPos = transform.position;
                YPos = OriginPos.y;
                upperUpTime += 1;
            }
            Vector3 newPos = transform.position;
            newPos.y = YPos;
            isAttack = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            //�������� �ڵ� ���� ����
            //rd.AddForce(Vector3.down * fallPower/3, ForceMode.VelocityChange);
        }
        //�� �� ���� �� �� ���� �������� �޺��� �ǰ�
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
            isAttack = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isJumpAttack = false;
            isAttack = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && isJumpAttack == true)
        {
            anim.ResetTrigger("CommonAttack");
            isAttack = false;
        }

        //������� 2Ÿ, 3Ÿ �� ������ȯ �ǵ���
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_1_Wait") ||
           anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2_Wait"))
        {
            isAttack = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3Combo_A_2") ||
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
        //���� ����� ����Ǿ߸� ������ ����ǰ�(�ִϸ��̼� ������ �� �������� ���� ����)
        if (isJumping == true)
        {
            Jump();
        }

        //Idle�϶� ��ų �� ���� false ����
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetBool("isIdle", true);
            isAttack = false;
            isSkill = false;
            anim.ResetTrigger("CommonAttack");
        }

        //�ٸ� ����� ��, Ȥ�ö� Move�� ����ǵ� �޸��� ���ϰ�
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
           anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack2") ||
           (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetBool("isRun")) ||
           (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && !anim.GetBool("isRun")))
        {
            moveSpd = 0;
        }

        //�뽬�� ��
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            moveSpd = moveSpeed * 1.25f;
        }
        else
        {
            moveSpd = moveSpeed;
        }

        //ĳ���� ��ų ����Ʈ
        LocalSkillYRot = transform.localEulerAngles.y;
        SkillYRot = transform.eulerAngles.y;
        if (PlayerPrefs.GetString("property") == "Fire")
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
        else if (PlayerPrefs.GetString("property") == "Ice")
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

    #region HP ����
    public virtual IEnumerator TakeDamage()
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

        if (PlayerHP <= 0) // �÷��̾ ������ ���ӿ��� â ���
        {
            anim.SetBool("isDie", true);
            yield return new WaitForSeconds(2.0f);
            GameObject.Find("EventSystem").GetComponent<GameEnd>().GameOver(true);
        }
    }
    public virtual void SetHp(float amount) // Hp ����
    {
        maxHP = amount;
        PlayerHP = maxHP;
    }
    public virtual void CheckHp() // HP üũ
    {
        string inputText = "HP " + PlayerHP.ToString("F0") + "/" + maxHP.ToString("F0");
        if (HpBar != null)
            HpBar.value = PlayerHP / maxHP;
        if (HpText != null)
            HpText.text = inputText;
    }
    #endregion

    #region �̵� ���� �Լ�
    protected virtual void WallCheck()
    {
        WallCollision = Physics.Raycast(transform.position + new Vector3(0, 1.0f, 0), transform.forward, 0.6f, LayerMask.GetMask("Wall", "Monster"));
    }

    protected virtual void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
    }

    protected virtual void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (!WallCollision)
        {
            transform.localPosition += moveVec * moveSpd * Time.deltaTime;
        }
        StartCoroutine(Delay(0.2f));
    }
    protected virtual void Move_anim()
    {
        anim.SetBool("isRun", moveVec != Vector3.zero);
    }
    protected virtual void Turn()
    {
        if (hAxis > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (hAxis < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
    }
    protected virtual void Dodge()
    {
        anim.SetTrigger("isDodge");
        //transform.Translate(Vector3.forward * moveSpeed * 2 * Time.deltaTime);
    }
    protected virtual void Jump()
    {
        anim.SetBool("isJump", true);
        rd.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
    }
    protected virtual void Fall()
    {
        anim.SetBool("isFall", true); //�������°����� ����
        rd.AddForce(Vector3.down * fallPower, ForceMode.VelocityChange);
    }
    protected virtual void Stay()
    {
        isJumping = false; //isJump, isFall�� �ٽ� false��
        anim.SetBool("isJump", false);
        anim.SetBool("isFall", false);
    }
    #endregion

    #region �浹 ���� �Լ�
    public virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Monster_Melee")
        {
            // �浹�� ���� ������Ʈ���� �ش� ��ũ��Ʈ�� �����ɴϴ�.
            MonoBehaviour monsterCtrl = col.gameObject.transform.root.GetComponentInChildren<MonoBehaviour>();

            // ������ ���� ��ũ��Ʈ�� ��ȿ���� Ȯ���մϴ�.
            if (monsterCtrl != null)
            {
                // ��ũ��Ʈ�� �̸��� �����ɴϴ�.
                string monsterScriptName = monsterCtrl.GetType().Name;

                // "Ctrl"�� �����Ͽ� ������ �̸��� �����ɴϴ�.
                string monsterName = monsterScriptName.Replace("Ctrl", "");

                // ���� �̸��� ����Ͽ� �ش� ������ ��ũ��Ʈ Ÿ���� �����ɴϴ�.
                System.Type monsterScriptType = System.Type.GetType(monsterScriptName);

                // ������ ��ũ��Ʈ�� �������� ���� ��ũ��Ʈ Ÿ������ ĳ�����մϴ�.
                object specificMonsterCtrl = Convert.ChangeType(monsterCtrl, monsterScriptType);

                // ���� ��ũ��Ʈ�� ĳ���õ� ��ü���� ATK ���� �����ɴϴ�.
                float atkValue = (float)((specificMonsterCtrl as MonoBehaviour).GetType().GetField("ATK").GetValue(specificMonsterCtrl));
                Debug.Log("������ ATK ��: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("�ش� ���Ϳ� ���� ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }

        else if (col.gameObject.tag == "Monster_Ranged")
        {
            // �浹�� ���� ���ݿ��� �ش� ��ũ��Ʈ�� �����ɴϴ�.
            MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // ������ ���� ��ũ��Ʈ�� ��ȿ���� Ȯ���մϴ�.
            if (monsterCtrl != null)
            {
                // ���� ��ũ��Ʈ�� ĳ���õ� ��ü���� ATK ���� �����ɴϴ�.
                float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
                Debug.Log("������ ATK ��: " + atkValue);
                Damage = atkValue;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("�ش� ���Ϳ� ���� ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }

    public virtual void OnTriggerStay(Collider col)
    {
        if (canTakeDamage == true && col.gameObject.tag == "Druid_Poison")
        {
            // �浹�� ���� ���ݿ��� �ش� ��ũ��Ʈ�� �����ɴϴ�.
            MonoBehaviour monsterCtrl = col.gameObject.GetComponent<MonoBehaviour>();

            // ������ ���� ��ũ��Ʈ�� ��ȿ���� Ȯ���մϴ�.
            if (monsterCtrl != null)
            {
                // ���� ��ũ��Ʈ�� ĳ���õ� ��ü���� ATK ���� �����ɴϴ�.
                float atkValue = (float)monsterCtrl.GetType().GetField("ATK").GetValue(monsterCtrl);
                Debug.Log("������ ATK ��: " + atkValue);
                Damage = atkValue;
                canTakeDamage = false;
                StartCoroutine(TakeDamage());
            }
            else
            {
                Debug.Log("�ش� ���Ϳ� ���� ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }
    protected virtual void OnCollisionStay(Collision collision) // �浹 ����
    {
        if (collision.gameObject.tag == "Floor")    // Tag�� Floor�� ������Ʈ�� �浹���� ��
        {
            Stay();
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")    // Tag�� Floor�� ������Ʈ�� �浹�� ������ ��
        {
            Fall();
        }
    }
    #endregion

    #region ��ų / ���� ���� �Լ�. �ڽ� ��ũ��Ʈ�� ����� ��Ű�� �Լ��� ����� ������ �´� ��ũ��Ʈ�� ����� �� �ֵ��� ��.
    protected virtual void Attack_anim()
    {
    }

    protected virtual void Skill_Q()
    {
    }

    protected virtual void Skill_W()
    {
    }

    protected virtual void Skill_E()
    {
    }
    #endregion

    #region Delay �Լ�
    protected virtual IEnumerator Delay(float seconds)
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
    #endregion
}
