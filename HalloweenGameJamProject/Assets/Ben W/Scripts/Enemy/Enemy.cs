using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public void TakeDamage(float damage)
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


