using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Campfire : MonoBehaviour, IPayLighting
{
    
    [SerializeField] private Canvas promptCanvas;
    [SerializeField] private GameObject innerLight;
    [SerializeField] private GameObject centerLight;
    [SerializeField] private GameObject outerLight;
    private LightSource lightSource;
    [SerializeField] private float LightTime;
    private Light2D innerLight2D;
    private Light2D centerLight2D;
    private Light2D outerLight2D;
    public float lightingCost { get; } = 10;
    public bool bActive{get;set;}
    public bool PlayerInRange { get; private set; }


    private float innerInitalLight;
    private float centerInitalLight;
    private float outerInitalLight;
    private float initialRadius;

    private void Awake()
    {
        innerLight2D = innerLight.GetComponent<Light2D>();
        centerLight2D = centerLight.GetComponent<Light2D>();
        outerLight2D = outerLight.GetComponent<Light2D>();
        lightSource = GetComponent<LightSource>();
        
        
         innerInitalLight = innerLight2D.intensity;
         centerInitalLight = centerLight2D.intensity;
         outerInitalLight = outerLight2D.intensity;
         initialRadius = lightSource.safeRadius;
    }

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

    public void Activate()
    {
        Debug.Log("Activating Campfire");
        
        innerLight2D.intensity = innerInitalLight;
        centerLight2D.intensity = centerInitalLight;
        outerLight2D.intensity = outerInitalLight ;
        lightSource.safeRadius = initialRadius;
        
        innerLight.SetActive(true);
        centerLight.SetActive(true);
        outerLight.SetActive(true);
        
        StartCoroutine(CampFireLit());
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
            Debug.Log("Player entered activation range");
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
        Debug.Log("Player left activation range");
    }


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
            

            if (LightPercentage <= 0.1)
            {
               bActive = false; 
               Debug.Log(" Campfire Dead");
            
            }
            yield return null;
        }
    }
}
