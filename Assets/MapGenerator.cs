using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    int w, h;

    public Vector2Int grid;
    public int separation;


    void Start()
    {
        CellularAutomata ca = map.GetComponent<CellularAutomata>();
        w = ca.width; 
        h = ca.height;

        GenerateFloor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            GenerateFloor();
        }
    }

    void GenerateFloor()
    {
        for (int x = 0; x < grid.x; x++)
        {
            for(int y = 0; y < grid.y; y++)
            {
                GenerateChunk(x, y, new Vector2Int(2, 2));
            }
        }
    }

    void GenerateChunk(int gridX, int gridY, Vector2Int doors)
    {
        GameObject instance = Instantiate(map, new Vector3(gridX * (w + separation), 0, gridY * (h + separation)), Quaternion.identity, transform);
        instance.GetComponent<CellularAutomata>().GenerateFullMap(doors);
        instance.AddComponent<MeshGenerator>();
    }
}
