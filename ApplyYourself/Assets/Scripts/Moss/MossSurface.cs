using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshCollider))]
public class MossSurface : MonoBehaviour
{
    [SerializeField] private int textureSize = 512;
    [SerializeField] private bool fillIn = false;
    [SerializeField] private MossSettings mossSettings;

    private Texture2D mossMask;
    private Color[] pixels;
    private List<int> changedPixels = new List<int>();
    private bool[] mossSpawned; 
    
    private bool[] visited;
    private Stack<int> stack;
    private List<GrowableObject> growables = new List<GrowableObject>();
    
    private Renderer mossRenderer;
    private Dictionary<Vector2Int, MossChunk> mossChunks;
    private float worldSize;
    private float mossSpawnChance;
    private float chunkSize;
    
    private static readonly int MossMaskID = Shader.PropertyToID("_MossMask");

    private void Awake()
    {
        InitializeMask();
        InitializeInstancing();
    }
    
    private void InitializeMask()
    {
        mossMask = new Texture2D(textureSize, textureSize, TextureFormat.R8, false)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear
        };

        pixels = new Color[textureSize * textureSize];
        visited = new bool[textureSize * textureSize];
        mossSpawned = new bool[textureSize * textureSize];
        stack = new Stack<int>();
        
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.black;

        mossMask.SetPixels(pixels);
        mossMask.Apply();
        
        mossRenderer = GetComponent<Renderer>();
        mossRenderer.material.SetTexture(MossMaskID, mossMask);
        
        Bounds bounds = mossRenderer.bounds;
        worldSize = (bounds.size.x + bounds.size.z) * 0.5f;
        
        float pixelWorldSize = worldSize / textureSize;
        float pixelWorldArea = pixelWorldSize * pixelWorldSize;

        mossSpawnChance = mossSettings.mossPerSquareMeter * pixelWorldArea;
        
