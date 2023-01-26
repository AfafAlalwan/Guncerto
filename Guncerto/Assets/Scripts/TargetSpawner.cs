using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.XR.CoreUtils;



public class TargetSpawner : MonoBehaviour
{
    [SerializeField] Transform[] lanes;
    [SerializeField] GameObject[] targetPrefab;

    Dictionary<Transform, List<Vector3>> lanesDictionary = new Dictionary<Transform, List<Vector3>>();
    List<Vector3> rotations = new List<Vector3>();

    XROrigin origin;
    void Start()
    {
        InitLists();
        origin = FindObjectOfType<XROrigin>();
        origin.transform.position = new Vector3(0.13f, 3.67f, 1.05f);
        origin.transform.eulerAngles = new Vector3(0, 90f, 0);
    }

    int randomLane, randomTarget, r;
    public void SpawnTarget()
    {
        r = Random.Range(0, lanes.Length);
        if (randomLane == r)
        {
            SpawnTarget();
            return;
        }
        else
            randomLane = r;

        randomTarget = Random.Range(0, targetPrefab.Length);
        GameObject newTarget = Instantiate(targetPrefab[randomTarget], lanesDictionary[lanes[randomLane]][0], Quaternion.Euler(new Vector3(0, Random.Range(0, 360), Random.Range(0, 360))));
        newTarget.GetComponent<MusicNote>().Movement(lanesDictionary[lanes[randomLane]].ToArray(), rotations[randomLane]);
    }

    void InitLists()
    {

        foreach (var lane in lanes)
        {
            List<Transform> children = new List<Transform>();
            List<Vector3> lanePositions = new List<Vector3>();
            Transform lastChild = null ;
            children = lane.GetComponentsInChildren<Transform>().Where(t => t != lane).ToList();
            foreach(var child in children)
            {
                lastChild = child;
                lanePositions.Add(new Vector3(child.position.x, child.position.y, child.position.z));
            }
            rotations.Add(lastChild.eulerAngles);
            lanesDictionary.Add(lane, lanePositions);
        }
      
    }
}
