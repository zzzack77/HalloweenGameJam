using UnityEngine;

public class MBGameManager : MonoBehaviour
{
    // Singleton instance for global variables, I put anything that isn't different between instances of objects in here.

    public static MBGameManager Instance { get; private set; }

    [Header ("Player Stats")]
    public int playerScore = 0;
    public int playerHealth = 100;
    public float playerSpeed = 5f;
    public float playerDamage = 25f;
    public int soulCount = 0;

    [Header("Soul Settings")]
    public float soulMinSpeed = 5f;
    public float soulMaxSpeed = 10f;
    public float soulHomingDelay = 0.1f;
    public float soulMinHomingForce = 5f;
    public float soulMaxHomingForce = 15f;

    [Header("Enemy Settings")]
    public float enemyMinHealth = 50f;
    public float enemyMaxHealth = 100f;
    public int enemyMinNumSouls = 1;
    public int enemyMaxNumSouls = 5;

    // Singleton pattern implementation
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}