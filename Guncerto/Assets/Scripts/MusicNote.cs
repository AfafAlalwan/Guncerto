using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    Vector3 SpawnPos, RemovePos;
    int BeatsShownInAdvance;
    int beatOfThisNote;
    int songPosInBeats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp( SpawnPos,RemovePos,(BeatsShownInAdvance - (beatOfThisNote - songPosInBeats)) / BeatsShownInAdvance);
    }
}
