using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Dan.Models;
using Dan.Main;
using System;
using System.Security.Cryptography.X509Certificates;



public class MainMenuScript : MonoBehaviour
{
    // Name of the scene to load when "Start" is clicked
    [SerializeField] private string gameSceneName = "SampleScene";

    private VisualElement root;
    private VisualElement settingsPanel;


    private LeaderboardControler leaderboardController;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get buttons
        var playButton = root.Q<Button>("play-button");
        var leaderboard = root.Q<Button>("leaderboard-button");
        var howToPlayButton = root.Q<Button>("howToPlay-button");
        var quitButton = root.Q<Button>("quit-button");

        // Get labels

        // Assign events
        playButton.clicked += StartGame;
        leaderboard.clicked += OpenLeaderboard;
        //howToPlayButton.clicked += OpenHowToPlay;
        quitButton.clicked += QuitGame;


        leaderboardController = FindFirstObjectByType<LeaderboardControler>();

        LoadLeaderboardUI();
    }
    
    

    public void LoadLeaderboardUI()
    {
        // Collect all label names in an array
        string[] labelNames = { "name1", "name2", "name3", "name4", "name5", "name6", "name6", "name7", "name8", "name9", "name10", };
        string[] labelScores = { "score1", "score2", "score3", "score4", "score5", "score6", "score7", "score8", "score9", "score10", };

        // Get the leaderboard entries
        Entry[] data = leaderboardController.GetLeaderboardEntries();

        // Loop through each label and assign text safely
        for (int i = 0; i < labelNames.Length; i++)
        {
            Label label = root.Q<Label>(labelNames[i]);

            if (label == null)
            {
                Debug.LogWarning($"Label '{labelNames[i]}' not found in UI.");
                continue;
            }

            // Check if there's corresponding data for this index
            if (data != null && i < data.Length)
            {
                label.text = data[i].Username;
            }
            else
            {
                // set placeholder if no entry exists
                label.text = "-";
            }
        }
        for (int i = 0; i < labelScores.Length; i++)
        {
            Label label = root.Q<Label>(labelScores[i]);

            if (label == null)
            {
                Debug.LogWarning($"Label '{labelScores[i]}' not found in UI.");
                continue;
            }

            // Check if there's corresponding data for this index
            if (data != null && i < data.Length)
            {
                label.text = data[i].Score.ToString();
            }
            else
            {
                // set placeholder if no entry exists
                label.text = "-";
            }
        }
        
        // Show personal entry on leaderboard
        Leaderboards.HalloweenGameJamLeaderboard.GetPersonalEntry(
            callback: OnEntryReceived,
            errorCallback: OnError
        );
    }
    void OnEntryReceived(Entry entry)
    {
        var playerRank = root.Q<Label>("playerRank");
        var playerName = root.Q<Label>("playerName");
        var playerScore = root.Q<Label>("playerScore");
        playerRank.text = entry.RankSuffix();
        playerName.text = entry.Username.ToString();
        playerScore.text = entry.Score.ToString();
    }
    void OnError(string error)
    {
        Debug.LogError("Failed to get leaderboard entry: " + error);
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    void OpenLeaderboard()
    {
        Debug.Log("pressed button");
        LoadLeaderboardUI();
    }

    void CloseHowToPlay()
    {
        if (settingsPanel != null)
        {
            root.Remove(settingsPanel);
            settingsPanel = null;
        }
    }

    void QuitGame()
    {
        int score = 0;
        //leaderboard.SetEntry(PlayerPrefs.GetString("username"), score);

        //leaderboard.SetEntry("Zack", 4);

        //Debug.Log("Quit Game");
        //Application.Quit();
    }
}
