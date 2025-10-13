using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private GameObject target;
    private float lastHit = 0f;
    [SerializeField] private GameObject shot;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position, target.transform.position) < 10f)
        {
            if ((Time.time - lastHit) > 2f)
            {
                Instantiate(shot, this.transform.position, Quaternion.identity);
                lastHit = Time.time;
            }
            
        }
    }
}
