using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    int doOnce = 0;
    [SerializeField] Animator speakers;
    //reference to Game Manager to call GameOver() function.
    [SerializeField] GameManager gameManager;

    //an AudioSource attached to this GameObject that will play the music.
    [SerializeField] AudioSource musicSource;

    //the current position of the song (in seconds)
    [SerializeField] public float songPosition;

    //the current position of the song (in beats)
    [SerializeField] float songPosInBeats;

    //the duration of a beat
    [SerializeField] float secPerBeat;

    //how much time (in seconds) has passed since the song started
    [SerializeField] float dsptimesong;

    //beats per minute of a song
    [SerializeField] float bpm;

    //keep all the position-in-beats of notes in the song
    [SerializeField] float[] notes;

    //the index of the next note to be spawned
    int nextIndex = 0;

    //The offset to the first beat of the song in seconds
    [SerializeField] float firstBeatOffset;

    [SerializeField] float beatsShownInAdvance;

    TargetSpawner targetSpawner;

    [SerializeField] float songDuration;

 
    void Start()
    {
        targetSpawner = GetComponent<TargetSpawner>();
        musicSource = GetComponent<AudioSource>();

        //calculate how many seconds is one beat
        //we will see the declaration of bpm later
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        var guns = FindObjectsOfType<Gun>();
        foreach (var gun in guns)
        {
            gun.UIMode = false;
        }

        speakers.SetFloat("bpm", bpm / 120f);

        //start the song
        musicSource.Play();

    }

    void Update()
    {
        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dsptimesong - firstBeatOffset);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;

        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            // Instantiate here
            targetSpawner.SpawnTarget();
            nextIndex++;
        }

        if (songPosition >= songDuration)
        {
            //gameManager.isGameOver = true;
            if (doOnce == 0)
            {
                gameManager.GameOver("Win");
                doOnce++;
            }
            StartCoroutine(LoadHomeScene());
            //call game over -giray
        }
    }

    public IEnumerator LoadHomeScene()
    {
        
        yield return new WaitForSeconds(3);
        
        Loader.Instance.LoadScene("Home Scene");
    }
}
