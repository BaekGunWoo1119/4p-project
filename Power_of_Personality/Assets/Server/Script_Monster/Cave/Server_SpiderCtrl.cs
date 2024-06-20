using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Server_SpiderCtrl : Server_MonsterCtrl
{
    public Transform FirePos;
    public override void Awake()
    {
        ATK = 0;
        MoveSpeed = 2.0f;
        Damage = 00.0f;
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
        yield return new WaitForSeconds(0.25f);
        Instantiate(AttackCollider, FirePos.position, FirePos.rotation);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);
        AttackCoolTime = 0;
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    public override IEnumerator TakeDamage(float CurDamage, string Property)
    {
        yield return base.TakeDamage(CurDamage, Property);
    }
    public override Vector3 GetHPBarPosition()
    {
        return base.GetHPBarPosition(); // ���ϴ� ��ġ�� ����
    }
    public override IEnumerator DamageTextAlpha(float CurDamage)
    {
        yield return base.DamageTextAlpha(CurDamage);
    }
    public override void Settarget()
    {
        base.Settarget();
    }

    [PunRPC]
    public override void Server_Attack()
    {
        base.Server_Attack();
    }

    [PunRPC]
    public override void RPCTakeDamage(float CurDamage, string Property)
    {
        base.RPCTakeDamage(CurDamage, Property);
    }
}
