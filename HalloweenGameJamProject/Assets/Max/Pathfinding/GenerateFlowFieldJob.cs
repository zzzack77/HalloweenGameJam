using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct GenerateFlowFieldJob : IJob
{
    // Input Data (read-only)
   
    [ReadOnly] public int2 gridSize;
    [ReadOnly] public int2 goalPos;
    [ReadOnly] public float maxFlowAngle;
    
    [ReadOnly] public NativeArray<int> costField;

    // Output Data 
    
    public NativeArray<int> integrationField;
    public NativeArray<float2> directionField;

    
    public void Execute()
    {
        //  Integration Field Calculation (Breadth-First Search) 
        

        int goalIndex = Index(goalPos.x, goalPos.y);
        integrationField[goalIndex] = 0;

        NativeQueue<int2> frontier = new NativeQueue<int2>(Allocator.Temp);
        frontier.Enqueue(goalPos);

        while (frontier.TryDequeue(out int2 currentPos))
        {
            int currentIndex = Index(currentPos.x, currentPos.y);
            int currentCost = integrationField[currentIndex];

            // Check 8 neighbors
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int2 neighborPos = new int2(currentPos.x + dx, currentPos.y + dy);
                    if (!InBounds(neighborPos)) continue;

                    int neighborIndex = Index(neighborPos.x, neighborPos.y);
                    if (costField[neighborIndex] == int.MaxValue) continue;

                    int moveCost = (dx != 0 && dy != 0) ? 14 : 10;
                    int newCost = currentCost + moveCost + costField[neighborIndex];

                    if (newCost < integrationField[neighborIndex])
                    {
                        integrationField[neighborIndex] = newCost;
                        frontier.Enqueue(neighborPos);
                    }
                }
            }
        }
        frontier.Dispose(); // Clean up temporary memory

        //  Direction Field Calculation 
        
for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                int idx = Index(x, y);
                if (integrationField[idx] == int.MaxValue)
                {
                    directionField[idx] = float2.zero;
                    continue;
                }

                // Find the single best neighbor, regardless of direction 
                int bestCost = int.MaxValue;
                int2 bestDir = int2.zero;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        int2 neighborPos = new int2(x + dx, y + dy);
                        if (!InBounds(neighborPos)) continue;

                        int neighborIndex = Index(neighborPos.x, neighborPos.y);
                        if (integrationField[neighborIndex] < bestCost)
                        {
                            bestCost = integrationField[neighborIndex];
                            bestDir = new int2(dx, dy);
                        }
                    }
                }

                //  Now, validate that best direction 
                bool bestIsDiagonal = bestDir.x != 0 && bestDir.y != 0;

                if (bestIsDiagonal)
                {
                    
                    float2 vectorToGoal = math.normalize((float2)goalPos - new float2(x, y));
                    float2 bestDirF2 = math.normalize((float2)bestDir);

                    
                    float angle = math.degrees(math.acos(math.dot(bestDirF2, vectorToGoal)));
                    
                    if (angle > maxFlowAngle)
                    {
                        bestCost = int.MaxValue;
                        bestDir = int2.zero;
                        
                        // Checks cardinal directions if the angle to the goal is too great.
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                // This condition isolates cardinal directions (up, down, left, right).
                                if (math.abs(dx) + math.abs(dy) != 1) continue;

                                int2 neighborPos = new int2(x + dx, y + dy);
                                if (!InBounds(neighborPos)) continue;
                                
                                int neighborIndex = Index(neighborPos.x, neighborPos.y);
                                if (integrationField[neighborIndex] < bestCost)
                                {
                                    bestCost = integrationField[neighborIndex];
                                    bestDir = new int2(dx, dy);
                                }
                            }
                        }
                    }
                }
                
                directionField[idx] = math.normalizesafe(new float2(bestDir.x, bestDir.y));
            }
        }
    }

    // Helper functions need to be inside the job struct.
    private int Index(int x, int y) => x + y * gridSize.x;
    private bool InBounds(int2 pos) => pos.x >= 0 && pos.y >= 0 && pos.x < gridSize.x && pos.y < gridSize.y;
}