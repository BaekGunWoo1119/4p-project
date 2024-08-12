using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;

    // Update is called once per frame
    void FixedUpdate()
    {
        timerText.text = Time.deltaTime.ToString();
    }
}
