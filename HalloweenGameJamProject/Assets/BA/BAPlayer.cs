using System.Collections;
using UnityEngine;


public class BAPlayer : MonoBehaviour
{
    public float lightHP = 100f; //light from lantern - acts as functional hp (percentage of max light) 
    public float speed = 5f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    public float speedPowerupDuration = 10f;
    public Animator animator;

    public AugmentStructure effects; 
    
    
    private IPayLighting interactable;

    private void OnEnable()
    {
        Powerup.OnSpeedBoost += SpeedPowerup;
    }

    private void OnDisable()
    {
        Powerup.OnSpeedBoost -= SpeedPowerup;
    }

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        effects = new AugmentStructure();
        effects.setAug(2, true);
    }

    void Update()
    {
        
        // Get input from WASD or Arrow keys
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveY = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Store movement direction
        moveInput = new Vector2(moveX, moveY).normalized;
        CheckMovement();
        
        
           if (interactable != null && Input.GetKeyDown(KeyCode.Space))
                {
                    interactable.CanActivate(ref lightHP);
                    Debug.Log("player activated");
                }
    }

    void FixedUpdate()
    {
        // Move the player using physics
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    public void SpeedPowerup()
    {
        speed = 10f;
        // Maybe apply some effect/sound effects
        // Potential UI update to show powerup
        StartCoroutine(SpeedPowerupTimer());
    }

    IEnumerator SpeedPowerupTimer()
    {
        yield return new WaitForSeconds(speedPowerupDuration);
        speed = 5f;
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
        if (lightHP > 100)
        {
            lightHP = 100;  //max light (100%) 
        }
    }

    //^

    public void CheckMovement()
    {
        if (moveInput != Vector2.zero)
        {
            // Player is moving
            animator.SetBool("IsMoving", true);
        }
        else
        {
            // Player is idle
            animator.SetBool("IsMoving", false);
        }   
    }
    
    
    public void SetInteractable(IPayLighting interactable)
        {
            this.interactable = interactable;
            Debug.Log("interactable set");
        }

}
