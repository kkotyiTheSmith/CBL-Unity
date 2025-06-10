using UnityEngine;
using System.IO;
using System.Text;

public class GridLogger : MonoBehaviour
{
    public MicInput micInput;
    public Vector2Int gridSize = new Vector2Int(10, 10); // 10x10 grid
    public float cellSize = 1.0f; // Each cell is 1 unit square
    public Vector2 origin = Vector2.zero; // World position of grid [0,0]
    private float saveInterval;
    private float saveTimer;

    private float[,] gridValues;
    private int[,] sampleCounts;

    void Start()
    {
        gridValues = new float[gridSize.x, gridSize.y];
        sampleCounts = new int[gridSize.x, gridSize.y];
    }

    void Update()
    {
        Vector3 worldPos = transform.position;
        Vector2Int gridPos = WorldToGrid(worldPos);

        if (IsValidGridPos(gridPos))
        {
            float dB = micInput.GetDecibels();
            int x = gridPos.x;
            int y = gridPos.y;

            // Averaging multiple samples
            gridValues[x, y] += dB;
            sampleCounts[x, y]++;
        }

        saveTimer += Time.deltaTime;
        if (saveTimer >= saveInterval)
        {
            saveTimer = 0f;
            SaveGridToCSV();
        }
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - origin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.z - origin.y) / cellSize); // z for 3D forward
        return new Vector2Int(x, y);
    }

    bool IsValidGridPos(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < gridSize.x && pos.y < gridSize.y;
    }

    public void SaveGridToCSV()
    {
        string path = Path.Combine(Application.dataPath, "MicGrid.csv");
        StringBuilder sb = new StringBuilder();

        for (int y = gridSize.y - 1; y >= 0; y--) // optional: flip vertically for spatial intuition
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                if (sampleCounts[x, y] > 0)
                    sb.Append((gridValues[x, y] / sampleCounts[x, y]).ToString("F2"));
                else
                    sb.Append("0");

                if (x < gridSize.x - 1)
                    sb.Append(",");
            }
            sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString());
        Debug.Log($"Grid saved to: {path}");
    }

    void OnApplicationQuit()
    {
        //SaveGridToCSV();
    }
}
