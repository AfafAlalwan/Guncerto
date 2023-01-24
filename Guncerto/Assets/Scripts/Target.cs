using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public ScoreManager scoreManager;
    public bool isHit = false;
    void Start()
    {
        try
        {
            scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager>();

        }
        catch { }
    }

    void Update()
    {
        if (isHit)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Target Miss Trigger" && !isHit)
        {
            Debug.Log("Miss");
            scoreManager.score -= 5;
            scoreManager.combo = 1;
            if (scoreManager.score < 0)
            {
                scoreManager.score = 0;
            }
            Destroy(gameObject);
        }
    }
}
