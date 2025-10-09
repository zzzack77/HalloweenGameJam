using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Play death animation
            // Update score
            Destroy(gameObject);
        }
    }
    
}
