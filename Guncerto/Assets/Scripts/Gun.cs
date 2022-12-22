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
    public Image AmmoImage;
    public Text AmmoText;
    public Animator GunRecoilAnim;
    public ActionBasedController controller;

    ScoreManager scoreManager;

    public bool isRecoilOn = false;
    public VisualEffect muzzleFlashVFX;   

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
        scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        currentAmmo = maxAmmo;
        AmmoImage.fillAmount = currentAmmo / maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        nextTimeToShoot = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {          
            if (AmmoImage.fillAmount < 1)
            {
                AmmoImage.fillAmount += Time.deltaTime / reloadTime ;
            }
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
        GunRecoilAnim.SetTrigger("IsRecoilOn");
        isRecoilOn = true;
        muzzleFlashVFX.Play(); //Play muzzle flash vfx
        currentAmmo--; //Decrease current ammo
        AmmoText.text = currentAmmo.ToString();
        AmmoImage.fillAmount = currentAmmo / maxAmmo;
        RaycastHit hit;
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit))//Send a hit from Muzzle 
        {
            if (hit.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
            {
                scoreManager.score += 5;
            }
            else if (hit.collider.gameObject.name == "InnerCollider")
            {
                scoreManager.score += 15;
            }

        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        AmmoImage.fillAmount = 0;

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        isReloading = false;
    }
}
