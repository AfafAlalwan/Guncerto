using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioClip[] cheerSounds;
    public AudioClip booClip;
    public AudioClip applause;
    AudioSource audioSource;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    public void PlayCheerSound(int cheerIndex)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = cheerSounds[cheerIndex];
            audioSource.Play();

        }       
    }
    public void PlayBooSound()
    {
        audioSource.clip = booClip;
        audioSource.Play();
    }

}
