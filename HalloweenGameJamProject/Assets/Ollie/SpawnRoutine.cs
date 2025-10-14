using UnityEngine;

public class SpawnRoutine : MonoBehaviour
{
    [SerializeField] private AccTimer timer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Zombie;
    [SerializeField] private GameObject Goblin;
    [SerializeField] private GameObject Skeleton;


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
        if(timer._interval > 2.5)
        {
            Instantiate(Zombie, (player.transform.position + ( new Vector3(Random.Range(-20, 20f) * 2 , Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
        }
        else if(timer._interval > 2)
        {
            int index = Random.Range(0, 10);
            if (index > 3)
            {
                Instantiate(Zombie, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
            else
            {
                Instantiate(Skeleton, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
        }
        else if(timer._interval > 1.5)
        {
            int index = Random.Range(0, 10);
            if(index > 5)
            {
                Instantiate(Zombie, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
            else if(index > 2)
            {
                Instantiate(Skeleton, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
            else
            {
                Instantiate(Goblin, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
        }
        else
        {
            int index = Random.Range(0, 10);
            if (index > 6)
            {
                Instantiate(Zombie, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
            else if (index > 3)
            {
                Instantiate(Skeleton, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }
            else
            {
                Instantiate(Goblin, (player.transform.position + (new Vector3(Random.Range(-20f, 20f) * 2, Random.Range(-20f, 20f) * 2, 0))), Quaternion.identity);
            }

        }


        //Debug.Log($"Interval: {timer._interval}  Tick at {Time.time:F2}s");

    }
}
