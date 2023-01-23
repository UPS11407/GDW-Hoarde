
using UnityEngine;

[CreateAssetMenu(fileName = "RGModScriptableObject", menuName = "ScriptableObjects/RailGunMods")]
public class RailgunModScriptableObject : ScriptableObject
{

    public enum FireMode { None, single, fullAuto };

    public FireMode fireMode;

    public float spreadModifier = 0.0f; //+
    public float fireDelayModifier = 0.0f; //+
    public int additionalBulletsPerShot = 0; //+
    public float reloadDelayModifier = 0.0f; //+
    public int magazineSizeModifier = 0; //+
    public bool enablesExplosionImpact = false; //bool
    public float explosionSizeIncrease = 0.0f; //+
    public float recoilModifier = 0.0f;

    public float chargeUpTimeRate = 50.0f; //per second. Max charge is 100
    public float initialMaxCharge = 500.0f; // For Full auto only
    public float chargeUpModifier = 0.0f; // 1 = 100%



}
