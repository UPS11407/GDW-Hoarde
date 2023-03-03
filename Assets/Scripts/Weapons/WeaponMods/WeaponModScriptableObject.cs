
using UnityEngine;

[CreateAssetMenu(fileName = "WModScriptableObject", menuName = "ScriptableObjects/WeaponMods")]
public class WeaponModScriptableObject : ScriptableObject
{
    
    public enum FireMode { None, single, burst, fullAuto };
    public enum DamageType { Standard, Incindiary, Slow};
    public FireMode fireMode;
    public DamageType damageType;
    
    public float spreadModifier = 0.0f; //+
    public float damageModifier = 0.00f; // 1 = 100%
    public float fireDelayModifier = 0.0f; //+
    public float bulletVelocityModifier = 0.0f; //+
    public int additionalBulletsPerShot = 0; //+
    public float reloadDelayModifier = 0.0f; //+
    public int magazineSizeModifier = 0; //+
    public bool enablesExplosionImpact = false; //bool
    public bool becomeProjectile = false;
    public float explosionSizeIncrease = 0.0f; //+
    public float recoilModifier = 0.0f;
    


}
