using UnityEngine;
using UnityEngine.Audio; // AudioMixer를 사용하기 위해 필요
using UnityEngine.UI; // Slider를 사용하기 위해 필요

public class SoundsVolume : MonoBehaviour
{
    public AudioMixer audioMixer; // Audio Mixer 참조
    public Slider musicVolumeSlider; // Music 트랙을 제어할 Slider 참조
    public string[] musicExposedParameter; // Music 트랙의 Exposed Parameter 이름

    void Start()
    {
        // Slider의 값이 변경될 때마다 UpdateMusicVolume 함수 호출
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        
        // Slider의 초기값을 Audio Mixer의 현재 Music 볼륨 값으로 설정
        float currentMusicVolume;
        for(int i = 0; i < musicExposedParameter.Length; i++ )
        {
            if (audioMixer.GetFloat(musicExposedParameter[i], out currentMusicVolume))
            {
                // dB 값을 0~1 범위의 Slider 값으로 변환
                musicVolumeSlider.value = Mathf.Pow(10, currentMusicVolume / 20); 
            }
        }
    }

    // Slider 값 변경에 따라 Music 트랙의 볼륨 업데이트
    public void UpdateMusicVolume(float sliderValue)
    {
        float volumeInDb;

        // Slider 값이 0일 때 -80dB로 설정
        if (sliderValue == 0)
        {
            volumeInDb = -80f; // 최소 볼륨
        }
        else
        {
            // Slider 값(0~1)을 dB 값(-80dB ~ 0dB)으로 변환
            volumeInDb = Mathf.Log10(sliderValue) * 20;
        }

        for (int i = 0; i < musicExposedParameter.Length; i++)
        {
            audioMixer.SetFloat(musicExposedParameter[i], volumeInDb);
        }
    }
}
