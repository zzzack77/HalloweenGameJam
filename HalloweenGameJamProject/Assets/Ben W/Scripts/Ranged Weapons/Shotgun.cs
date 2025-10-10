using UnityEngine;

public class Shotgun : PlayerRangedWeapon
{
    [SerializeField] private Transform[] shotPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int maxAmmo = 2;
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
        // Rotates the weapon to the cursor position
        RotateWeapon();
    }

    // Shoots the bullets and reduces ammo, also reloads if ammo reaches 0
    public override void Shoot()
    {
        for (int i = 0; i < shotPoint.Length; i++)
        {
            Instantiate(bullet, shotPoint[i].position, shotPoint[i].rotation);
        }
        // Play shot sound
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadCountdown(2, maxAmmo));
        }

    }


}
