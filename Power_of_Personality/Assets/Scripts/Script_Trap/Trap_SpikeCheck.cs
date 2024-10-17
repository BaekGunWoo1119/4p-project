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
        Spike.transform.localPosition = new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z);
        yield return new WaitForSeconds(2f);
        Spike.transform.localPosition = new Vector3(this.transform.localPosition.x, -2, this.transform.localPosition.z);
        Spike.SetActive(false);
    }

}
