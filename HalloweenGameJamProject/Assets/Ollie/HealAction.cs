using UnityEngine;

public class HealAction : MonoBehaviour
{
    [SerializeField] private CircleCollider2D CircleCollider2D;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            enemy.GainHealth(5f);
        }
    }


}
