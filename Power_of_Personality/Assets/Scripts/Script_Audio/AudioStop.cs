using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStop : MonoBehaviour
{
    private AudioSource thisAudio;
    void Start()
    {
        thisAudio = this.GetComponent<AudioSource>();
        thisAudio.Stop();
    }
}
