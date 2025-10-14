using UnityEngine;

public class CampfireScript : MonoBehaviour
{

    public GameObject litCampFire;
    public GameObject unlitCampFire;

    public GameObject litLights;
    public GameObject unlitLights;

    public GameObject particleSystem;
    public GameObject uiCanvas;

    public Collider2D collider;

    private bool fireLit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        ExtinguishFire();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        uiCanvas.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        uiCanvas.SetActive(false);
    }
    void ExtinguishFire()
    {
        fireLit = false;
        litCampFire.SetActive(false);
        unlitCampFire.SetActive(true);

        litLights.SetActive(false);
        unlitLights.SetActive(true);

        particleSystem.SetActive(false);
        uiCanvas.SetActive(false);
    }

    void LightFire()
    {
        fireLit = true;
        uiCanvas.SetActive(false);

        litCampFire.SetActive(false);
        unlitCampFire.SetActive(true);

        litLights.SetActive(false);
        unlitLights.SetActive(true);

        particleSystem.SetActive(false);
    }
}
