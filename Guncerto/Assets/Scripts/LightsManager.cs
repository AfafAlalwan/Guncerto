using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class LightsManager : MonoBehaviour
{
    [SerializeField] float firstVerseEnd, firstChorusEnd, secondVerseEnd, secondChorusEnd, bridgeEnd, endSong;


    [SerializeField] Transform LaserGroup1, LaserGroup2, ConeGroup1, ConeGroup2;

    List<Transform> laserGroup1, laserGroup2, coneGroup1, coneGroup2;
    SongManager song;
    void Start()
    {
        song = FindObjectOfType<SongManager>();
        laserGroup1 = new List<Transform>();
        laserGroup2 = new List<Transform>();
        coneGroup1 = new List<Transform>();
        coneGroup2 = new List<Transform>();
        laserGroup1.AddRange(LaserGroup1.GetComponentsInChildren<Transform>().Where(t => t != LaserGroup1));
        laserGroup2.AddRange(LaserGroup2.GetComponentsInChildren<Transform>().Where(t => t != LaserGroup2));
        coneGroup1.AddRange(ConeGroup1.GetComponentsInChildren<Transform>().Where(t => t != ConeGroup1));
        coneGroup2.AddRange(ConeGroup2.GetComponentsInChildren<Transform>().Where(t => t != ConeGroup2));

        StartCoroutine(Verse1());

    }


    IEnumerator Verse1()
    {
        foreach (var light in coneGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }

        while (song.songPosition <= firstVerseEnd)
        {
             foreach(var light in coneGroup1)
             {
                light.DOLocalRotate(new Vector3(light.localEulerAngles.x,light.localEulerAngles.y, Random.Range(100f, 200f)), 2f, RotateMode.Fast);
             }   


            yield return new WaitForSeconds(2f);
        }

        //foreach (var light in coneGroup1)
        //{
        //    light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        //}

        StartCoroutine(Chorus1());

    }

    IEnumerator Chorus1()
    {
        foreach (var light in coneGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }

        while (song.songPosition <= firstChorusEnd)
        {
            foreach (var light in coneGroup1)
            {
                light.DOLocalRotate(new Vector3(light.localEulerAngles.x, light.localEulerAngles.y, Random.Range(100f, 200f)), 2f, RotateMode.Fast);
            }

            foreach (var light in coneGroup2)
            {
                if (light.localPosition.x > 0)
                    light.DOLocalRotate(new Vector3(light.localEulerAngles.x, Random.Range(0, 150f), light.localEulerAngles.z), 2f, RotateMode.Fast);
                else
                    light.DOLocalRotate(new Vector3(light.localEulerAngles.x, Random.Range(150f, 250f), light.localEulerAngles.z), 2f, RotateMode.Fast);

            }


            yield return new WaitForSeconds(2f);
        }

        foreach (var light in coneGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }

        StartCoroutine(Verse2());

    }

    IEnumerator Verse2()
    {

        while (song.songPosition <= secondVerseEnd)
        {
            foreach (var light in coneGroup1)
            {
                light.DOLocalRotate(new Vector3(light.localEulerAngles.x, light.localEulerAngles.y, Random.Range(100f, 200f)), 2f, RotateMode.Fast);
            }


            yield return new WaitForSeconds(2f);
        }

        //foreach (var light in coneGroup1)
        //{
        //    light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        //}

        StartCoroutine(Chorus2());

    }

    IEnumerator Chorus2()
    {
        foreach (var light in coneGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }

        while (song.songPosition <= firstChorusEnd)
        {
            foreach (var light in coneGroup1)
            {
                light.DOLocalRotate(new Vector3(light.localEulerAngles.x, light.localEulerAngles.y, Random.Range(100f, 200f)), 2f, RotateMode.Fast);
            }

            foreach (var light in coneGroup2)
            {
                if (light.localPosition.x > 0)
                    light.DOLocalRotate(new Vector3(light.localEulerAngles.x, Random.Range(0, 150f), light.localEulerAngles.z), 2f, RotateMode.Fast);
                else
                    light.DOLocalRotate(new Vector3(light.localEulerAngles.x, Random.Range(150f, 250f), light.localEulerAngles.z), 2f, RotateMode.Fast);

            }


            yield return new WaitForSeconds(2f);
        }

        foreach (var light in coneGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }

        foreach (var light in coneGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }

        StartCoroutine(Ending());

    }

    IEnumerator Bridge()
    {
        foreach(var light in laserGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }

        while (song.songPosition <= bridgeEnd)
        {
            // rotate the mother
            foreach (var light in laserGroup1)
            {
                light.DOLocalRotate(new Vector3(light.localEulerAngles.x, light.localEulerAngles.y, Random.Range(100f, 200f)), 2f, RotateMode.Fast); // to do
            }

  
            yield return new WaitForSeconds(2f);
        }

        foreach (var light in laserGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }

        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        foreach (var light in laserGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }
        foreach (var light in laserGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(1, 2f);
        }


        while (song.songPosition <= endSong)
        {
            // rotate the mother
            LaserGroup1.DOLocalRotate(new Vector3(LaserGroup1.localEulerAngles.x, LaserGroup1.localEulerAngles.y, Random.Range(0, 360f)), 2f, RotateMode.Fast);
            foreach (var light in laserGroup1)
            {
                light.DOLocalRotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 1f, RotateMode.Fast); // to do
            }
            LaserGroup2.DOLocalRotate(new Vector3(LaserGroup1.localEulerAngles.x, LaserGroup2.localEulerAngles.y, Random.Range(0, 360f)), 2f, RotateMode.Fast);
            foreach (var light in laserGroup2)
            {
                light.DOLocalRotate(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)), 1f, RotateMode.Fast); // to do
            }


            yield return new WaitForSeconds(2f);
        }

        foreach (var light in laserGroup1)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }
        foreach (var light in laserGroup2)
        {
            light.GetComponent<MeshRenderer>().material.DOFade(0, 2f);
        }
    }

}
