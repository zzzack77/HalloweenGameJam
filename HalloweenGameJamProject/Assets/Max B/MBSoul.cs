using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MBSoul : MonoBehaviour
{
    // Enum to represent the current state of the soul
    private enum SoulState
    {
        Drifting,           // Initial random movement after spawn
        ReturningToOrbit,   // Moving back to its orbit center if not homing
        Orbiting,           // Circling around its orbit center
        Homing              // Attracted to the player
    }

    private Rigidbody2D rb;
    private Transform player;
    PlayerStats playerStats;
    BAPlayer baplayer;
    private SoulState state = SoulState.Drifting; // Current state of the soul

    // --- Inspector Parameters ---

    [Header("Soul Parameter Ranges")]
    [Tooltip("The range of initial drifting speed for the soul when it spawns. Higher values make the soul move faster at the start.")]
    public Vector2 speedRange = new Vector2(2f, 4f);

    [Tooltip("If true, the soul will randomly choose to curve left or right when orbiting or homing.")]
    public bool randomizeCurveSign = true;

    [Tooltip("The range of how much the soul curves when homing toward the player. Higher values make the path more arched or spiraled.")]
    public Vector2 curveAmountRange = new Vector2(0.3f, 0.7f);

    [Tooltip("The range of how fast the soul orbits around its center point. Higher values make the soul circle its orbit center more quickly.")]
    public Vector2 orbitSpeedRange = new Vector2(1f, 2f);

    [Tooltip("The range of distances from the orbit center at which the soul orbits. Larger values make the soul orbit in a wider circle.")]
    public Vector2 orbitRadiusRange = new Vector2(0.7f, 1.5f);

    [Header("Orbit Spring Ranges")]
    [Tooltip("The range of how strongly the soul is pulled back to its orbit radius while orbiting. Higher values make the soul snap to its orbit path more tightly.")]
    public Vector2 springStrengthRange = new Vector2(5f, 7f);

    [Tooltip("The range of how quickly the soul's radial velocity is dampened while orbiting. Higher values make the soul settle into its orbit more quickly, with less bouncing.")]
    public Vector2 dampingRange = new Vector2(0.18f, 0.22f);

    [Header("Return Spring Ranges")]
    [Tooltip("The range of how strongly the soul is pulled back to its orbit center when returning to orbit. Higher values make the soul return more quickly and with a snappier motion.")]
    public Vector2 returnSpringStrengthRange = new Vector2(6f, 8f);

    [Tooltip("The range of how quickly the soul's radial velocity is dampened while returning to orbit. Higher values reduce overshoot and make the return smoother.")]
    public Vector2 returnDampingRange = new Vector2(0.22f, 0.28f);

    [Header("Wispy Effect Range")]
    [Tooltip("The range of random jitter or wispy movement added to the soul's path. Higher values make the soul's movement more erratic and magical.")]
    public Vector2 noiseMagnitudeRange = new Vector2(0.01f, 0.04f);

    [Header("Soul Behavior")]
    [Tooltip("The range of delay before the soul starts checking for the player to home in on. Higher values make the soul wait longer before it can start homing.")]
    public Vector2 homingDelayRange = new Vector2(0.8f, 1.2f);

    [Tooltip("The range of distances at which the soul will start homing toward the player. Larger values make the soul start homing from farther away.")]
    public Vector2 attractionRadiusRange = new Vector2(1.8f, 2.2f);

    [Tooltip("Strength of the gravity-like pull toward the player during homing.")]
    public float gravityStrength = 10f;

    [Header("Randomized Parameters (Read-Only)")]
    // These are the actual values chosen for this soul instance at runtime
    public float speed;
    public float curveSign;
    public float curveAmount;
    public float orbitSpeed;
    public float orbitRadius;
    public float springStrength;
    public float damping;
    public float returnSpringStrength;
    public float returnDamping;
    public float noiseMagnitude;
    public float homingDelay;
    public float attractionRadius;

    private Vector2 orbitCenter; // The center point around which the soul orbits

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>(); 
        baplayer = GameObject.Find("Player").GetComponent<BAPlayer>();

        // Randomize all parameters for unique behavior per soul
        speed = Random.Range(speedRange.x, speedRange.y);
        curveSign = randomizeCurveSign ? (Random.value < 0.5f ? -1f : 1f) : curveSign;
        curveAmount = Random.Range(curveAmountRange.x, curveAmountRange.y);
        orbitSpeed = Random.Range(orbitSpeedRange.x, orbitSpeedRange.y);
        orbitRadius = Random.Range(orbitRadiusRange.x, orbitRadiusRange.y);
        springStrength = Random.Range(springStrengthRange.x, springStrengthRange.y);
        damping = Random.Range(dampingRange.x, dampingRange.y);
        returnSpringStrength = Random.Range(returnSpringStrengthRange.x, returnSpringStrengthRange.y);
        returnDamping = Random.Range(returnDampingRange.x, returnDampingRange.y);
        noiseMagnitude = Random.Range(noiseMagnitudeRange.x, noiseMagnitudeRange.y);
        homingDelay = Random.Range(homingDelayRange.x, homingDelayRange.y);
        attractionRadius = Random.Range(attractionRadiusRange.x, attractionRadiusRange.y);

        // Set initial random drift direction and speed
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        rb.linearVelocity = direction * speed;

        orbitCenter = transform.position;

        // After a delay, check if the player is in range to start homing
        Invoke(nameof(TryActivateHoming), homingDelay);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // State machine: handle behavior based on current state
        switch (state)
        {
            //case SoulState.Drifting:
            //    // Do nothing, just drifting until homing is checked
            //    break;
            case SoulState.ReturningToOrbit:
                ReturnToOrbitCenter();
                break;
            case SoulState.Orbiting:
                OrbitPhysics();
                CheckForPlayerInRange();
                break;
            case SoulState.Homing:
                HomeToPlayer();
                break;
        }

        // Always face the direction of movement, regardless of state
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }

    // Called after the homing delay to determine next state
    void TryActivateHoming()
    {
        if (Vector2.Distance(transform.position, player.position) <= attractionRadius)
        {
            SetState(SoulState.Homing);
        }
        else
        {
            SetState(SoulState.ReturningToOrbit);
        }
    }

    // While orbiting, check if the player has come into range to start homing
    void CheckForPlayerInRange()
    {
        if (Vector2.Distance(transform.position, player.position) <= attractionRadius)
        {
            SetState(SoulState.Homing);
        }
    }

    // Helper to change state, prevents redundant transitions
    void SetState(SoulState newState)
    {
        if (state == newState) return;
        state = newState;
    }

    // Handles the soul returning to its orbit center with a spring-damper system
    void ReturnToOrbitCenter()
    {
        Vector2 toCenter = (orbitCenter - (Vector2)transform.position);
        float distance = toCenter.magnitude;

        // Wispy noise for magical effect
        Vector2 noise = Random.insideUnitCircle * noiseMagnitude;
        Vector2 desiredPos = orbitCenter + (toCenter.normalized * orbitRadius) + noise;
        Vector2 springForce = (desiredPos - (Vector2)transform.position) * returnSpringStrength;

        // Damping only in the radial direction (prevents "thumping" against the orbit line)
        Vector2 radialDir = toCenter.normalized;
        float radialVel = Vector2.Dot(rb.linearVelocity, radialDir);
        Vector2 radialDamping = -radialDir * radialVel * returnDamping;

        rb.AddForce(springForce + radialDamping, ForceMode2D.Force);

        // Switch to orbiting when close enough to the orbit radius
        if (Mathf.Abs(distance - orbitRadius) < 0.04f)
        {
            SetState(SoulState.Orbiting);
        }
    }

    // Handles the soul's orbiting behavior using a spring-damper system
    void OrbitPhysics()
    {
        Vector2 toSoul = (Vector2)transform.position - orbitCenter;
        float currentRadius = toSoul.magnitude;

        // Calculate tangent direction for smooth orbiting
        Vector2 tangent = new Vector2(-toSoul.y, toSoul.x).normalized * curveSign;
        Vector2 noise = Random.insideUnitCircle * noiseMagnitude;
        float desiredSpeed = orbitSpeed * orbitRadius;
        Vector2 desiredVelocity = tangent * desiredSpeed + noise;

        // Spring force pulls the soul toward the orbit line
        Vector2 springTarget = orbitCenter + toSoul.normalized * orbitRadius;
        Vector2 springForce = (springTarget - (Vector2)transform.position) * springStrength;

        // Damping only in the radial direction (reduces oscillation across the orbit line)
        Vector2 radialDir = toSoul.normalized;
        float radialVel = Vector2.Dot(rb.linearVelocity, radialDir);
        Vector2 radialDamping = -radialDir * radialVel * damping;

        rb.AddForce(springForce + radialDamping, ForceMode2D.Force);

        // Blend current velocity toward tangent for smooth, wispy orbiting
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVelocity, 0.08f);
    }

    // Handles the soul's homing behavior toward the player, with a gravity-like pull
    void HomeToPlayer()
    {
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position);
        float distance = toPlayer.magnitude;
        Vector2 toPlayerNorm = toPlayer.normalized;

        // Gravity-like force: increases as the soul gets closer to the player
        float gravityForce = gravityStrength / Mathf.Max(distance, 0.1f);

        // Curved homing direction for magical effect
        Vector2 perpendicular = new Vector2(-toPlayerNorm.y, toPlayerNorm.x) * curveSign;
        Vector2 noise = Random.insideUnitCircle * noiseMagnitude;
        Vector2 targetVelocity = (toPlayerNorm + perpendicular * curveAmount).normalized * speed + noise;

        // Add gravity-like force toward player
        Vector2 gravityVelocity = toPlayerNorm * gravityForce;

        // Combine velocities for smooth, wispy homing
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity + gravityVelocity, 0.1f);
    }

    // Handles collection of the soul by the player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            baplayer.lightIncreaser(playerStats.SoulLightGain);

            Destroy(gameObject);
            //MBGameManager.Instance.soulCount += 1;
        }
    }
}
