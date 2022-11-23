
using UnityEngine;

[CreateAssetMenu(fileName = "WModScriptableObject", menuName = "ScriptableObjects/WeaponMods")]
public class WeaponModScriptableObject : ScriptableObject
{
    
    public enum FireMode { None, single, burst, fullAuto };

    public FireMode fireMode;
    
    public float spreadModifier = 0.0f; //+
    public float damageModifier = 0.00f; //%
    public float fireDelayModifier = 0.0f; //+
    public float bulletVelocityModifier = 0.0f; //+
    public int additionalBulletsPerShot = 0; //+
    public float dashDamageModifier = 0.0f; //%
    public float dashDelayModifier = 0.0f; //+
    public float reloadDelayModifier = 0.0f; //+
    public int magazineSizeModifier = 0; //+
    public bool enablesExplosionImpact = false; //bool
    public float explosionSizeIncrease = 0.0f; //+
    public float recoilModifier = 0.0f;
    


}
