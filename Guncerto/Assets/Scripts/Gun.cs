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
    [Header("References")]
    public Animator UIShakeAnim;
    public Image AmmoImage;
    public Text AmmoText;
    public Animator GunRecoilAnim;
    public ActionBasedController controller;
    ScoreManager scoreManager;
    public VisualEffect muzzleFlashVFX;   
    public GameObject Muzzle;
    LineRenderer laserLine;
    MonoBehaviour camMono;

    [Header("General Stats")]
    public bool isRecoilOn = false;
    public float maxAmmo;
    public float currentAmmo;//This can be made private later
    public float reloadTime;
    public float range = 30;
    public bool isReloading;//This can be made private later
    public float fireCoolDown; //Lower values mean faster shooting
    private float nextTimeToShoot = 0f;

    [Header("Shotgun")]
    public bool isShotgun = false;
    public int bulletsPerShot = 5;
    public float inaccuracyDistance =  5;

    [Header("Laser")]
    public GameObject Laser;
    public float fadeDuration = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        camMono = Camera.main.GetComponent<MonoBehaviour>();//Mono beh of the main camera to allow Coroutine on deactivated objects.
        scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        currentAmmo = maxAmmo;
        AmmoImage.fillAmount = currentAmmo / maxAmmo;
        AmmoText.text = currentAmmo.ToString();
        nextTimeToShoot = 20f;
        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserLine.SetPosition(0, Muzzle.transform.position);
        RaycastHit laserHit;

        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out laserHit, range)) //Laser 
        {
            laserLine.SetPosition(1, laserHit.point);
        }
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
        if (isShotgun)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                if (Physics.Raycast(Muzzle.transform.position, GetShootingDirection(), out hit2, range))//Send a hit from Muzzle for HitPlane
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
                Vector3 shootingDir = GetShootingDirection();
                if (Physics.Raycast(Muzzle.transform.position, shootingDir, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
                {
                    CreateLaser(hit1.point);
                    if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
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
                }
                else
                {
                    CreateLaser(Muzzle.transform.position + shootingDir * range);
                    scoreManager.combo = 1;
                    scoreManager.score -= 10;
                    
                }
            }
        }
        else
        {
            if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit2, range))//Send a hit from Muzzle for HitPlane
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
            if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
            {
                CreateLaser(hit1.point);
                if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
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
            }
            else
            {
                CreateLaser(Muzzle.transform.position + Muzzle.transform.forward * range);
                scoreManager.combo = 1;
                scoreManager.score -= 10;
            }
        }
        #region Shoot
        //if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit2, range))//Send a hit from Muzzle for HitPlane
        //{
        //    if (hit2.collider.gameObject.name == "HitPlane")
        //    {
        //        hit2Hit = true;

        //    }
        //    else
        //    {
        //        hit2Hit = false;
        //    }
        //}
        //else
        //{
        //    hit2Hit = false;
        //}
        //if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
        //{
        //    if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
        //    {
        //        if (hit2Hit == true)
        //        {
        //            scoreManager.AddScore(10);
        //            scoreManager.combo++;
        //            UIShakeAnim.SetTrigger("ScoreAdded");
        //        }
        //        else
        //        {
        //            scoreManager.combo = 1;
        //            scoreManager.score -= 10;
        //        }

        //    }
        //    else if (hit1.collider.gameObject.name == "InnerCollider" && hit2Hit == true)
        //    {
        //        if (hit2Hit == true)
        //        {
        //            scoreManager.AddScore(20);
        //            scoreManager.combo++;
        //            UIShakeAnim.SetTrigger("ScoreAdded");
        //        }
        //        else
        //        {
        //            scoreManager.combo = 1;
        //            scoreManager.score -= 10;
        //        }
        //    }
        //    //else if(hit1.collider.gameObject.name != "InnerCollider" && hit1.collider.gameObject.name != "OuterCollider")
        //    //{
        //    //    scoreManager.combo = 1;
        //    //    scoreManager.score -= 10;
        //    //}
        //    //else 
        //    //{
        //    //    scoreManager.combo = 1;
        //    //    scoreManager.score -= 10;
        //    //}

        //}
        //else
        //{
        //    scoreManager.combo = 1;
        //    scoreManager.score -= 10;
        //}
        #endregion


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
    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = Muzzle.transform.position + Muzzle.transform.forward * range;
        targetPos = new Vector3(targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
        targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
        targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance));

        Vector3 direction = targetPos - Muzzle.transform.position;
        return direction.normalized;
    }
    void CreateLaser(Vector3 end)
    {
        LineRenderer lr = Instantiate(Laser).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] {Muzzle.transform.position, end});
        camMono.StartCoroutine(FadeLaser(lr));//Set the coroute on camera
    }
    IEnumerator FadeLaser(LineRenderer lr)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
            yield return null;
        }
    }

}
