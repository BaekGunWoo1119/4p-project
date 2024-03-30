using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Server_SwordAura_MoveForward : MonoBehaviour
{
    private float speed = 25f;
    private float SkillDuration;
    private Vector3 SkillDir;
    private GameObject EffectGen;

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
        if (SkillDuration > 1)
        {
            Destroy(this.gameObject);
            SkillDuration = 0;
        }
    }
}
