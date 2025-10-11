using UnityEngine;

public class MBEnemy : MonoBehaviour
{
    // Enemy stats that should be changed in inspector for testing etc.
    private float health;
    private int souls;
    public GameObject soulPrefab;

    void Start()
    {
        health = Random.Range(MBGameManager.Instance.enemyMinHealth, MBGameManager.Instance.enemyMaxHealth);
        souls = Random.Range(MBGameManager.Instance.enemyMinNumSouls, MBGameManager.Instance.enemyMaxNumSouls);
    }

    // Call this method to apply damage to the enemy
    public void TakeDamage(float amount)
    {
        health -= amount;
        CheckHealth();
    }

    // Destroys the enemy when health is 0 or below
    void CheckHealth()
    {
        if (health <= 0f)
        {
            SpawnSouls();
            Destroy(gameObject);
        }
    }

    // Spawns the specified number of souls at the enemy's position
    void SpawnSouls()
    {
        if (soulPrefab == null) return;

        for (int i = 0; i < souls; i++)
        {
            Instantiate(soulPrefab, transform.position, Quaternion.identity);
        }
    }
}
