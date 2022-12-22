using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    public Vector3 upRecoil;
    private Vector3 originalRot;
    public bool isRecoilOn = false;
    public VisualEffect muzzleFlashVFX;
    public Text AmmoText;
    public ActionBasedController controller;

    public GameObject Muzzle;
    public float maxAmmo;
    public float currentAmmo;//This can be made private later
    public float reloadTime;
    public bool isReloading;//This can be made private later

    public float fireCoolDown; //Lower values mean faster shooting
    private float nextTimeToShoot = 0f;


    // Start is called before the first frame update
    void Start()
    {
        originalRot = transform.localEulerAngles;
        currentAmmo = maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        nextTimeToShoot = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecoilOn)
        {
            AddRecoil();
            isRecoilOn = false;
        }
        else
        {
            StopRecoil();
        }
        nextTimeToShoot += Time.deltaTime;
        controller.activateAction.action.performed += Action_performed;//This gets the input from assigned controller(press something after += to auto complete)
        controller.selectAction.action.performed += Action_performed2;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (nextTimeToShoot >= fireCoolDown)
        {
            if (currentAmmo <= maxAmmo && currentAmmo > 0 && isReloading == false)
            {
                Shoot();
                nextTimeToShoot = 0f;
                //decrease ammo counter on screen here
            }

        }

    }
    private void Action_performed2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

    }
    void Shoot()
    {
        isRecoilOn = true;
        muzzleFlashVFX.Play(); //Play muzzle flash vfx
        currentAmmo--; //Decrease current ammo
        AmmoText.text = currentAmmo.ToString();
        RaycastHit hit;
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit))//Send a hit from Muzzle 
        {
            if (hit.transform.GetComponent<BoxCollider>()) //This can be changed later or can add more colliders for more points
            {
                Debug.Log(hit.transform.name);
            }

        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        isReloading = false;
    }
    void AddRecoil()
    {
        transform.localEulerAngles += upRecoil;
    }
    void StopRecoil()
    {
        transform.localEulerAngles = originalRot;
    }
}
