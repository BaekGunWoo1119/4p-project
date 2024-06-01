using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl_Warrior : CameraCtrl
{
    //상속
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FollowPlayer()
    {
        base.FollowPlayer();
    }

    protected override IEnumerator Setoffset(float wait)
    {
        yield return base.Setoffset(wait);
    }

    public override void moveStop(float seconds)
    {
        base.moveStop(seconds);
    }

    public override void ShakeCamera(float Amount, float Duration, string zoom)
    {
        base.ShakeCamera(Amount, Duration, zoom);
    }

    public override void ShakeCamera_Update()
    {
        base.ShakeCamera_Update();
    }

    public override void FocusCamera(float xP, float yP, float zP, float R, float Duration, string value)
    {
        base.FocusCamera(xP, yP, zP, R, Duration, value);
    }

    public override void FocusCamera_Update()
    {
        base.FocusCamera_Update();
    }

    public override void UltimateCamera(float SkillYRot)
    {
        StartCoroutine(Skill_E_Camera(SkillYRot));
    }

    IEnumerator Skill_E_Camera(float SkillYRot)
    {
        yield return new WaitForSeconds(2.0f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x - 5, target.transform.position.y + 2.5f, target.transform.position.z, 60, 5.3f, "round");
        }
        else
        {
            FocusCamera(target.transform.position.x - 2.5f, target.transform.position.y + 2.5f, target.transform.position.z, -30, 5.3f, "round");
        }
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.6f);
        ShakeCamera(0.5f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.8f);
        ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.2f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(1.2f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(1.0f);
        ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(1f);
        moveStop(0.1f);
    }

    public override void JumpCamera()
    {
        FocusCamera(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z - 9, 0, 0.2f, "null");
    }

}
