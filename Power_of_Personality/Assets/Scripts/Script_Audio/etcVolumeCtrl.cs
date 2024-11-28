using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // AudioMixer를 사용하기 위해 필요
using UnityEngine.UI;

public class etcVolumeCtrl : MonoBehaviour
{
    private AudioSource thisAudio;
    private Slider musicVolumeSlider; // Music 트랙을 제어할 Slider 참조

    // Start is called before the first frame update
    void Start()
    {
        thisAudio = this.GetComponent<AudioSource>();
        musicVolumeSlider = GameObject.FindWithTag("SoundEffect").GetComponent<Slider>();
        thisAudio.volume = musicVolumeSlider.value;
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }

    public void UpdateMusicVolume(float sliderValue)
    {
        thisAudio.volume = sliderValue;
    }
}
