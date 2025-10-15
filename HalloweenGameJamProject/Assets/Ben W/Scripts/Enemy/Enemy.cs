using System.Collections;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public GameObject soulPrefab;

    [SerializeField] private Rigidbody2D rb;
    private AugmentStructure effects;
    private BAPlayer player;
    private bool explode;
    [SerializeField] private GameObject Exploder;
    [SerializeField] private GameObject Burner;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int index;
    private int souls;


    Coroutine freezeCo;
    RigidbodyConstraints2D originalConstraints;

    public void Start()
    {
        explode = false;    
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BAPlayer>();
        originalConstraints = rb.constraints;
    }



    public void TakeDamage(float damage)
    {
        effects = player.effects;
        if(effects == null)
        {
            Debug.Log("null effects ");
        }
        health -= damage;
        ApplyEffects(effects);
        if (health <= 0)
        {
            Debug.Log("Death");
            SpawnSouls();
            // Play death animation
            // Update score
            if (explode)
            {
                spriteRenderer.enabled = false;
                Explosion();
            }
            else
            {
                
                
                Destroy(gameObject);

            }
        }
    }

    public void ApplyEffects(AugmentStructure augmentStructure)
    {
        if (augmentStructure != null)
        {
            //Debug.Log("past enemy");
            if (augmentStructure.burn == true)
            {
                Burner.SetActive(true);
                index = 5;
                burn();
            }
            if (augmentStructure.freeze == true)
            {
                freeze(2.5f);
            }
            if(augmentStructure.explode == true)
            {
                explode = true;
            }
        }
    }

    private void Explosion()
    {
        Exploder.SetActive(true);
    }

    public void freeze(float seconds)
    {
        if (freezeCo != null) StopCoroutine(freezeCo);
        freezeCo = StartCoroutine(FreezeRoutine(seconds));
    }

    private IEnumerator FreezeRoutine(float seconds)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(seconds);

        rb.constraints = originalConstraints;

        freezeCo = null;
    }

    public void burn()
    {
        if (index <= 0)
        {
            Burner.SetActive(false);
            return;
        }
        else
        {
            health--;
            if (health <= 0)
            {
                Debug.Log("burned to death");
                SpawnSouls();
                // Play death animation
                // Update score
                if (explode)
                {
                    spriteRenderer.enabled = false;
                    Explosion();
                }
                else
                {
                   
                    Destroy(gameObject);

                }
            }
            index--;
            Invoke("burn", 1f);
        }
    }

    void SpawnSouls()
    {
        if (soulPrefab == null) return;
        souls = Random.Range(1, 3);

        for (int i = 0; i < souls; i++)
        {
            Instantiate(soulPrefab, transform.position, Quaternion.identity);
        }
    }

}


