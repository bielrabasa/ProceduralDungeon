using UnityEditor.VersionControl;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    int w, h;

    public Vector2Int grid;
    public int separation;

    public Material wallMat;

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
                CreateCorridor(x, y, false);
            }
        }
    }

    void GenerateChunk(int gridX, int gridY, Vector2Int doors)
    {
        GameObject instance = Instantiate(map, new Vector3(gridX * (w + separation), 0, gridY * (h + separation)), Quaternion.identity, transform);
        instance.GetComponent<CellularAutomata>().GenerateFullMap(doors);
        instance.AddComponent<MeshGenerator>();
    }

    void CreateCorridor(int x, int y, bool vertical)
    {
        GameObject corridor = new GameObject("Corridor");
        corridor.AddComponent<MeshRenderer>().material = wallMat;

        corridor.AddComponent<MeshFilter>().mesh = CorridorScript.GetCorridorInstance();

        Vector3 pos = new Vector3(++x * (w + separation), 0, ++y * (h + separation));
        if (vertical)
        {
            pos.z -= separation / 2;
            pos.x -= (w / 2f + separation);
            corridor.transform.Rotate(new Vector3(0, 90, 0));
        }
        else
        {
            pos.x -= separation / 2;
            pos.z -= (h / 2f + separation);
        }

        corridor.transform.position = pos; 
        corridor.transform.parent = transform;
    }
}
