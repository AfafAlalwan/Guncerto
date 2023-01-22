using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Gun Shotgun1;
    public Gun Shotgun2;
    public Gun Pistol1;
    public Gun Pistol2;
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
            Shotgun1.inaccuracyDistance = 3; // adjust shotgun spread according to difficulty
            Shotgun2.inaccuracyDistance = 3;
            scoreManager.difficultyScale = 0.5f;
            //Debug.Log("Easy");
        }
        else if (other.gameObject.name == "Normal Difficulty Cube")
        {
            Shotgun1.inaccuracyDistance = 2;
            Shotgun2.inaccuracyDistance = 2;
            scoreManager.difficultyScale = 1;
            //Debug.Log("Normal");
        }
        else if (other.gameObject.name == "Hard Difficulty Cube")
        {
            Shotgun1.inaccuracyDistance = 1;
            Shotgun2.inaccuracyDistance = 1;
            scoreManager.difficultyScale = 1.5f;
            //Debug.Log("Hard");
        }

        if (other.gameObject.name == "Typing Area")
        {
            Pistol1.UIMode = true;
            Pistol2.UIMode = true;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Typing Area")
        {
            Pistol1.UIMode = false;
            Pistol2.UIMode = false;
        }
    }
}
