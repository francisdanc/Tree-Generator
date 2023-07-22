using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateTree : MonoBehaviour
{
    private int treeHeight;
    private float deviation = 0.3f;
    private int treeSegments;
    private double deviationPoint;
    private float treeRadius;
    public Material material;
    private int deviationCount = 1;
    private bool meshCreated = false;
    

    [SerializeField] private bool autogenerate = false;

    void Start()
    {
        deviationPoint = treeHeight * 0.4f;
        treeSegments = treeHeight;
        GenerateTrunk();
    }

    public void GenerateTrunk() {

        UnityEngine.Random.InitState(GetInstanceID());

        treeHeight = UnityEngine.Random.Range(5, 11);
        treeRadius = UnityEngine.Random.Range(0.5f, 0.8f);
        deviationPoint = treeHeight * 0.4f;
        treeSegments = treeHeight;
        Material[] materials = new Material[treeSegments + 1];
        
        

        float segmentHeight = treeHeight / treeSegments;
        float height = 0;

        //Array of all points for all hexagons
        Vector3 [] points = new Vector3[6 * (treeSegments + 1)];
        Vector2 [] uvs = new Vector2[points.Length];

        for(int i = 0; i < points.Length; i++){
            uvs[i] = new Vector2(points[i].x, points[i].y);
        }
        
        //Loop through the hexagons 
        for (int i = 0; i < treeSegments + 1; i ++) {
            //for each hexagon generate the points needed for the hexagon
            treeRadius -= treeRadius / treeHeight;
            Vector3 [] vertices = generateHexagon(treeRadius, height);
            deviationCount++;
            height += segmentHeight;


            //for every point in the hexagon add it to the main vertex array
            for (int v = 0; v < 6; v++) {
                points[i * 6 + v] = vertices[v];
            }
            
        }
        for (int i = 0; i < points.Length; i++)
        {
            int nextIndex = (i + 1) % points.Length;
            Debug.DrawLine(points[i], points[nextIndex], Color.red, Mathf.Infinity);
        }
        //(tree segments * 6) * 6
        //Generate triangles

        int [] triangles = new int [((treeSegments * 6) * 6) + 24];
        int [] trianglesBottom = new int [] {0, 1, 2, 2, 3, 4, 4, 5, 0, 0, 2, 4};
        int [] sides = new int [] {1, 0, 6, 6, 7, 1, 2, 1, 7, 7, 8, 2, 3, 2, 8, 8, 9, 3, 4, 3, 9, 9, 10 ,4, 5, 4, 10, 10, 11, 5, 0, 5, 11, 11, 6, 0};
        int [] trianglesTop = new int[trianglesBottom.Length];

        for (int i = 0; i < trianglesBottom.Length; i++) {
            trianglesTop[i] = trianglesBottom [i] + 6 * treeSegments;
        }
       
        
        int sideIndex = (trianglesBottom.Length * 2);
        //create the trianglwa for the sides of each segment
        for (int i = 0; i < treeSegments; i++) {
            int [] sideTriangles = new int[sides.Length];
            for (int j = 0; j < sides.Length; j++) {
                triangles[sideIndex] = sides[j] + (i * 6);
                sideIndex++;
            }
           
        }

        Array.Copy(trianglesBottom, 0, triangles, 0, trianglesBottom.Length);
        Array.Copy(trianglesTop, 0, triangles, trianglesBottom.Length, trianglesTop.Length);


        //render the mesh
        
        Mesh mesh = new Mesh();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        renderer.material = material;
        meshCreated = true;


        
        }


        public Vector3 [] generateHexagon(float radius, float height) {
        
        float deviationAmount = UnityEngine.Random.Range(0f, deviation);
        float angleIncrement = 2 * Mathf.PI / 6;
        Vector3 [] vertices = new Vector3 [6];
        int coinToss = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < 6; i ++) {
            
            float angle = i * angleIncrement;
            float x = radius * Mathf.Cos(angle);
            float y = height;
            float z = radius * Mathf.Sin(angle);
            if (y < deviationPoint){
                vertices[i] = new Vector3(x, y, z);
            } else if (y > deviationPoint){
                if(coinToss % 2 != 0 && deviationCount % 2 != 0){
                    vertices[i] = new Vector3(x + deviationAmount, y, z + deviationAmount);

                } else if (coinToss % 2 == 0 && deviationCount % 2 != 0) {
                    vertices[i] = new Vector3(x - deviationAmount, y, z - deviationAmount);

                } else if (deviationCount % 2 == 0) {
                    vertices[i] = new Vector3(x, y, z);

                }
            }
        }

        return vertices;
    }



}