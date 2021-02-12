﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRender : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    public float scale = 1f;
    float adjScale;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
    }
    void Start()
    {
        GenerateVoxelMesh(new VoxelData());
        UpdateMesh();
    }

    void GenerateVoxelMesh(VoxelData data)
    {
        for (int z = 0; z < data.Depth; z++)
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();

            for (int x = 0; x < data.Width; x++)
            {
                if(data.GetCell(x,z) == 0)
                {
                    continue;
                }
                MakeCube(adjScale, new Vector3((float)x * scale, 0, (float)z * scale));   
            }
        }
    }

    void MakeCube(float cubeScale, Vector3 cubePos)
    {
        

        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }

    }

    void MakeFace(int dir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(Cube.faceVertices(dir, faceScale, facePos));
        int vCount = vertices.Count;

        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}