using System.Collections.Generic;
using UnityEngine;

public class FireWheel : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public float tickInterval = 1f; // how often to apply damage
    private float timer;
    public float rotationSpeed;

    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Update()
    {
        // Rotate around Z axis (2D rotation)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= playerStats.fireWheelTickInterval)
        {
            timer = 0f;

            for (int i = enemiesInRange.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemiesInRange[i];
                if (enemy == null)
                {
                    enemiesInRange.RemoveAt(i);
                    continue;
                }

                enemy.TakeDamage(playerStats.fireWheelDamage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
