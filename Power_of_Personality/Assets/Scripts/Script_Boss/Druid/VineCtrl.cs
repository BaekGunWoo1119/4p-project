using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineCtrl : MonoBehaviour
{
    public GameObject Vine_Collider;
    public float Vine_xGrowthRate = 10;
    public float maxDistance;

    public float ATK = 20;

    void Update()
    {
        if (Vine_Collider.transform.localScale.x <= maxDistance)
        {
            Vector3 newScale = Vine_Collider.transform.localScale;
            newScale.x += Vine_xGrowthRate * Time.deltaTime;
            Vine_Collider.transform.localScale = newScale;
        }    
    }
}
