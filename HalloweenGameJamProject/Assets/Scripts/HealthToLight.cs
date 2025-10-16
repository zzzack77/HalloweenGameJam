using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthToLight : MonoBehaviour
{
    public GameObject outerlightObject;
    public GameObject innerlightObject;
    private Light2D outerLight;
    private Light2D innerLight;


    [SerializeField]
    private float outerLightMinimumFalloffIntensity = 0.6f;

    public float replaceWPlayerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        outerLight = outerlightObject.GetComponent<Light2D>();
        innerLight = innerlightObject.GetComponent<Light2D>();
    }

    public void AdjustLight(float playerHealth)
    {
        outerLight.falloffIntensity = Mathf.Lerp(1f, 0.6f, playerHealth / 100);
        outerLight.intensity = Mathf.Lerp(0.1f, 2.9f, playerHealth / 100);

        innerLight.falloffIntensity = Mathf.Lerp(1f, 0.5f, playerHealth / 100);
        innerLight.intensity = Mathf.Lerp(1f, 5.3f, playerHealth / 100);

    }
    

}
