using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshCollider))]
public class MossSurface : MonoBehaviour
{
    [SerializeField] private int textureSize = 512;

    private Texture2D mossMask;
    private Color[] pixels;

    public Texture2D MossMask => mossMask;

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

        // start black
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.black;

        mossMask.SetPixels(pixels);
        mossMask.Apply();

        GetComponent<Renderer>().material.SetTexture(MossMaskID, mossMask);
    }

    public void PaintCircle(Vector2 uv, float radius01, float strength)
    {
        int cx = (int)(uv.x * textureSize);
        int cy = (int)(uv.y * textureSize);
        int radius = Mathf.CeilToInt(radius01 * textureSize);

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
                pixels[index].r = Mathf.Max(pixels[index].r, value);
            }
        }

        mossMask.SetPixels(pixels);
        mossMask.Apply();
    }
}