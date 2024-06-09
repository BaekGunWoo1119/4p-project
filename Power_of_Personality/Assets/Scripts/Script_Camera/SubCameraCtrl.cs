using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraCtrl : CameraCtrl
{
    //상속
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public override void UltimateCamera_Warrior(float SkillYRot)
    {
    }

    public override void UltimateCamera_Rogue(float SkillYRot)
    {
    }

    public override void UltimateCamera_Archer(float SkillYRot)
    {
    }

    public override void UltimateCamera_Wizard(float SkillYRot)
    {
    }

    public override void JumpCamera_Warrior()
    {
        FocusCamera(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z - 9, 0, 0.2f, "null");
    }

}
