using UnityEngine;
using System.Collections;

public class SluttyVooDooDoctor : MonoBehaviour
{
    [SerializeField] private CircleCollider2D col;
    private float attackCD;
    private float lastHit = 0f;
    [SerializeField] private GameObject healOB;

    private void Start()
    {
        StartCoroutine(heal());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Time.time - lastHit) > 2f)
        {
            if (collision.collider.CompareTag("Player"))
            {
                lastHit = Time.time;

                BAPlayer player = collision.collider.GetComponent<BAPlayer>();
                player.lightReducer(10f);

            }
        }
    }

    private IEnumerator heal()
    {
        while (true)
        {
            healOB.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            healOB.SetActive(false);
            yield return new WaitForSeconds(5f);

        }
    }






}
