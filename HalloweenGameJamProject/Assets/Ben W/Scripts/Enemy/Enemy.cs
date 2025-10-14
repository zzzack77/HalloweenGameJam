using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    [SerializeField] private Rigidbody2D rb;
    private AugmentStructure effects;
    private BAPlayer player;
    private bool explode;
    [SerializeField] private GameObject Exploder;
    

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
            // Play death animation
            // Update score
            if (explode)
            {
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
            Debug.Log("past enemy");
            if (augmentStructure.burn == true)
            {
                burn();
            }
            else if (augmentStructure.freeze == true)
            {
                freeze(2.5f);
            }
            else if(augmentStructure.explode == true)
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
        int index = 3;
        if (index <= 0)
        {
            return;
        }
        else
        {
            health--; 
            index--;
            Invoke("burn", 1f);
        }
    }
    
}


