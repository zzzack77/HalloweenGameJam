using UnityEngine;
using UnityEngine.Rendering;

public class Explosion : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D cc2d;
    [SerializeField] private GameObject enemy;
    private float tempA;
    void Start()
    {
        tempA = 0f;
    }

    void Update()
    {

        tempA += 0.01f;
        if (tempA >= 0.3)
        {
            GameObject.Destroy(enemy);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            enemy.TakeDamage(5f);
        }
    }

}