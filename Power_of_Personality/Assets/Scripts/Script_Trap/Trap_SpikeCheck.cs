using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_SpikeCheck : MonoBehaviour
{
    public GameObject Spike;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            StartCoroutine(SpikeUp());
        }
    }

    public IEnumerator SpikeUp()
    {
        yield return new WaitForSeconds(0.75f);
        Spike.SetActive(true);
        Spike.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        yield return new WaitForSeconds(2f);
        Spike.transform.position = new Vector3(this.transform.position.x, -2, this.transform.position.z);
        Spike.SetActive(false);
    }

}
