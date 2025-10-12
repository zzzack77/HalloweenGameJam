using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Player player;
    private Enemy enemy;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(player.meleeDamage);
        }
    }
}
