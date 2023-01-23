using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Railgun : MonoBehaviour
{

    InputAction fire;
    InputAction reload;
    InputAction swapMod;
    InputAction swapWeapon;
    public PlayerInput playerContr;
    bool fireButtonPressed = false;
    bool canShoot = true;
    bool canReload = true;
    PlayerMovement playerMovement;

    [SerializeField] GameObject weaponModCanvas;

    //public GunStatScriptableObjects pistolStats;
    //public GunStatScriptableObjects rifleStats;

    [SerializeField] GunStatScriptableObjects gunStats;
    [SerializeField] RailgunModScriptableObject railGunMod;
    

    public enum CurrentWeapon
    {
        pistol = 0,
        rifle = 1
    };

    public CurrentWeapon currentWeapon;

    private AudioSource audioSource;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip emptySound;



    Transform parentTransform;

    [SerializeField] Camera playerCamera;

    [SerializeField] CameraShake cameraShake;
    float shakeDuration;
    float shakeMagnitude;
    [SerializeField] CameraRecoil cameraRecoil;


    //public GameObject pistol;
    //public GameObject rifle;

    [SerializeField] GameObject otherGun;
    [SerializeField] GameObject debugObject;

    public TextMeshProUGUI currentAmmoDisplay;
    public TextMeshProUGUI maxAmmoDisplay;


    float shootTime;
    float shootDelay;
    float reloadDelay;
    float modDelay;// = 4.0f;

    int currentAmmo;
    int maxAmmo;

    float damage;
    int bulletsPerShot;
    float spread;
    public float chargeTime;

    float recoil;

    bool initialChargeDone = false;
    bool isExplosive;
    float explosionSize;

    [SerializeField] Light muzzleLight;
    float muzzleLightTime;
    float muzzleLightDuration = 0.1f;

    [SerializeField] LineRenderer railLine;
    [SerializeField] GameObject lineStart;

    //[SerializeField] WeaponModScriptableObject singleFire;
    //[SerializeField] WeaponModScriptableObject fullAutoFire;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = GetComponentInParent<Transform>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        UpdateWeaponStats();
        currentAmmo = maxAmmo;
        UpdateDisplay();

    }

    private void Update()
    {
        if (fireButtonPressed == true)
        {
            if (currentAmmo > 0)
            {
                chargeTime += railGunMod.chargeUpTimeRate * Time.deltaTime;
            } else if (chargeTime > 100 && (railGunMod.fireMode == RailgunModScriptableObject.FireMode.single || initialChargeDone == true))
            {
                chargeTime = 100;
            }

            if(railGunMod.fireMode == RailgunModScriptableObject.FireMode.fullAuto && chargeTime >= railGunMod.initialMaxCharge)
            {
                Fire();
                initialChargeDone = true;
            }


            //Fire();


            if (railGunMod.fireMode == RailgunModScriptableObject.FireMode.fullAuto && chargeTime >= 100 && initialChargeDone == true)
            {
                
                Fire();
                
            }

        } else if (fireButtonPressed == false)
        {
            initialChargeDone = false;
            if (chargeTime > 0)
            {
                Fire();
            }
            
        }
        
        if(chargeTime > 0)
        {
            muzzleLight.enabled = true;
            muzzleLight.intensity = 1 * (chargeTime / 100);
            muzzleLight.color = Color.Lerp(new Color(0,128,255,1), new Color(255,22,0,1), (chargeTime / 100));
            //Debug.Log($"FOV: {Mathf.Lerp(60, 45, chargeTime / 100)}");
            //Camera.main.fieldOfView = Mathf.Lerp(90, 60, chargeTime / 100);

        }
        
    }

    

    private void Awake()
    {
        playerContr = new PlayerInput();
    }

    private void OnEnable()
    {
        fire = playerContr.Player.Fire;
        fire.Enable();
        fire.started += ctx => fireButtonPressed = true;
        fire.canceled += ctx => fireButtonPressed = false;
        reload = playerContr.Player.Reload;
        reload.Enable();
        reload.performed += ctx => RunReload();
        swapMod = playerContr.Player.SwapMod;
        swapMod.Enable();
        swapMod.started += ctx => OpenMenu();

        swapWeapon = playerContr.Player.SwapWeapon;
        swapWeapon.Enable();
        swapWeapon.performed += ctx => SwapWeapon();


    }
    private void OnDisable()
    {
        fire.Disable();
        reload.Disable();
        swapMod.Disable();
        swapWeapon.Disable();
    }

    void RunReload()
    {
        if (canReload)
        {
            StartCoroutine(Reload());
        }
    }

    void SwapWeapon()
    {
        //  Debug.Log("Switch");
        otherGun.SetActive(true);
        otherGun.GetComponent<Gun>().enabled = true;
        otherGun.GetComponent<Gun>().UpdateDisplay();
        gameObject.SetActive(false);
        gameObject.GetComponent<Gun>().enabled = false;
    }


    public void OpenMenu()
    {
        playerMovement.enableLook = false;
        canShoot = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        weaponModCanvas.SetActive(true);
    }

    public void CloseMenu()
    {
        playerMovement.enableLook = true;
        canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        weaponModCanvas.SetActive(false);
        UpdateWeaponStats();
    }

    public void UpdateWeaponStats()
    {
        maxAmmo = gunStats.magazineSize + railGunMod.magazineSizeModifier;
        if (maxAmmo <= 0)
        {
            maxAmmo = 1;
        }

        shootDelay = gunStats.fireDelay + railGunMod.fireDelayModifier;

        reloadDelay = gunStats.reloadDelay + railGunMod.reloadDelayModifier;

        bulletsPerShot = gunStats.bulletsPerShot + railGunMod.additionalBulletsPerShot;

        spread = gunStats.spread + railGunMod.spreadModifier;
        if (spread < 0)
        {
            spread = 0;
        }

        recoil = gunStats.recoil + railGunMod.recoilModifier;

        damage = gunStats.damage;

        explosionSize = railGunMod.explosionSizeIncrease; ;

        if (railGunMod.enablesExplosionImpact)
        {
            isExplosive = true;
        }
        else
        {
            isExplosive = false;
        }

        

        canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        shakeMagnitude = damage * bulletsPerShot * 0.5f;
        shakeDuration = shootDelay * 0.25f;
        //currentAmmo = maxAmmo;
        shootTime = Time.time + modDelay;

        UpdateDisplay();

        //Debug.Log("Ammo " + maxAmmo);
    }

    public void Fire()
    {
        if (currentAmmo > 0 && Time.time > shootDelay + shootTime && canShoot == true)
        {
            //Play Shoot Sound
            shootTime = Time.time;
            UseAmmo(currentAmmo);


            for (int i = 0; i < bulletsPerShot; i++)
            {
                RaycastHit hit;
                Vector3 bulletDir = playerCamera.transform.forward;
                bulletDir.Normalize();
                bulletDir += new Vector3(Random.Range(-spread, spread) / 25, Random.Range(-spread, spread) / 25, Random.Range(-spread, spread) / 25);

                if (Physics.Raycast(playerCamera.transform.position, bulletDir, out hit, float.MaxValue))
                {
                    //Debug.Log(hit.transform);
                    //Debug.Log(spread);
                    //Instantiate(debugObject, hit.point, transform.rotation);
                    if (hit.transform.tag == "Enemy")
                    {

                        hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(damage + damage * (chargeTime / 100) * railGunMod.chargeUpModifier);
                        //Debug.Log($"Added Damage: {damage * (chargeTime / 100) * railGunMod.chargeUpModifier}");
                        //Debug.Log($"Charge: { (chargeTime / 100)}");
                        //Debug.Log($"railGunMod.chargeUpModifier: { railGunMod.chargeUpModifier}");


                    }
                    chargeTime = 0;
                    LineRenderer line = Instantiate(railLine, lineStart.transform.position, Quaternion.identity, gameObject.transform);
                    line.startWidth = (float)(damage * 0.05);
                    StartCoroutine(SpawnLine(line, hit));

                }
                
            }

            

            audioSource.PlayOneShot(fireSound);
            cameraRecoil.Recoil(-recoil, recoil * 0.5f, recoil * 0.175f);
            StartCoroutine(muzzleFlash(0.05f));



            //uncomment when tuned

            //StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            UpdateDisplay();

        }
        else if (currentAmmo == 0 && Time.time > shootDelay + shootTime && canShoot == true)
        {
            shootTime = Time.time;
            audioSource.PlayOneShot(emptySound);
        }

    }
    IEnumerator muzzleFlash(float duration)
    {
        muzzleLight.enabled = true;
        muzzleLight.intensity = damage * bulletsPerShot;
        yield return new WaitForSeconds(duration);
        muzzleLight.enabled = false;
    }

    IEnumerator SpawnLine(LineRenderer line, RaycastHit hit)
    {
        float time = 0;
        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, hit.point);

        
        while (time <= 1)
        {
            
            time += Time.deltaTime;

            yield return null;
        }
        

        Destroy(line.gameObject);
    }
    public void UpdateDisplay()
    {
        currentAmmoDisplay.text = currentAmmo.ToString();
        maxAmmoDisplay.text = maxAmmo.ToString();
    }

    /*
    public void Reload()
    {
        //audioSource.PlayOneShot(reloadSound, 1.0f);
        shootTime = Time.time + reloadDelay;
        
        currentAmmo = maxAmmo;

        UpdateDisplay();
    }
    */
    IEnumerator Reload()
    {
        canShoot = false;
        canReload = false;

        audioSource.PlayOneShot(reloadSound);
        audioSource.pitch = 0.94f;
        yield return new WaitForSeconds(reloadDelay);

        canShoot = true;
        canReload = true;
        currentAmmo = maxAmmo;

        UpdateDisplay();
    }

    public void UseAmmo(int ammo)
    {
        //bullets[ammo].gameObject.SetActive(false);
        currentAmmo--;
    }

    public void changeRailGunMod(RailgunModScriptableObject mod)
    {
        if (mod != null)
        {
            railGunMod = mod;
        }

    }
    

}
