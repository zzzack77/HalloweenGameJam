using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlowAgent : MonoBehaviour
{
    public float moveSpeed = 3f;
    

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