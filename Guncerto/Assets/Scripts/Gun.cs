using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public GameObject pianoKeyHighlight;
    GameObject lastSpawned;
    public GunAudioManager gunAudioManager;
    public GameManager gameManager;
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
    public int missCount;
    public bool UIMode = false;
    public float maxAmmo;
    public float currentAmmo;//This can be made private later
    public float reloadTime;
    public float range = 30;
    public bool isReloading;//This can be made private later
    public float fireCoolDown; //Lower values mean faster shooting
    private float nextTimeToShoot = 0f;
    [Range(0f, 1f)]
    public float hapticIntensity;
    public float hapticDuration;
    

    [Header("Shotgun")]
    public bool isShotgun = false;
    public int bulletsPerShot = 9;
    public float inaccuracyDistance = 5;

    [Header("Laser")]
    public GameObject HitLaser;
    public GameObject AimLaser;
    public float fadeDuration = 0.3f;

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

    void Update()
    {
        //CreateAimLaser(Muzzle.transform.position + Muzzle.transform.forward * range);      
        laserLine.SetPosition(0, Muzzle.transform.position);
        RaycastHit laserHit;
        if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out laserHit, range, LayerMask.GetMask("Box") + LayerMask.GetMask("Default"))) //Laser 
        {
            laserLine.SetPosition(1, laserHit.point);
        }

        if (isReloading)
        {
            if (AmmoImage.fillAmount < 1)
            {
                AmmoImage.fillAmount += Time.deltaTime / reloadTime;
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
            TriggerHaptic(controller);
            if (currentAmmo <= maxAmmo && currentAmmo > 0 && isReloading == false)
            {
                if (gameObject.activeSelf)
                {
                    Shoot();
                    nextTimeToShoot = 0f;
                }

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
        if (gameObject.activeSelf)
        {
            GunRecoilAnim.SetTrigger("IsRecoilOn");
            muzzleFlashVFX.Play(); //Play muzzle flash vfx
            //currentAmmo--; //Decrease current ammo
            AmmoText.text = currentAmmo.ToString();
            AmmoImage.fillAmount = currentAmmo / maxAmmo;
            //gunAudioManager.isShotgun = isShotgun;
            gunAudioManager.PlaySound();
        }

        bool hit1Hit = false;
        bool hit2Hit = false;

        int hitPlaneHitNo = 0;
        int hitPlaneNoHitNo = 0;

        int outerHitNo = 0;
        int innerHitNo = 0;
        int noHitNo = 0;

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hitKey;
        if (isShotgun && gameObject.activeSelf)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                if (Physics.Raycast(Muzzle.transform.position, GetShootingDirection(), out hit2, range, LayerMask.GetMask("HitPlane")))//Send a hit from Muzzle for HitPlane
                {
                    if (hit2.collider.gameObject.name == "HitPlane")
                    {
                        //hit2Hit = true;
                        hitPlaneHitNo++;
                    }
                    else
                    {
                        //hit2Hit = false;
                        hitPlaneNoHitNo++;
                    }
                }
                else
                {
                    //hit2Hit = false;
                    hitPlaneNoHitNo++;
                }

                if (hitPlaneHitNo >= hitPlaneNoHitNo)
                {
                    hit2Hit = true;
                }
                else
                {
                    hit2Hit = false;
                }

                Vector3 shootingDir = GetShootingDirection();
                if (Physics.Raycast(Muzzle.transform.position, shootingDir, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
                {
                    if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
                    {
                        if (hit1.collider.gameObject.GetComponentInParent<Target>().name == "Shotgun" || hit1.collider.gameObject.GetComponentInParent<Target>().name == "Default")
                        {
                            if (hit2Hit == true)
                            {
                                outerHitNo++;
                                CreateHitLaser(hit1.point, "Outer");
                                hit1.collider.gameObject.GetComponentInParent<Target>().isHit = true;
                            }
                            else
                            {
                                noHitNo++;
                                CreateHitLaser(hit1.point, "NoHit");
                            }
                        }
                        else
                        {
                            CreateHitLaser(hit1.point, "NoHit");
                            noHitNo++;
                        }
                    }
                    else if (hit1.collider.gameObject.name == "InnerCollider")
                    {
                        if (hit1.collider.gameObject.GetComponentInParent<Target>().name == "Shotgun" || hit1.collider.gameObject.GetComponentInParent<Target>().name == "Default")
                        {
                            if (hit2Hit == true)
                            {
                                innerHitNo++;
                                CreateHitLaser(hit1.point, "Inner");
                                hit1.collider.gameObject.GetComponentInParent<Target>().isHit = true;
                            }
                            else
                            {
                                noHitNo++;
                                CreateHitLaser(hit1.point, "NoHit");
                            }
                        }
                        else
                        {
                            CreateHitLaser(hit1.point, "NoHit");
                            noHitNo++;
                        }
                    }
                }
                else
                {
                    noHitNo++;
                    CreateHitLaser(Muzzle.transform.position + shootingDir * range, "NoHit");
                }
            }

            //Actual Calculation
            if (innerHitNo + outerHitNo >= noHitNo)//Inner hit
            {
                if (innerHitNo >= outerHitNo)
                {
                    scoreManager.AddScore(20);
                    scoreManager.combo++;
                    UIShakeAnim.SetTrigger("ScoreAdded");
                }
                else//Outer Hit
                {
                    scoreManager.AddScore(10);
                    scoreManager.combo++;
                    UIShakeAnim.SetTrigger("ScoreAdded");
                }
            }
            else//Nohit
            {
                scoreManager.combo = 1;
                scoreManager.score -= 10;
                if (scoreManager.score < 0)
                {
                    scoreManager.score = 0;
                }
                scoreManager.MissCalculate();
            }
        }
        else if (!isShotgun && gameObject.activeSelf) //not shotgun
        {
            if (UIMode)
            {
                if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hitKey, range, LayerMask.GetMask("Key")))
                {
                    hitKey.transform.gameObject.GetComponent<ButtonVR>().onRelease.Invoke();
                    //hitKey.transform.gameObject.GetComponent<ButtonVR>().Release();
                    //Debug.Log(hitKey.transform.gameObject.name);

                }
                if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hitKey, range))
                {
                    //if (hitKey.collider.gameObject.name == "Play Button")
                    //{
                    //    MainMenu.Instance.PlaySong();
                    //}
                }
            }
            else // not UI mode
            {
                if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit3, range))//Send a hit from Muzzle for HitPlane
                {
                    if (hit3.collider.gameObject.name != null)
                    {
                        //Debug.Log(hit3.collider.gameObject.name);
                    }
                }
                if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit2, range, LayerMask.GetMask("HitPlane")))//Send a hit from Muzzle for HitPlane
                {
                    if (hit2.collider.gameObject.name == "HitPlane")
                    {
                        hit2Hit = true;
                        //Debug.Log("hitplane hit");

                    }
                    else
                    {
                        hit2Hit = false;
                        //Debug.Log("hitplane nohit");
                        //CheckMiss();
                        scoreManager.combo = 1;
                        scoreManager.score -= 10;
                        if (scoreManager.score < 0)
                        {
                            scoreManager.score = 0;
                        }
                        scoreManager.MissCalculate();
                    }
                }
                else
                {
                    hit2Hit = false;
                    //Debug.Log("hitplane nohit");
                   // CheckMiss();
                    scoreManager.combo = 1;
                    scoreManager.score -= 10;
                    if (scoreManager.score < 0)
                    {
                        scoreManager.score = 0;
                    }
                    scoreManager.MissCalculate();
                }
                if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
                {

                    if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
                    {
                        if (hit1.collider.gameObject.GetComponentInParent<Target>().name == "Pistol" || hit1.collider.gameObject.GetComponentInParent<Target>().name == "Default")
                        {
                            if (hit2Hit == true)
                            {
                                CreateHitLaser(hit1.point, "Outer");
                                scoreManager.AddScore(10);
                                scoreManager.combo++;
                                UIShakeAnim.SetTrigger("ScoreAdded");
                                hit1.collider.gameObject.GetComponentInParent<Target>().isHit = true;
                                //Debug.Log("outer hit");
                            }
                            else
                            {
                                CreateHitLaser(hit1.point, "NoHit");
                                scoreManager.combo = 1;
                                scoreManager.score -= 10;
                                if (scoreManager.score < 0)
                                {
                                    scoreManager.score = 0;
                                }
                                //CheckMiss();
                                //Debug.Log("outer no hit");
                                scoreManager.MissCalculate();
                            }
                        }                      
                        else
                        {
                            CreateHitLaser(hit1.point, "NoHit");
                            scoreManager.combo = 1;
                            scoreManager.score -= 10;
                            if (scoreManager.score < 0)
                            {
                                scoreManager.score = 0;
                            }
                            //CheckMiss();
                            //Debug.Log("outer no hit");
                            scoreManager.MissCalculate();
                        }

                    }
                    else if (hit1.collider.gameObject.name == "InnerCollider")
                    {
                        if (hit1.collider.gameObject.GetComponentInParent<Target>().name == "Pistol" || hit1.collider.gameObject.GetComponentInParent<Target>().name == "Default")
                        {
                            if (hit2Hit == true)
                            {
                                CreateHitLaser(hit1.point, "Inner");
                                scoreManager.AddScore(20);
                                scoreManager.combo++;
                                UIShakeAnim.SetTrigger("ScoreAdded");
                                hit1.collider.gameObject.GetComponentInParent<Target>().isHit = true;
                                //Debug.Log("inner hit");
                            }
                            else
                            {
                                CreateHitLaser(hit1.point, "NoHit");
                                scoreManager.combo = 1;
                                scoreManager.score -= 10;
                                if (scoreManager.score < 0)
                                {
                                    scoreManager.score = 0;
                                }
                                //Debug.Log("inner no hit");
                                //CheckMiss();
                                scoreManager.MissCalculate();
                            }
                        }                       
                        else
                        {
                            CreateHitLaser(hit1.point, "NoHit");
                            scoreManager.combo = 1;
                            scoreManager.score -= 10;
                            if (scoreManager.score < 0)
                            {
                                scoreManager.score = 0;
                            }
                            //Debug.Log("inner no hit");
                            //CheckMiss();
                            scoreManager.MissCalculate();
                        }
                    }
                }
                else
                {
                    CreateHitLaser(Muzzle.transform.position + Muzzle.transform.forward * range, "NoHit");
                    scoreManager.combo = 1;
                    scoreManager.score -= 10;
                    if (scoreManager.score < 0)
                    {
                        scoreManager.score = 0;
                    }
                    //Debug.Log("no inner outer hit");
                    //CheckMiss();
                    scoreManager.MissCalculate();
                }
            }
           
        }
        #region Shoot Yedek
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
        #region Shotgun Yedek
        //if (isShotgun)
        //{
        //    for (int i = 0; i < bulletsPerShot; i++)
        //    {
        //        if (Physics.Raycast(Muzzle.transform.position, GetShootingDirection(), out hit2, range))//Send a hit from Muzzle for HitPlane
        //        {
        //            if (hit2.collider.gameObject.name == "HitPlane")
        //            {
        //                hit2Hit = true;
        //            }
        //            else
        //            {
        //                hit2Hit = false;
        //            }
        //        }
        //        else
        //        {
        //            hit2Hit = false;
        //        }
        //        Vector3 shootingDir = GetShootingDirection();
        //        if (Physics.Raycast(Muzzle.transform.position, shootingDir, out hit1, range, LayerMask.GetMask("Box")))//Send a hit from Muzzle 
        //        {

        //            if (hit1.collider.gameObject.name == "OuterCollider") //This can be changed to hit.collider.gameObject.name or hit.collider.tag
        //            {
        //                if (hit2Hit == true)
        //                {
        //                    hit1Hit = true;
        //                    CreateLaser(hit1.point, "Outer");
        //                    scoreManager.AddScore(10);
        //                    scoreManager.combo++;
        //                    UIShakeAnim.SetTrigger("ScoreAdded");
        //                }
        //                else
        //                {
        //                    CreateLaser(hit1.point, "NoHit");
        //                    scoreManager.combo = 1;
        //                    scoreManager.score -= 10;
        //                }
        //            }
        //            else if (hit1.collider.gameObject.name == "InnerCollider")
        //            {
        //                if (hit2Hit == true)
        //                {
        //                    hit1Hit = true;
        //                    CreateLaser(hit1.point, "Inner");
        //                    scoreManager.AddScore(20);
        //                    scoreManager.combo++;
        //                    UIShakeAnim.SetTrigger("ScoreAdded");

        //                }
        //                else
        //                {
        //                    CreateLaser(hit1.point, "NoHit");
        //                    scoreManager.combo = 1;
        //                    scoreManager.score -= 10;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            CreateLaser(Muzzle.transform.position + shootingDir * range, "NoHit");
        //            scoreManager.combo = 1;
        //            scoreManager.score -= 10;

        //        }
        //    }
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
    void CreateHitLaser(Vector3 end, string hitInfo)
    {
        GameObject hitLaser = Instantiate(HitLaser);
        LineRenderer lr = hitLaser.GetComponent<LineRenderer>();
        if (hitInfo == "Inner")
        {
            lr.startColor = new Color(0, 1, 0);
            lr.endColor = new Color(0, 0.3f, 0);
        }
        else if (hitInfo == "Outer")
        {
            lr.startColor = new Color(1, 1, 0);
            lr.endColor = new Color(0.3f, 0.3f, 0);
        }
        else if (hitInfo == "NoHit")
        {
            lr.startColor = new Color(1, 0, 0);
            lr.endColor = new Color(0.3f, 0, 0);
        }

        lr.SetPositions(new Vector3[2] { Muzzle.transform.position, end });
        camMono.StartCoroutine(FadeLaser(lr, hitLaser));//Set the coroute on camera

    }
    IEnumerator FadeLaser(LineRenderer lr, GameObject hitLaser)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
            yield return null;
        }
        Destroy(hitLaser);
    }
    private void OnEnable()
    {
        nextTimeToShoot = 0.7f;
    }
    private void OnDisable()
    {
        muzzleFlashVFX.Stop(); //Stop muzzle flash vfx
    }

    private void PistolHit()
    {

    }
    private void ShotgunHit()
    {

    }
    //public void CheckMiss()
    //{
    //    missCount++;
    //    if (missCount >= 3)
    //    {
    //        gameManager.isGameOver = true;
    //        gameManager.GameOver();
    //    }

    //}
    public void TriggerHaptic(XRBaseController controller)
    {
        if (hapticIntensity > 0)
        {
            controller.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
    }
}
