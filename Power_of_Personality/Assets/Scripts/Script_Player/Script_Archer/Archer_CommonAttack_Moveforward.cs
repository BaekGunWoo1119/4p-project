using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer_CommonAttack_Moveforward : MonoBehaviour
{
    private float speed = 30f;
    private float SkillDuration;
    private Vector3 SkillDir;

    void Awake()
    {
        SkillDuration = 0;
        SkillDir = new Vector3(0, 0, 1);
    }

    void Update()

    {
        transform.Translate(-SkillDir * speed * Time.deltaTime);
        SkillDuration = SkillDuration + Time.deltaTime;
        if (SkillDuration > 1.5)
        {
            Destroy(this.gameObject);
            SkillDuration = 0;
        }
    }
}
