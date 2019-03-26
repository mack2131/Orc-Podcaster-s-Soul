using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class WaterPlaneGenerator : MonoBehaviour {

    public float size = 1;
    public int gridSize = 16;

    private MeshFilter filter;

	// Use this for initialization
	void Start () 
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    Mesh GenerateMesh()
    {
        Mesh m = new Mesh();

        List<Vector3> verticies = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < gridSize + 1; x++)
        {
            for (int y = 0; y < gridSize + 1; y++)
            {
                verticies.Add(new Vector3(-size * 0.5f + size * (x / ((float)gridSize)), 0, -size * 0.5f + size * (y / ((float)gridSize))));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x / (float)gridSize, y / (float)gridSize));
            }
        }

        List<int> triangles = new List<int>();
        int vertCount = gridSize + 1;
        for (int i = 0; i < vertCount * vertCount - vertCount; i++)
        {
            if ((i + 1) % vertCount == 0)
            {
                continue;
            }
            triangles.AddRange(new List<int>(){
                i + 1 + vertCount, i + vertCount, i,
                i, i + 1, i + vertCount + 1});
        }
        m.SetVertices(verticies);
        m.SetNormals(normals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0);
        return m;
    }
}
