using System.Collections;
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
    
    public float regenerationInterval = 0.2f;
    private Vector3 lastGridOrigin; // Tracks the grid's last position
   
    
    [Header("Goal")]
    public Transform goal;
    
    [Range(0f, 180f)]
    public float maxAngle = 75f;

    [Header("Costs")] 
    [Tooltip("Cost added to cells occupied by other agents")]
    public int agentCost = 5;

    
    [Tooltip("Highest cost added to cells directly next to an obstacle.")]
    public int obstacleProximityPenalty = 20;

    
    [Tooltip("How many cells away from an obstacle the proximity penalty will spread.")]
    public int maxProximityDistance = 4;

    private FlowField field;
    public List<FlowAgent> allAgents = new List<FlowAgent>();
    
    
    
    [SerializeField] private  MasterGrid masterGrid;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        field = new FlowField(width, height, cellSize, origin);
       
    }
    void OnDestroy()
    {
        if (field != null)
        {
            field.Dispose();
        }
    }

    void Start()
    {
        lastGridOrigin = Vector3.one * float.MaxValue; // Force an update on the first run
        StartCoroutine(UpdateFieldRoutine());
    }
   

    void Update()
    {
    
    }
    
    private IEnumerator UpdateFieldRoutine()
    {
        while (true)
        {
            if (goal != null)
            {
                // Calculate where the grid's bottom-left corner should be to keep the player centered.
                float halfWorldWidth = (width * cellSize) * 0.5f;
                float halfWorldHeight = (height * cellSize) * 0.5f;
                Vector3 newOrigin = new Vector3(  Mathf.RoundToInt(goal.position.x)- halfWorldWidth, Mathf.RoundToInt(goal.position.y) - halfWorldHeight, 0);

                // We only need to do the expensive path calculation IF the player has moved enough
                // to require shifting the grid's origin point.
                if (Vector3.SqrMagnitude(newOrigin - lastGridOrigin) > cellSize * cellSize)
                {
                    lastGridOrigin = newOrigin;
                    field.origin = newOrigin;

                    // Now, generate the full field including agent avoidance
                    UpdateFieldCostsAndGenerate();
                }
            }
            yield return new WaitForSeconds(regenerationInterval);
        }
    }

    void UpdateFieldCostsAndGenerate()
    {
        // 1. Reset all dynamic costs since the grid has shifted.
        field.CompleteJob(); // Use the old, simple reset method for this.

        
        if (masterGrid != null)
        {
            field.UpdateCostsFromMasterGrid(masterGrid, obstacleProximityPenalty, maxProximityDistance);

        }
        
        // 2. Add costs for agents currently within the grid.
        foreach (var agent in allAgents)
        {
            field.WorldToGrid(agent.transform.position, out int gx, out int gy);
            // AddCost should check if the agent is within the new grid bounds
            field.AddCost(gx, gy, agentCost);
        }
        // 3. Generate the master flow field.
        field.GenerateFieldAysnc(goal.position, maxAngle);
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