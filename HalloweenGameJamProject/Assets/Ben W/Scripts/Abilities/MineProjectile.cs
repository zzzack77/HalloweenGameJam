using UnityEngine;

public class MineProjectile : PlayerProjectile
{
    PlayerStats playerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        timeAlive = 1.5f;
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        TravelForward();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(playerStats.MineDamage);
            Destroy(gameObject);
        }
    }

}
