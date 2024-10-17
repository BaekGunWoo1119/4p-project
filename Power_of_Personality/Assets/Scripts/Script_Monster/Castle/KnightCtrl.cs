using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;

public class KnightCtrl : MonsterCtrl
{
    public GameObject AttackEffect1;
    public GameObject AttackEffect2;
    public GameObject AttackEffect3;
    public override void Awake()
    {
        ownWeakProperty = "Ice";
        maxHP = 1000f;
        ATK = 15.0f;
        DEF = 200f;
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
        Distance = Vector3.Distance(transform.position, PlayerTr.position);

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit && !isSpawn&&!anim.GetBool("isAttack"))
        {
            anim.SetBool("isRun", true);
            StartCoroutine(Trace());
        }
        if(Distance <= attackRadius){
            anim.SetBool("isRun", false);
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
    public override IEnumerator TakeDamage(float Damage)
    {
        yield return base.TakeDamage(Damage);
    }
    public override Vector3 GetHPBarPosition()
    {
        return base.GetHPBarPosition(); // 원하는 위치로 수정
    }
    public override IEnumerator DamageTextAlpha(float Damage)
    {
        yield return base.DamageTextAlpha(Damage);
    }

    public override void Attack_On()
    {
        if(EffectGen != null && AttackEffect != null)
        {
            AttackCollider.SetActive(false);
            AttackCollider.SetActive(true);
            atkAudio.PlayOneShot(atkAudio.clip); //공격 시 재생 오디오 재생(09.30)
            GameObject effect_on = Instantiate(AttackEffect, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
        }
    }
    public void ComboAttack_On_1(){
        if(EffectGen != null && AttackEffect != null)
        {
            AttackCollider.SetActive(false);
            AttackCollider.SetActive(true);
            atkAudio.PlayOneShot(atkAudio.clip); //공격 시 재생 오디오 재생(09.30)
            GameObject effect_on = Instantiate(AttackEffect1, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
        }
    }
    public void ComboAttack_On_2(){
        if(EffectGen != null && AttackEffect != null)
        {
            AttackCollider.SetActive(false);
            AttackCollider.SetActive(true);
            atkAudio.PlayOneShot(atkAudio.clip); //공격 시 재생 오디오 재생(09.30)
            GameObject effect_on = Instantiate(AttackEffect2, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
        }
    }
    public void ComboAttack_On_3(){
        if(EffectGen != null && AttackEffect != null)
        {
            AttackCollider.SetActive(false);
            AttackCollider.SetActive(true);
            atkAudio.PlayOneShot(atkAudio.clip); //공격 시 재생 오디오 재생(09.30)
            GameObject effect_on = Instantiate(AttackEffect3, EffectGen.transform.position, EffectGen.transform.rotation);
            Destroy(effect_on, 3f);
        }
    }
    public void Attack_Off()
    {
        anim.SetBool("isAttack", false);
        AttackCoolTime = 0;
    }
    public void Attack_Off_2(){
        AttackCollider.SetActive(false);
    }
}
