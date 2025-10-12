using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Dan.Models;

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
        var howToPlayButton = root.Q<Button>("howToPlay-button");
        var quitButton = root.Q<Button>("quit-button");

        // Get labels

        // Assign events
        playButton.clicked += StartGame;
        howToPlayButton.clicked += OpenHowToPlay;
        quitButton.clicked += QuitGame;


        leaderboardController = FindFirstObjectByType<LeaderboardControler>();

        

        //username1.text = "hello";
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
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
        var username1 = root.Q<Label>("name-1");

        Entry[] data = leaderboardController.GetLeaderboardEntries();
        username1.text = data[0].Username;
        int score = 0;
        //leaderboard.SetEntry(PlayerPrefs.GetString("username"), score);

        //leaderboard.SetEntry("Zack", 4);

        //Debug.Log("Quit Game");
        //Application.Quit();
    }
}
