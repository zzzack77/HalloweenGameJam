using UnityEngine;

public class LightProximity : MonoBehaviour
{
    public float safeDistance = 5f; // how close player must stay
    private bool isSafe = false;

    void Update()
    {
        if (LightManager.Instance == null) return;

        bool nearLight = LightManager.Instance.IsNearAnyLight(transform.position);

        if (nearLight && !isSafe)
        {
            isSafe = true;
            Debug.Log("Entered light area");
        }
        else if (!nearLight && isSafe)
        {
            isSafe = false;
            Debug.Log("Left light area - danger!");

            // Player is in danger
        }
    }
}
