
using UnityEngine;
using System.IO;

public class NoiseVisualizer : MonoBehaviour
{
    [Header("Prefab & Data")]
    public GameObject tilePrefab;
    public string fileName = "MicGrid.csv";
    public float cellSize = 1.0f;

    [Header("Value Range & Refresh")]
    public float minValue = 30f;
    public float maxValue = 100f;
    public float refreshInterval = 2f; // seconds

    private float timer = 0f;

    void Start()
    {
        LoadAndDraw();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= refreshInterval)
        {
            timer = 0f;
            RefreshHeatmap();
        }
    }

    // Load data and draw initial heatmap
    void LoadAndDraw()
    {
        string path = Path.Combine(Application.dataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"Noise data file not found at {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        DrawHeatmap(lines);
    }

    // Clear existing tiles and reload
    void RefreshHeatmap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        LoadAndDraw();
    }

    // Draw tiles based on CSV lines
    void DrawHeatmap(string[] lines)
    {
        float offsetZ = (lines.Length - 1) * cellSize / 2f;

        for (int z = 0; z < lines.Length; z++)
        {
            string[] values = lines[z].Split(',');
            float offsetX = (values.Length - 1) * cellSize / 2f;

            for (int x = 0; x < values.Length; x++)
            {
                if (float.TryParse(values[x].Trim(), out float noiseVal))
                {
                    float planeY = transform.position.y; 
                    Vector3 pos = new Vector3(x * cellSize - offsetX, planeY + 0.0001f, (lines.Length - z - 1) * cellSize - offsetZ);
                    GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                    tile.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);
                    if (noiseVal != 0f)
			{
    			    Color col = GetColorForValue(noiseVal, minValue, maxValue);
    			    tile.GetComponent<Renderer>().material.color = col;
			}
                }
            }
        }
    }

    // Map a value to a color gradient from blue to red
    Color GetColorForValue(float value, float minValue, float maxValue)
    {
        float t = Mathf.InverseLerp(minValue, maxValue, value);
        return Color.Lerp(Color.blue, Color.red, t);
    }
}
