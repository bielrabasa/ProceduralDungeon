using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    [Range(0, 100)]
    public int fillPercentage = 50;

    public int width = 100;
    public int height = 60;

    public bool[,] map;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) IterateMapOnce();
    }

    //------------GENERATE-------------
    public void GenerateFullMap(Vector2Int doors)
    {
        GenerateRandom(fillPercentage);

        for (int i = 0; i < 15; i++)
        {
            IterateMapOnce();
        }

        if (!AfterAnalyse())
        {
            GenerateFullMap(doors); //Generate again in case of error
            return;
        }

        CreateDoors(doors);
    }

    bool AfterAnalyse()
    {
        return AnalyseMap.GetFixedMap(ref map, width, height);
    }

    void CreateDoors(Vector2Int doors)
    {
        //Carve doors
        AnalyseMap.CreateAllDoors(ref map, width, height, doors);
        
        //Smooth
        for (int i = 0; i < 5; i++)
        {
            IterateMapOnce();
        }

        //Carve again
        AnalyseMap.CreateAllDoors(ref map, width, height, doors);
    }


    //------------TOOLS-------------

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
                    map[x,z] = Random.Range(0f, 100f) <= fillPer;
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
                if(nb > 4) newMap[x,z] = true;
                else if(nb < 4) newMap[x,z] = false;
                else newMap[x,z] = map[x,z];
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

    //------------DRAW-------------
    /*private void OnDrawGizmos()
    {
        if(map == null) return;

        for(int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Gizmos.color = map[x, z]? Color.black: Color.white;
                Gizmos.DrawCube(new Vector3(x + transform.position.x, 0, z + transform.position.z), Vector3.one * .5f);
            }
        }
    }*/
}
