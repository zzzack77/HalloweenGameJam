using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    // Name of the scene to load when "Start" is clicked
    [SerializeField] private string gameSceneName = "SampleScene";
    [SerializeField] private VisualTreeAsset mainMenuAsset;
    [SerializeField] private VisualTreeAsset settingsAsset;

    private VisualElement root;
    private VisualElement settingsPanel;


    private LeaderboardControler leaderboard;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get buttons
        var playButton = root.Q<Button>("play-button");
        var howToPlayButton = root.Q<Button>("howToPlay-button");
        var quitButton = root.Q<Button>("quit-button");

        // Assign events
        playButton.clicked += StartGame;
        howToPlayButton.clicked += OpenHowToPlay;
        quitButton.clicked += QuitGame;


        leaderboard = FindFirstObjectByType<LeaderboardControler>();
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
        int score = 0;
        //leaderboard.SetEntry(PlayerPrefs.GetString("username"), score);

        leaderboard.SetEntry("kingbob", 20);

        //Debug.Log("Quit Game");
        //Application.Quit();
    }
}
