using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCtrl : MonoBehaviour
{
    private TextMeshProUGUI CoinText;

    private void Start()
    {
        PlayerPrefs.SetFloat("Coin", 0);
        CoinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            float currentCoin = PlayerPrefs.GetFloat("Coin", 0);
            PlayerPrefs.SetFloat("Coin", currentCoin + 1);
            Debug.Log("���� = " + PlayerPrefs.GetFloat("Coin"));
            CoinText.text = PlayerPrefs.GetFloat("Coin").ToString();
        }
    }
}
