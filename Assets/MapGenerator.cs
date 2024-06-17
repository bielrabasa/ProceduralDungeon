using UnityEditor.VersionControl;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    public int roomNumber = 10;
    int w, h;

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
        //TODO: DO NOT generate room where one already is
        int x = 0, y = 0;
        int lastRoomPos = -1;

        for (int i = 0; i < roomNumber; i++)
        {
            int nextRoomPos = Random.Range(0, 4);
            int nowX = x, nowY = y;

            Vector2Int doors = new Vector2Int(0, 0);
            

            //Check for doors to next room & create corridor
            if(i != roomNumber - 1)
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
