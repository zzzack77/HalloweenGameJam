using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
public class FlowField
{
    public int width;
    public int height;
    public float cellSize;
    public Vector3 origin;
   

    private NativeArray<int> integrationField;
    private NativeArray<float2> directionField;
    private NativeArray<int> costField;

    private JobHandle flowFieldJobHandle;
    private bool bJobScheduled = false;
    public FlowField(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        

        integrationField = new NativeArray<int>(width*height, Allocator.Persistent);
        directionField = new NativeArray<float2>(width*height, Allocator.Persistent);
        costField = new NativeArray<int>(width*height, Allocator.Persistent);

        // default costs = 1
        for (int i = 0; i < costField.Length; i++) costField[i] = 1;
    }
    public void Dispose()
    {
        // Ensure any running job is complete before we dispose of the data it's using.
        flowFieldJobHandle.Complete();
        integrationField.Dispose();
        directionField.Dispose();
        costField.Dispose();
    }
    private int Index(int x, int y) => x + y * width;
    private bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    public void SetObstacle(int x, int y, bool blocked)
    {
        if (!InBounds(x, y)) return;
        costField[Index(x, y)] = blocked ? int.MaxValue : 1;
        
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return origin + new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0);
    }

    public void WorldToGrid(Vector3 worldPos, out int gx, out int gy)
    {
        Vector3 local = worldPos - origin;
        gx = Mathf.FloorToInt(local.x / cellSize);
        gy = Mathf.FloorToInt(local.y / cellSize);
    }

    /// <summary>
    /// Builds the integration and direction fields for a given goal.
    /// </summary>
   
    public void GenerateFieldAysnc(Vector3 goalWorld , float maxAngle = 75f)
    {
        //ensures previous job is completed
        flowFieldJobHandle.Complete();
        
        WorldToGrid(goalWorld, out int goalX, out int goalY);
        if(!InBounds(goalX, goalY)) return;
        // reset integration field
        for (int i = 0; i < integrationField.Length; i++)
        {
            integrationField[i] = int.MaxValue;
        }
        // setup of multithreaded job
        var job = new GenerateFlowFieldJob
        {
            // input data
            gridSize = new int2(width, height),
            goalPos = new int2(goalX, goalY),
            maxFlowAngle = maxAngle,
            costField = this.costField,

            // output data
            integrationField = this.integrationField,
            directionField = this.directionField
        };
        
        //schedule the job
        flowFieldJobHandle = job.Schedule();
        bJobScheduled = true;
    }

    public Vector2 GetDirectionAtWorld(Vector3 worldPos)
    {
        flowFieldJobHandle.Complete();
        
        WorldToGrid(worldPos, out int gx, out int gy);
        if (!InBounds(gx, gy)) return Vector2.zero;

        float2 dir = directionField[Index(gx, gy)];
        return new Vector2(dir.x, dir.y);
    }

    public void CompleteJob()
    {
        if (bJobScheduled)
        {
            flowFieldJobHandle.Complete();
            bJobScheduled = false;
        }
    }
    
    /// <summary>
    /// Resets all non-obstacle cells back to their default cost of 1.
    /// This is crucial for clearing agent costs from the previous Search.
    /// </summary>
    public void ResetDynamicCosts()
    {
        for (int i = 0; i < costField.Length; i++)
        {
            // Do not reset permanent obstacles
            if (costField[i] != int.MaxValue)
            {
                costField[i] = 1;
            }
        }
    }

    /// <summary>
    /// Increases the cost of a specific grid cell.
    /// </summary>
    public void AddCost(int x, int y, int amount)
    {
        if (!InBounds(x, y)) return;
        
        int idx = Index(x, y);
        // Do not add cost to permanent obstacles
        if (costField[idx] != int.MaxValue)
        {
            costField[idx] += amount;
        }
    }


    // Optional for debug visualisation
    public void DrawGizmos()
    {
        
        if (bJobScheduled)
        {
            flowFieldJobHandle.Complete();
        }

        
        // when the game isn't running but the editor still tries to draw gizmos.
        if (!directionField.IsCreated || !costField.IsCreated)
        {
            return;
        }
        
        Gizmos.color = Color.green;
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 pos = GridToWorld(x, y);
            Vector2 dir = directionField[Index(x, y)];
            Gizmos.DrawLine(pos, pos + new Vector3(dir.x,dir.y, 0 ) * (cellSize * 0.5f));
        }
        
        Gizmos.color = Color.red;
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (costField[Index(x,y)] ==  int.MaxValue)
                
                
                Gizmos.DrawCube(GridToWorld(x, y), Vector3.one * cellSize);
        }
    }
    
   // MODIFIED: This method now handles the proximity cost calculation.
    public void UpdateCostsFromMasterGrid(MasterGrid masterGrid, int proximityPenalty, int maxDistance)
    {
        if (masterGrid == null)
        {
            ResetDynamicCosts();
            return;
        }

        // A 2D array to store the distance of each cell from the nearest obstacle.
        // We initialize it with a large value to represent "not yet calculated".
        int[][] obstacleDistances = new int[width][];
        for (int index = 0; index < width; index++)
        {
            obstacleDistances[index] = new int[height];
        }

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        masterGrid.WorldToGrid(origin, out int startMasterX, out int startMasterY);

        // --- Step 1: Initialize all costs and find the initial set of obstacle cells ---
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                obstacleDistances[x][y] = int.MaxValue;
                int masterX = startMasterX + x;
                int masterY = startMasterY + y;
                int idx = Index(x, y);

                if (masterGrid.IsObstacle(masterX, masterY))
                {
                    costField[idx] = int.MaxValue;
                    obstacleDistances[x][y] = 0; // This is an obstacle cell, distance is 0.
                    frontier.Enqueue(new Vector2Int(x, y));
                }
                else
                {
                    costField[idx] = 1; // Default cost for a clear cell.
                }
            }
        }

        // --- Step 2: Perform a Breadth-First Search (BFS) to calculate distances ---
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            int currentDist = obstacleDistances[current.x][current.y];

            // Stop propagating if we've reached the maximum distance.
            if (currentDist >= maxDistance) continue;
            
            // Check all 8 neighbors.
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                
                int nx = current.x + dx;
                int ny = current.y + dy;

                // If neighbor is in bounds and we've found a shorter path to it...
                if (InBounds(nx, ny) && obstacleDistances[nx][ny] > currentDist + 1)
                {
                    obstacleDistances[nx][ny] = currentDist + 1;
                    frontier.Enqueue(new Vector2Int(nx, ny));
                }
            }
        }

        // --- Step 3: Apply the penalty based on the calculated distances ---
        if (proximityPenalty > 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int dist = obstacleDistances[x][y];

                    // If the cell is near an obstacle (but not an obstacle itself)
                    if (dist > 0 && dist <= maxDistance)
                    {
                        // This formula creates a falloff effect: cells closer to obstacles get a higher penalty.
                        int penalty = (proximityPenalty * (maxDistance - dist + 1)) / maxDistance;
                        costField[Index(x, y)] += penalty;
                    }
                }
            }
        }
    }
}
