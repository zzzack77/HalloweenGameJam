using UnityEngine;

public class MBBullet : MonoBehaviour
{
    /* 
      
    Script for bullet, most of the logic is handled by the player controller
    so this just handles lifetime and collision with enemies.
     
     */

    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MBEnemy enemy = other.GetComponent<MBEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(MBGameManager.Instance.playerDamage);
            Destroy(gameObject);
        }
    }
}
