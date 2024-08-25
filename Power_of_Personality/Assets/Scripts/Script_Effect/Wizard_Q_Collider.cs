using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Q_Collider : MonoBehaviour
{ 
    public GameObject QSkill_Collider;
    public float QSkill_zGrowthRate;
    public float maxDistance;

    void Start()
    {
        bool isHasSet = GetComponentInParent<ATKEffect_Control>().HasSet;
        float ColliderDistanceAdd = GetComponentInParent<ATKEffect_Control>().EffectDistanceAdd;

        if(isHasSet)
        {
            maxDistance = maxDistance * ColliderDistanceAdd;
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if (QSkill_Collider.transform.localScale.z < maxDistance + 2)
        {
            Vector3 newScale = QSkill_Collider.transform.localScale;
            newScale.z += QSkill_zGrowthRate * Time.deltaTime;
            QSkill_Collider.transform.localScale = newScale;
        }
    }
}
