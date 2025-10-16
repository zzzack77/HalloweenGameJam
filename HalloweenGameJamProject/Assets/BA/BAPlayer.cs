using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BAPlayer : MonoBehaviour
{
    //public float lightHP = 100f; //light from lantern - acts as functional hp (percentage of max light) 
    //public float speed = 5f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerStats playerStats;
    
    //public float speedPowerupDuration = 10f;
    public Animator animator;

    [SerializeField] GameObject mine;
    [SerializeField] GameObject fireWheel;

    public AugmentStructure effects;

    public GameObject DeathScreen;


    private IPayLighting interactable;


   private float PlayerStatsLightHP;
    private void OnEnable()
    {
        Time.timeScale = 1f;

        playerStats = GetComponent<PlayerStats>();

        Powerup.OnSpeedBoost += SpeedPowerup;
        AbilityManager.OnMineAbility += ThrowMine;
        AbilityManager.OnFireWheelAbility += ActivateFireWheel;

        playerStats.LightHP = playerStats.LightHPStart;
        Debug.Log(playerStats.LightHP);
        Debug.Log(playerStats.LightHPStart);

    }

    private void OnDisable()
    {
        Powerup.OnSpeedBoost -= SpeedPowerup;
        AbilityManager.OnMineAbility -= ThrowMine;
        AbilityManager.OnFireWheelAbility -= ActivateFireWheel;
    }

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        //effects = new AugmentStructure();
        //effects.setAug(0, true);
        //effects.setAug(1, true);
        //effects.setAug(2, true);


        playerStats.BulletDamage = 2; 
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
            //playerStats.LightHP = PlayerStatsLightHP;
            interactable.CanActivate(playerStats);
                    Debug.Log("player activated");
                }
    }

    void FixedUpdate()
    {
        // Move the player using physics
        rb.MovePosition(rb.position + moveInput * playerStats.MovementSpeed * Time.fixedDeltaTime);
    }



    public void SpeedPowerup()
    {
        playerStats.MovementSpeed = 10f;
        // Maybe apply some effect/sound effects
        // Potential UI update to show powerup
        StartCoroutine(SpeedPowerupTimer());
    }

    IEnumerator SpeedPowerupTimer()
    {
        yield return new WaitForSeconds(playerStats.SpeedPowerupDuration);
        playerStats.MovementSpeed = 5f;
    }

    //Ollie stuff
    public void lightReducer(float value)
    {
        playerStats.LightHP -= value;
        if (playerStats.LightHP < 0)
        {
            //this is for death stuff 
            Death();
        }
    }

    //void lightBurn()
    //{
    //    lightReducer(playerStats.LightHPLossRate * Time.deltaTime);
    //}

    public void Death()
    {
        //Adds up score and saves it
        PlayerPrefs.SetInt("Score",playerStats.Score);

        // optional: pause game time
        Time.timeScale = 0f;

        
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;

        

        DeathScreen.SetActive(true);

    }

    public void lightIncreaser(float value)
    {
        playerStats.LightHP += value;
        if (playerStats.LightHP > 100)
        {
            playerStats.LightHP = 100;  //max light (100%) 
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

    public void ThrowMine()
    {
        if (playerStats.CanThrowMine)
        {
            Instantiate(mine, transform.position, Quaternion.identity);
            StartCoroutine(CooldownHelper.CooldownRoutine(val => playerStats.CanThrowMine = val, playerStats.MineThrowCooldown));
        }

    }

    public void ActivateFireWheel()
    {
        if (playerStats.CanUseFireWheel)
        {
            StartCoroutine(FireWheelRoutine());
        }
    }

    private IEnumerator FireWheelRoutine()
    {
        playerStats.CanUseFireWheel = false;
        fireWheel.SetActive(true);
        yield return new WaitForSeconds(playerStats.FireWheelDuration);
        fireWheel.SetActive(false);
        yield return new WaitForSeconds(playerStats.FireWheelCooldown);
        playerStats.CanUseFireWheel = true;
    }
}
