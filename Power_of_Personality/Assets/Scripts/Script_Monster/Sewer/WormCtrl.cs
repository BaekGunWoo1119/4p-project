using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;

public class WormCtrl : MonsterCtrl
{
    public override void Awake()
    {
        ownWeakProperty ="Ice";
        maxHP = 500f;
        ATK = 10.0f;
        DEF = 150;
        MoveSpeed = 2.0f;
        Damage = 10.0f;
        TraceRadius = 10.0f;
        attackRadius = 4.0f;
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void SetHP(float amount)
    {
        base.SetHP(maxHP);
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
        yield return base.Attack();
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    public override IEnumerator TakeDamage(float Damage)
    {
        yield return base.TakeDamage(Damage);
    }
    public override Vector3 GetHPBarPosition()
    {
        return transform.position + Vector3.up * 5.0f; // ���ϴ� ��ġ�� ����
    }
    public override IEnumerator DamageTextAlpha(float Damage)
    {
        yield return base.DamageTextAlpha(Damage);
    }
    public override void Attack_On()
    {
        base.Attack_On();
    }
}
