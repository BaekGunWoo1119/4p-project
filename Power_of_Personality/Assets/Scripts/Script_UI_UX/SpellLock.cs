using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLock : MonoBehaviour
{
    public string PlayMode;
    public GameObject[] Spell;

    // Start is called before the first frame update
    void Start()
    {
        PlayMode = PlayerPrefs.GetString("PlayMode");
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayMode == "Single"){
            Spell[7].SetActive(true);
        }
        else if(PlayMode == "Multi"){
            Spell[5].SetActive(true);
        }
    }
}
