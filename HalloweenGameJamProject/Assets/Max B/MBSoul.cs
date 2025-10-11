using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MBSoul : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private bool homingActive = false;
    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        // Pick a random speed and direction
        speed = Random.Range(MBGameManager.Instance.soulMinSpeed, MBGameManager.Instance.soulMaxSpeed);
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        rb.linearVelocity = direction * speed;

        // Start homing after a delay
        Invoke(nameof(ActivateHoming), MBGameManager.Instance.soulHomingDelay);
    }

    void FixedUpdate()
    {
        if (!homingActive || player == null) return;

        // Calculate a curved homing direction
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x) * (Random.value < 0.5f ? -1f : 1f);
        float curve = 0.5f;
        Vector2 targetVelocity = (toPlayer + perpendicular * curve).normalized * MBGameManager.Instance.soulMaxSpeed;

        // Smoothly blend current velocity toward target velocity
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.1f);
    }


    void ActivateHoming()
    {
        homingActive = true;
    }

    // Collect soul on player collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            MBGameManager.Instance.soulCount += 1;
        }
    }
}
