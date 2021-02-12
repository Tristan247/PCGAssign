using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Triangle : MonoBehaviour
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
        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };
        //array of integers
        //3 points which create the triangle
        triangles = new int[] { 0, 1, 2};
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private void AddMaterials()
    {
        //method which adds materials
        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        materialList = new List<Material>();
        materialList.Add(redMat);
    }
}
