using UnityEngine;


[ExecuteAlways]
public class LightSource : MonoBehaviour
{
    public float safeRadius = 5f;
    public Color radiusColor = new Color(1f, 1f, 0f, 0.25f); // soft yellow translucent

    private void Start()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.RegisterLight(this);
    }

    void OnEnable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.RegisterLight(this);
    }

    void OnDisable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.UnregisterLight(this);
    }

    // Draws a visible circle in Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(transform.position, safeRadius);
    }

    // Draw filled circle for easier visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(radiusColor.r, radiusColor.g, radiusColor.b, 0.15f);
        Gizmos.DrawSphere(transform.position, safeRadius);
    }
}
