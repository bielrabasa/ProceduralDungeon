using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    public int roomNumber = 10;
    List<Vector2Int> roomPlaces = new List<Vector2Int>();
    int w, h;

    public int separation;

    public Material wallMat;
    public Material floorMat;

    void Start()
    {
        CellularAutomata ca = map.GetComponent<CellularAutomata>();
        w = ca.width;
        h = ca.height;

        GenerateFloor();
    }

    void GenerateFloor()
    {
        roomPlaces.Clear();

        int x = 0, y = 0;
        int lastRoomPos = -1;

        for (int i = 0; i < roomNumber; i++)
        {
            int nextRoomPos = 0;
            int nowX = x, nowY = y;

            Vector2Int doors = new Vector2Int(0, 0);

            if (i != roomNumber - 1)
            {
                bool roomFound = false;
                while (!roomFound)
                {
                    nextRoomPos = Random.Range(0, 4); //Generate Random position

                    //Recheck nextRoomPos depending on if it a possible place
                    Vector2Int roomToCheck = new Vector2Int(nowX, nowY);
                    switch (nextRoomPos)
                    {
                        case 0: //TOP
                            roomToCheck.y++;
                            break;
                        case 1: //RIGHT
                            roomToCheck.x++;
                            break;
                        case 2: //BOTTOM
                            roomToCheck.y--;
                            break;
                        case 3: //LEFT
                            roomToCheck.x--;
                            break;
                    }

                    //TEST
                    if (IsFree(roomToCheck) && AdjacentSpaces(roomToCheck) >= 3) roomFound = true;
                }

                //Check for doors to next room & create corridor
                switch (nextRoomPos)
                {
                    case 0: //TOP
                        doors.y = 1;
                        y++;
                        CreateCorridor(nowX, nowY, true);
                        break;
                    case 1: //RIGHT
                        doors.x = 1;
                        x++;
                        CreateCorridor(nowX, nowY, false);
                        break;
                    case 2: //BOTTOM
                        doors.y = -1;
                        y--;
                        CreateCorridor(nowX, nowY - 1, true);
                        break;
                    case 3: //LEFT
                        doors.x = -1;
                        x--;
                        CreateCorridor(nowX - 1, nowY, false);
                        break;
                }
            }

            //Check for doors to last room
            switch (lastRoomPos)
            {
                case 0: //TOP
                    if (doors.y == 0) doors.y = 1;
                    else doors.y = 2;
                    break;
                case 1: //RIGHT
                    if (doors.x == 0) doors.x = 1;
                    else doors.x = 2;
                    break;
                case 2: //BOTTOM
                    if (doors.y == 0) doors.y = -1;
                    else doors.y = 2;
                    break;
                case 3: //LEFT
                    if (doors.x == 0) doors.x = -1;
                    else doors.x = 2;
                    break;
            }

            lastRoomPos = nextRoomPos + 2;
            if (lastRoomPos >= 4) lastRoomPos -= 4;

            GenerateChunk(nowX, nowY, doors);
        }
    }

    int AdjacentSpaces(Vector2Int pos)
    {
        int n = 0;
        n += IsFree(new Vector2Int(pos.x, pos.y + 1))? 1 : 0;
        n += IsFree(new Vector2Int(pos.x + 1, pos.y))? 1 : 0;
        n += IsFree(new Vector2Int(pos.x, pos.y - 1))? 1 : 0;
        n += IsFree(new Vector2Int(pos.x - 1, pos.y))? 1 : 0;
        return n;
    }

    bool IsFree(Vector2Int pos)
    {
        return !roomPlaces.Contains(pos);
    }

    //GENERATE
    void GenerateChunk(int gridX, int gridY, Vector2Int doors)
    {
        roomPlaces.Add(new Vector2Int(gridX, gridY));

        GameObject instance = Instantiate(map, new Vector3(gridX * (w + separation), 0, gridY * (h + separation)), Quaternion.identity, transform);
        instance.GetComponent<CellularAutomata>().GenerateFullMap(doors);
        instance.AddComponent<MeshGenerator>();
        instance.GetComponent<Renderer>().material = wallMat;

        CreateFloor(gridX, gridY);
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

    void CreateFloor(int x, int y)
    {
        GameObject floor = new GameObject("Floor");
        floor.AddComponent<MeshRenderer>().material = wallMat;
        floor.AddComponent<MeshFilter>().mesh = FloorScript.GetFloorInstance();
        floor.GetComponent<Renderer>().material = floorMat;

        floor.transform.position = new Vector3((x + 0.5f) * (w + separation), 0, (y + 0.5f) * (h + separation));
        floor.transform.parent = transform;
    }
}
