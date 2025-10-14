using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontal, vertical, 0);
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
