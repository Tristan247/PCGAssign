using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    public float scale = 1f;
    public int posX;
    public int posY;
    public int posZ;
    float adjScale;
    private List<Material> materialList;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
    }
    void Start()
    {
        MakeCube(adjScale, new Vector3((float)posX * scale, (float)posY * scale, (float)posZ * scale));
        UpdateMesh();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        AddMaterials();
        meshRenderer.materials = materialList.ToArray();
    }

    public void MakeCube(float cubeScale, Vector3 cubePos)
    {
        //gets the faces and uses them to build the cube
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for(int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }
            
    }

    void MakeFace(int dir, float faceScale, Vector3 facePos)
    {
        //generates the cubes faces
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

    private void AddMaterials()
    {
        //method which adds the materials
        Material greenMat = new Material(Shader.Find("Specular"));
        greenMat.color = Color.green;

        materialList = new List<Material>();
        materialList.Add(greenMat);
    }
}
