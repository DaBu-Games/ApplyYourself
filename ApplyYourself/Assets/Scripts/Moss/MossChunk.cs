using UnityEngine;
using System.Collections.Generic;

public class MossChunk
{
    private Dictionary<int, List<Matrix4x4>> matricesPerType = new Dictionary<int, List<Matrix4x4>>();
    private Mesh combinedMesh;
    private GameObject chunkObject;
    private bool hasChanges = true;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    private MossSettings mossSettings;
    
    public Vector3 WorldPosition => chunkObject.transform.position;
    
    public MossChunk(Transform parent, MossSettings settings, Vector3 position)
    {
        mossSettings = settings;
        
        chunkObject = new GameObject("MossChunk");
        chunkObject.transform.SetParent(parent);
        chunkObject.transform.position = position;
        chunkObject.transform.rotation = Quaternion.identity;

        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        
        combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = combinedMesh; 
    }

    public void AddMatrix(int mossTypeIndex, Matrix4x4 matrix)
    {
        if (!matricesPerType.TryGetValue(mossTypeIndex, out var list))
        {
            list = new List<Matrix4x4>();
            matricesPerType[mossTypeIndex] = list;
        }

        list.Add(matrix);
        hasChanges = true;
    }
    
    public void UpdateMesh()
    {
        if (!hasChanges ) return;
        
        combinedMesh.Clear();
        
        var combineList = new List<CombineInstance>();
        var materials = new List<Material>();

        foreach (var instance in matricesPerType)
        {
            int mossTypeIndex = instance.Key;
            var matrices = instance.Value;

            if (matrices.Count == 0)
                continue;

            Mesh baseMesh = mossSettings.mossTypes[mossTypeIndex].mesh;
            var subCombines = new CombineInstance[matrices.Count];

            for (int i = 0; i < matrices.Count; i++)
            {
                subCombines[i] = new CombineInstance
                {
                    mesh = baseMesh,
                    transform = matrices[i]
                };
            }

            Mesh subMesh = new Mesh();
            subMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            subMesh.CombineMeshes(subCombines, true, true, false);

            combineList.Add(new CombineInstance
            {
                mesh = subMesh,
                transform = Matrix4x4.identity
            });

            materials.Add(mossSettings.mossTypes[mossTypeIndex].material);
        }

        if (combineList.Count > 0)
        {
            combinedMesh.CombineMeshes(combineList.ToArray(), false, false, false);
            meshRenderer.materials = materials.ToArray();
        }

        combinedMesh.RecalculateBounds();
        combinedMesh.UploadMeshData(false);

        hasChanges = false;
    }
}