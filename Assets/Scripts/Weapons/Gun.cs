using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Gun : MonoBehaviour
{
    InputAction fire;
    InputAction reload;
    InputAction swapMod;
    InputAction swapWeapon;
    public PlayerInput playerContr;
    bool fireButtonPressed = false;
    bool canShoot = true;
    bool canReload = true;

    [SerializeField] GameObject weaponModCanvas;

    //public GunStatScriptableObjects pistolStats;
    //public GunStatScriptableObjects rifleStats;

    [SerializeField] GunStatScriptableObjects gunStats;
    [SerializeField] WeaponModScriptableObject barrelMod;
    [SerializeField] WeaponModScriptableObject gripMod;
    [SerializeField] WeaponModScriptableObject magMod;
    [SerializeField] WeaponModScriptableObject ammoMod;

    public enum CurrentWeapon
    {
        pistol = 0,
        rifle = 1
    };

    public CurrentWeapon currentWeapon;

    Transform parentTransform;

    [SerializeField] CameraShake cameraShake;
    float shakeDuration;
    float shakeMagnitude;

    [SerializeField] GameObject bulletPrefab;

    //public GameObject pistol;
    //public GameObject rifle;

    [SerializeField] GameObject otherGun;

    public TextMeshProUGUI currentAmmoDisplay;
    public TextMeshProUGUI maxAmmoDisplay;


    float shootTime;
    float shootDelay;
    float reloadDelay;
    float modDelay;

    int currentAmmo;
    int maxAmmo;

    float damage;
    float bulletVelocity;
    int bulletsPerShot;
    float spread;

    float recoil;

    bool isExplosive;
    float explosionSize;


    //[SerializeField] WeaponModScriptableObject singleFire;
    //[SerializeField] WeaponModScriptableObject fullAutoFire;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = GetComponentInParent<Transform>();
        UpdateWeaponStats();
        
    }

    private void Update()
    {
        if (fireButtonPressed == true)
        {
            Fire();
            if(gripMod.fireMode == WeaponModScriptableObject.FireMode.single)
            {
                fireButtonPressed = false;
            }
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
        reload.performed += ctx => StartCoroutine(Reload());
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
        canShoot = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        weaponModCanvas.SetActive(true);

    }

    public void CloseMenu()
    {
        canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        weaponModCanvas.SetActive(false);
        UpdateWeaponStats();
    }

    public void UpdateWeaponStats()
    {
        maxAmmo = gunStats.magazineSize + barrelMod.magazineSizeModifier + gripMod.magazineSizeModifier + ammoMod.magazineSizeModifier + magMod.magazineSizeModifier;
        if (maxAmmo <= 0)
        {
            maxAmmo = 1;
        }

        shootDelay = gunStats.fireDelay + barrelMod.fireDelayModifier + gripMod.fireDelayModifier + ammoMod.fireDelayModifier + magMod.fireDelayModifier;

        reloadDelay = gunStats.reloadDelay + barrelMod.reloadDelayModifier + gripMod.reloadDelayModifier + ammoMod.reloadDelayModifier + magMod.reloadDelayModifier;

        bulletVelocity = gunStats.bulletVelocity + barrelMod.bulletVelocityModifier + gripMod.bulletVelocityModifier + ammoMod.bulletVelocityModifier + magMod.bulletVelocityModifier;

        bulletsPerShot = gunStats.bulletsPerShot + barrelMod.additionalBulletsPerShot + gripMod.additionalBulletsPerShot + ammoMod.additionalBulletsPerShot + magMod.additionalBulletsPerShot;

        spread = gunStats.spread + barrelMod.spreadModifier + gripMod.spreadModifier + ammoMod.spreadModifier + magMod.spreadModifier;
        if (spread < 0)
        {
            spread = 0;
        }

        recoil = gunStats.recoil + barrelMod.recoilModifier + gripMod.recoilModifier + ammoMod.recoilModifier + magMod.recoilModifier;

        damage = gunStats.damage + (gunStats.damage * barrelMod.damageModifier) + (gunStats.damage * gripMod.damageModifier) + (gunStats.damage * ammoMod.damageModifier) + (gunStats.damage * magMod.damageModifier);

        explosionSize = barrelMod.explosionSizeIncrease + magMod.explosionSizeIncrease + ammoMod.explosionSizeIncrease + gripMod.explosionSizeIncrease;

        if (barrelMod.enablesExplosionImpact || magMod.enablesExplosionImpact || ammoMod.enablesExplosionImpact || gripMod.enablesExplosionImpact)
        {
            isExplosive = true;
        } else
        {
            isExplosive = false;
        }

        canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        shakeMagnitude = damage * bulletsPerShot * 0.5f;
        shakeDuration = shootDelay * 0.25f;
        currentAmmo = maxAmmo;
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
            

            for(int i = 0; i< bulletsPerShot; i++)
            {
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward * 1, transform.rotation);
                bullet.GetComponent<Bullet>().damage = this.damage;
                bullet.GetComponent<Bullet>().isExplosive = this.isExplosive;
                bullet.GetComponent<Bullet>().explosionRange = this.explosionSize;
                bullet.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;
                bullet.transform.rotation = gameObject.transform.rotation;
                bullet.transform.Rotate(new Vector3(90, 0, 0));
                Destroy(bullet, 5.0f);
            }

            //uncomment when tuned

            //StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            UpdateDisplay();

        } else
        {
            //Play Empty Sound
        }

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

    public void changeBarrel(WeaponModScriptableObject mod)
    {
        if (mod != null)
        {
            barrelMod = mod;
        }
        
    }
    public void changeMag(WeaponModScriptableObject mod)
    {
        if (mod != null)
        {
            magMod = mod;
        }
    }
    public void changeAmmo(WeaponModScriptableObject mod)
    {
        if (mod != null)
        {
            ammoMod = mod;
        }
    }
    public void changeGrip(WeaponModScriptableObject mod)
    {
        if (mod != null)
        {
            gripMod = mod;
        }
    }

}
