using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineIsArmed = false;
    [SerializeField] private Transform[] mineShotPoint;
    [SerializeField] private GameObject projectile;
    private void Start()
    {
        StartCoroutine(mineArm());
        StartCoroutine(ActivateCollider());
    }
    private IEnumerator mineArm()
    {
        yield return new WaitForSeconds(1.5f);
        mineIsArmed = true;
        // Play arm sound (click and a beep)
    }

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

    IEnumerator ActivateCollider()
    {
        Collider2D col = GetComponent<CircleCollider2D>();
        col.enabled = false;
        yield return new WaitForFixedUpdate();  // wait for next physics step
        col.enabled = true; // now things will "enter"
    }
}
