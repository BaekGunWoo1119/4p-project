using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FlowerCheck : MonoBehaviour
{
    public GameObject Posion;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            StartCoroutine(PosionOn());
        }
    }

    public IEnumerator PosionOn()
    {   
        yield return new WaitForSeconds(1f);
        Posion.SetActive(true);
        yield return new WaitForSeconds(3f);
        Posion.SetActive(false);
    }
}
