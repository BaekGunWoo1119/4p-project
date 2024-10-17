using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_on : MonoBehaviour
{
    public float Delay;
    public GameObject ActiveObject;


    void Start()
    {
        StartCoroutine(ActivateObject());
    }

    IEnumerator ActivateObject()
    {
        yield return new WaitForSeconds(Delay);

        ActiveObject.SetActive(true);
    }
}
