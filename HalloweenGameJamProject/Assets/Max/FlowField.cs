using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public int width;
    public int height;
    public float cellSize;
    public Vector3 origin;
   

    private int[] integrationField;
    private Vector2[] directionField;
    private int[] costField;

    public FlowField(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        

        integrationField = new int[width * height];
        directionField = new Vector2[width * height];
        costField = new int[width * height];

        // default costs = 1
        for (int i = 0; i < costField.Length; i++) costField[i] = 1;
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
   
    public void GenerateField(Vector3 goalWorld , float maxAngle = 75f)
    {
        WorldToGrid(goalWorld, out int goalX, out int goalY);
        if (!InBounds(goalX, goalY)) return;

        int size = width * height;
        for (int i = 0; i < size; i++)
            integrationField[i] = int.MaxValue;

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        int goalIndex = Index(goalX, goalY);
        integrationField[goalIndex] = 0;
        frontier.Enqueue(new Vector2Int(goalX, goalY));

        /*
         * carries out a breadthFirst search on all cells in the grid
         */
        while (frontier.Count > 0)
        {
            var currentVector = frontier.Dequeue();
            int currentX = currentVector.x, currentY = currentVector.y;
            int currentIndex = Index(currentX, currentY);

            for (int directionX = -1; directionX <= 1; directionX++)
            for (int directionY = -1; directionY <= 1; directionY++)
            {
                if (directionX == 0 && directionY == 0) continue;
                int newX = currentX + directionX, newY = currentY + directionY;
                if (!InBounds(newX, newY)) continue;

                int newIndex = Index(newX, newY);
                if (costField[newIndex] == int.MaxValue) continue;

                int moveCost = (directionX != 0 && directionY != 0) ? 14 : 10;
                int newCost = integrationField[currentIndex] + moveCost + costField[newIndex];

               

                

                if (newCost < integrationField[newIndex])
                {
                    integrationField[newIndex] = newCost;
                    frontier.Enqueue(new Vector2Int(newX, newY));
                }
            }
        }
        
       // Build direction field
       
       /*
        * Loops through the grid to add the direction to the goal in every cell
        */
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int idx = Index(x, y);
            if (integrationField[idx] == int.MaxValue)
            {
                directionField[idx] = Vector2.zero;
                continue;
            }

            // --- Find the single best neighbor, regardless of direction ---
            int bestCost = int.MaxValue;
            Vector2 bestDir = Vector2.zero;

            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx, ny = y + dy;
                if (!InBounds(nx, ny)) continue;

                int nIdx = Index(nx, ny);
                if (integrationField[nIdx] < bestCost)
                {
                    bestCost = integrationField[nIdx];
                    bestDir = new Vector2(dx, dy);
                }
            }

            // --- Now, validate that best direction ---
            bool bestIsDiagonal = bestDir.x != 0 && bestDir.y != 0;

            if (bestIsDiagonal)
            {
                Vector2 vectorToGoal = (Vector2)goalWorld - (Vector2)GridToWorld(x, y);
                if (Vector2.Angle(bestDir, vectorToGoal) > maxAngle)
                {
                    
                    bestCost = int.MaxValue;
                    bestDir = Vector2.zero;
                    //Checks cardinal directions if the angle to the goal is to great
                    for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        
                        if (Mathf.Abs(dx) + Mathf.Abs(dy) != 1) continue;

                        int nx = x + dx, ny = y + dy;
                        if (!InBounds(nx, ny)) continue;
                        
                        int nIdx = Index(nx, ny);
                        if (integrationField[nIdx] < bestCost)
                        {
                            bestCost = integrationField[nIdx];
                            bestDir = new Vector2(dx, dy);
                        }
                    }
                }
            }

            directionField[idx] = bestDir.normalized;
        }
        
    }

    public Vector2 GetDirectionAtWorld(Vector3 worldPos)
    {
        WorldToGrid(worldPos, out int gx, out int gy);
        if (!InBounds(gx, gy)) return Vector2.zero;
        return directionField[Index(gx, gy)];
    }
    
    /// <summary>
    /// Resets all non-obstacle cells back to their default cost of 1.
    /// This is crucial for clearing agent costs from the previous frame.
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
        Gizmos.color = Color.green;
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 pos = GridToWorld(x, y);
            Vector2 dir = directionField[Index(x, y)];
            Gizmos.DrawLine(pos, pos + new Vector3(dir.x,dir.y, 0 ) * (cellSize * 0.5f));
        }
    }
}
