using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    int w, h;

    public Vector2Int grid;


    void Start()
    {
        CellularAutomata ca = map.GetComponent<CellularAutomata>();
        w = ca.width; 
        h = ca.height;

        Generate();
    }

    void Generate()
    {
        for (int x = 0; x < grid.x; x++)
        {
            for(int y = 0; y < grid.y; y++)
            {
                Instantiate(map, new Vector3(x * w, 0, y * h), Quaternion.identity, transform).GetComponent<CellularAutomata>().GenerateFullMap();
            }
        }
    }
}
