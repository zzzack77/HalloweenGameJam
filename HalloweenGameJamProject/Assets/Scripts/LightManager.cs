using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    // Store both transform and radius
    [SerializeField]
    private List<LightSource> lights = new List<LightSource>();

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

    public void UnregisterLight(LightSource light)
    {
        if (lights.Contains(light))
            lights.Remove(light);
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
}
