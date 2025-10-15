using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public float lightHP = 100f; //light from lantern - acts as functional hp (percentage of max light) 
    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private GameObject fireWheel;

    //public float speed = 5f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public float speedPowerupDuration = 10f;

    public float meleeCooldown = 1f;
    private bool canMeleeAttack = true;

    public float meleeDamage = 3;

    [SerializeField] GameObject mine;
   
    public float mineThrowCooldown = 10f;

    

    // Subcribe and describe from events
    private void OnEnable()
    {
        Powerup.OnSpeedBoost += SpeedPowerup;
        AbilityManager.OnMineAbility += ThrowMine;
    }

    private void OnDisable()
    {
        Powerup.OnSpeedBoost -= SpeedPowerup;
        AbilityManager.OnMineAbility -= ThrowMine;
    }

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

        // Store movement direction
        moveInput = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Mouse0) && canMeleeAttack )
        {
            meleeAttack.SetActive(true);
            // Melee Cooldown
            StartCoroutine(CooldownHelper.CooldownRoutine(val => canMeleeAttack = val, meleeCooldown));

        }
        if (Input.GetKeyUp(KeyCode.Mouse0) )
        {
            meleeAttack.SetActive(false);
        }

     
    }

    void FixedUpdate()
    {
        // Move the player using physics
        rb.MovePosition(rb.position + moveInput * playerStats.movementSpeed * Time.fixedDeltaTime);
    }

    public void SpeedPowerup()
    {
        playerStats.movementSpeed = playerStats.boostedSpeed;
        // Maybe apply some effect/sound effects
        // Potential UI update to show powerup
        StartCoroutine(SpeedPowerupTimer());
    }

    IEnumerator SpeedPowerupTimer()
    {
        yield return new WaitForSeconds(speedPowerupDuration);
        playerStats.movementSpeed = playerStats.baseSpeed;
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
    


    public void ThrowMine()
    {
        if (playerStats.canThrowMine)
        {
            Instantiate(mine, transform.position, Quaternion.identity);
            StartCoroutine(CooldownHelper.CooldownRoutine(val => playerStats.canThrowMine = val, mineThrowCooldown));
        }
        
    }

    public void ActivateFireWheel()
    {
        if (playerStats.canUseFireWheel)
        {
            fireWheel.SetActive(true);
            StartCoroutine(CooldownHelper.CooldownRoutine(val => playerStats.canUseFireWheel = val, playerStats.fireWheelDuration));
        }
        if (!playerStats.canUseFireWheel)
        {
            fireWheel.SetActive(false);
        }
        
    }
    
    
    
}
