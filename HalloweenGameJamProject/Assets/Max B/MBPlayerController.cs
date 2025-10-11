using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MBPlayerController : MonoBehaviour
{
    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDir;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public Transform firePoint;

    void Start()
    {
        moveSpeed = MBGameManager.Instance.playerSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    // Handling movement and shooting input stuff
    void Update()
    {
        RotateTowardMouse();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 forward = lookDir.normalized;
        Vector2 right = new Vector2(forward.y, -forward.x);

        moveInput = (forward * moveY + right * moveX).normalized;

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    // Applies movement in FixedUpdate for consistent physics updates
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // Rotates the player to face the mouse cursor
    void RotateTowardMouse()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        lookDir = (mouseWorldPos - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.SetRotation(angle);
    }

    // Simple shooting method that instantiates a bullet and gives it velocity
    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
            bulletRb.linearVelocity = firePoint.right * bulletSpeed;
    }
}