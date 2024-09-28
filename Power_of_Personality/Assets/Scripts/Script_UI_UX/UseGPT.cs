using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseGPT : MonoBehaviour
{
    public Toggle useGPT;
    public void Start()
    {
        useGPT.onValueChanged.AddListener(setUseGPT);
    }
    public void setUseGPT(bool isOn)
    {
        if(isOn)
        {
            PlayerPrefs.SetInt("UseGPT", 1);
            Debug.Log("UseGPT 값은 다음과 같습니다. = " + PlayerPrefs.GetInt("UseGPT"));
        }
        else
        {
            PlayerPrefs.SetInt("UseGPT", 0);
            Debug.Log("UseGPT 값은 다음과 같습니다. = " + PlayerPrefs.GetInt("UseGPT"));
        }
    }
}
