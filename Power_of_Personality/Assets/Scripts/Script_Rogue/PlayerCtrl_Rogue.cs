using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCtrl_Rogue : PlayerCtrl
{
    private bool isDash = false;
    private bool isDashAttack = false;

    protected override void Start()
    {
        base.Start();   // PlayerCtrl의 Start문을 상속 받아서 실행

        // 이동속도, 점프 힘, 떨어지는 힘을 도적에 맞게 설정
        moveSpeed = 7;
        JumpPower = 14;
        fallPower = 4;

        // 도적의 Dash는 따로 실행
        StartCoroutine(DashListener());
        isDashAttack = false;
    }

    protected override void Update()
    {
        base.Update(); // PlayerCtrl의 Update문을 상속 받아서 실행

        //대쉬일 때
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            moveSpd = moveSpeed * 1.25f;
        }
        else
        {
            moveSpd = moveSpeed;
        }
    }
    #region HP 설정
    public override void SetHp(float amount)
    {
        base.SetHp(amount);
    }
    public override void CheckHp()
    {
        base.CheckHp();
    }
    #endregion

    #region 이동 관련 함수
    protected override void WallCheck()
    {
        base.WallCheck();
    }

    protected override void GetInput()
    {
        base.GetInput();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Move_anim()
    {
        base.Move_anim();
    }

    protected override void Turn()
    {
        base.Turn();
    }

    protected override void Dodge()
    {
        base.Dodge();
    }

    protected override void Jump()
    {
        base.Jump();
    }

    protected override void Fall()
    {
        base.Fall();
    }
    protected override void Stay()
    {
        base.Stay();
    }
    #endregion

    #region 충돌 관련 함수
    protected override void OnCollisionStay(Collision collision) // 충돌 감지
    {
        base.OnCollisionStay(collision);
    }
    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
    }
    #endregion

    #region 공격 관련 함수
    protected override void Attack_anim()
    {
        anim.SetTrigger("CommonAttack");
        isAttack = true;
    }
    protected override void Skill_Q()
    {
        isSkill = true;
        anim.SetTrigger("Skill_Q");
        StartCoroutine(MoveForwardForSeconds(1.0f));
        QSkillCoolTime = 0;
    }

    protected override void Skill_W()
    {
        anim.SetTrigger("Skill_W");
        isSkill = true;
        WSkillCoolTime = 0;
    }

    protected override void Skill_E()
    {
        anim.SetTrigger("Skill_E");
        isSkill = true;
        StartCoroutine(Skill_E_Move());
    }

    //도적 스킬 E 카메라 무브 및 스킬 공격
    IEnumerator Skill_E_Move()
    {
        //스킬 나갈 시 카메라 무빙(얼굴 포커스, 멈춤)
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x + 3.0f, transform.position.y + 1.5f, transform.position.z, -90, 1.1f, "forward");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 3.0f, transform.position.y + 1.5f, transform.position.z, 90, 1.1f, "forward");
        }
        yield return new WaitForSeconds(1.2f);
        //스킬 사용 시 카메라 무빙(등 포커스)
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x - 3.0f, transform.position.y + 2.5f, transform.position.z, 90, 3.0f, "forward");
        }
        else
        {
            mainCamera.GetComponent<CameraCtrl>().FocusCamera(transform.position.x + 3.0f, transform.position.y + 2.5f, transform.position.z, -90, 3.0f, "forward");
        }
        yield return new WaitForSeconds(1.0f);
        ESkillCoolTime = 0;
    }
    public void comboAttack_1_on()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack1_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void comboAttack_2_on()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack2_Effect, EffectGen.transform.position, Quaternion.Euler(0, SkillYRot - 90, 120));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void comboAttack_3_on()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(90, SkillYRot - 90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(Attack3_Effect, EffectGen.transform.position, Quaternion.Euler(90, SkillYRot - 90, 0));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void comboAttack_off()
    {
        Destroy(SkillEffect);
    }

    public void skill_Q_on()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillQ_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void skill_W_on()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
        else
        {
            SkillEffect = Instantiate(SkillW_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
            SkillEffect.transform.parent = EffectGen.transform;
        }
    }

    public void skill_E1_0n()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE1_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
        }
    }

    public void skill_E2_0n()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE2_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot - 90, 90f));
        }
    }

    public void skill_E3_0n()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot, 90f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE3_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 1f, EffectGen.transform.position.z), Quaternion.Euler(0f, SkillYRot, 90f));
        }
    }

    public void skill_E4_0n()
    {
        if (LocalSkillYRot == 90 || (LocalSkillYRot < 92 && LocalSkillYRot > 88))
        {
            SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
        }
        else
        {
            SkillEffect = Instantiate(SkillE4_Effect, EffectGen.transform.position, Quaternion.Euler(0f, SkillYRot - 90, 0f));
        }
    }
    protected override void SkillCoolTimeCharge()
    {
        base.SkillCoolTimeCharge();
    }
    #endregion

    #region 도적 Dash 함수
    IEnumerator DashListener()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                yield return Dash("Right");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                yield return Dash("Left");
            }

            //Dash 종료
            if (anim.GetBool("isDash") && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                isDash = false;
                anim.SetBool("isDash", false);
            }
            else if (anim.GetBool("isDash"))
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
            if (direct == "Right" &&
                Input.GetKeyDown(KeyCode.RightArrow) &&      //방향키 버튼 눌렀을 때
                !isDash)                                    //isDash가 false라면
            {
                isDash = true;
                anim.SetBool("isDash", true);

                yield break;
            }
            else if (direct == "Left" &&
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
    #endregion

    #region 스킬이나 공격 움직임, Delay 등 세부 조정 함수
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
    protected override IEnumerator Delay(float seconds)
    {
        yield return base.Delay(seconds);
    }
    protected override void SetIce()
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
    protected override void SetFire()
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
    #endregion
}
