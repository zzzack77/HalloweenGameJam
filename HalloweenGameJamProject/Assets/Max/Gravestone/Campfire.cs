using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Campfire : MonoBehaviour, IPayLighting
{
    // Audio
    public AudioSource campfireStartAudio;
    public AudioSource campfireIdelAudio;

    // Sprites
    public GameObject litCampFire;
    public GameObject unlitCampFire;

    // Particles
    public ParticleSystem particles;

    // Lights
    [SerializeField] private Canvas promptCanvas;
    [SerializeField] private GameObject innerLight;
    [SerializeField] private GameObject centerLight;
    [SerializeField] private GameObject outerLight;
    private Light2D innerLight2D;
    private Light2D centerLight2D;
    private Light2D outerLight2D;
    private LightSource lightSource;
    [SerializeField] private float LightTime;
    [SerializeField] private float lightWarmUpTime;

    // Light intensity values
    private float innerInitalLight;
    private float centerInitalLight;
    private float outerInitalLight;
    private float initialRadius;

    // Lighting and player checks for activation
    public float lightLevelUnlit = 0.1f;
    public float lightingCost { get; } = 10;
    public bool bActive{get;set;}
    public bool PlayerInRange { get; private set; }

    private void Awake()
    {
        innerLight2D = innerLight.GetComponent<Light2D>();
        centerLight2D = centerLight.GetComponent<Light2D>();
        outerLight2D = outerLight.GetComponent<Light2D>();
        lightSource = GetComponent<LightSource>();
        particles = GetComponent<ParticleSystem>();
        particles.Stop();


        innerInitalLight = innerLight2D.intensity;
        centerInitalLight = centerLight2D.intensity;
        outerInitalLight = outerLight2D.intensity;
        initialRadius = lightSource.safeRadius;

        ExtinguishFire();
    }

    // Returns true if player can activate a campfire
    public bool CanActivate(ref float playerLight)
    {
        if (playerLight >= lightingCost && bActive == false)
        {
            playerLight -= lightingCost;;
            bActive = true;
            Activate();
            promptCanvas.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    // Call to activate campfire with soft start lighting
    public void Activate()
    {
        campfireStartAudio.Play();
        campfireIdelAudio.Play();

        particles.Play();
        litCampFire.gameObject.SetActive(true);
        unlitCampFire.gameObject.SetActive(false);

        StartCoroutine(CampFirePreLit());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || bActive) return;
        PlayerInRange = true;
        BAPlayer player = other.GetComponent<BAPlayer>();
        if (player)
        {
            player.SetInteractable(this);
            if (promptCanvas)
            {
                promptCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            //Debug.Log("Player entered activation range");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerInRange = false;
        BAPlayer player = other.GetComponent<BAPlayer>();
        if (player)
        {
            player.SetInteractable(null); 
            if(promptCanvas)
            {
                promptCanvas.gameObject.SetActive(false);
            }
        }
        //Debug.Log("Player left activation range");
    }

    // Extinguishes Fire
    void ExtinguishFire()
    {
        // Audio handeling
        campfireIdelAudio.Stop();

        // Set lights to dim
        innerLight2D.intensity = innerInitalLight * 0.1f;
        centerLight2D.intensity = centerInitalLight * lightLevelUnlit;
        outerLight2D.intensity = outerInitalLight * lightLevelUnlit;
        lightSource.safeRadius = 0;
        
        // Update sprite
        litCampFire.gameObject.SetActive(false);
        unlitCampFire.gameObject.SetActive(true);

        particles.Stop();
    }

    // Call after CampFirePreLit for soft lighting effects
    IEnumerator CampFireLit()
    {
        float elapsed = 0f;
        float LightPercentage = 1.0f;
        
        while (bActive)
        {
            elapsed += Time.deltaTime;
            LightPercentage = Mathf.Lerp(1.0f,0f,elapsed / LightTime);
            
            innerLight2D.intensity = innerInitalLight *LightPercentage;
            centerLight2D.intensity = centerInitalLight * LightPercentage;
            outerLight2D.intensity = outerInitalLight * LightPercentage;
            lightSource.safeRadius = initialRadius *  LightPercentage;
            

            if (LightPercentage <= lightLevelUnlit)
            {
               bActive = false;
               ExtinguishFire();
               Debug.Log(" Campfire Dead");
            
            }
            yield return null;
        }
    }

    // Eases lights in for smoothness
    IEnumerator CampFirePreLit()
    {
        float elapsed = 0f;
        float lightPercentage = 0.1f;

        // gradually increase until fully lit
        while (lightPercentage < 1f)
        {
            elapsed += Time.deltaTime;

            // Smoothly interpolate light intensity over time
            lightPercentage = Mathf.Lerp(0.1f, 1f, elapsed / lightWarmUpTime);

            // Apply lighting changes
            innerLight2D.intensity = innerInitalLight * lightPercentage;
            centerLight2D.intensity = centerInitalLight * lightPercentage;
            outerLight2D.intensity = outerInitalLight * lightPercentage;
            lightSource.safeRadius = initialRadius * lightPercentage;

            // Debug log to track progress
            Debug.Log($"Light %: {lightPercentage:F2}");

            // Check if the threshold is reached, then trigger next coroutine (once)
            if (lightPercentage > 0.9f)
            {
                StartCoroutine(CampFireLit());
            }

            yield return null; // wait for next frame
        }
    }
}
