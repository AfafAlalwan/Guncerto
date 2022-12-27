using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GunSwitcher : MonoBehaviour
{
    
    
    public InputActionReference GunChangeActionUp;
    public InputActionReference GunChangeActionDown;
    public int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        //SelectWeapon(selectedWeapon);
        slctWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        GunChangeActionUp.action.performed += Action_performed;
        GunChangeActionDown.action.performed += Action_performed2;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!gameObject.GetComponentInChildren<Gun>().isReloading)
        {
            SwitchWeaponUp();
        }
    }
    private void Action_performed2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!gameObject.GetComponentInChildren<Gun>().isReloading)
        {
            SwitchWeaponDown();
        }
    }
    void SwitchWeaponUp()
    {
        //int previousWeapon = selectedWeapon;

        //selectedWeapon = (selectedWeapon + 1) % transform.childCount;
        //SelectWeapon(selectedWeapon);

        //if (previousWeapon != selectedWeapon)
        //{
        //    SelectWeapon(selectedWeapon);
        //}
        int previousWeapon = selectedWeapon;
        if (selectedWeapon >= transform.childCount - 1)
        {
            selectedWeapon = 0;
        }
        else
        {
            selectedWeapon++;
        }
        slctWeapon();
    }
    void SwitchWeaponDown()
    {
        //int previousWeapon = selectedWeapon;

        //selectedWeapon = (selectedWeapon - 1) % transform.childCount;
        //SelectWeapon(selectedWeapon);

        //if (previousWeapon != selectedWeapon)
        //{
        //    SelectWeapon(selectedWeapon);
        //}
        int previousWeapon = selectedWeapon;
        if (selectedWeapon <= 0)
        {
            selectedWeapon = transform.childCount - 1;
        }
        else
        {
            selectedWeapon--; 
        }
        slctWeapon();
    }
    void SelectWeapon(int newWeapon)
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == newWeapon);
            i++;
        }
    }
    void slctWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
