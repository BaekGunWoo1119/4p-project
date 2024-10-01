using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orge_StraightTentacleCtrl : MonoBehaviour
{
    public GameObject Tentacle_Collider;
    public float Tentacle_zGrowthRate = 10;
    public float maxDistance;

    public float ATK = 30;

    void Update()
    {
        if (Tentacle_Collider.transform.localScale.z <= maxDistance)
        {
            Vector3 newScale = Tentacle_Collider.transform.localScale;
            newScale.z += Tentacle_zGrowthRate * Time.deltaTime;
            Tentacle_Collider.transform.localScale = newScale;
        }    
    }
}
