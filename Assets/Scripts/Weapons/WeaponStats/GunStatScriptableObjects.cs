using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GunStatScriptableObject", menuName = "ScriptableObjects/GunStats")]
public class GunStatScriptableObjects : ScriptableObject
{
    public float spread = 0.0f;
    public float damage = 1.0f;
    public float fireDelay = 0.5f;
    public float bulletVelocity = 100.0f;
    public int bulletsPerShot = 1;
    public float reloadDelay = 3.0f;
    public int magazineSize = 10;
    public float recoil = 1.0f;
    public bool isCharged = false;



    

}
