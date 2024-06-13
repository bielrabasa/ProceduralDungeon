using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
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

        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Generate();
        }
    }

    void Generate()
    {
        for (int x = 0; x < grid.x; x++)
        {
            for(int y = 0; y < grid.y; y++)
            {
                GameObject instance = Instantiate(map, new Vector3(x * (w + separation), 0, y * (h + separation)), Quaternion.identity, transform);
                instance.GetComponent<CellularAutomata>().GenerateFullMap();
                instance.AddComponent<MeshGenerator>();
            }
        }
    }
}
