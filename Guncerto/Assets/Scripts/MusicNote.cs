using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MusicNote : MonoBehaviour
{

    Sequence sequence;
 
    public void Movement(Vector3[] path, Vector3 lastRotation)
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, 3f)).Join(transform.DORotate(lastRotation, 3f, RotateMode.FastBeyond360)).Append(transform.DOMoveY(transform.position.y - 10f, 3f));
    }

    //call this when hitting the target. 
    public void TargetHitResponse()
    {
        sequence.Kill();
        // show explosion 
        // effects
        //sounds 
    }
}
