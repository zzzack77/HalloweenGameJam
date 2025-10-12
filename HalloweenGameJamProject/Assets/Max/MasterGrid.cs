using UnityEngine;

public class MasterGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 50;
    public int height = 50;
    public float cellSize = 1f;
    public Vector3 origin = Vector3.zero;

    private bool[,] obstacleMap;
    
    [Header("Obstacle Detection")]
    public LayerMask obstacleMask;
    public bool autoDetectObstacles = true;
    void Awake()
    {
        obstacleMap = new bool[width , height];
        GenerateObstacleMap();
     
    }
    
   
    public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    public bool IsObstacle(int x, int y)
    {
        if (!InBounds(x, y)) return true; // Treat out of bounds as obstacle
        return obstacleMap[x, y];
    }

    public void SetObstacle(int x, int y, bool blocked)
    {
        if (!InBounds(x, y)) return;
        obstacleMap[x, y] = blocked;
    }

    public void WorldToGrid(Vector3 worldPos, out int gx, out int gy)
    {
        Vector3 local = worldPos - origin;
        gx = Mathf.FloorToInt(local.x / cellSize);
        gy = Mathf.FloorToInt(local.y / cellSize);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return origin + new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0);
    }
    public void GenerateObstacleMap()
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 worldPos = GridToWorld(x, y);
            bool blocked = Physics2D.OverlapCircle(worldPos, cellSize * 0.4f, obstacleMask) != null;
            obstacleMap[x, y] = blocked;
        }
    }
    public void DrawGizmos()
    {
        if (obstacleMap == null) return;

        Gizmos.color = Color.yellow;
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (obstacleMap[x, y])
                Gizmos.DrawCube(GridToWorld(x, y), Vector3.one * cellSize );
        }
    }

    void OnDrawGizmos()
    {
        DrawGizmos();
    }
}