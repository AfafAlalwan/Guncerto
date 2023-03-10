using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public List<GameObject> missObjects = new List<GameObject>();
    public GameAudioManager gameAudioManager;

    public GameManager gameManager;
    public float difficultyScale;

    public Text scoreText;
    public float score;

    public float combo;
    public Text comboText;

    public int miss = 0;
    public int maxMiss = 6;
    public Text missText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        combo = 1;
        //gameAudioManager.PlayIdleSound();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        missText.text = (miss/2).ToString();
    }
    public void AddScore(int scoreToAdd)
    {
        
        if (combo >= 1 && combo <= 10)
        {
            score += scoreToAdd * 1 * difficultyScale;
            
        }
        else if (combo > 10 && combo <= 20)
        {
            score += scoreToAdd * 2 * difficultyScale;
           
        }
        else if (combo > 20 && combo <= 30)
        {
            score += scoreToAdd * 3 * difficultyScale;
           
        }
        else if (combo > 30)
        {
            score += scoreToAdd * 4 * difficultyScale;
        }
        if (combo == 10)
        {
            gameAudioManager.PlayCheerSound(0);
        }
        else if (combo == 20)
        {
            gameAudioManager.PlayCheerSound(1);
        }
        else if (combo == 30)
        {
            gameAudioManager.PlayCheerSound(1);
        }
        if (combo % 29 == 0)
        {
            miss -= 2;
            ExtraLife();
        }
    }
    public void MissCalculate()
    {
        if (PlayerPrefs.GetString("GameDifficulty") == "Easy")
        {

        }
        else if (PlayerPrefs.GetString("GameDifficulty") == "Hard")
        {
            miss++;
            if (miss == maxMiss)
            {
                gameManager.isGameOver = true;
                gameManager.GameOver("Lose");
            }
            if (miss == 2)
            {
                missObjects[0].SetActive(true);
                //gameAudioManager.PlayRecordScratch();
            }
            else if (miss == 4)
            {
                missObjects[1].SetActive(true);
                //gameAudioManager.PlayRecordScratch();
            }
            else if (miss == 6)
            {
                missObjects[2].SetActive(true);
                //gameAudioManager.PlayRecordScratch();
            }
        }

        //miss++;
        //if (miss == maxMiss)
        //{
        //    gameManager.isGameOver = true;
        //    gameManager.GameOver("Lose");
        //}
        //if (miss == 2)
        //{
        //    missObjects[0].SetActive(true);
        //    //gameAudioManager.PlayRecordScratch();
        //}
        //else if (miss == 4)
        //{
        //    missObjects[1].SetActive(true);
        //    //gameAudioManager.PlayRecordScratch();
        //}
        //else if (miss == 6)
        //{
        //    missObjects[2].SetActive(true);
        //    //gameAudioManager.PlayRecordScratch();
        //}
        gameAudioManager.PlayRecordScratch();
    }
    public void ExtraLife()
    {
        if (missObjects[0].activeSelf)
        {
            if (missObjects[1].activeSelf)
            {
                missObjects[1].SetActive(false);
            }
            else
            {
                missObjects[0].SetActive(false);
            }
        }
    }
}
