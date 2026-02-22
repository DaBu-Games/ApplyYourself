using UnityEngine;

[CreateAssetMenu(menuName = "Moss/Moss Settings")]
public class MossSettings : ScriptableObject
{
    public MossType[] mossTypes;

    [Header("Spawn Settings")] 
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float mossPerSquareMeter = 50f;
    public float targetMossPerChunk = 1000f;
    public float maxChunkSize = 10f;
    public float minChunkSize = 3f;
}


[System.Serializable]
public struct MossType
{
    public Mesh mesh;
    public Material material;
}