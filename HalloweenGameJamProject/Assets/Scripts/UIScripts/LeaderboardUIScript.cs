using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Dan.Models;
using Dan.Main;
using System;
using System.Security.Cryptography.X509Certificates;


public class LeaderboardUIScript : MonoBehaviour
{
    public GameObject mainMenuObject;
    public AudioSource clickAudio;
    public AudioSource hoverAudio;


    private VisualElement root;
    private VisualElement settingsPanel;

    private LeaderboardControler leaderboardController;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        var backButton = root.Q<Button>("back-button");
        var refreshButton = root.Q<Button>("refresh-button");

        backButton.clicked += BackButtonPress;
        refreshButton.clicked += RefreshButtonPress;

        backButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });
        refreshButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });


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
        leaderboardController = FindFirstObjectByType<LeaderboardControler>();

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

    void BackButtonPress()
    {
        clickAudio.Play();
        mainMenuObject.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    void RefreshButtonPress()
    {
        clickAudio.Play();
        LoadLeaderboardUI();
    }
    void PlayHoverSound()
    {
        hoverAudio.Play();
    }
}
