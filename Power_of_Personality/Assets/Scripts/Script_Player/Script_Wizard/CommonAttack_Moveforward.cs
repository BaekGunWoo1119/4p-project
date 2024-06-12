using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CommonAttack_Moveforward : MonoBehaviour
{
    private float speed = 10f;
    private float SkillDuration;
    private Vector3 SkillDir;
    private GameObject EffectGen;
    private float Damage = 10.0f;

    void Start()
    {
        SkillDuration = 0;
        EffectGen = GameObject.Find("EffectGen");
        SkillDir = EffectGen.transform.forward;
    }

    void Update()
    {
        transform.Translate(SkillDir * speed * Time.deltaTime);
        SkillDuration = SkillDuration + Time.deltaTime;
        if (SkillDuration > 1.5)
        {
            Destroy(this.gameObject);
            SkillDuration = 0;
        }
    }
}
