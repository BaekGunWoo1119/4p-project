using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Golem : BossCtrl
{
    public GameObject ArmSwing_Effect;
    public GameObject GroundStamp_Effect;
    public GameObject FallingRock_Effect;
    public GameObject GroundSmash_Effect;
    public GameObject PowerSmash_Effect;
    public GameObject JumpSmash_Effect;

    void Update()
    {
        SkillYRot = transform.localEulerAngles.y;

        //Debug.Log(SkillYRot);
    }

    #region 공격 이펙트 스크립트
    public void ArmSwing_1()
    {
        if (SkillYRot == 180 || (SkillYRot > 130 && SkillYRot < 230))
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(45, -90, 0));
        }
        else
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(45, -90, 0));
        }
    }

    public void ArmSwing_2()
    {
        if (SkillYRot == 180 || (SkillYRot > 130 && SkillYRot < 230))
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(-70, -40, 0));
        }
        else
        {
            SkillEffect = Instantiate(ArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2, EffectGen.transform.position.z), Quaternion.Euler(-70, -40, 0));
        }
    }
    public void GroundStamp()
    {
        SkillEffect = Instantiate(GroundStamp_Effect, new Vector3(EffectGen.transform.position.x - 0.5f, EffectGen.transform.position.y, EffectGen.transform.position.z + 1), Quaternion.Euler(0, 0, 0));
    }
    public void FallingRock()
    {
        SkillEffect = Instantiate(FallingRock_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 7f, EffectGen.transform.position.z + 5f), Quaternion.Euler(0, 0, 0));
    }
    public void GroundSmash()
    {
        SkillEffect = Instantiate(GroundSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }
    public void PowerSmash()
    {
        SkillEffect = Instantiate(PowerSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }
    public void JumpSmash()
    {
        SkillEffect = Instantiate(JumpSmash_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z + 5), Quaternion.Euler(0, 0, 0));
    }

    #endregion
}

