using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private CircleCollider2D col;
    private float attackCD;
    private float lastHit = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Time.time - lastHit) > 2f)
        {
            if (collision.collider.CompareTag("Player"))
            {
                lastHit = Time.time;

                BAPlayer player = collision.collider.GetComponent<BAPlayer>();
                player.lightReducer(10f);

                //PlayerScript player2 = collision.collider.GetComponent<PlayerScript>();
                //player2.lightReducer(10f);
            }
        }
    }

}