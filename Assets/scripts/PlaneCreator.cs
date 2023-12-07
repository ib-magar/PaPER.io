using UnityEngine;
using System.Collections.Generic;

public class PlaneCreator : MonoBehaviour
{
    public List<Vector3> waypoints;
    public float height = 1.0f;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = waypoints.ToArray();
        for (int i=0;i<vertices.Length;i++) {
           // vertices[i] = new Vector3(vertices[i].x, vertices[i].y, height);
        }


        int[] triangles = new int[(waypoints.Count - 2) * 3];

        for (int i = 0; i < waypoints.Count - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    
   
}
