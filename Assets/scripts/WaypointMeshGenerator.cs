using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointMeshGenerator : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypoints;
    [SerializeField] private float meshHeight;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private MeshCollider meshCollider;

    public Material pathmesh;
    public LayerMask meshlayer;
    void Start()
    {
       // GenerateMesh();
    }


    public void GenerateMeshFromWayPoints(List<Vector3> _waypoints)
    {
        waypoints = _waypoints;
        GenerateMesh();
    }
    private void GenerateMesh()
    {
        GameObject pathMesh = new GameObject("path mesh");
        pathMesh.layer = 6;

        meshFilter = pathMesh.AddComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        meshCollider = pathMesh.AddComponent<MeshCollider>();
        //pathMesh.GetComponent<MeshCollider>().isTrigger=true;
        meshCollider.convex = true;
        pathMesh.AddComponent<MeshRenderer>();
        pathMesh.GetComponent<MeshRenderer>().material=pathmesh;

        // Calculate total number of vertices and triangles
        int numVertices = waypoints.Count * 2;
        int numTriangles = (waypoints.Count - 2) * 3 * 2; // Two triangles per segment

        // Create arrays for vertices and triangles
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];

        // Loop through waypoints and generate vertices
        for (int i = 0; i < waypoints.Count; i++)
        {
            vertices[i * 2] = waypoints[i];
            vertices[i * 2 + 1] = waypoints[i] + Vector3.up * meshHeight;
        }

        // Generate triangles for each segment
        int triangleIndex = 0;
        for (int i = 1; i < waypoints.Count - 1; i++)
        {
            // Bottom triangle
            triangles[triangleIndex++] = (i + 1) * 2; // Reverse order
            triangles[triangleIndex++] = i * 2 + 1;
            triangles[triangleIndex++] = i * 2;

            // Top triangle
            triangles[triangleIndex++] = (i + 1) * 2 + 1; // Reverse order
            triangles[triangleIndex++] = i * 2 + 1;
            triangles[triangleIndex++] = (i + 1) * 2;
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        meshCollider.sharedMesh = mesh;

        GenerateTopMesh();
    }

    void GenerateTopMesh()
    {
        GameObject newObj = new GameObject("Top mesh");
        Mesh mesh = new Mesh();
        newObj.AddComponent<MeshFilter>().mesh = mesh;
        newObj.AddComponent<MeshRenderer>();
        newObj.GetComponent<MeshRenderer>().material = pathmesh;

        Vector3[] vertices = waypoints.ToArray();

        // Reverse the order of vertices for outward visibility
        vertices = vertices.Reverse().ToArray();

        int[] triangles = new int[(waypoints.Count - 2) * 3];

        for (int i = 0; i < waypoints.Count - 2; i++)
        {
            // Reverse the order of indices for each triangle
            triangles[i * 3] = 0;
            triangles[i * 3 + 2] = i + 1;
            triangles[i * 3 + 1] = i + 2;
        }

        newObj.transform.position = new Vector3(newObj.transform.position.x, meshHeight, newObj.transform.position.z);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    
}
