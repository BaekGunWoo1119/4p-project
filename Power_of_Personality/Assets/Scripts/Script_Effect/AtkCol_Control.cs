using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCol_Control : MonoBehaviour
{
    public GameObject MainEffect;
    public GameObject ATKCollider;
    public float Delay;
    public float DelTime;

    public float Max_ATKtime;
    private float ATKtime;

    void Start()
    {
        StartCoroutine(ActivateObject());
        StartCoroutine(DeactivateObject());
    }

    IEnumerator ActivateObject()
    {
        yield return new WaitForSeconds(Delay);

        ATKCollider.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(DelTime);

        ATKCollider.GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(ATKtime < Max_ATKtime)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                ATKtime++;
                Debug.Log(ATKtime);
            }
        } 
        else if(ATKtime == Max_ATKtime)
        {
            Debug.Log("삭제");
            Destroy(ATKCollider);
        }
    }
}
