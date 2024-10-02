using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonSoundsManager : MonoBehaviour
{
    private AudioSource audioSource; // 사운드를 재생할 AudioSource
    public AudioClip buttonClickSound; // 재생할 사운드 클립
    private static ButtonSoundsManager instance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 다른 SoundsManager 인스턴스가 존재하면, 자신을 파괴
        }
        audioSource = this.GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // 씬이 로드될 때 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬이 로드될 때마다 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {       
        // 씬 로드 시 아래 함수 실행
        BtnSoundPlus();
    }

    private void BtnSoundPlus()
    {
        Button[] allButtons = FindObjectsOfType<Button>();
        Toggle[] allToggles = FindObjectsOfType<Toggle>();

        // 모든 버튼의 클릭 이벤트에 리스너 추가
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(PlayButtonSound);
        }

        // 모든 토글의 클릭 이벤트에 리스너 추가
        foreach (Toggle toggle in allToggles)
        {
            toggle.onValueChanged.AddListener(delegate { PlayButtonSound(); });
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
