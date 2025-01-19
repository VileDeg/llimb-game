using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeMeshGenerator : MonoBehaviour
{
    public float coneAngle = 90f; // Angle of the cone in degrees
    public float coneRadius = 5f; // Default radius of the cone
    public int segments = 50; // Number of segments for smoothness

    private Mesh _mesh;

    private void Awake()
    {
        GenerateConeMesh();
    }

    public void SetConeRadius(float radius)
    {
        coneRadius = radius; // Update radius dynamically
        GenerateConeMesh(); // Regenerate the cone mesh
    }

    private void GenerateConeMesh()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        int verticesCount = segments + 2; // +2 for the center and the last point closing the loop
        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[segments * 3]; // Each segment has 3 indices for the triangle

        // Set the center of the cone
        vertices[0] = Vector3.zero;

        float angleStep = coneAngle / segments;
        float startAngle = -coneAngle / 2;

        // Generate vertices for the cone
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            float radians = currentAngle * Mathf.Deg2Rad;

            vertices[i + 1] = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0) * coneRadius;

            if (i < segments)
            {
                // Define triangles
                int triangleIndex = i * 3;
                triangles[triangleIndex] = 0; // Center point
                triangles[triangleIndex + 1] = i + 1; // Current vertex
                triangles[triangleIndex + 2] = i + 2; // Next vertex
            }
        }

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals(); // Optional for lighting
    }
}
