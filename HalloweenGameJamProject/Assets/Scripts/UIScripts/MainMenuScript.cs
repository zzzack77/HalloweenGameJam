using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Dan.Models;
using Dan.Main;
using System;

public class MainMenuScript : MonoBehaviour
{
    // Name of the scene to load when "Start" is clicked
    [SerializeField] private string gameSceneName = "SampleScene";
    [SerializeField] private VisualTreeAsset mainMenuAsset;
    [SerializeField] private VisualTreeAsset settingsAsset;

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
        howToPlayButton.clicked += OpenHowToPlay;
        quitButton.clicked += QuitGame;


        leaderboardController = FindFirstObjectByType<LeaderboardControler>();
    }
    
    

    public void LoadLeaderboardUI()
    {
        // Collect all label names in an array
        string[] labelNames = { "name1", "name2", "name3", "name4", "name5", "name6" };
        string[] labelScores = { "score1", "score2", "score3", "score4", "score5", "score6" };

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

    void OpenHowToPlay()
    {
        if (settingsPanel == null && settingsAsset != null)
        {
            settingsPanel = settingsAsset.CloneTree();
            root.Add(settingsPanel);

            var backButton = settingsPanel.Q<Button>("back-button");
            if (backButton != null)
                backButton.clicked += CloseHowToPlay;
        }
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
        Leaderboards.HalloweenGameJamLeaderboard.UploadNewEntry("Another Guy", 42);
        int score = 0;
        //leaderboard.SetEntry(PlayerPrefs.GetString("username"), score);

        //leaderboard.SetEntry("Zack", 4);

        //Debug.Log("Quit Game");
        //Application.Quit();
    }
}
