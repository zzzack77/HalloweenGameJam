using System.Collections;
using UnityEngine;

public class PlayerRangedWeapon : MonoBehaviour
{
    public int currentAmmo = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public virtual void Shoot()
    {

    }

    public void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    public IEnumerator ReloadCountdown(float reloadTime, int maxAmmo)
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
    }

   
}
