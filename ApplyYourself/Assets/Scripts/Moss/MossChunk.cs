using UnityEngine;
using System.Collections.Generic;

public class MossChunk
{
    private List<Matrix4x4> matrices = new List<Matrix4x4>();
    private Mesh combinedMesh;
    private GameObject chunkObject;
    private bool hasChanges = true;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    public Vector3 WorldPosition => chunkObject.transform.position;
    
    public MossChunk(Transform parent, Material mossMaterial, Vector3 position)
    {
        chunkObject = new GameObject("MossChunk");
        chunkObject.transform.SetParent(parent);
        chunkObject.transform.position = position;
        chunkObject.transform.rotation = Quaternion.identity;

        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mossMaterial;

        combinedMesh = new Mesh();
        meshFilter.mesh = combinedMesh;
    }

    public void AddMatrix(Matrix4x4 matrix)
    {
        matrices.Add(matrix);
        hasChanges = true;
    }
    
    public void UpdateMesh(Mesh baseMesh)
    {
        if (!hasChanges || matrices.Count == 0) return;

        CombineInstance[] combines = new CombineInstance[matrices.Count];
        for (int i = 0; i < matrices.Count; i++)
        {
            combines[i] = new CombineInstance
            {
                mesh = baseMesh,
                transform = matrices[i]
            };
        }

        combinedMesh.Clear();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combines, true, true, false);
        combinedMesh.RecalculateBounds();
        combinedMesh.UploadMeshData(false);
        hasChanges = false;
    }
}