using UnityEngine;

public class PlayerAimFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public bool isFlipped = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < transform.position.x)
        {
            // Mouse is to the left
            spriteRenderer.flipX = true;

            isFlipped = true;
        }
        else
        {
            // Mouse is to the right
            spriteRenderer.flipX = false;

            isFlipped = false;
        }
    }
}
