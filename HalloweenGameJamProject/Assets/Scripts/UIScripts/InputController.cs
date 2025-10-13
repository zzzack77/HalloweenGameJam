using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
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

    private int testScore = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        leaderboardController = FindFirstObjectByType<LeaderboardControler>();
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;


        textInput = root.Q<TextField>("textInput");
        feedbackLabel = root.Q<Label>("errorMessage");
        
        textInput.maxLength = 16;
        submitButton = root.Q<Button>("enterUsername");

        this.textInput.RegisterValueChangedCallback(evt =>
        {
            string input = evt.newValue.Trim();
            this.textInput.value = input; // trim spaces live

            ValidateUsername(input);
        });


        submitButton.clicked += () => HandleSubmit(textInput.value);
    }

    void HandleSubmit(string username)
    {
        // Check if username is valid using the same validation logic
        if (GetValidationError(username) == null)
        {
            // Save username to PlayerPrefs
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("Score", testScore);
            PlayerPrefs.Save(); // ensure it’s written to disk

            submitButton.text = "Saved!";
            Debug.Log($"Username saved: {username}");
            leaderboardController = FindFirstObjectByType<LeaderboardControler>();

            leaderboardController.SetEntry(username, 50);

            // Open main menu leaderboard
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
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
}
