using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl_Rogue : CameraCtrl
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
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x + 3.0f, target.transform.position.y + 1.5f, target.transform.position.z, -90, 1.1f, "forward");
        }
        else
        {
            FocusCamera(target.transform.position.x - 3.0f, target.transform.position.y + 1.5f, target.transform.position.z, 90, 1.1f, "forward");
        }
        yield return new WaitForSeconds(1.1f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z - 5.0f, 0, 2.0f, "forward");
        }
        else
        {
            FocusCamera(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z - 5.0f, 180, 2.0f, "forward");
        }
        ShakeCamera(0.4f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.7f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x - 8, target.transform.position.y + 2f, target.transform.position.z + 3.0f, 60, 2.3f, "round");
        }
        else
        {
            FocusCamera(target.transform.position.x - 5.5f, target.transform.position.y + 2.2f, target.transform.position.z + 3.0f, -30, 2.3f, "round");
        }
        yield return new WaitForSeconds(0.5f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        moveStop(0.1f);
    }

    public override void JumpCamera()
    {

    }

}
