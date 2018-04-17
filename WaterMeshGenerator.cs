using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WaterMeshGenerator : MonoBehaviour {

    public float waterWidth = 10;
    public float waterHeight = 4;
    public int waterDivision = 100;


    public float timeToMaxWaveHeight = 2; 

    public float waveHeight = 1;
    public float wavePercentage = 0;

    public List<Vector3> newVertices = new List<Vector3>();
    public List<int> newTriangles = new List<int>();
    public List<Vector2> newUV = new List<Vector2>();
    private Mesh mesh;



    public float waveOffset;
    /*static*/
    float waterFragmentWidth;
    float timeFromStart;


    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        waterFragmentWidth = waterWidth / waterDivision;
        Init();
    }
    // Use this for initialization
    void Init()
    {
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
        /*
        1-----2
        | \   |
        |   \ |
        4-----3*/
        float x = transform.position.x - waterWidth/2;
        float y = transform.position.y;
        float z = transform.position.z;

        float xUvOffset = (1f / waterDivision);

        for(int i=0; i<waterDivision; i++)
        {
            newVertices.Add(new Vector3(x, y + CalculateHeight(i), z));
            newVertices.Add(new Vector3(x + waterFragmentWidth, y + CalculateHeight(i+1), z));
            newVertices.Add(new Vector3(x + waterFragmentWidth, y - waterHeight, z));
            newVertices.Add(new Vector3(x, y - waterHeight, z));

            int tOffset = i * 4; 
            newTriangles.Add(0 + tOffset);
            newTriangles.Add(1 + tOffset);
            newTriangles.Add(3 + tOffset);
            newTriangles.Add(1 + tOffset);
            newTriangles.Add(2 + tOffset);
            newTriangles.Add(3 + tOffset);

            //float xUv = i / waterDivision;

            newUV.Add(new Vector2(xUvOffset * i, 1));
            newUV.Add(new Vector2(xUvOffset * i + xUvOffset, 1));
            newUV.Add(new Vector2(xUvOffset * i + xUvOffset, 0));
            newUV.Add(new Vector2(xUvOffset * i, 0));
            
            x += waterFragmentWidth;
        }
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.RecalculateNormals();
    }

    float CalculateHeight(float i)
    {
        float y=0;
        float width = ((i / waterDivision)*2 - 1f - waveOffset) * Mathf.Lerp(4f, 1f,wavePercentage);
        //width -= Mathf.Lerp(5f, 2f, wavePercentage) /4 ;
        y = WaveFunction(width);
        //Debug.Log(y);
        return y * waveHeight * wavePercentage;
    }
    float WaveFunction(float t)
    {
        t = Mathf.Clamp(t,-1,1);

        if(t<=0)
            return (Mathf.Cos(t* Mathf.PI)/2 + 0.5f);
        else
            return (Mathf.Cos(Mathf.Pow(t,Mathf.Lerp(0.5f,0.3f,wavePercentage)) * Mathf.PI) / 2 + 0.5f);
    }
    void UpdateMesh()
    {
        //mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update () {
        //UpdateMesh();
        timeFromStart += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        if (Input.GetKey(KeyCode.Space))
        {
            wavePercentage = Mathf.Clamp01(wavePercentage + Time.deltaTime/timeToMaxWaveHeight);
        }
        else
        {
            wavePercentage /= 1.1f;
        }

        Init();
    }
}
