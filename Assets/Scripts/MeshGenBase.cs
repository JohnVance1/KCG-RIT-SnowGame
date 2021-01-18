/**
 * @author  Lingxiao Yu
 * @github  http://github.com/KHN190
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class MeshGenBase : MonoBehaviour
{
    protected Mesh mesh;
    protected Material defaultMat;
    protected readonly List<Vector3> vertices = new List<Vector3>();
    protected readonly List<int> triangles = new List<int>();

    public virtual void SaveAsset()
    {
        if (mesh == null)
        {
            Debug.Log("Not generated, cannot save.");
            return;
        }
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        path = FileUtil.GetProjectRelativePath(path);
        Debug.Log("Saving mesh to: " + path);

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
    }

    protected void FlipNormals()
    {
        if (mesh == null)
        {
            Debug.Log("Not generated, cannot flip.");
            return;
        }
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] *= -1;
        mesh.normals = normals;

        for (int m = 0; m < mesh.subMeshCount; m++)
        {
            int[] tris = mesh.GetTriangles(m);
            for (int i = 0; i < tris.Length; i += 3)
            {
                int temp = tris[i + 0];
                tris[i + 0] = tris[i + 1];
                tris[i + 1] = temp;
            }
            mesh.SetTriangles(tris, m);
        }
    }

    protected void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    protected void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
}