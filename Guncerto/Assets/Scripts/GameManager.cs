using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Volume volume;
    public GameAudioManager audioManager;
    public AudioSource gameAudoManger;
    public AudioSource gameIdleAudoManger;
    public AudioSource audioSource;
    public List<float> scores = new List<float>();
    public GameObject MissTrigger;
    public List<Text> LeaderTexts = new List<Text>();
    public bool isGameOver = false;
    public Text leaderboardText;
    public ScoreManager scoreManager;
    public Keyboard keyboard;
    public SongManager songManager;
    public LeaderBoard leaderBoard;
    void Start()
    {

        //PlayerPrefs.DeleteAll();
        //GetHighScores();
    }

    void Update()
    {
        if (isGameOver)
        {
            if (audioSource.pitch >= 0)
            {
                audioSource.pitch -= 0.3f * Time.deltaTime;
            }
            if (volume.profile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value += 0.3f * Time.deltaTime;
            }
            //else
            //{
            //    songManager.enabled = false;
            //}

        }
    }
    public void GameOver(string winOrLose)
    {
        isGameOver = true;
        if (winOrLose == "Win")
        {
            //play clap sound

        }
        else if (winOrLose == "Lose")
        {
            songManager.enabled = false;
            gameAudoManger.Stop();
            gameIdleAudoManger.Stop();
            MissTrigger.SetActive(false);
            audioManager.PlayBooSound();
            songManager.StartCoroutine(songManager.LoadHomeScene());
        }
        leaderBoard.SubmitButton();
        
    }
    
}
