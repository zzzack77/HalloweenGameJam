using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player")]
    public float movementSpeed;
    public float baseSpeed;
    public float boostedSpeed;
    public float lightHP;

    [Header("Weapons")]
    public float meleeDamage;
    public float bulletDamage;
    public int shotgunMaxAmmo;
    public int pistolMaxAmmo;
    public float bulletDamageIncrease;

    [Header("Abilities")]
    public float mineDamage;
    public float mineArmTime;
    public float fireWheelDamage;
    public float fireWheelTickInterval;

    [Header("Cooldowns")]
    public float meleeCooldown;
    public float mineCooldown;
    public float fireWheelCooldown;

    [Header("Durations")]
    public float bulletDamageBoostDuration;
    public float speedPowerupDuration;
    public float fireWheelDuration;

    [Header("Booleans")]
    public bool canThrowMine = true;
    public bool canUseFireWheel = true;

}
