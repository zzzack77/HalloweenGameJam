using UnityEngine;

public class MasterGrid :MonoBehaviour
{
    public readonly int width;
    public readonly int height;
    public readonly float  cellSize;
    private bool[] obstacleGrid;



   
    
    private int Index(int x, int y) => x + y * width;
    public bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    // A method to mark a cell as an obstacle
    public void SetObstacle(int x, int y, bool isBlocked)
    {
        if (IsInBounds(x, y))
        {
            obstacleGrid[Index(x, y)] = isBlocked;
        }
    }

    // A method to check if a cell is an obstacle
    public bool IsObstacle(int x, int y)
    {
        if (!IsInBounds(x, y)) return true; // Treat out-of-bounds as an obstacle
        return obstacleGrid[Index(x, y)];
    }
}
