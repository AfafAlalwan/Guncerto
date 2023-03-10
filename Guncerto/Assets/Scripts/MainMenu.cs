using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using Unity.XR.CoreUtils;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    string selectedSong = string.Empty;
    [HideInInspector] public bool nameEntered = false; 

    [SerializeField] Animator screenAnimator;
    [SerializeField] Animator leftCurtainAnimator, rightCurtainAnimator;

    [SerializeField] ButtonVR playButton;
    TextMeshProUGUI playUI;

    XROrigin origin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        var guns = FindObjectsOfType<Gun>();
        foreach(var gun in guns)
        {
            gun.UIMode = true;
        }

        playButton = GetComponentInChildren<ButtonVR>();
        playUI = playButton.GetComponentInChildren<TextMeshProUGUI>();
        playUI.text = "On Air";
        origin = FindObjectOfType<XROrigin>();
        origin.transform.position = new Vector3(0.13f, 3.67f, 1.05f);
        origin.transform.eulerAngles = new Vector3(0,90f,0);
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
            playUI.text = "PLAY";

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
        nameEntered = false;
    }



}
