using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerStats playerStats;

    [Header("PlayerStats")]
    public int score = 0;


    void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    //                   //
    //  SCORE MANAGEMENT //
    //                   //
    public void IncreaseScore(int scoreVal)
    {
        playerStats.Score += scoreVal;
    }

    public void DecreaseScore(int scoreVal)
    {
        playerStats.Score -= scoreVal;
        //if (score < 0) score = 0; //do we want this? -maxb
    }

    public void ResetScore()
    {
        playerStats.Score = 0;
    }
}
