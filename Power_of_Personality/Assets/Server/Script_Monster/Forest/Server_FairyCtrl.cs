using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Server_FairyCtrl : Server_MonsterCtrl
{
    public Transform FirePos;
    public override void Awake()
    {
        ownWeakProperty ="Fire";
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
        yield return null;
    }
    public override void Attack_On(){
        atkAudio.PlayOneShot(atkAudio.clip); 
        StartCoroutine(Attack_On_2());
    }
    public override IEnumerator Attack_On_2(){
        Instantiate(AttackCollider, FirePos.position, FirePos.rotation);
        yield return new WaitForSeconds(0.5f*(1f/AnimSpeed));
        anim.SetBool("isAttack", false);
        AttackCoolTime = 0;
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
        return base.GetHPBarPosition(); // ���ϴ� ��ġ�� ����
    }
    public override IEnumerator DamageTextAlpha(float CurDamage)
    {
        yield return base.DamageTextAlpha(CurDamage);
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
    [PunRPC]
    public override void RPCDamage(float CurDamage){
        base.RPCDamage(CurDamage);
    }
}
