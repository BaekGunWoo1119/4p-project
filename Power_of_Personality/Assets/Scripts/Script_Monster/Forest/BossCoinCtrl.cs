using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCoinCtrl : MonoBehaviour
{
    private TextMeshProUGUI CoinText;
    private AudioSource audio;

    private void Start()
    {
        CoinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        audio = GameObject.Find("CoinText").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            audio.PlayOneShot(audio.clip);
            Destroy(this.gameObject);
            float currentCoin = PlayerPrefs.GetFloat("Coin", 0);
            PlayerPrefs.SetFloat("Coin", currentCoin + 20);
        }
    }
}
