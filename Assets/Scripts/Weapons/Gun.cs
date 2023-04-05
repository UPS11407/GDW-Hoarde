using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Gun : MonoBehaviour
{
    bool fireButtonPressed = false;
    public bool canShoot = true;
    public bool canReload = true;
    public bool canSwap = true;
    public bool isReloading = false;
    
    //public GunStatScriptableObjects pistolStats;
    //public GunStatScriptableObjects rifleStats;

    [SerializeField] GunStatScriptableObjects gunStats;
    public RailgunModScriptableObject railGunMod;
    public WeaponModScriptableObject barrelMod;
    public WeaponModScriptableObject gripMod;
    public WeaponModScriptableObject magMod;
    public WeaponModScriptableObject ammoMod;

    private AudioSource audioSource;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip emptySound;

    [SerializeField] Camera playerCamera;

    [SerializeField] CameraShake cameraShake;
    [SerializeField] CameraRecoil cameraRecoil;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject bloodSplatter;
 
    public TextMeshProUGUI currentAmmoDisplay;
    public TextMeshProUGUI maxAmmoDisplay;

    [SerializeField] Inventory inventory; 

    bool bursting = false;

    float shootTime;
    float shootDelay;
    float reloadDelay;
    float modDelay;// = 4.0f;

    public int currentAmmo;
    public int maxAmmo;

    public bool isUnarmed;

    float damage;
    float bulletVelocity;
    int bulletsPerShot;
    public float spread;

    float recoil;

    bool isProjectile;

    bool isExplosive;
    float explosionSize;

    bool isCharged = false;
    bool charging = false;
    float chargeTime;
    bool initialChargeDone = false;

    public Animator _animator;
    public bool modReloadRequired;

    [SerializeField] LineRenderer railLine;
    [SerializeField] TrailRenderer bulletTracer;
    [SerializeField] GameObject tracerStart;
    [SerializeField] MuzzleFlash muzzleFlash;

    [SerializeField] float laserDuration = 0.75f;
    [SerializeField] float trailDuration = 1.0f;

    bool isRunning;

    public AudioClip railgunCharge;

    //[SerializeField] WeaponModScriptableObject singleFire;
    //[SerializeField] WeaponModScriptableObject fullAutoFire;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _animator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (!isUnarmed)
        {
            UpdateWeaponStats();
        }

        currentAmmo = maxAmmo;
        UpdateDisplay();
    }

    private void Update()
    {
        isRunning = GameObject.Find("Player").GetComponent<PlayerControlsManager>().sprintingAnim;
        if (isUnarmed)
        {

            GameObject.Find("Player").GetComponent<HeadBob>().enabled = false;

            if (fireButtonPressed == true)
            {
                Fire();
            }

            return;
        }
        else
        {
            GameObject.Find("Player").GetComponent<HeadBob>().enabled = true;
        }

        if (isCharged && !isRunning)
        {
            if (fireButtonPressed == true)
            {
                if (currentAmmo > 0)
                {
                    chargeTime += railGunMod.chargeUpTimeRate * Time.deltaTime;

                    if (!charging)
                    {
                        if (railGunMod.name == "SpeedCharge3000")
                        {
                            audioSource.pitch = 1.5f;
                        }
                        audioSource.PlayOneShot(railgunCharge);
                    }
                    
                    charging = true;
                }
                else if (chargeTime > 100 && (railGunMod.fireMode == RailgunModScriptableObject.FireMode.single || initialChargeDone == true))
                {
                    chargeTime = 100;
                }

                if (railGunMod.fireMode == RailgunModScriptableObject.FireMode.fullAuto && chargeTime >= railGunMod.initialMaxCharge)
                {
                    Fire();
                    initialChargeDone = true;
                }

                if (railGunMod.fireMode == RailgunModScriptableObject.FireMode.fullAuto && chargeTime >= 100 && initialChargeDone == true)
                {

                    Fire();

                }

            }
            else if (fireButtonPressed == false)
            {
                initialChargeDone = false;
                if (chargeTime > 0)
                {
                    Fire();
                }

            }
            if (chargeTime > 0)
            {
                //   ETHAN PLEASE MAKE A DIFFERENT LIGHT FOR THE CHARGEUP
                //muzzleLight.enabled = true;
                //muzzleLight.intensity = 1 * (chargeTime / 100);
                //muzzleLight.color = Color.Lerp(new Color(0, 128, 255, 1), new Color(255, 22, 0, 1), (chargeTime / 100));
            }
        }


        else if(fireButtonPressed == true && !bursting)
        {
            Fire();

            if (gripMod.fireMode == WeaponModScriptableObject.FireMode.burst)
            {
                fireButtonPressed = false;
                StartCoroutine(BurstFire());
                
            }

            if (gripMod.fireMode == WeaponModScriptableObject.FireMode.single)
            {
                fireButtonPressed = false;
            }
        }

    }

    public void ToggleFireButton(bool toggle)
    {
        fireButtonPressed = toggle;
    }

    public void RunReload()
    {
        if (canReload && currentAmmo != maxAmmo && inventory.GetAmmoCount() > 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void ToggleSelf(bool toggle)
    {
        gameObject.SetActive(toggle);
        gameObject.GetComponent<Gun>().enabled = toggle;
    }
    


    public void UpdateWeaponStats()
    {
        maxAmmo = gunStats.magazineSize + barrelMod.magazineSizeModifier + gripMod.magazineSizeModifier + ammoMod.magazineSizeModifier + magMod.magazineSizeModifier + railGunMod.magazineSizeModifier;
        if (maxAmmo <= 0)
        {
            maxAmmo = 1;
        }

        shootDelay = gunStats.fireDelay + barrelMod.fireDelayModifier + gripMod.fireDelayModifier + ammoMod.fireDelayModifier + magMod.fireDelayModifier;

        reloadDelay = gunStats.reloadDelay + barrelMod.reloadDelayModifier + gripMod.reloadDelayModifier + ammoMod.reloadDelayModifier + magMod.reloadDelayModifier;

        bulletVelocity = gunStats.bulletVelocity + barrelMod.bulletVelocityModifier + gripMod.bulletVelocityModifier + ammoMod.bulletVelocityModifier + magMod.bulletVelocityModifier;

        bulletsPerShot = gunStats.bulletsPerShot + barrelMod.additionalBulletsPerShot + gripMod.additionalBulletsPerShot + ammoMod.additionalBulletsPerShot + magMod.additionalBulletsPerShot;

        spread = gunStats.spread + barrelMod.spreadModifier + gripMod.spreadModifier + ammoMod.spreadModifier + magMod.spreadModifier + railGunMod.spreadModifier;
        if (spread < 0)
        {
            spread = 0;
        }

        recoil = gunStats.recoil + barrelMod.recoilModifier + gripMod.recoilModifier + ammoMod.recoilModifier + magMod.recoilModifier + railGunMod.recoilModifier;
        if (recoil < 0)
        {
            recoil = 0;
        }

        damage = gunStats.damage + (gunStats.damage * barrelMod.damageModifier) + (gunStats.damage * gripMod.damageModifier) + (gunStats.damage * ammoMod.damageModifier) + (gunStats.damage * magMod.damageModifier);

        explosionSize = barrelMod.explosionSizeIncrease + magMod.explosionSizeIncrease + ammoMod.explosionSizeIncrease + gripMod.explosionSizeIncrease + railGunMod.explosionSizeIncrease;



        if (barrelMod.enablesExplosionImpact || magMod.enablesExplosionImpact || ammoMod.enablesExplosionImpact || gripMod.enablesExplosionImpact || railGunMod.enablesExplosionImpact)
        {
            isExplosive = true;
        } else
        {
            isExplosive = false;
        }


        
        if (gunStats.isCharged)
        {
            isCharged = true;
        } else
        {
            isCharged = false;
        }

        if (ammoMod.becomeProjectile)
        {
            isProjectile = true;
        } else
        {
            isProjectile = false;
        }

        if (gripMod.fireMode == WeaponModScriptableObject.FireMode.burst)
        {
            shootDelay = 0;

        }

        canShoot = true;
        
        shootTime = Time.time + modDelay;

        UpdateDisplay();
    }

    public void ToggleFire(bool toggle)
    {
        fireButtonPressed = toggle;
    }

    public void Fire()
    {
        if (isUnarmed)
        {
            GameObject.Find("Player").GetComponent<Player>().QuickMelee();
            return;
        }

        charging = false;
        audioSource.Stop();
        audioSource.pitch = 0.94f;

        if (currentAmmo > 0 && Time.time > shootDelay + shootTime && canShoot == true)
        {
            shootTime = Time.time;
            currentAmmo--;
            
            
            for (int i = 0; i < bulletsPerShot; i++)
            {
                if (isProjectile) { 
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.parent.position + transform.parent.forward * 1, transform.parent.rotation);
                bullet.GetComponent<Bullet>().damage = this.damage;
                bullet.GetComponent<Bullet>().isExplosive = this.isExplosive;
                bullet.GetComponent<Bullet>().explosionRange = this.explosionSize;
                bullet.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;
                bullet.transform.rotation = gameObject.transform.rotation;
                bullet.transform.Rotate(new Vector3(0, 0, 0));
                Destroy(bullet, 5.0f);
                }
                else
                {
                        
                    RaycastHit hit;
                    Vector3 bulletDir = playerCamera.transform.forward;
                    bulletDir.Normalize();
                    bulletDir += new Vector3(Random.Range(-spread, spread) / 25, Random.Range(-spread, spread) / 25, Random.Range(-spread, spread) / 25);

                    if (Physics.Raycast(playerCamera.transform.position, bulletDir, out hit, float.MaxValue))
                    {
                        
                        //Instantiate(debugObject, hit.point, transform.rotation);
                        if (isCharged)
                        {
                            if (hit.transform.tag == "Enemy")
                            {
                                hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(damage + damage * (chargeTime / 100) * railGunMod.chargeUpModifier);
                            }
                            
                            LineRenderer line = Instantiate(railLine, tracerStart.transform.position, Quaternion.identity, gameObject.transform);
                            line.startWidth = (float)(damage * 0.05);
                            StartCoroutine(SpawnLine(line, hit));

                        } else
                        {
                            if (hit.transform.tag == "Enemy")
                            {

                                
                                if(ammoMod.damageType == WeaponModScriptableObject.DamageType.Incindiary)
                                {
                                    hit.transform.gameObject.GetComponent<EnemyBase>().ApplyDamageOverTime(damage * 0.5f);
                                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(damage * 0.5f);
                                } else if (ammoMod.damageType == WeaponModScriptableObject.DamageType.Slow)
                                {
                                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
                                    hit.transform.gameObject.GetComponent<EnemyBase>().ApplySlow(damage);
                                }
                                else
                                {
                                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
                                }
                                GameObject bloodParticles = (GameObject)Instantiate(bloodSplatter, hit.point, hit.transform.rotation);

                            }
                            TrailRenderer tracer = Instantiate(bulletTracer, tracerStart.transform.position, Quaternion.identity);
                            tracer.startWidth = (float)(damage * 0.1);
                            StartCoroutine(SpawnTrail(tracer, hit));
                        }

                        
                    }
                    chargeTime = 0;
                }
                    
            }
            
            

            audioSource.PlayOneShot(fireSound);
            cameraRecoil.Recoil(-recoil, recoil * 0.5f, recoil * 0.175f);
            muzzleFlash.FlashMuzzle(damage, bulletsPerShot, 0.03f);

            UpdateDisplay();

        } else if (currentAmmo == 0 && Time.time > shootDelay + shootTime && canShoot == true)
        {
            shootTime = Time.time;
            audioSource.PlayOneShot(emptySound);
        }

    }

    IEnumerator SpawnLine(LineRenderer line, RaycastHit hit)
    {
        //line.gameObject.layer = 10;
        float time = 0;
        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, hit.point);

        Color laserColor = line.material.GetColor("_EmissionColor");
        while (time <= laserDuration)
        {
            line.material.color = Color.Lerp(new Color(line.material.color.r, line.material.color.g, line.material.color.b, 1), new Color(line.material.color.r, line.material.color.g, line.material.color.b, 0), time / laserDuration);
            line.material.SetColor("_EmissionColor", laserColor * Mathf.Pow(2.0f, Mathf.Lerp(3, -10, time / laserDuration)));
            time += Time.deltaTime;

            yield return null;
        }


        Destroy(line.gameObject);
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        trail.gameObject.layer = 10;
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time <= trailDuration)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }
    public void UpdateDisplay()
    {
        if (!isUnarmed)
        {
            currentAmmoDisplay.text = currentAmmo.ToString();
            currentAmmoDisplay.color = Color.white;

            maxAmmoDisplay.text = inventory.GetAmmoCount().ToString();
            maxAmmoDisplay.color = Color.white;
        }
        else
        {
            currentAmmoDisplay.text = "-";
            currentAmmoDisplay.color = Color.gray;

            maxAmmoDisplay.text = "-";
            maxAmmoDisplay.color = Color.gray;
        }

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
        isReloading = true;

        int amountToReload = maxAmmo - currentAmmo;

        if (inventory.GetAmmoCount() - amountToReload < 0)
        {
            amountToReload = inventory.GetAmmoCount();
        }

        audioSource.PlayOneShot(reloadSound);
        audioSource.pitch = 0.94f;
        yield return new WaitForSeconds(reloadDelay);
        
        canShoot = true;
        canReload = true;
        isReloading = false;
        currentAmmo += amountToReload;
        inventory.AddAmmo(-amountToReload, inventory.selectedAmmo);

        UpdateDisplay();
    }

    IEnumerator BurstFire()
    {
        bursting = true;
        if(currentAmmo >= 1)
        {
            yield return new WaitForSeconds(0.05f);
                Fire();
            yield return new WaitForSeconds(0.05f);
                Fire();
            yield return new WaitForSeconds(2f);
        } else
        {
            yield return new WaitForSeconds(0.05f);
                Fire();
            yield return new WaitForSeconds(2f);
        }
        
        bursting = false;
    }

    public void changeBarrel(WeaponModScriptableObject mod)
    {
        if (mod != null)
        {
            barrelMod = mod;
        }
        
    }    
    public void changeBarrel(RailgunModScriptableObject mod)
    {
        if (mod != null)
        {
            railGunMod = mod;
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
