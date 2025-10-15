using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private Enemy enemy;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(playerStats.MeleeDamage);
        }
    }
}
