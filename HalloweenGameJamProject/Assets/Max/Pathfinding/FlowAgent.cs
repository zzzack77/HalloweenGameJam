using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlowAgent : MonoBehaviour
{
    public float moveSpeed = 3f;
    [SerializeField] private bool bDestroyOnExitOfGrid = false;
    // A unique ID to link this GameObject to its data in the manager's arrays.
    public int AgentId { get; private set; }
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (FlowFieldManager.Instance != null)
        {
            FlowFieldManager.Instance.allAgents.Add(this);
        }
    }

    void FixedUpdate()
    {
        Vector2 dir = FlowFieldManager.Instance.GetDirection(transform.position);

        if (dir.sqrMagnitude > 0.01f)
        {
           

            // Move along flow direction
            Vector2 move = dir.normalized * (moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + move);
        }
        else if (dir == Vector2.zero && bDestroyOnExitOfGrid)
        {

            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        // Remove this agent from the manager's list
        if (FlowFieldManager.Instance != null)
        {
            FlowFieldManager.Instance.allAgents.Remove(this);
        }
    }


}