using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeTester : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public Slider slider; // 참조할 Slider
    public AudioSource audioSource; // 사운드를 재생할 AudioSource

    private bool isPlaying = false; // 현재 사운드가 재생 중인지 확인

    void Start()
    {
        // AudioSource가 미리 할당되어 있지 않다면, 현재 오브젝트에서 AudioSource 컴포넌트를 찾음
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        StopSound();
        // Slider의 값이 변경될 때마다 호출되는 리스너 추가
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Slider 값이 변경되었을 때 호출
    private void OnSliderValueChanged(float value)
    {
        // 사운드의 볼륨을 Slider 값에 맞춰 조정
        audioSource.volume = value;

        // 사운드가 재생 중이 아니면 재생 시작
        if (!isPlaying)
        {
            PlaySound();
        }
    }

    // Slider 조작을 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 사운드가 재생 중이 아니면 재생 시작
        if (!isPlaying)
        {
            PlaySound();
        }
    }

    // Slider 조작을 멈췄을 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // 사운드가 재생 중이면 정지
        if (isPlaying)
        {
            StopSound();
        }
    }

    // 사운드를 재생하는 함수
    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    // 사운드를 정지하는 함수
    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}