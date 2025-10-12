using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer playerSprite;
    private SpriteRenderer GunSprite;

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip weapon vertically when the player is flipped
        if (playerSprite.flipX)
        {
            transform.localScale = new Vector3(1, -1, 1); // flips vertically
            GunSprite.flipY = true; 
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
