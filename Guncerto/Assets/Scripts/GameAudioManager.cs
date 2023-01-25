using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    int scratchTimes;
    public ScoreManager scoreManager;
    public AudioClip[] cheerSounds;
    public AudioClip booClip;
    public AudioClip applause;
    public AudioClip recordScratchClip;
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
    public void PlayRecordScratch()
    {
        StartCoroutine(PlaySong());
    }
    IEnumerator PlaySong()
    {
        audioSource.clip = recordScratchClip;
        audioSource.Play();
        scratchTimes++;
        if (scoreManager.miss / 2 == 3)
        {
            yield return new WaitForSeconds(recordScratchClip.length);
            PlayBooSound();
        }
        
    }

}
