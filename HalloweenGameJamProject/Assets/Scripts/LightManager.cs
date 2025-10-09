using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    // Store both transform and radius
    [SerializeField]
    private List<LightSource> lights = new List<LightSource>();
    [SerializeField]
    private List<ShadowSource> shadows = new List<ShadowSource>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterLight(LightSource light)
    {
        if (!lights.Contains(light))
            lights.Add(light);
    }
    public void RegisterShadow(ShadowSource shadow)
    {
        if (!shadows.Contains(shadow))
            shadows.Add(shadow);
    }

    public void UnregisterLight(LightSource light)
    {
        if (lights.Contains(light))
            lights.Remove(light);
    }
    public void UnegisterShadow(ShadowSource shadow)
    {
        if (shadows.Contains(shadow))
            shadows.Remove(shadow);
    }

    // Check if player is near *any* light based on each light’s radius
    public bool IsNearAnyLight(Vector2 playerPos)
    {
        

        foreach (LightSource light in lights)
        {
            if (Vector2.Distance(playerPos, light.transform.position) <= light.safeRadius)
                return true;
        }
        return false;
    }
    public bool IsPlayerCastingShadow(Vector2 playerPos)
    {
        foreach (LightSource light in lights)
        {
            foreach (ShadowSource shadow in shadows)
            {
                if (Vector2.Distance(shadow.transform.position, light.transform.position) < Vector2.Distance(playerPos, light.transform.position))
                {
                    return true;
                }
            }
        }
        return false;
    }
        
}
