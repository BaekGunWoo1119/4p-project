using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeeCtrl : MonsterCtrl
{
    public GameObject AttackEffect;
    public GameObject EffectGen;
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
        Debug.Log(curHP);
        Debug.Log(PlayerPrefs.GetString("property"));
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

    public override IEnumerator TakeDamage()
    {
        yield return base.TakeDamage();
    }
    public override Vector3 GetHPBarPosition()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        return base.GetHPBarPosition(); // 원하는 위치로 수정
    }
    public override IEnumerator DamageTextAlpha()
    {
        yield return base.DamageTextAlpha();
=======
        return base.GetHPBarPosition(); // ���ϴ� ��ġ�� ����
=======
        return base.GetHPBarPosition(); // 원하는 위치로 수정
>>>>>>> origin/JDH
    }
    public void Attack_On()
    {
        GameObject effect_on = Instantiate(AttackEffect, EffectGen.transform.position, EffectGen.transform.rotation);
        Destroy(effect_on,3f);
<<<<<<< HEAD
>>>>>>> 69ad485245a5502374f104c1b296884cf76f889a
=======
>>>>>>> origin/JDH
    }
}
