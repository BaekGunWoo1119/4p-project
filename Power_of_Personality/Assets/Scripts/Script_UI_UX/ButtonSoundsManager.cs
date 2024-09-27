using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundsManager : MonoBehaviour
{
    public AudioSource audioSource; // 사운드를 재생할 AudioSource
    public AudioClip buttonClickSound; // 재생할 사운드 클립

    void Start()
    {
        // 모든 버튼의 클릭 이벤트에 리스너 추가
        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(PlayButtonSound);
        }
    }

    // 버튼 클릭 시 사운드 재생
    public void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
