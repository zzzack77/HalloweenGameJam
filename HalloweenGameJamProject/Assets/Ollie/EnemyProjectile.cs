using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float timeAlive = 4;
    public float shootForce = 0.05f;
    private float lastHit = 0f;
    private GameObject target;
    private float maxSpeed = 10f;
    private float acceleration = 20f;
    private Vector2 dir;
    public float rotationSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        dir = ((Vector2)target.transform.position - rb.position).normalized;

    }

    void Update()
    {
        // Rotate around Z axis (2D rotation)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        GO();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            lastHit = Time.time;

            BAPlayer player = collision.GetComponent<Collider2D>().GetComponent<BAPlayer>();
            player.lightReducer(10f);
            GameObject.Destroy(this.gameObject);

        }
    }

    public void GO()
    {
        timeAlive -= Time.deltaTime;
        if (timeAlive < 0)
        {
            Destroy(gameObject);
        }
        rb.AddForce(dir * acceleration, ForceMode2D.Force);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
}
