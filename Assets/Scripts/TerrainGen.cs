using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGen : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    [SerializeField]
    public float bumpHeight = 5f;
    public float bumpy = 5f;
    public int xSize = 20; //size of the x scale
    public int zSize = 20; //size of the z scale

    public bool test;
    public bool spawnWater;
    public GameObject water;
    public GameObject tree;

    public float waterHeight = 15f;

    public bool fluid;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();

        if (spawnWater)
        {
            AddWater();
        }
    }

    private void Update()
    {
        if(fluid)
        {
            CreateShape();
            UpdateMesh();
        }    
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //perlin noise is used to create the mountain like terrain
                float y = Mathf.PerlinNoise(x * bumpy * .3f, z * bumpy * .3f) * bumpHeight;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tri = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + xSize + 1;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + xSize + 1;
                triangles[tri + 5] = vert + xSize + 2;

                vert++;
                tri += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    //onValidate is used to keep the terrain visible in the editor
    private void OnValidate()
    {
        Start();
    }


    void AddWater()
    {
        //water plane is generated
        GameObject waterGameObject = GameObject.Find("water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(0, waterHeight, 0);
    }
}
