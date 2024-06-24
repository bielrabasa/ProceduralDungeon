using System.Collections.Generic;
using UnityEngine;

public static class CorridorScript
{
    static int length = 10;
    static int width = 4;
    static int height = 3;

    static Mesh toInstance = null;

    public static Mesh GetCorridorInstance()
    {
        if (toInstance != null) return toInstance;

        //Create Mesh & Object
        toInstance = new Mesh();
        toInstance.name = "CorridorMesh";

        //Create Structure
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        //Left face
        vertices.Add(new Vector3(-length / 2, 0, -width / 2));
        vertices.Add(new Vector3(length / 2, 0, -width / 2));
        vertices.Add(new Vector3(-length / 2, height, -width / 2));
        vertices.Add(new Vector3(length / 2, height, -width / 2));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(3);

        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(2);

        //Right face
        vertices.Add(new Vector3(-length / 2, 0, width / 2));
        vertices.Add(new Vector3(length / 2, 0, width / 2));
        vertices.Add(new Vector3(-length / 2, height, width / 2));
        vertices.Add(new Vector3(length / 2, height, width / 2));

        triangles.Add(4);
        triangles.Add(7);
        triangles.Add(5);

        triangles.Add(4);
        triangles.Add(6);
        triangles.Add(7);

        //UVs for 2 faces
        for (int i = 0; i < 2; i++)
        {
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(length, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(length, 1));
        }

        //Update Mesh
        toInstance.Clear();
        toInstance.vertices = vertices.ToArray();
        toInstance.triangles = triangles.ToArray();
        toInstance.uv = uvs.ToArray();
        toInstance.RecalculateNormals();

        return toInstance;
    }
}
