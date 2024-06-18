using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Server_BeeCtrl : Server_MonsterCtrl
{
    public override void Awake()
    {
        WeakProperty = "Fire";
        ATK = 10.0f;
        DEF = 100;
        MoveSpeed = 2.0f;
        Damage = 10.0f;
        TraceRadius = 10.0f;
        attackRadius = 3.0f;
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
        yield return base.Attack();
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }
    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
    }
    public override IEnumerator TakeDamage(float CurDamage, string Property)
    {
        yield return base.TakeDamage(CurDamage, Property);
    }
    public override Vector3 GetHPBarPosition()
    {
        return base.GetHPBarPosition(); // 원하는 위치로 수정
    }
    public override IEnumerator DamageTextAlpha()
    {
        yield return base.DamageTextAlpha();
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
