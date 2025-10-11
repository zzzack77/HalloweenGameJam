using UnityEngine;
using System.Collections;

public class LightProximity : MonoBehaviour
{
    [Header("Settings")]
    public float fadeSpeed = 1f;      // fade-out speed
    public float fadeInSpeed = 3f;    // fade-in speed (higher = faster)
    public float dangerDelay = 17f;   // seconds before death

    [Header("Audio Sources")]
    public AudioSource dangerAudio;   // danger sound
    public AudioSource thudding;      // heartbeat sound

    [SerializeField]
    private bool isSafe = true;
    [SerializeField]
    private bool inDanger = false;
    private float dangerTimer;

    void Update()
    {
        if (LightManager.Instance == null) return;

        bool nearLight = LightManager.Instance.IsNearAnyLight(transform.position);

        if (nearLight)
        {
            if (!isSafe)
            {
                isSafe = true;
                OnEnterLight();
            }
        }
        else
        {
            if (isSafe)
            {
                isSafe = false;
                OnLeaveLight();
            }
        }

        // Handle danger timer
        if (!isSafe)
        {
            dangerTimer += Time.deltaTime;
            if (dangerTimer >= dangerDelay)
            {
                OnDeath();
            }
        }
    }

    private void OnLeaveLight()
    {
        inDanger = true;
        dangerTimer = 0f;

        // Fade in danger and heartbeat
        if (dangerAudio)
        {
            dangerAudio.volume = 0f;
            if (!dangerAudio.isPlaying) dangerAudio.Play();
            StartCoroutine(FadeInAudio(dangerAudio, fadeInSpeed));
        }

        if (thudding)
        {
            thudding.volume = 0f;
            thudding.loop = true;
            if (!thudding.isPlaying) thudding.Play();
            StartCoroutine(FadeInAudio(thudding, fadeInSpeed));
        }
    }

    private void OnEnterLight()
    {
        inDanger = false;

        // Fade out both audios smoothly
        if (dangerAudio)
            StartCoroutine(FadeOutAudio(dangerAudio, fadeSpeed));

        if (thudding)
            StartCoroutine(FadeOutAudio(thudding, fadeSpeed));
    }

    private void OnDeath()
    {
        if (!inDanger) return;

        Debug.Log("Player died after staying out of light too long.");

        StopAudioInstant(dangerAudio);
        StopAudioInstant(thudding);

        inDanger = false;

        // Example:
        // FindObjectOfType<PlayerDeath>()?.Death();
    }

    private IEnumerator FadeOutAudio(AudioSource audio, float speed)
    {
        if (!audio || !audio.isPlaying) yield break;

        float startVolume = audio.volume;

        while (audio.volume > 0f)
        {
            audio.volume -= Time.deltaTime * speed;
            yield return null;
        }

        audio.Stop();
        audio.volume = startVolume;
    }

    private IEnumerator FadeInAudio(AudioSource audio, float speed)
    {
        if (!audio) yield break;

        float targetVolume = 1f;

        while (audio.volume < targetVolume)
        {
            audio.volume += Time.deltaTime * speed;
            yield return null;
        }

        audio.volume = targetVolume;
    }

    private void StopAudioInstant(AudioSource audio)
    {
        if (!audio) return;
        audio.Stop();
        audio.volume = 1f;
    }
}