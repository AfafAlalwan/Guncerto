using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioClip[] cheerSounds;
    public AudioClip crowdIdle;
    public AudioClip applause;
    AudioSource audioSource;
    //public bool isIdlePlaying = true;
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
            //isIdlePlaying = false;

        }
        //else
        //{
        //    if (isIdlePlaying)
        //    {
        //        audioSource.clip = cheerSounds[cheerIndex];
        //        audioSource.Play();
        //        isIdlePlaying = false;
        //    }
        //}
        
    }
    //public void PlayIdleSound()
    //{
    //    if (!audioSource.isPlaying)
    //    {
    //        audioSource.clip = crowdIdle;
    //        audioSource.Play();
    //        isIdlePlaying = true;
    //    }
        

    //}
}
