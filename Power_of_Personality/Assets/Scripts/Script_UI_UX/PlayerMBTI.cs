using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMBTI : MonoBehaviour
{
    private TMP_Text MBTI1;
    private TMP_Text MBTI2;

    // Start is called before the first frame update
    void Start()
    {
        MBTI1 = GameObject.Find("Player-MBTI").GetComponent<TMP_Text>();
        MBTI2 = GameObject.Find("MBTI-Out").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MBTI1.text = PlayerPrefs.GetString("PlayerMBTI");
        MBTI2.text = PlayerPrefs.GetString("PlayerMBTI");
    }
}
