using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Easy Difficulty Cube")
        {
            scoreManager.difficultyScale = 0.5f;
            Debug.Log("Easy");
        }
        else if (other.gameObject.name == "Normal Difficulty Cube")
        {
            scoreManager.difficultyScale = 1;
            Debug.Log("Normal");
        }
        else if (other.gameObject.name == "Hard Difficulty Cube")
        {
            scoreManager.difficultyScale = 1.5f;
            Debug.Log("Hard");
        }
    }
}
