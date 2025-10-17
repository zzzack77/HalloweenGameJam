using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerStats playerStats;

    [Header("PlayerStats")]
    public int score = 0;
    public SpriteRenderer playerSpriteRenderer;

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

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }
    

    //                   //
    //  SCORE MANAGEMENT //
    //                   //
    public void IncreaseScore(int scoreVal)
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Debug.Log("Increase score");
        playerStats.Score += scoreVal;
        //playerStats.Score
    }

    public void DecreaseScore(int scoreVal)
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerStats.Score -= scoreVal;
        //if (score < 0) score = 0; //do we want this? -maxb
    }

    public void ResetScore()
    {
        playerStats.Score = 50;
    }

    //                        //
    //  Hit Flash Management  //
    //                        //
    public void HitFlash(SpriteRenderer spriteRenderer)
    {
        StartCoroutine(HitFlashCoroutine(spriteRenderer));
    }
    public void HitFlashPlayer()
    {
        StartCoroutine(HitFlashCoroutine(playerSpriteRenderer));
    }

    private System.Collections.IEnumerator HitFlashCoroutine(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
