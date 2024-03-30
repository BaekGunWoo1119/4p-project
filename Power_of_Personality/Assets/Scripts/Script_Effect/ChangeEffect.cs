using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffect : MonoBehaviour
{
    public GameObject fireCircle;
    public GameObject iceCircle;
    public GameObject fireSpin;
    public GameObject iceSpin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("property") == "Ice"
        && iceCircle != null && iceSpin != null)
        {
            StartCoroutine(Effect("Ice"));
        }
        else if(PlayerPrefs.GetString("property") == "Fire"
        && fireCircle != null && fireSpin != null)
        {
            StartCoroutine(Effect("Fire"));
        }
    }

    IEnumerator Effect(string EffectType)
    {
        if(EffectType == "Ice")
        {
            iceCircle.SetActive(true);
            fireCircle.SetActive(false);
            iceSpin.SetActive(true);
            fireSpin.SetActive(false);
        }
        else if(EffectType == "Fire")
        {
            iceCircle.SetActive(false);
            fireCircle.SetActive(true);
            iceSpin.SetActive(false);
            fireSpin.SetActive(true);
        }

        yield break;
    }
}
