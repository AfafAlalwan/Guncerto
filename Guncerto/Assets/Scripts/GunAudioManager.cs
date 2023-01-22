using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioManager : MonoBehaviour
{
    public AudioClip[] pistolSounds;
    public AudioClip shotgunSound;
    public bool isShotgun;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {       
        if (isShotgun)
        {
            audioSource.clip = shotgunSound;
            audioSource.Play();
        }
        else
        {
            int rand = Random.Range(0, pistolSounds.Length);
            audioSource.clip = pistolSounds[rand];
            audioSource.Play();
        }
    }
}
