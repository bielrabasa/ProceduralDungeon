using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] int wallHeight = 3;

    Mesh mesh;
    Vector3[] ver;
    int[] tri;
    Vector2[] uv;

    CellularAutomata ca;

    void Start()
    {
        ca = GetComponent<CellularAutomata>();

        //Attach mesh to mesh filter
        mesh = new Mesh();
        mesh.name = "MeshFromPixels";
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        if(ca.isFinished)
        {
            GenerateMesh();
            ca.isFinished = false;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = ver;
        mesh.triangles = tri;
        mesh.uv = uv;
        //mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void GenerateMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < ca.width; x++)
        {
            for (int z = 0; z < ca.height; z++)
            {
                if (ca.map[x, z])
                {
                    // Check 4 directions
                    if (IsSpace(x - 1, z)) AddFace(ref vertices, ref triangles, ref uvs, x, z, Vector3.left);
                    if (IsSpace(x + 1, z)) AddFace(ref vertices, ref triangles, ref uvs, x, z, Vector3.right);
                    if (IsSpace(x, z - 1)) AddFace(ref vertices, ref triangles, ref uvs, x, z, Vector3.back);
                    if (IsSpace(x, z + 1)) AddFace(ref vertices, ref triangles, ref uvs, x, z, Vector3.forward);
                }
            }
        }

        ver = vertices.ToArray();
        tri = triangles.ToArray();
        uv = uvs.ToArray();

        UpdateMesh();
    }

    bool IsSpace(int x, int z)
    {
        if (x < 0 || x >= ca.width || z < 0 || z >= ca.height)
        {
            return true; //out of map
        }
        return !ca.map[x, z];
    }

    void AddFace(ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, int x, int z, Vector3 direction)
    {
        Vector3 v0, v1, v2, v3;

        if (direction == Vector3.left)
        {
            v0 = new Vector3(x, 0, z);
            v1 = new Vector3(x, wallHeight, z);
            v2 = new Vector3(x, wallHeight, z + 1);
            v3 = new Vector3(x, 0, z + 1);
        }
        else if (direction == Vector3.right)
        {
            v0 = new Vector3(x + 1, 0, z);
            v1 = new Vector3(x + 1, wallHeight, z);
            v2 = new Vector3(x + 1, wallHeight, z + 1);
            v3 = new Vector3(x + 1, 0, z + 1);
        }
        else if (direction == Vector3.back)
        {
            v0 = new Vector3(x, 0, z);
            v1 = new Vector3(x + 1, 0, z);
            v2 = new Vector3(x + 1, wallHeight, z);
            v3 = new Vector3(x, wallHeight, z);
        }
        else // Vector3.forward
        {
            v0 = new Vector3(x, 0, z + 1);
            v1 = new Vector3(x + 1, 0, z + 1);
            v2 = new Vector3(x + 1, wallHeight, z + 1);
            v3 = new Vector3(x, wallHeight, z + 1);
        }

        int vertIndex = vertices.Count;

        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        if (direction == Vector3.left || direction == Vector3.back)
        {
            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 1);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 3);
            triangles.Add(vertIndex + 2);
        }
        else
        {
            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 2);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 3);
        }

        if (direction == Vector3.left || direction == Vector3.right)
        {
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }
        else
        {
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
        }
    }
}

