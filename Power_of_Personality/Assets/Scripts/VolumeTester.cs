using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeTester : MonoBehaviour//, IBeginDragHandler, IEndDragHandler
{
    /* 잦은 오류로 그냥 해당 코드 안쓰기로 함. 기술적인 문제 X(09.21)
    public Slider slider; // 참조할 Slider
    public AudioSource audioSource; // 사운드를 재생할 AudioSource
    public AudioClip soundClip; // 재생할 사운드 클립

    private bool isPlaying = false; // 현재 사운드가 재생 중인지 확인

    void Start()
    {
        // AudioSource가 미리 할당되어 있지 않다면, 현재 오브젝트에서 AudioSource 컴포넌트를 찾음
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // AudioClip이 할당되어 있는지 확인
        if (soundClip == null)
        {
            Debug.LogError("SoundClip이 할당되지 않았습니다.");
        }

        // Slider가 할당되어 있는지 확인하고, 값 변경 리스너 추가
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("Slider가 할당되지 않았습니다.");
        }

        StopSound();
    }

    // Slider 값이 변경되었을 때 호출
    private void OnSliderValueChanged(float value)
    {
        // 사운드의 볼륨을 Slider 값에 맞춰 조정
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }

    // Slider 조작을 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 사운드가 재생 중이 아니면 재생 시작
        if (!isPlaying && audioSource != null && soundClip != null)
        {
            PlaySound();
        }
    }

    // Slider 조작을 멈췄을 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // 사운드가 재생 중이면 정지
        if (isPlaying && audioSource != null)
        {
            StopSound();
        }
    }

    // 사운드를 재생하는 함수
    private void PlaySound()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
            isPlaying = true;
            Debug.Log("Sound Started");
        }
    }

    // 사운드를 정지하는 함수
    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isPlaying = false;
            Debug.Log("Sound Stopped");
        }
    }
    */
}