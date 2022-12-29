using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float difficultyScale;

    public Text scoreText;
    public float score;

    public float combo;
    public Text comboText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        combo = 1;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
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
    }
}
