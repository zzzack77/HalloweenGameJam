using UnityEngine;

public class TestFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // When "L" key is pressed, trigger the hit flash
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (spriteRenderer != null && GameManager.Instance != null)
            {
                GameManager.Instance.HitFlash(spriteRenderer);
            }
        }
    }
}
