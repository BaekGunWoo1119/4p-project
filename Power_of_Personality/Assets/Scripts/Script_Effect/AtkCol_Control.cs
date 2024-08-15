using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCol_Control : MonoBehaviour
{
    public GameObject targetObject;
    public float Delay;
    public float DelTime;

    void Start()
    {
        StartCoroutine(ActivateObject());
        StartCoroutine(DeactivateObject());
    }

    IEnumerator ActivateObject()
    {
        yield return new WaitForSeconds(Delay);

        targetObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(DelTime);

        targetObject.GetComponent<BoxCollider>().enabled = false;
    }
}
