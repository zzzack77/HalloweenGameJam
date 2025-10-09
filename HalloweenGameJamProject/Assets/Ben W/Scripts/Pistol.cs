using UnityEngine;

public class Pistol : PlayerRangedWeapon
{
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int maxAmmo = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot();

        }
        RotateWeapon();
    }

    public override void Shoot()
    {
        
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        
        // Play shot sound
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadCountdown(2, maxAmmo));
        }

    }
}
