using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Plane : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    private List<Material> materialList;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MeshData();
        UpdateMesh();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        AddMaterials();
        meshRenderer.materials = materialList.ToArray();
    }


    void MeshData()
    {
        //array of vertices
        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 4), new Vector3(4, 0, 0), new Vector3(4, 0, 4) };
        //array of integers
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private void AddMaterials()
    {
        //method used to add the material
        Material blueMat = new Material(Shader.Find("Specular"));
        blueMat.color = Color.blue;

        materialList = new List<Material>();
        materialList.Add(blueMat);
    }
}