        float chunkArea = mossSettings.targetMossPerChunk / mossSettings.mossPerSquareMeter;
        chunkSize = math.clamp(Mathf.Sqrt(chunkArea), mossSettings.minChunkSize, mossSettings.maxChunkSize);
    }
    
    private void InitializeInstancing() 
    {
        mossChunks = new Dictionary<Vector2Int, MossChunk>();
    }

    public void RegisterGrowable(GrowableObject obj)
    {
        if (!growables.Contains(obj))
            growables.Add(obj);
    }

    public void PaintCircle(Vector2 uv, float radiusWorld)
    {
        int cx = (int)(uv.x * textureSize);
        int cy = (int)(uv.y * textureSize);
        
        float radius01 = radiusWorld / worldSize;

        int radius = Mathf.CeilToInt(radius01 * textureSize);
        
        bool paintedSomething = false;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                int px = cx + x;
                int py = cy + y;

                if (px < 0 || py < 0 || px >= textureSize || py >= textureSize)
                    continue;

                float distSq = x * x + y * y;
                if (distSq > radius * radius) continue;

                int index = py * textureSize + px;

                if (pixels[index].r < 1f)
                {
                    pixels[index].r = 1f;
                    paintedSomething = true;
                    changedPixels.Add(index);
                }
            }
        }

        if (paintedSomething)
        {
            CheckGrowables();
            SpawnFromChangedPixels();
            
            mossMask.SetPixels(pixels);
            mossMask.Apply();
        }
    }
    
    private void CheckGrowables()
    {
        for (int i = growables.Count - 1; i >= 0; i--)
        {
            GrowableObject obj = growables[i];

            if (IsAreaEnclosed(obj.UV))
            {
                obj.Grow();
                growables.RemoveAt(i);
            }
        }
    }
    
    private bool IsAreaEnclosed(Vector2 uv)
    {
        int startX = (int)(uv.x * textureSize);
        int startY = (int)(uv.y * textureSize);

        if (!IsInsideTexture(startX, startY))
            return false;

        int startIndex = startY * textureSize + startX;
        
        Array.Clear(visited, 0, visited.Length);
        stack.Clear();

        stack.Push(startIndex);
        visited[startIndex] = true;

        while (stack.Count > 0)
        {
            int index = stack.Pop();
            int x = index % textureSize;
            int y = index / textureSize;

            // If flood reaches edge → not enclosed
            if (x == 0 || y == 0 || x == textureSize - 1 || y == textureSize - 1)
                return false;

            TryVisit(x + 1, y);
            TryVisit(x - 1, y);
            TryVisit(x, y + 1);
            TryVisit(x, y - 1);
        }

        if (fillIn)
        {
            for (int i = 0; i < visited.Length; i++)
            {
                if (visited[i])
                {
                    pixels[i].r = 1f;
                    changedPixels.Add(i);
                }
            }
        }
        
        return true;
    }

    private void TryVisit(int x, int y)
    {
        if (!IsInsideTexture(x, y))
            return;

        int index = y * textureSize + x;

        if (visited[index])
            return;

        if (pixels[index].r > 0.9f)
            return;

        visited[index] = true;
        stack.Push(index);
    }

    private bool IsInsideTexture(int x, int y)
    {
        return x >= 0 && y >= 0 && x < textureSize && y < textureSize;
    }
    
    private void SpawnFromChangedPixels()
    {
        Debug.Log("changedPixels: " + changedPixels.Count);
        foreach (int index in changedPixels)
        {
            if (mossSpawned[index])
                continue;
            
            float expected = mossSpawnChance;
            int spawnCount = Mathf.FloorToInt(expected);

            if (Random.value < (expected - spawnCount))
                spawnCount++;
            
            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 uv = IndexToUV(index);

                if (TryGetWorldFromUV(uv, out Vector3 worldPos, out Vector3 normal))
                {
                    SpawnMossAt(index, worldPos, normal);
                }
            }
            
            if (spawnCount > 0)
                mossSpawned[index] = true;
        }
        
        changedPixels.Clear();
    }
    
    private Vector2 IndexToUV(int index)
    {
        int x = index % textureSize;
        int y = index / textureSize;

        return new Vector2(
            (x + 0.5f) / textureSize,
            (y + 0.5f) / textureSize
        );
    }
    
    private bool TryGetWorldFromUV(Vector2 uv, out Vector3 worldPos, out Vector3 normal)
    {
        worldPos = Vector3.zero;
        normal = Vector3.up;

        Bounds bounds = mossRenderer.bounds;

        // Shoot from above the surface
        Vector3 origin = bounds.min + new Vector3((1f - uv.x) * bounds.size.x, bounds.size.y + 0.01f, (1f - uv.y) * bounds.size.z);
        Ray ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, bounds.size.y * 2f))
        {
            worldPos = hit.point;
            normal = hit.normal;
            return true;
        }

        return false;
    }
    
    private void SpawnMossAt(int index, Vector3 worldPos, Vector3 normal)
    {
        int mossIndex = Random.Range(0, mossSettings.mossTypes.Length);

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal);
        rot *= Quaternion.Euler(0, Random.Range(0, 360f), 0);
        
        float scale = Random.Range(mossSettings.minScale, mossSettings.maxScale);
        
        Vector2Int chunkKey = new Vector2Int(
            Mathf.FloorToInt(worldPos.x / chunkSize),
            Mathf.FloorToInt(worldPos.z / chunkSize)
        );
        
        if (!mossChunks.TryGetValue(chunkKey, out MossChunk chunk))
        {
            chunk = new MossChunk( 
                transform, 
                mossSettings,
                new Vector3(chunkKey.x * chunkSize, 0, chunkKey.y * chunkSize)
            );
            
            mossChunks.Add(chunkKey, chunk);
        }
        
        Vector3 localPos = worldPos - chunk.WorldPosition;

        Matrix4x4 localMatrix = Matrix4x4.TRS(
            localPos,
            rot,
            Vector3.one * scale
        );

        chunk.AddMatrix(mossIndex, localMatrix);
        mossSpawned[index] = true;
    }
    
    private void LateUpdate()
    {
        foreach (var chunk in mossChunks.Values)
        {
            chunk.UpdateMesh();
        }
    }
}