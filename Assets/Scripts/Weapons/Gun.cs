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

    //public GunStatScriptableObjects pistolStats;
    //public GunStatScriptableObjects rifleStats;

    [SerializeField] GunStatScriptableObjects gunStats;
    [SerializeField] RailgunModScriptableObject railGunMod;
    [SerializeField] WeaponModScriptableObject barrelMod;
    [SerializeField] WeaponModScriptableObject gripMod;
    [SerializeField] WeaponModScriptableObject magMod;
    [SerializeField] WeaponModScriptableObject ammoMod;

    private AudioSource audioSource;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip emptySound;

    [SerializeField] Camera playerCamera;

    [SerializeField] CameraShake cameraShake;
    [SerializeField] CameraRecoil cameraRecoil;

    [SerializeField] GameObject bulletPrefab;

    //public GameObject pistol;
    //public GameObject rifle;

    [SerializeField] GameObject otherGun;

    [SerializeField] GameObject bloodSplatter;
 
    public TextMeshProUGUI currentAmmoDisplay;
    public TextMeshProUGUI maxAmmoDisplay;


    bool bursting = false;

    float shootTime;
    float shootDelay;
    float reloadDelay;
    float modDelay;// = 4.0f;

    int currentAmmo;
    int maxAmmo;

    float damage;
    float bulletVelocity;
    int bulletsPerShot;
    float spread;

    float recoil;

    bool isProjectile;

    bool isExplosive;
    float explosionSize;

    bool isCharged = false;
    float chargeTime;
    bool initialChargeDone = false;

    public bool modReloadRequired;

    [SerializeField] LineRenderer railLine;
    [SerializeField] TrailRenderer bulletTracer;
    [SerializeField] GameObject tracerStart;
    [SerializeField] MuzzleFlash muzzleFlash;

    //[SerializeField] WeaponModScriptableObject singleFire;
    //[SerializeField] WeaponModScriptableObject fullAutoFire;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateWeaponStats();
        currentAmmo = maxAmmo;
        UpdateDisplay();
    }

    private void Update()
    {

        if (isCharged)
        {
            if (fireButtonPressed == true)
            {
                if (currentAmmo > 0)
                {
                    chargeTime += railGunMod.chargeUpTimeRate * Time.deltaTime;
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
        if (canReload && currentAmmo != maxAmmo)
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

        canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        
        if (modReloadRequired)
        {
            currentAmmo = 0;
            Reload();
            modReloadRequired = false;
        }
        
        shootTime = Time.time + modDelay;

        UpdateDisplay();

        //Debug.Log("Ammo " + maxAmmo);
    }

    public void ToggleFire(bool toggle)
    {
        fireButtonPressed = toggle;
    }

    public void Fire()
    {
        if (currentAmmo > 0 && Time.time > shootDelay + shootTime && canShoot == true)
        {
            //Play Shoot Sound
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
                                
                                Debug.Log($"Added Damage: {damage * (chargeTime / 100) * railGunMod.chargeUpModifier}");
                                Debug.Log($"ChargeTime: { (chargeTime)}");
                                Debug.Log($"Charge: { (chargeTime / 100)}");
                                Debug.Log($"railGunMod.chargeUpModifier: { railGunMod.chargeUpModifier}");


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
                                GameObject bloodParticles = (GameObject)Instantiate(bloodSplatter, hit.transform.position, hit.transform.rotation);

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

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time <= 1)
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

    IEnumerator BurstFire()
    {
        bursting = true;
        if(currentAmmo >= 1)
        {
            yield return new WaitForSeconds(shootDelay);
                Fire();
            yield return new WaitForSeconds(shootDelay);
                Fire();
            yield return new WaitForSeconds(shootDelay * 3);
        } else
        {
            yield return new WaitForSeconds(shootDelay);
                Fire();
            yield return new WaitForSeconds(shootDelay * 3);
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

    public void ModReload()
    {
        modReloadRequired = true;
    }
}
