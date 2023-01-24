using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    string selectedSong = string.Empty;
    bool nameEntered = false; //TODO: make it true when name is entered and call OnAir();

    [SerializeField] Animator screenAnimator;
    [SerializeField] Animator leftCurtainAnimator, rightCurtainAnimator;

    [SerializeField] ButtonVR playButton;

    void Start()
    {
        var guns = FindObjectsOfType<Gun>();
        foreach(var gun in guns)
        {
            gun.UIMode = true;
        }

        playButton = GetComponentInChildren<ButtonVR>();

    }

    public void SongSelection(string songName)
    {
        selectedSong = songName;
        OnAir();
    }

    public void OnAir()
    {
        if(nameEntered && !selectedSong.Equals(string.Empty))
        {
            playButton.GetComponent<BoxCollider>().enabled = true;
            //TODO: change text 
        }
    }

    public async void PlaySong()
    {
        //TODO: disable controller
 
        screenAnimator.SetBool("play", true);
        leftCurtainAnimator.SetBool("play", true); 
        rightCurtainAnimator.SetBool("play", true);

        await Task.Delay(2000);

        Loader.Instance.LoadScene(selectedSong);
        
    }

    //IEnumerator EntranceAnimation()
    //{

    //    yield return new WaitForSeconds(2f);

    //    Loader.Instance.LoadScene(selectedSong);

    //}
    // implement a return back 

}
