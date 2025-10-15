using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using Dan.Models;
using Dan.Main;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "SampleScene";
    public GameObject leaderboardObject;
    public GameObject howToPlayObject;
    private VisualElement root;
    public AudioSource clickAudio;
    public AudioSource hoverAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get buttons
        var playButton = root.Q<Button>("play-button");
        var leaderboardButton = root.Q<Button>("leaderboard-button");
        var howToPlayButton = root.Q<Button>("howToPlay-button");
        var quitButton = root.Q<Button>("quit-button");

        // Assign events
        playButton.clicked += StartGame;
        leaderboardButton.clicked += OpenLeaderboard;
        howToPlayButton.clicked += OpenHowToPlay;
        quitButton.clicked += QuitGame;


        playButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });
        leaderboardButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });
        howToPlayButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });
        quitButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });

    }
    void StartGame()
    {
        clickAudio.Play();

        SceneManager.LoadScene(gameSceneName);
    }
    void OpenLeaderboard()
    {
        clickAudio.Play();
        leaderboardObject.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        //LoadLeaderboardUI();
    }

    void OpenHowToPlay()
    {
        clickAudio.Play();
        howToPlayObject.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void QuitGame()
    {
        clickAudio.Play();
        Debug.Log("pressed button");
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    void PlayHoverSound()
    {
        hoverAudio.Play();
    }
}
