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
    public Animator UIShakeAnim;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIShakeAnim.SetTrigger("ScoreAdded");
        }
        nextTimeToShoot += Time.deltaTime;
        controller.activateAction.action.performed += Action_performed;//This gets the input from assigned controller(press tab tab after += to auto complete)
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
            else if (currentAmmo == 0 && isReloading == false)
            {
                StartCoroutine(Reload());
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
        bool hit1Hit;
        bool hit2Hit = false;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit2, 30))//Send a hit from Muzzle for HitPlane
        {
            if (hit2.collider.gameObject.name == "HitPlane")
            {
                hit2Hit = true;

            }
            else
            {
                hit2Hit = false;
            }
        }
        else
        {
            hit2Hit = false;
        }
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit1, 30, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
        {
            if (hit1.collider.gameObject.name == "OuterCollider" ) //This can be changed to hit.collider.gameObject.name or hit.collider.tag
            {
                if (hit2Hit == true)
                {
                    scoreManager.AddScore(10);
                    scoreManager.combo++;
                    UIShakeAnim.SetTrigger("ScoreAdded");
                }
                else
                {
                    scoreManager.combo = 1;
                    scoreManager.score -= 10;
                }

            }
            else if (hit1.collider.gameObject.name == "InnerCollider" && hit2Hit == true)
            {
                if (hit2Hit == true)
                {
                    scoreManager.AddScore(20);
                    scoreManager.combo++;
                    UIShakeAnim.SetTrigger("ScoreAdded");
                }
                else
                {
                    scoreManager.combo = 1;
                    scoreManager.score -= 10;
                }
            }
            //else if(hit1.collider.gameObject.name != "InnerCollider" && hit1.collider.gameObject.name != "OuterCollider")
            //{
            //    scoreManager.combo = 1;
            //    scoreManager.score -= 10;
            //}
            //else 
            //{
            //    scoreManager.combo = 1;
            //    scoreManager.score -= 10;
            //}

        }
        else
        {
            scoreManager.combo = 1;
            scoreManager.score -= 10;
        }
        
    }
    IEnumerator Reload()
    {
        if (isReloading == false)
        {
            isReloading = true;
            AmmoImage.fillAmount = 0;

            yield return new WaitForSeconds(reloadTime);
            currentAmmo = maxAmmo;
            AmmoText.text = currentAmmo.ToString();
            isReloading = false;
        }
        
    }
}
