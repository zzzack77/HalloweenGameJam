using UnityEngine;

public class PlayerAimFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool IsFacingLeft;

    [SerializeField] GameObject playerArm;

    public Vector2 localScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsFacingLeft = true;
        
    }

    void Update()
    {

        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if (mousePosition.x < transform.position.x && !IsFacingLeft)
        //{
        //    Vector2 localScale = transform.localScale;
        //    localScale.x *= -1f;
        //    transform.localScale = localScale;
        //    IsFacingLeft = true;
        //}
        //else
        //{
        //    Vector2 localScale = transform.localScale;
        //    localScale.x *= 1f;
        //    transform.localScale = localScale;
        //    IsFacingLeft = false;
        //}

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x && IsFacingLeft)
        {
            Flip(false);
            FlipArm(playerArm, false);
        }
        else if (mousePosition.x > transform.position.x && !IsFacingLeft)
        {
            Flip(true);
            FlipArm(playerArm, true);
        }

        

    }

    void Flip(bool faceLeft)
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        IsFacingLeft = faceLeft;
    }

    void FlipArm(GameObject arm, bool faceLeft)
    {
        Vector3 localScale = arm.transform.localScale;
        localScale.y *= -1f;
        arm.transform.localScale = localScale;
        IsFacingLeft = faceLeft;
    }
}
