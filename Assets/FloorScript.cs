using System.Collections.Generic;
using UnityEngine;

public static class FloorScript
{
    static int width = 110;
    static int depth = 70;

    static Mesh toInstance = null;

    public static Mesh GetFloorInstance()
    {
        if (toInstance != null) return toInstance;

        //Create Mesh & Object
        toInstance = new Mesh();
        toInstance.name = "FloorMesh";

        //Create Structure
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        vertices.Add(new Vector3(-width / 2, 0, -depth / 2));
        vertices.Add(new Vector3(-width / 2, 0, depth / 2));
        vertices.Add(new Vector3(width / 2, 0, depth / 2));
        vertices.Add(new Vector3(width / 2, 0, -depth / 2));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, depth));
        uvs.Add(new Vector2(width, depth));
        uvs.Add(new Vector2(width, 0));

        //Update Mesh
        toInstance.Clear();
        toInstance.vertices = vertices.ToArray();
        toInstance.triangles = triangles.ToArray();
        toInstance.uv = uvs.ToArray();
        toInstance.RecalculateNormals();

        return toInstance;
    }
}
