using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player")]
    public float movementSpeed;
    public float lightHP;

    [Header("Weapons")]
    public float meleeDamage;
    public float bulletDamage;

    [Header("Abilities")]
    public float mineDamage;
    public float mineArmTime;
    public float fireWheelDamage;

    [Header("Cooldowns")]
    public float meleeCooldown;
    public float mineCooldown;
    public float fireWheelCooldown;

    [Header("Durations")]
    public float bulletDamageBoostDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
