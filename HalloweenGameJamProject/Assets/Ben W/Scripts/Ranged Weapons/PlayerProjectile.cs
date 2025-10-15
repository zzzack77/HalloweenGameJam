using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public PlayerStats playerStats;
    public Rigidbody2D rb;
    public float timeAlive = 4;
    public float shootForce = 0.2f;
    //public float damage = 2f;
    //public float damageIncrease = 2f;
    //public float damageBoostDuration = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        StartCoroutine(Despawn());
    }
    
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
        
    }

    private void OnEnable()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Powerup.OnDamageBoost += DamageBoostPowerup;
    }

    private void OnDisable()
    {
        Powerup.OnDamageBoost -= DamageBoostPowerup;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(playerStats.BulletDamage);
            Destroy(gameObject);
        }
    }

    

    public void DamageBoostPowerup()
    {
        playerStats.BulletDamage += playerStats.BulletDamageIncrease;
        StartCoroutine(DamageBoostTimer());
    }

    IEnumerator DamageBoostTimer()
    {
        yield return new WaitForSeconds(playerStats.BulletDamageBoostDuration);
        playerStats.BulletDamage -= playerStats.BulletDamageIncrease;
    }
}
