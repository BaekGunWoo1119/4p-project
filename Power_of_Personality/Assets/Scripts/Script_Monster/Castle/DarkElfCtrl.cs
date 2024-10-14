using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;

public class DarkElfCtrl : MonsterCtrl
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
    public override void Start()
    {
        base.Start();
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
        Distance = Vector3.Distance(transform.position, PlayerTr.position);

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit && !isSpawn)
        {
            anim.SetBool("isRun", true);
            StartCoroutine(Trace());
        }

        if (Distance <= attackRadius && AttackCoolTime >= 3.0f*(1f/AnimSpeed) && !isDie && hitCount <= 0 && !isSpawn)
        {
            anim.SetBool("isRun", false);
            StartCoroutine(Attack());
        }
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
        return base.GetHPBarPosition(); // ���ϴ� ��ġ�� ����
    }
    public override IEnumerator DamageTextAlpha(float Damage)
    {
        yield return base.DamageTextAlpha(Damage);
    }
    public override IEnumerator Attack_On_2(){
        Instantiate(AttackCollider, FirePos.position, FirePos.rotation, this.transform);
        yield return new WaitForSeconds(0.5f*(1f/AnimSpeed));
        anim.SetBool("isAttack", false);
        AttackCoolTime = 0;
    }
}
