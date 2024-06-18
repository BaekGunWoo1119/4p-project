using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Attack_MoveForward : MonoBehaviour
{
    Rigidbody rb;
    private float SkillYRot;
    public float speed = 10f;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        SkillYRot = players[0].transform.eulerAngles.y;
    }

    void Update()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            Vector3 getVel = this.transform.right * speed;
            rb.velocity = getVel;
        }
        else{
            Vector3 getVel = this.transform.right * speed;
            rb.velocity = getVel;
        }
    }
}
