using UnityEngine;
using System.Collections;

public class TangentSpaceVisualizer : MonoBehaviour
{
    public float offset = 0.1f;
    public float scale = 0.1f;
    void OnDrawGizmos()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector4[] tangents = mesh.tangents;
        for (int i = 0; i < vertices.Length; i++)
        {
            ShowTangentSpace(
                transform.TransformPoint(vertices[i]), 
                transform.TransformDirection(normals[i]),
                transform.TransformDirection(tangents[i]),
                tangents[i].w);
        }
    }

    private void ShowTangentSpace(Vector3 vertex, Vector3 normal, Vector3 tangent, float binormalSign)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(vertex + normal * offset, normal * scale);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(vertex, tangent* scale);

        Gizmos.color = Color.blue;
        Vector3 binormal = Vector3.Cross(normal, tangent) * binormalSign;
        Gizmos.DrawRay(vertex, binormal * scale);
    }
}
