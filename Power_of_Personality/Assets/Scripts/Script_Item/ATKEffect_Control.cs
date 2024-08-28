using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKEffect_Control : MonoBehaviour
{
    public bool isCommon;
    public GameObject MainEffect;
    public bool isProjectile;
    public GameObject Projectile;
    
    public float EffectSizeAdd;
    public float EffectDistance;
    public float EffectDistanceAdd;

    void Start()
    {
        if(Status.set5_3_Activated)
        {
            if(isCommon)
            {
                MainEffect.transform.localScale = new Vector3(EffectSizeAdd, EffectSizeAdd, EffectSizeAdd);

                if(isProjectile)
                {
                    EffectDistance = Projectile.GetComponent<ObjectMoveDestroy_Distance>().maxDistance;
                    Projectile.GetComponent<ObjectMoveDestroy_Distance>().maxDistance = EffectDistance * EffectDistanceAdd;
                }
            }
        }

        if(Status.set6_3_Activated)
        {
            if(!isCommon)
            {
                MainEffect.transform.localScale = new Vector3(EffectSizeAdd, EffectSizeAdd, EffectSizeAdd);

                if(isProjectile)
                {
                    EffectDistance = Projectile.GetComponent<ObjectMoveDestroy_Distance>().maxDistance;
                    Projectile.GetComponent<ObjectMoveDestroy_Distance>().maxDistance = EffectDistance * EffectDistanceAdd;
                }
            }
        }
    }
}
