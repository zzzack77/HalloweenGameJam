using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("PlayerStats")]
    public int score = 0;

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

    //                   //
    //  SCORE MANAGEMENT //
    //                   //
    public void IncreaseScore(int scoreVal)
    {
        score += scoreVal;
    }

    public void DecreaseScore(int scoreVal)
    {
        score -= scoreVal;
        //if (score < 0) score = 0; //do we want this? -maxb
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void GetScoreForLeaderboard()
    {
        PlayerPrefs.SetInt("Score", score); //Is this what you wanted Zack? -maxb
                                            //you can also just use "GameManager.Instance.score" as a variable to get the score from anywhere if thats easier?
    }
}
