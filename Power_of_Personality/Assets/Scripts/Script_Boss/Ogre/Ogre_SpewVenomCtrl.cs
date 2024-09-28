using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre_SpewVenomCtrl : MonoBehaviour
{
    public float Delay;
    public GameObject ATKCollider;
    public float ATK = 30;


    void Start()
    {
        StartCoroutine(ActivateObject());
    }

    IEnumerator ActivateObject()
    {
        yield return new WaitForSeconds(Delay);

        ATKCollider.GetComponent<BoxCollider>().enabled = true;
    }
    
}
