using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private float LastXPos;

    public bool Inverse = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        LastXPos = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        float currentXPos = transform.position.x;
        float direction = currentXPos - LastXPos;
        if (Inverse = true)
        {
            if (direction > 0.01f) //Moving right
            {
                spriteRenderer.flipX = true;
            }
            else if (direction < -0.01f) // Moving left
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (direction > 0.01f) //Moving right
            {
                spriteRenderer.flipX = false;
            }
            else if (direction < -0.01f) // Moving left
            {
                spriteRenderer.flipX = true;
            }
        }
       

        LastXPos = currentXPos;
    }
}
