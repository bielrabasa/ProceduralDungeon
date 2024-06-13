using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] int wallHeight = 3;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    CellularAutomata ca;

    void Start()
    {
        ca = GetComponent<CellularAutomata>();

        //Attach mesh to mesh filter
        gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();
        mesh.name = "MeshFromPixels";
        gameObject.AddComponent<MeshFilter>().mesh = mesh;

        
        GenerateMesh();
        UpdateMesh();
    }
    
    /*void CreateGrid()
    {
        //Vertices in the grid
        vertices = new Vector3[(width + 1) * (depth + 1)];

        for (int z = 0, i = 0; z <= depth; z++)
            for (int x = 0; x <= width; x++, i++)
            {
                float height = GetHeight(x + xOffset * width, z + zOffset * depth, 20f, 3, 2, .5f) * terrainHeight;
                vertices[i] = new Vector3(x, height, z);
            }

        //Create all triangles
        triangles = new int[6 * width * depth];

        int currentVertice = 0;
        int numOfTriangles = 0;

        for (int z = 0; z < depth; z++, currentVertice++)
            for (int x = 0; x < width; x++)
            {
                triangles[numOfTriangles + 0] = currentVertice + 0;
                triangles[numOfTriangles + 1] = currentVertice + width + 1;
                triangles[numOfTriangles + 2] = currentVertice + 1;
                triangles[numOfTriangles + 3] = currentVertice + 1;
                triangles[numOfTriangles + 4] = currentVertice + width + 1;
                triangles[numOfTriangles + 5] = currentVertice + width + 2;

                numOfTriangles += 6;
                currentVertice++;
            }

        //Set Vertex Color (instead of UVs <- only for texture)
        colors = new Color[vertices.Length];
        for (int z = 0, i = 0; z <= depth; z++)
            for (int x = 0; x <= width; x++, i++)
                colors[i] = terrainColorGradient.Evaluate(vertices[i].y / terrainHeight);
    }*/

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uv;
        //mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void GenerateMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int x = 0; x < ca.width; x++)
        {
            for (int z = 0; z < ca.height; z++)
            {
                if (ca.map[x, z])
                {
                    // Add vertices for a cube face
                    AddCubeFace(vertices, triangles, x, z);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void AddCubeFace(List<Vector3> vertices, List<int> triangles, int x, int z)
    {
        // Cube face vertices
        Vector3 v0 = new Vector3(x, 0, z);
        Vector3 v1 = new Vector3(x + 1, 0, z);
        Vector3 v2 = new Vector3(x + 1, 1, z);
        Vector3 v3 = new Vector3(x, 1, z);

        int vertIndex = vertices.Count;

        // Add vertices
        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        // Add triangles (two for a quad)
        triangles.Add(vertIndex);
        triangles.Add(vertIndex + 2);
        triangles.Add(vertIndex + 1);

        triangles.Add(vertIndex);
        triangles.Add(vertIndex + 3);
        triangles.Add(vertIndex + 2);
    }

}

