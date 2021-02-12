﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Road : MonoBehaviour
{
    [SerializeField]
    private float radius = 30f; // this defines the radius of the path

    [SerializeField]
    private float segments = 300f;

    [SerializeField]
    private float lineWidth = 0.3f; // middle white line road marker

    [SerializeField]
    private float roadWidth = 8f; // width of the road on each side of the line

    [SerializeField]
    private float edgeWidth = 1f; // width of our road barrier at the edge of our road

    [SerializeField]
    private float edgeHeight = 1f;

    [SerializeField]
    private int submeshSize = 6;

    [SerializeField]
    private float wavyness = 5f;

    [SerializeField]
    private float waveScale = 0.1f;

    [SerializeField]
    private Vector2 waveOffset;

    [SerializeField]
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    [SerializeField]
    private bool stripeCheck = true;

    [SerializeField]
    private GameObject car, wall, trigger;

    private List<Material> materialList;

    void Start()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        MeshBuilder meshBuilder = new MeshBuilder(submeshSize);

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        AddMaterials();
        meshRenderer.materials = materialList.ToArray();

        //Race track is divided into segments
        //Create the points and store them in a list
        float segmentDegrees = 360f / segments;

        List<Vector3> points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
        }

        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * control;
        }

        //defines the path
        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(meshBuilder, pPrev, pCurr, pNext);
        }

        int randomlocation = (int)Random.Range(2, segments - 1);
        wall.transform.position = points[randomlocation - 1]; //the wall is spawned everytime behind the player which blocks them from reversing into the collider trigger
        wall.transform.LookAt(points[randomlocation]);
        trigger.transform.position = points[randomlocation - 2]; //the trigger is spawned everytime behind the wall which swaps scenes when the player collides with it
        trigger.transform.LookAt(points[randomlocation - 1]);
        car.transform.position = points[randomlocation + 4]; //car is spawned randomly on the track
        car.transform.LookAt(points[randomlocation + 5]);

        meshFilter.mesh = meshBuilder.CreateMesh();

        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }

    //the road sections are created
    //black road on each side
    //white stripe in the middle
    private void ExtrudeRoad(MeshBuilder meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {
        //Road Line
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 0);

        //Road
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 1);

        int stripeSubmesh = 2;

        if (stripeCheck)
        {
            stripeSubmesh = 3;
        }

        stripeCheck = !stripeCheck;

        //Edge
        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Edge Top
        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Edge
        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

    }

    //quads are created
    private void MakeRoadQuad(MeshBuilder meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        //Build Outer Track
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshBuilder.BuildTriangle(topRight, bottomRight, bottomLeft, submesh);

        //Build Inner Track
        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshBuilder.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }

    private void AddMaterials()
    {
        //method which add the materials
        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        materialList = new List<Material>();
        materialList.Add(whiteMat);
        materialList.Add(blackMat);
        materialList.Add(redMat);
        materialList.Add(redMat);
    }

    private void OnValidate()
    {
        Start();
    }
}