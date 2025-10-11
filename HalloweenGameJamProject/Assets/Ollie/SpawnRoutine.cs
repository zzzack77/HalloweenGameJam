using UnityEngine;

public class SpawnRoutine : MonoBehaviour
{
    [SerializeField] private AccTimer timer;

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
        Debug.Log($"Tick at {Time.time:F2}s");
    }
}
