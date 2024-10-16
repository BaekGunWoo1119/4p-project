using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wizard_CommonAttack_Moveforward : MonoBehaviour
{
    private float speed = 10f;
    private float SkillDuration;
    private Vector3 SkillDir;
    private float Damage = 10.0f;

    void Start()
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
