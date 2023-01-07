using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MusicNote : MonoBehaviour
{

    //[SerializeField] Transform lane1;
    //Vector3[] lane1Positions = new Vector3[6];

    Sequence sequence;
    void Start()
    {

        //var positions = lane1.GetComponentsInChildren<Transform>().Where(t => t != lane1);
        //lane1Positions = new Vector3[positions.Count()];
        //int i = 0;
        //foreach(var pos in positions)
        //{
        //    lane1Positions[i] = pos.position;
        //    i++;
        //}
  
        //sequence = DOTween.Sequence();
        //// give it random rotation and random lane 
        //sequence.Append(transform.DOPath(lane1Positions, 3f)).Join(transform.DORotate(new Vector3(0,90f,90f), 3f, RotateMode.FastBeyond360));
    }
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            sequence.Kill();
        }
    }

    public void Movement(Vector3[] path)
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, 3f)).Join(transform.DORotate(new Vector3(0, 90f, 90f), 3f, RotateMode.FastBeyond360));
    }
}
