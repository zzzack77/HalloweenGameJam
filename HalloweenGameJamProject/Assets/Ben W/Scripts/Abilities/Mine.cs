using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineIsArmed = false; 
    [SerializeField] private Transform[] mineShotPoint; // The transform of where projectiles will shoot from
    [SerializeField] private GameObject projectile;

    
    private void Start()
    {
        StartCoroutine(mineArm());
        
    }

    // Countdown to activate the mine
    private IEnumerator mineArm()
    {
        yield return new WaitForSeconds(1.5f);
        mineIsArmed = true;
        // Play arm sound (click and a beep)
    }

    // Explodes the mine, firing projectiles in all directions
    private void DetonateMine()
    {
        for (int i = 0; i < mineShotPoint.Length; i++)
        {
            Instantiate(projectile, mineShotPoint[i].position, mineShotPoint[i].rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && mineIsArmed)
        {
            Debug.Log("Enemy has entered mine radius!");
            DetonateMine();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && mineIsArmed)
        {
            Debug.Log("Enemy has entered mine radius!");
            DetonateMine();
            Destroy(gameObject);
        }
    }

    
}
