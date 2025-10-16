using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class LightProximity : MonoBehaviour
{
    public float safeDistance = 5f; // how close player must stay
    public AudioSource dangerAudio; // assign in inspector
    public AudioSource deathAudio;
    public float fadeSpeed = 0.1f;    // speed of fade-out
    public float dangerDelay = 17f; // seconds before death

    private bool isSafe = false;
    private Coroutine dangerCoroutine;
    private PlayerStats playerStats;
    private BAPlayer player;

    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        
    }

    void Update()
    {
        if (LightManager.Instance == null) return;

        LightBurn();
        lightGain();

        bool nearLight = LightManager.Instance.IsNearAnyLight(transform.position);

        if (nearLight && !isSafe)
        {
            // Entered safe area
            isSafe = true;
            Debug.Log("Entered light area");

            // Fade out danger sound if playing
            if (dangerCoroutine != null)
            {
                StopCoroutine(dangerCoroutine);
                dangerCoroutine = null;
            }
            StartCoroutine(FadeOutDangerAudio());
        }
        else if (!nearLight && isSafe)
        {
            // Left safe area
            isSafe = false;
            Debug.Log("Left light area - danger!");

            // Start danger sequence
            if (dangerCoroutine == null)
                dangerCoroutine = StartCoroutine(DangerSequence());
        }
    }

    private IEnumerator DangerSequence()
    {
        // Start looping danger audio
        if (dangerAudio != null && !dangerAudio.isPlaying)
        {
            dangerAudio.volume = 1f;
            dangerAudio.loop = true;
            dangerAudio.Play();
        }

        float elapsed = 0f;

        while (elapsed < dangerDelay)
        {
            if (isSafe) yield break; // player returned to light — cancel death
            elapsed += Time.deltaTime;
            yield return null;
        }

        // If still unsafe after delay, trigger death
        Debug.Log("Player died after staying out of light too long.");
        //FindObjectOfType<PlayerDeath>()?.Death();
    }

    private IEnumerator FadeOutDangerAudio()
    {
        if (dangerAudio == null) yield break;

        while (dangerAudio.volume > 0f)
        {
            dangerAudio.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        dangerAudio.Stop();
        dangerAudio.volume = 1f; // reset for next time
    }

    void LightBurn()
    {
        if( isSafe == false)
        {
            // Player is not in light, player will take damage
            player = GameObject.Find("Player").GetComponent<BAPlayer>();
            player.lightReducer(playerStats.LightHPLossRate * Time.deltaTime);
        }
    }
    void lightGain()
    {
        if (isSafe == true)
        {
            // Player is in campfire light, player will heal
            player = GameObject.Find("Player").GetComponent<BAPlayer>();
            player.lightIncreaser(playerStats.CamplightRegenRate * Time.deltaTime);
        }
    }


}
