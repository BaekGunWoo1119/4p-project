using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre_RaiseTentacle : MonoBehaviour
{
    public GameObject Tentacle_Collider;
    public float Tentacle_zGrowthRate = 10;
    public float maxScale;
    public float maxDistance;
    private Vector3 startPosition;
    

    public float ATK = 30;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Tentacle_Collider.transform.localScale.z <= maxScale)
        {
            Vector3 newScale = Tentacle_Collider.transform.localScale;
            newScale.z += Tentacle_zGrowthRate * Time.deltaTime;
            Tentacle_Collider.transform.localScale = newScale;
        } 
        else if(Vector3.Distance(startPosition, transform.position) < maxDistance -maxScale/2)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Tentacle_zGrowthRate);
        }    
    }
}
