using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int score;

    public int combo;
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
            score += scoreToAdd * 1;
            
        }
        else if (combo > 10 && combo <= 20)
        {
            score += scoreToAdd * 2;
        }
        else if (combo > 20)
        {
            score += scoreToAdd * 3;
        }
    }
}
