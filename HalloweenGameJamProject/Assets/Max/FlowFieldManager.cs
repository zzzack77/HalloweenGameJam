using System.Collections.Generic;
using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int width = 50;
    public int height = 50;
    public float cellSize = 1f;
    public Vector3 origin = Vector3.zero;
    
   
    
    [Header("Goal")]
    public Transform goal;
    
    [Range(0f, 180f)]
    public float maxAngle = 75f;

    [Tooltip("Cost added to cells occupied by other agents")]
    public int agentCost = 5;

    private FlowField field;
    public List<FlowAgent> allAgents = new List<FlowAgent>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        field = new FlowField(width, height, cellSize, origin);
    }

   

    void Update()
    {
        if (goal != null)
        {
            // 1. Reset costs from the previous frame
            field.ResetDynamicCosts();

            // 2. Add new costs based on current agent positions
            foreach (var agent in allAgents)
            {
                field.WorldToGrid(agent.transform.position, out int gx, out int gy);
                field.AddCost(gx, gy, agentCost);
            }

            // 3. Generate the field with all costs applied
            field.GenerateField(goal.position, maxAngle);
        }
    }

    public Vector2 GetDirection(Vector3 worldPos)
    {
        return field.GetDirectionAtWorld(worldPos);
    }

    public void SetObstacle(Vector3 worldPos, bool blocked)
    {
        field.WorldToGrid(worldPos, out int gx, out int gy);
        field.SetObstacle(gx, gy, blocked);
    }

    void OnDrawGizmos()
    {
        if (field != null)
            field.DrawGizmos();
    }
}