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
    public UnityEvent onPress;
    public UnityEvent onRelease;
    bool isPressed;

    void Start()
    {
        isPressed = false;
    }
 public void Release()
    {
        Debug.Log("yes");
    }



}
