using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float lightHP = 100f; //light from lantern - acts as functional hp (percentage of max light) 
    public float speed = 5f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from WASD or Arrow keys
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveY = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1f, 1f);
        }

        // Store movement direction
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Move the player using physics
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }



    //Ollie stuff
    public void lightReducer(float value)
    {
        lightHP -= value;
        if (lightHP < 0)
        {
            //this is for death stuff 
        }
    }

    public void lightIncreaser(float value)
    {
        lightHP += value;
        if(lightHP > 100)
        {
            lightHP = 100;  //max light (100%) 
        }
    }

    //^


}
