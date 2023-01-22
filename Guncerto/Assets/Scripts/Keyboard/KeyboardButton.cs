using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButton : MonoBehaviour
{
    Keyboard keyboard;
    public Text buttonText;
    void Start()
    {
        //buttonText.text = gameObject.name;
        keyboard = GetComponentInParent<Keyboard>();
        //buttonText = GetComponentInChildren<Text>();
        if (buttonText.text.Length == 1)
        {
            NameButtonToText();
            GetComponentInChildren<ButtonVR>().onRelease.AddListener(delegate { keyboard.InsertChar(buttonText.text); });
        }
    }
    public void NameButtonToText()
    {
        buttonText.text = gameObject.name;
    }
}
