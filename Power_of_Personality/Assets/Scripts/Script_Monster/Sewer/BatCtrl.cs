using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;

public class BatCtrl : MonsterCtrl
{
    public Transform FirePos;
    public override void Awake()
    {
        ATK = 5;
        MoveSpeed = 2.0f;
        Damage = 10.0f;
        TraceRadius = 10.0f;
        attackRadius = 5.0f;
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void SetHP(float amount)
    {
        base.SetHP(amount);
    }
    public override void CheckHP()
    {
        base.CheckHP();
    }

    public override IEnumerator FindPlayer()
    {
        yield return base.FindPlayer();
    }

    public override void DistanceCheck()
    {
        base.DistanceCheck();
    }

    public override IEnumerator Trace()
    {
        yield return base.Trace();
    }
    public override IEnumerator Attack()
    {
        AttackCoolTime = 0;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.3f);
        Instantiate(AttackCollider, FirePos.position, FirePos.rotation);
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("isAttack", false);
        AttackCoolTime = 0;
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    public override IEnumerator TakeDamage()
    {
        yield return base.TakeDamage();
    }
    public override Vector3 GetHPBarPosition()
    {
        return base.GetHPBarPosition(); // 원하는 위치로 수정
    }
}
