using UnityEngine;

public class MineProjectile : PlayerProjectile
{
    
    public float rotationSpeed = 150f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        timeAlive = 1.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        // Rotate around Z axis (2D rotation)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
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
