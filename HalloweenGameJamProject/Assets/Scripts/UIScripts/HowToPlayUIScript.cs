using UnityEngine;
using UnityEngine.UIElements;

public class HowToPlayScript : MonoBehaviour
{
    public GameObject mainMenuObject;
    public AudioSource clickAudio;
    public AudioSource hoverAudio;

    private VisualElement root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        var backButton = root.Q<Button>("back-button");
        backButton.clicked += BackButtonPress;
        backButton.RegisterCallback<MouseEnterEvent>(evt => { PlayHoverSound(); });
    }

    void BackButtonPress()
    {
        clickAudio.Play();
        mainMenuObject.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    void PlayHoverSound()
    {
        hoverAudio.Play();
    }
}
