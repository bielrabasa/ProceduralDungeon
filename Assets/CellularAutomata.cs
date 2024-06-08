using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    [Range(0, 100)]
    public int fillPercentage = 50;

    public const int width = 100;
    public const int height = 60;

    bool[,] map;

    private void Start()
    {
        GenerateRandom(fillPercentage);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) GenerateRandom(fillPercentage);
        if(Input.GetKeyDown(KeyCode.Return)) IterateMapOnce();
    }

    void GenerateRandom(float fillPer)
    {
        map = new bool[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Outer layer is death
                if(x == 0 || z == 0 || x == width-1 || z == height - 1)
                {
                    map[x,z] = true;
                }
                else
                {
                    map[x,z] = Random.Range(0f, 100f / fillPer) <= 1f;
                }
            }
        }
    }

    void IterateMapOnce()
    {
        bool[,] newMap = new bool[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Outer layer is death
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                {
                    newMap[x, z] = true;
                    continue;
                }

                int nb = numNeighbours(x,z);
                //TODO: Tweak Rules
                //Death -> Live when exactly 3 neighbours
                if (!map[x, z] && nb == 3) newMap[x, z] = true;

                //Live -> Death when less than 2 or more than 4 neighbours
                else if(map[x, z] && (nb < 2 || nb > 4)) newMap[x, z] = false;
            }
        }

        map = newMap;
    }

    int numNeighbours(int posX, int posZ)
    {
        int n = 0;

        for(int x = posX - 1; x <= posX + 1; x++) 
        { 
            for(int z = posZ - 1; z <= posZ + 1; z++)
            {
                if (x == posX && z == posZ) continue;

                n += map[x, z]? 1 : 0;
            }
        }

        return n;
    }

    private void OnDrawGizmos()
    {
        //TODO: draw smth
        for(int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Gizmos.color = map[x, z]? Color.black: Color.white;
                Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
            }
        }
    }
}
