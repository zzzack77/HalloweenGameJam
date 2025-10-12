using UnityEngine;

public class SpawnRoutine : MonoBehaviour
{
    [SerializeField] private AccTimer timer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Zombie;


    void Awake()
    {
        timer.Tick += OnTick;        
    }

    void OnEnable()
    {
        timer.Begin();
    }

    void OnDisable()
    {
        timer.End();
    }

    void OnTick()
    {
        Instantiate(Zombie, (player.transform.position + new Vector3(Random.Range(-30f,30f), Random.Range(-30f, 30f), 0)), Quaternion.identity);
        Debug.Log($"Tick at {Time.time:F2}s");
    }
}
