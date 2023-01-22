using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
        
    }
    public void GameOver()
    {
        leaderBoard.SubmitButton();

        songManager.enabled = false;

        MissTrigger.SetActive(false);
    }
    
}
