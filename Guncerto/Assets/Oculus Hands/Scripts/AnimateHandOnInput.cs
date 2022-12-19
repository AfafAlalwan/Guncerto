using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{

    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gribAnimationAction;
    public Animator handAnimator;


    float gripValue;
    float triggerValue;

    void Start()
    {
      // inputActionProperty should change from pc to vr 
      //  gribAnimationAction.reference.Set()
    }

    void Update()
    {
        triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat(Constants.Trigger, triggerValue);

        gripValue = gribAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat(Constants.Grip, gripValue);

        //Debug.Log(triggerValue);
    }
}
