using System.Collections;
using UnityEngine;

public class PlayerRangedWeapon : MonoBehaviour
{
    public int currentAmmo = 0;
    
    [SerializeField] protected AudioClip[] shootClip;          // sound for shooting
    [SerializeField] protected AudioClip reloadClip;         // sound for reloading
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
        if (SoundFXManager.Instance != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(reloadClip, transform, 1f);
        }
        
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        
    }

   
}
