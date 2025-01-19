using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeMeshGenerator : MonoBehaviour
{
    public float coneAngle = 90f; // Angle of the cone in degrees
    public float coneRadius = 10f; // Radius of the cone
    public int segments = 30; // Number of segments

    private Mesh _mesh;

    private void Awake()
    {
        GenerateConeMesh();
    }

    public void GenerateConeMesh()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        int verticesCount = segments + 2; // +2 for center and closing the loop
        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[segments * 3]; // Each triangle has 3 indices

        // Center vertex
        vertices[0] = Vector3.zero;

        float angleStep = coneAngle / segments;
        float startAngle = -coneAngle / 2;

        // Generate vertices for the cone
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            float radians = currentAngle * Mathf.Deg2Rad;

            // Apply the coneRadius directly
            vertices[i + 1] = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0) * coneRadius;

            if (i < segments)
            {
                // Add triangles
                int triangleIndex = i * 3;
                triangles[triangleIndex] = 0; // Center
                triangles[triangleIndex + 1] = i + 1; // Current vertex
                triangles[triangleIndex + 2] = i + 2; // Next vertex
            }
        }

        // Assign vertices and triangles to the mesh
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals(); // Optional for lighting
    }

}

