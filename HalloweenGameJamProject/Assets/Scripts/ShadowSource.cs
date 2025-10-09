using UnityEngine;

[ExecuteAlways]
public class ShadowSource : MonoBehaviour
{
    private void Start()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.RegisterShadow(this);
    }

    void OnEnable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.RegisterShadow(this);
    }

    void OnDisable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.UnegisterShadow(this);
    }
}
