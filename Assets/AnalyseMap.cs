using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class AnalyseMap
{
    public Vector2Int ini;

    public bool GetAll(ref bool[,] map, int w, int h)
    {
        if (!GetEmptyCenterPoint(ref map, w, h)) return false;
        return GetOpenSpace(ref map, w, h);
    }

    public bool GetEmptyCenterPoint(ref bool[,] map, int w, int h)
    {
        int x = w / 3;
        int z = h / 2;

        while (map[x, z])
        {
            x++;
            if (x == w) return false; //Couldn't find open space in center line
        }

        ini = new Vector2Int(x, z);
        return true;
    }

    public bool GetOpenSpace(ref bool[,] map, int w, int h)
    {
        List<Vector2Int> visited = new List<Vector2Int>();
        Stack<Vector2Int> frontier = new Stack<Vector2Int>();

        frontier.Push(ini);
        while(frontier.Count > 0)
        {
            Vector2Int curr = frontier.Pop(); //Get new tile to expand

            Vector2Int[] nb = new Vector2Int[4];
            nb[0] = curr + Vector2Int.left;
            nb[1] = curr + Vector2Int.right;
            nb[2] = curr + Vector2Int.up;
            nb[3] = curr + Vector2Int.down;

            //Check frontier surrounding tiles to visit
            foreach (Vector2Int v in nb)
            {
                if (!map[v.x, v.y] && !visited.Contains(v)) //is open space & non visited
                {
                    frontier.Push(v);
                    visited.Add(v);
                }
            }
        }

        //If area too small, repeat
        if (visited.Count < (w * h) / 3) return false;

        //Set newMap to black
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                map[x, y] = true;
            }
        }

        //Fill empty space in white
        foreach(Vector2Int p in visited)
        {
            map[p.x, p.y] = false;
        }

        return true;
    }
}


