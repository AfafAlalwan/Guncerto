using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    public TMP_Text AmmoText;
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
        currentAmmo = maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        nextTimeToShoot = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        nextTimeToShoot += Time.deltaTime;
        controller.activateAction.action.performed += Action_performed;//This gets the input from assigned controller(press Alt and Enter after +=)
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (nextTimeToShoot >= fireCoolDown)
        {
            if (currentAmmo <= maxAmmo && currentAmmo > 0)
            {
                Shoot();
                nextTimeToShoot = 0f;
                //decrease ammo counter on screen here
            }
            
        }
        
    }

    void Shoot()
    {
        currentAmmo--; //Decrease current ammo
        AmmoText.text = currentAmmo.ToString();
        RaycastHit hit;
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit))//Send a hit from Muzzle 
        {
            if (hit.transform.GetComponent<BoxCollider>())
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
        isReloading = false;
    }
}
