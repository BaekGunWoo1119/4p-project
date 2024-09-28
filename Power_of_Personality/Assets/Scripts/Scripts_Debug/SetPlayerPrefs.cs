using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("BonusStat",10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
