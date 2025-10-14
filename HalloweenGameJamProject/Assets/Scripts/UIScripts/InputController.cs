using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class LeaderboardInputController : MonoBehaviour
{
    private LeaderboardControler leaderboardController;

    public UIDocument mainMenuUIDocument;
    public UIDocument leaderboardUIDocument;

    public GameObject mainMenu;

    private VisualElement root;

    private TextField textInput;
    private Label feedbackLabel;
    private Button submitButton;
    private Button returnButton;

    private int testScore = 64;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        // Get doc and ui root
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Find UI elements
        textInput = root.Q<TextField>("textInput");
        textInput.maxLength = 16;
        feedbackLabel = root.Q<Label>("errorMessage");
        submitButton = root.Q<Button>("enterUsername");
        returnButton = root.Q<Button>("returnButton");

        // input validation checks
        this.textInput.RegisterValueChangedCallback(evt =>
        {
            string input = evt.newValue.Trim();
            this.textInput.value = input; // trim spaces live

            ValidateUsername(input);
        });

        // On buttons clicks
        submitButton.clicked += () => HandleSubmit(textInput.value);
        returnButton.clicked += () => ReturnButtonPress();
    }

    void HandleSubmit(string username)
    {
        // Check if username is valid using the same validation logic
        if (GetValidationError(username) == null)
        {
            // Save username to PlayerPrefs
            PlayerPrefs.SetString("Username", username);


            // --------- REMOVE LINE OF CODE ---------------
            PlayerPrefs.SetInt("Score", testScore);
            PlayerPrefs.Save(); // ensure it’s written to disk

            submitButton.text = "Saved!";
            Debug.Log($"Username saved: {username}");

            //Debug.Log("Username from PlayerPrefs: " + PlayerPrefs.GetString("Username", "No name saved"));

            leaderboardController = FindFirstObjectByType<LeaderboardControler>();

            leaderboardController.SetEntry(username, PlayerPrefs.GetInt("Score"));


            // Open main menu leaderboard
            //mainMenu.SetActive(true);
            //this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Invalid username. Not saved.");
        }
    }
    private void ValidateUsername(string name)
    {
        string error = GetValidationError(name);

        if (error == null)
        {
            // ✅ Valid username
            textInput.RemoveFromClassList("invalid");
            textInput.AddToClassList("valid");
            feedbackLabel.text = "✓ Looks good!";
            feedbackLabel.style.color = Color.green;
        }
        else
        {
            // ❌ Invalid username
            textInput.RemoveFromClassList("valid");
            textInput.AddToClassList("invalid");
            feedbackLabel.text = error;
            feedbackLabel.style.color = Color.red;
        }
    }
    private string GetValidationError(string name)
    {
        // Empty or too short
        if (string.IsNullOrWhiteSpace(name))
            return "Username cannot be empty.";

        if (name.Length < 3)
            return "Must be at least 3 characters.";

        if (name.Length > 16)
            return "Maximum 16 characters.";

        // Allowed characters
        if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_-]+$"))
            return "Only letters, numbers, _ and - allowed.";

        // Disallowed words
        string[] banned = { "admin", "mod", "test" };
        if (banned.Any(w => name.ToLower().Contains(w)))
            return "That name is not allowed.";

        return null; // no error
    }
    void ReturnButtonPress()
    {
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
