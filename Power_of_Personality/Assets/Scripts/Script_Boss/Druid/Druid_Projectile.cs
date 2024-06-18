using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid_Projectile : MonoBehaviour
{
    private float SkillDuration;
    private GameObject EffectGen;
    private Rigidbody rb;
    public float ATK = 10;

    private void Awake()
    {
        EffectGen = GameObject.Find("EffectGen-Boss");
        rb = GetComponent<Rigidbody>();
        rb.AddForce(EffectGen.transform.forward.normalized * 10, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        SkillDuration = SkillDuration + Time.deltaTime;
        if (SkillDuration > 3)
        {
            Destroy(this.gameObject);
            SkillDuration = 0;
        }
    }
}
