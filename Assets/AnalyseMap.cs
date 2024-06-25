using System.Collections.Generic;
using UnityEngine;

public static class AnalyseMap
{
    public static Vector2Int playerPos = Vector2Int.zero;

    public static void GetFixedMap(ref bool[,] map, int w, int h, ref bool isAnalised, ref bool analiseFailed)
    {
        Vector2Int ini = Vector2Int.zero;
        if (!GetEmptyCenterPoint(ref map, w, h, ref ini)) analiseFailed = true;
        else if (!GetOpenSpace(ref map, w, h, ini)) analiseFailed = true;

        //Set playerPos
        if (!analiseFailed && playerPos == Vector2Int.zero)
        {
            playerPos = ini;
            Debug.Log(playerPos);
        }

        isAnalised = true;
    }

    public static bool GetEmptyCenterPoint(ref bool[,] map, int w, int h, ref Vector2Int ini)
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

    public static bool GetOpenSpace(ref bool[,] map, int w, int h, Vector2Int ini)
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

    public static void CreateAllDoors(ref bool[,] map, int w, int h, Vector2Int doors)
    {
        if (doors.y == 2 || doors.y == 1 ) CreateDoor(ref map, w, h, Vector2Int.up);
        if (doors.y == 2 || doors.y == -1) CreateDoor(ref map, w, h, Vector2Int.down);

        if (doors.x == 2 || doors.x == 1 ) CreateDoor(ref map, w, h, Vector2Int.right);
        if (doors.x == 2 || doors.x == -1) CreateDoor(ref map, w, h, Vector2Int.left);
    }

    public static void CreateDoor(ref bool[,] map, int w, int h, Vector2Int doorPos)
    {
        Vector2Int sPos = Vector2Int.zero;

        if (doorPos.x == 0) sPos.x = w / 2 - 2; //door in the top or bottom
        if (doorPos.y == 0) sPos.y = h / 2 - 2; //door in the left or right

        if (doorPos.x == 1) sPos.x = w - 1;
        if (doorPos.x == -1) sPos.x = 0;

        if (doorPos.y == 1) sPos.y = h - 1;
        if (doorPos.y == -1) sPos.y = 0;

        for (int i = 0; i < 4; i++) //How wide is the door
        {
            Vector2Int curr = sPos;
            curr.x += (doorPos.x == 0) ? i : 0; //if doors are top or bottom
            curr.y += (doorPos.y == 0) ? i : 0; //if doors are left or right
            
            while (map[curr.x, curr.y]) //while there are walls in the position looking break them
            {
                map[curr.x, curr.y] = false;
                curr -= doorPos;
            }

        }
    }
}


