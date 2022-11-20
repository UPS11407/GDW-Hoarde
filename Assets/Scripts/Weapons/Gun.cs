using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunStatScriptableObjects gunStats;
    [SerializeField] WeaponModScriptableObject barrelMod;
    [SerializeField] WeaponModScriptableObject gripMod;
    [SerializeField] WeaponModScriptableObject magMod;
    [SerializeField] WeaponModScriptableObject ammoMod;



    public Transform parentTransform;

    //[SerializeField] CameraShake cameraShake;
    float shakeDuration;
    float shakeMagnitude;

    [SerializeField] GameObject bulletPrefab;


    float shootTime;
    [SerializeField] float shootDelay;
    [SerializeField] float reloadDelay;

    [SerializeField] int currentAmmo;
    [SerializeField] int maxAmmo;

    [SerializeField] float damage;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    [SerializeField] float spread;

    [SerializeField] float recoil;

    [SerializeField] bool isExplosive;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = GetComponentInParent<Transform>();
        UpdateWeaponStats();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    public void UpdateWeaponStats()
    {
        maxAmmo = gunStats.magazineSize + barrelMod.magazineSizeModifier + gripMod.magazineSizeModifier + ammoMod.magazineSizeModifier + magMod.magazineSizeModifier;

        shootDelay = gunStats.fireDelay + barrelMod.fireDelayModifier + gripMod.fireDelayModifier + ammoMod.fireDelayModifier + magMod.fireDelayModifier;

        reloadDelay = gunStats.reloadDelay + barrelMod.reloadDelayModifier + gripMod.reloadDelayModifier + ammoMod.reloadDelayModifier + magMod.reloadDelayModifier;

        bulletVelocity = gunStats.bulletVelocity + barrelMod.bulletVelocityModifier + gripMod.bulletVelocityModifier + ammoMod.bulletVelocityModifier + magMod.bulletVelocityModifier;

        bulletsPerShot = gunStats.bulletsPerShot + barrelMod.additionalBulletsPerShot + gripMod.additionalBulletsPerShot + ammoMod.additionalBulletsPerShot + magMod.additionalBulletsPerShot;

        spread = gunStats.spread + barrelMod.spreadModifier + gripMod.spreadModifier + ammoMod.spreadModifier + magMod.spreadModifier;

        recoil = gunStats.recoil + barrelMod.recoilModifier + gripMod.recoilModifier + ammoMod.recoilModifier + magMod.recoilModifier;

        damage = gunStats.damage + (gunStats.damage * barrelMod.damageModifier) + (gunStats.damage * gripMod.damageModifier) + (gunStats.damage * ammoMod.damageModifier) + (gunStats.damage * magMod.damageModifier);

        shakeDuration = shootDelay * 0.25f;
        shakeMagnitude = damage * bulletsPerShot * 0.5f;
        Reload();
    }

    public void Fire()
    {
        if (currentAmmo > 0 && Time.time > shootDelay + shootTime)
        {
            //Play Shoot Sound
            shootTime = Time.time;
            UseAmmo(currentAmmo);
            //StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            for(int i = 0; i<= bulletsPerShot; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1, transform.rotation);
                bullet.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.transform.rotation = gameObject.transform.rotation;
                bullet.transform.Rotate(new Vector3(90, 0, 0));
                Destroy(bullet, 5.0f);
            }
            
            
        } else
        {
            //Play Empty Sound
        }

    }

    public void Reload()
    {
        //audioSource.PlayOneShot(reloadSound, 1.0f);
        shootTime = Time.time + reloadDelay;
        /*
        foreach (GameObject ammo in bullets)
        {
            ammo.gameObject.SetActive(true);
        }
        */
        currentAmmo = maxAmmo;
    }

    public void UseAmmo(int ammo)
    {
        //bullets[ammo].gameObject.SetActive(false);
        currentAmmo--;
    }

}
