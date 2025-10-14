using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float timeAlive = 4;
    public float shootForce = 0.2f;
    public float damage = 2f;
    public float damageIncrease = 2f;
    public float damageBoostDuration = 8f;

    private AugmentStructure effects;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        effects = GameObject.FindGameObjectWithTag("Player").GetComponent<BAPlayer>().effects;
    }

    // Update is called once per frame
    void Update()
    {
        TravelForward();
    }

    private void OnEnable()
    {
        Powerup.OnDamageBoost += DamageBoostPowerup;
    }

    private void OnDisable()
    {
        Powerup.OnDamageBoost -= DamageBoostPowerup;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void TravelForward()
    {
        rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        timeAlive -= Time.deltaTime;
        if (timeAlive < 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageBoostPowerup()
    {
        damage += damageIncrease;
        StartCoroutine(DamageBoostTimer());
    }

    IEnumerator DamageBoostTimer()
    {
        yield return new WaitForSeconds(damageBoostDuration);
        damage -= damageIncrease;
    }
}
