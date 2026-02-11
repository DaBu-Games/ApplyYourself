using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshCollider))]
public class MossSurface : MonoBehaviour
{
    [SerializeField] private int textureSize = 512;

    private Texture2D mossMask;
    private Color[] pixels;
    
    private bool[] visited;
    private Stack<int> stack;
    private List<GrowableObject> growables = new List<GrowableObject>();
    
    private static readonly int MossMaskID = Shader.PropertyToID("_MossMask");

    private void Awake()
    {
        mossMask = new Texture2D(
            textureSize,
            textureSize,
            TextureFormat.R8,
            false
        );

        mossMask.wrapMode = TextureWrapMode.Clamp;
        mossMask.filterMode = FilterMode.Bilinear;

        pixels = new Color[textureSize * textureSize];
        visited = new bool[textureSize * textureSize];
        stack = new Stack<int>();
        
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.black;

        mossMask.SetPixels(pixels);
        mossMask.Apply();

        GetComponent<Renderer>().material.SetTexture(MossMaskID, mossMask);
    }
    
    public void RegisterGrowable(GrowableObject obj)
    {
        if (!growables.Contains(obj))
            growables.Add(obj);
    }

    public void PaintCircle(Vector2 uv, float radius01, float strength)
    {
        int cx = (int)(uv.x * textureSize);
        int cy = (int)(uv.y * textureSize);
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

                float dist = Mathf.Sqrt(x * x + y * y) / radius;
                if (dist > 1f) continue;

                float value = Mathf.Clamp01(1f - dist) * strength;

                int index = py * textureSize + px;
                float old = pixels[index].r;
                float newValue = Mathf.Max(old, value);

                if (newValue > old)
                {
                    pixels[index].r = newValue;
                    paintedSomething = true;
                }
            }
        }

        if (paintedSomething)
        {
            mossMask.SetPixels(pixels);
            mossMask.Apply();
            CheckGrowables();
        }
    }
    
    private void CheckGrowables()
    {
        for (int i = growables.Count - 1; i >= 0; i--)
        {
            GrowableObject obj = growables[i];

            Vector2 uv = obj.UV;

            if (IsAreaEnclosed(uv))
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

        if (!IsInside(startX, startY))
            return false;

        int startIndex = startY * textureSize + startX;
        
        if (pixels[startIndex].r > 0.1f)
            return false;

        // Reset visited
        System.Array.Clear(visited, 0, visited.Length);
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
        
        return true;
    }

    private void TryVisit(int x, int y)
    {
        if (!IsInside(x, y))
            return;

        int index = y * textureSize + x;

        if (visited[index])
            return;

        if (pixels[index].r > 0.1f)
            return;

        visited[index] = true;
        stack.Push(index);
    }

    private bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < textureSize && y < textureSize;
    }
}