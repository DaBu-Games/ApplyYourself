using System;
using System.Collections.Generic;
using UnityEngine;

public class MossTrail : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float mossWidth = 2f;
    [SerializeField] private float minDistance = 0.5f;
    
    private readonly List<Vector3> mossPoints = new List<Vector3>();
    private Mesh mossMesh;

    private void Awake()
    {
        mossMesh = new Mesh();
        meshFilter.mesh = mossMesh;
    }

    private void Update()
    {
        AddPoint(playerTransform.position);
    }

    private void AddPoint(Vector3 point)
    {
        if (mossPoints.Count == 0 || Vector3.Distance( mossPoints[^1], point) >= minDistance)
        {
            mossPoints.Add(point);
            Debug.Log(point);
            GenerateMossMesh();
        }
    }

    private void GenerateMossMesh()
    {
        if(mossPoints.Count < 2)
            return;
        
        int vertCount = mossPoints.Count * 2;
        Vector3[] verts = new Vector3[vertCount];
        
        int[] tris = new int[(mossPoints.Count - 1) * 6];
        Vector2[] uvs = new Vector2[vertCount];

        for (int i = 0; i < mossPoints.Count; i++)
        {
            Vector3 forward;
            if (i == 0)
            {
                forward = mossPoints[1] - mossPoints[0];
            }
            else if (i == mossPoints.Count - 1)
            {
                forward = mossPoints[i] - mossPoints[i - 1];
            }
            else
            {
                forward = mossPoints[i + 1] - mossPoints[i - 1];
            }
            forward = forward.normalized;

            Vector3 side = Vector3.Cross(Vector3.down, forward).normalized;
            
            verts[i * 2]     = mossPoints[i] + side * (mossWidth * 0.5f);
            verts[i * 2 + 1] = mossPoints[i] - side * (mossWidth * 0.5f);

            float v = i / (float)mossPoints.Count;
            uvs[i * 2]     = new Vector2(0, v);
            uvs[i * 2 + 1] = new Vector2(1, v);
        }

        int t = 0;
        for (int i = 0; i < mossPoints.Count - 1; i++)
        {
            int v = i * 2;
            tris[t++] = v;
            tris[t++] = v + 2;
            tris[t++] = v + 1;

            tris[t++] = v + 1;
            tris[t++] = v + 2;
            tris[t++] = v + 3;
        }

        mossMesh.Clear();
        mossMesh.vertices = verts;
        mossMesh.triangles = tris;
        mossMesh.uv = uvs;
        mossMesh.RecalculateNormals();
    }
    
}
