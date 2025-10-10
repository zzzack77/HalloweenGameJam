using System;
using UnityEngine;

public enum PowerupType
{
    SpeedBoost,
    DamageBoost,
    RingOfFire
}
public class Powerup : MonoBehaviour
{
    public PowerupType powerupType;

    // Events
    public static event Action OnSpeedBoost;
    public static event Action OnDamageBoost;
    public static event Action OnRingOfFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (powerupType)
            {
                case PowerupType.SpeedBoost:
                    OnSpeedBoost?.Invoke();
                    break;

                case PowerupType.DamageBoost:
                    OnDamageBoost?.Invoke();
                    break;

                case PowerupType.RingOfFire:
                    OnRingOfFire?.Invoke();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
