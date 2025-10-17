using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    [Header("Player")]
    private float movementSpeed = 5f;
    private float boostedSpeed = 10f;
    private float lightHP;
    private float lightHPStart = 50f;
    private float lightHPMax = 100f;
    private float lightHPLossRate = 1f;
    private float soulLightGain = 4f;
    private float camplightRegenRate = 8f;
    private int score;
    


    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float BoostedSpeed { get => boostedSpeed; set => boostedSpeed = value; }
    public float LightHP 
    { 
        get => lightHP;
        set
        {
            lightHP = value;
            healthToLight.AdjustLight(value);
            text1.text = "Heath : " + (Mathf.Round(LightHP * 10) / 10f).ToString();
            //healthToLight.AdjustLight();
        } 
    }
    public float LightHPStart { get => lightHPStart; set => lightHPStart = value; }
    public float LightHPMax { get => lightHPMax; set => lightHPMax = value; }
    public float LightHPLossRate { get => lightHPLossRate; set => lightHPLossRate = value; }
    public float SoulLightGain { get => soulLightGain; set => soulLightGain = value; }
    public float CamplightRegenRate { get => camplightRegenRate; set => camplightRegenRate = value; }
    public int Score { get => score; 
        set { 
            score = value;
            text2.text = "Score : " + value;
        } 
    }


    [Header("Sinks")]
    private float campfireLightCost = 15f;
    private float graveStoneCost = 20f;

    public float CampfireLightCost { get => campfireLightCost; set => campfireLightCost = value; }
    public float GraveStoneCost { get => graveStoneCost; set => graveStoneCost = value; }

    [Header("Weapons")]
    private float meleeDamage;
    private float bulletDamage;
    private int shotgunMaxAmmo;
    private int pistolMaxAmmo;
    private int rifleMaxAmmo;
    private float bulletDamageIncrease;

    public float MeleeDamage { get => meleeDamage; set => meleeDamage = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public int ShotgunMaxAmmo { get => shotgunMaxAmmo; set => shotgunMaxAmmo = value; }
    public int PistolMaxAmmo { get => pistolMaxAmmo; set => pistolMaxAmmo = value; }
    public int RifleMaxAmmo { get => rifleMaxAmmo; set => rifleMaxAmmo = value; }
    public float BulletDamageIncrease { get => bulletDamageIncrease; set => bulletDamageIncrease = value; }



    [Header("Abilities")]
    private float mineDamage = 5f;
    private float mineArmTime = 1f;
    private float fireWheelDamage = 1f;
    private float fireWheelTickInterval = 1f;

    public float MineDamage { get => mineDamage; set => mineDamage = value; }
    public float MineArmTime { get => mineArmTime; set => mineArmTime = value; }
    public float FireWheelDamage { get => fireWheelDamage; set => fireWheelDamage = value; }
    public float FireWheelTickInterval { get => fireWheelTickInterval; set => fireWheelTickInterval = value; }

    [Header("Augments")]
    private float flameDamage;
    private float explosionRadius;
    private float explosionDamage;
    private float freezeEffect; 
    private float LoseAgroDuration;

    public float FlameDamage { get => flameDamage; set => flameDamage = value; }
    public float ExplosionRadius { get => explosionRadius; set => explosionRadius = value; }
    public float ExplosionDamage { get => explosionDamage; set => explosionDamage = value; }
    public float FreezeEffect { get => freezeEffect; set => freezeEffect = value; }
    public float LoseAgroDuration1 { get => LoseAgroDuration; set => LoseAgroDuration = value; }

    [Header("Cooldowns")]
    private float meleeCooldown;
    private float mineCooldown;
    private float fireWheelCooldown = 15f;
    private float explosionCooldown;
    private float flameCooldown;
    private float mineThrowCooldown = 10f;

    public float MeleeCooldown { get => meleeCooldown; set => meleeCooldown = value; }
    public float MineCooldown { get => mineCooldown; set => mineCooldown = value; }
    public float FireWheelCooldown { get => fireWheelCooldown; set => fireWheelCooldown = value; }
    public float ExplosionCooldown { get => explosionCooldown; set => explosionCooldown = value; }
    public float FlameCooldown { get => flameCooldown; set => flameCooldown = value; }
    public float MineThrowCooldown { get => mineThrowCooldown; set => mineThrowCooldown = value; }



    [Header("Durations")]
    private float bulletDamageBoostDuration;
    private float speedPowerupDuration;
    private float fireWheelDuration = 10f;

    public float BulletDamageBoostDuration { get => bulletDamageBoostDuration; set => bulletDamageBoostDuration = value; }
    public float SpeedPowerupDuration { get => speedPowerupDuration; set => speedPowerupDuration = value; }
    public float FireWheelDuration { get => fireWheelDuration; set => fireWheelDuration = value; }


    [Header("Booleans")]
    private bool canThrowMine = true;
    private bool canUseFireWheel = true;

    public bool CanThrowMine { get => canThrowMine; set => canThrowMine = value; }
    public bool CanUseFireWheel { get => canUseFireWheel; set => canUseFireWheel = value; }

    private HealthToLight healthToLight;

    public TMP_Text text1;
    public TMP_Text text2;

    private void Awake()
    {
        healthToLight = GetComponent<HealthToLight>();

    }

    private void Update()
    {
        
        if (text1 != null)
            text1.text = "Heath : " + (Mathf.Round(LightHP * 10) / 10f).ToString();

        if (text2 != null)
        {
            text2.text = "Score : " + Score.ToString();
            //Debug.Log ("Light HP: " + LightHP);
            Debug.Log("Score: " + score);
        }

    }
}
