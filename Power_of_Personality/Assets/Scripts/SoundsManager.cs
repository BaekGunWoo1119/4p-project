using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource audioSource; // 재생할 오디오 소스
    public static float loopStartTime; // 반복 시작 지점 (초 단위)
    public static float loopEndTime;  // 반복 끝 지점 (초 단위)
    public float fadeDuration = 0.5f;  // 페이드 지속 시간 (초 단위)
    public string musicName;
    private static SoundsManager instance;
    public AudioClip[] cilpSource;

    void Awake()
    {
        Sound_Check();
    }

    // 오디오 번호 0번 : 메인배경음 / 1번 : 숲 배경음 / 2번 : 숲 보스음 / 3번 : 동굴 / 4번 : 하수구 (10.02)
    void Start()
    {
        if(musicName == "MainBGM")
        {
            loopStartTime = 0;
            loopEndTime = 137f;
        }

        if(musicName == "Forest")
        {
            loopStartTime = 0;
            loopEndTime = 167f;
        }

        if(musicName == "Forest_Boss")
        {
            loopStartTime = 1f;
            loopEndTime = 95f;
        }

        if(musicName == "Cave")
        {
            loopStartTime = 16f;
            loopEndTime = 55f;
        }

        if(musicName == "Cave_Boss")
        {
            loopStartTime = 1f;
            loopEndTime = 95f;
        }

        if(musicName == "Sewer")
        {
            loopStartTime = 0;
            loopEndTime = 63f;
        }

        if(musicName == "Sewer_Boss")
        {
            loopStartTime = 80f;
            loopEndTime = 152f;
        }
        // 처음부터 특정 구간만 반복 재생 및 페이드 아웃
        StartCoroutine(PlayLoopAudio());
    }

    void Update()
    {
        Sound_Check(); 
    }

    void Sound_Check()
    {
        if(musicName == "MainBGM")
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject); // 이미 다른 SoundsManager 인스턴스가 존재하면, 자신을 파괴
            }
        }
        else if(musicName == "Forest" || musicName == "Cave" || musicName == "Sewer")
        {
            instance = this;
        }
    }

    public static void Change_Sounds(string soundsName)
    {
       instance.StartCoroutine(instance.Fade_Change(soundsName));
    }

    IEnumerator PlayLoopAudio()
    {
        if(musicName == "Forest" || musicName == "Cave" || musicName == "Sewer")
        {
            audioSource.Play();
        }
        else
        {
            audioSource.time = loopStartTime;
            audioSource.Play();
        }
        yield return new WaitForSeconds(loopStartTime);
        while (true)
        {
            // 지정된 시작 지점으로 이동
            audioSource.time = loopStartTime;
            audioSource.Play();

            // 지정된 끝 지점까지 재생
            yield return new WaitForSeconds(loopEndTime - loopStartTime);

            // 오디오 일시 정지
            audioSource.Pause();
        }
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Pause();
    }

    IEnumerator FadeIn()
    {
        float targetVolume = 1;
        audioSource.volume = 0;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public IEnumerator Fade_Change(string soundsName)
    {
        // 페이드 아웃
        StartCoroutine(FadeOut());

        // 페이드 아웃이 끝나길 기다림
        yield return new WaitForSeconds(fadeDuration);

        //오디오 소스 변경
        musicName = soundsName;
        if(musicName == "Forest_Boss")
        {
            audioSource.clip = cilpSource[2];
            loopStartTime = 1f;
            loopEndTime = 95f;
        }
        else if(musicName == "Forest")
        {
            audioSource.clip = cilpSource[1];
            loopStartTime = 0f;
            loopEndTime = 167f;
        }
        else if(musicName == "Cave_Boss")
        {
            audioSource.clip = cilpSource[3];
            loopStartTime = 56f;
            loopEndTime = 159f;
        }
        else if(musicName == "Cave")
        {
            audioSource.clip = cilpSource[3];
            loopStartTime = 16f;
            loopEndTime = 55f;
        }
        else if(musicName == "Sewer")
        {
            audioSource.clip = cilpSource[4];
            loopStartTime = 0;
            loopEndTime = 63f;
        }
        else if(musicName == "Sewer_Boss")
        {
            audioSource.clip = cilpSource[4];
            loopStartTime = 80f;
            loopEndTime = 152f;
        }

        //음악 재생
        StartCoroutine(PlayLoopAudio());

        // 페이드 인
        StartCoroutine(FadeIn());
    }
}
