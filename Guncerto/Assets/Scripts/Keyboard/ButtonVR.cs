/**************************************************
Copyright : Copyright (c) RealaryVR. All rights reserved.
Description: Script for VR Button functionality.
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    //public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    bool isPressed;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            //button.transform.localPosition = new Vector3(0, 0.003f, 0);
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        
            //button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        
    }
    

}
