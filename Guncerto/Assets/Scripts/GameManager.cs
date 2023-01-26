using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool win;
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
            //else
            //{
            //    songManager.enabled = false;
            //}
            
        }
    }
    public void GameOver()
    {
        if (win)
        {

        }
        leaderBoard.SubmitButton();

        songManager.enabled = false;

        gameAudoManger.Stop();
        gameIdleAudoManger.Stop();
        MissTrigger.SetActive(false);

        audioManager.PlayBooSound();
    }
    
}
