using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    //grid settings
    public float cellSize = 1;
    public Vector3 gridOffset;
    public int GridSize;

    //Circle
    public float radius;
    public int amountOfRings;
    public int quadsPerRing;
    public int height;
    public float slope;
    float[] radiuses;
    public float HoleRadius;

    //Use this for initialization
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    //Start is called before the first frame update
    void Start()
    {

        CalculateRadiuses();
        MakeDiscreteProceduralGrid();
        UpdateMesh();
    }

    
    private void MakeDiscreteProceduralGrid()
    {
        //set array sizes
        vertices = new Vector3[amountOfRings * quadsPerRing * 4];
        triangles = new int[amountOfRings * quadsPerRing * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset
        float vertexOffset = cellSize * 0.5f;



        //set ring numbers
        float PiSlice = (2 * Mathf.PI) / quadsPerRing;





        for (int x = 0; x < amountOfRings; x++)
        {
            for(int y = 0; y < quadsPerRing; y++)
            {
                Vector3 CellOffset = new Vector3(x*cellSize, 0, y*cellSize);

                if(x%2 == 0)
                {
                    vertices[v]     = new Vector3(Mathf.Sin( PiSlice * y) * radiuses[x], x * height, Mathf.Cos(PiSlice * y) * radiuses[x]);
                    vertices[v + 2] = new Vector3(Mathf.Sin((PiSlice * y) + (PiSlice * 0.5f)) * radiuses[x+1], (x + 1) * height, Mathf.Cos((PiSlice * y) + (PiSlice * 0.5f)) * radiuses[x+1]);
                    vertices[v + 1] = new Vector3(Mathf.Sin( PiSlice * (y + 1)) * radiuses[x], x * height, Mathf.Cos(PiSlice * (y + 1)) * radiuses[x]);
                    vertices[v + 3] = new Vector3(Mathf.Sin((PiSlice * (y + 1)) + (PiSlice * 0.5f)) * radiuses[x+1], (x + 1) * height, Mathf.Cos((PiSlice * (y + 1)) + (PiSlice * 0.5f)) * radiuses[x+1]);
                }
                else
                {
                    vertices[v]     = new Vector3(Mathf.Sin((PiSlice * y) + (PiSlice * 0.5f))* radiuses[x], x * height, Mathf.Cos(PiSlice * y + (PiSlice * 0.5f)) * radiuses[x]);
                    vertices[v + 2] = new Vector3(Mathf.Sin((PiSlice * y) + (PiSlice)) * radiuses[x+1], (x + 1) * height, Mathf.Cos((PiSlice * y) + (PiSlice)) * radiuses[x+1]);
                    vertices[v + 1] = new Vector3(Mathf.Sin( PiSlice * (y + 1) + (PiSlice * 0.5f)) * radiuses[x], x * height, Mathf.Cos(PiSlice * (y + 1) + (PiSlice * 0.5f)) * radiuses[x]);
                    vertices[v + 3] = new Vector3(Mathf.Sin((PiSlice * (y + 1)) + (PiSlice)) * radiuses[x+1], (x + 1) * height, Mathf.Cos((PiSlice * (y + 1)) + (PiSlice)) * radiuses[x+1]);
                }
                

                triangles[t] = v;
                triangles[t + 1] = v+1;
                triangles[t + 2] = v+2;
                triangles[t + 3] = v + 2;
                triangles[t + 4] = v + 1;
                triangles[t + 5] = v+3;

                v += 4;
                t += 6;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void CalculateRadiuses()
    {
        radiuses = new float[amountOfRings+2];

        for(int i = 0; i < amountOfRings+2; i++)
        {
            radiuses[amountOfRings+1 - i] = (i * slope)*(i * slope);
        }
    }
}
