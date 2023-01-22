using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo
{
    public string name;
    public float score;
    public PlayerInfo(string name, float score)
    {
        this.name = name;
        this.score = score;
    }
}
public class LeaderBoard : MonoBehaviour
{
    public Text leaderboardText;
    public Keyboard keyboard;
    public ScoreManager scoreManager;

    List<PlayerInfo> collectedStats;

    void Start()
    {
        collectedStats = new List<PlayerInfo>();
        LoadLeaderboard();
        //PlayerPrefs.DeleteAll();
        //leaderboardText.text = "";
    }

    void Update()
    {
        
    }

    public void SubmitButton()
    {
        PlayerInfo stats = new PlayerInfo(keyboard.newName, scoreManager.score);

        collectedStats.Add(stats);

        SortStats();
    }

    void SortStats()
    {
        for (int i = collectedStats.Count - 1; i > 0; i--)
        {
            if (collectedStats[i].score > collectedStats[i - 1].score)
            {
                PlayerInfo tempInfo = collectedStats[i - 1];

                collectedStats[i - 1] = collectedStats[i];

                collectedStats[i] = tempInfo;
            }
        }

        UpdatePlayerPrefsString();
    }

    void UpdatePlayerPrefsString()
    {
        string stats = "";

        for (int i = 0; i < collectedStats.Count; i++)
        {
            stats += collectedStats[i].name + ",";
            stats += collectedStats[i].score + ",";
        }

        PlayerPrefs.SetString("LeaderBoards", stats);

        UpdateLeaderBoardVisual();
    }

    void UpdateLeaderBoardVisual()
    {
        leaderboardText.text = "";

        for (int i = 0; i <= collectedStats.Count - 1; i++)
        {
            leaderboardText.text += collectedStats[i].name + " - " + collectedStats[i].score + "\n";
        }
    }

    void LoadLeaderboard()
    {
        string stats = PlayerPrefs.GetString("LeaderBoards", "");

        string[] stats2 = stats.Split(',');

        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
            PlayerInfo loadedInfo = new PlayerInfo(stats2[i], float.Parse(stats2[i + 1]) );

            collectedStats.Add(loadedInfo);

            UpdateLeaderBoardVisual();
        }
    }
}
