using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

public enum KorokEmotion
{
    Sad,
    Happy,
    Excited
}

public class KorokEntity : MonoBehaviour
{
    [System.Serializable]
    public struct EmotionMaterials
    {
        public KorokEmotion emotion;
        public Material frontMaterial;
        public Material backMaterial;
    }
    
    [SerializeField] private EmotionMaterials[] emotionPairs = new EmotionMaterials[]{
        new EmotionMaterials { emotion = KorokEmotion.Sad },
        new EmotionMaterials { emotion = KorokEmotion.Happy },
        new EmotionMaterials { emotion = KorokEmotion.Excited }
    };
    
    [SerializeField] private KorokEmotion currentEmotion = KorokEmotion.Excited;
    [SerializeField] private MeshRenderer frontMeshRenderer;
    [SerializeField] private MeshRenderer backMeshRenderer;

    private void Awake()
    {
        UpdateSpriteRenderers();
    }

    public void SetEmotion(KorokEmotion emotion)
    {
        currentEmotion = emotion;
        UpdateSpriteRenderers();
    }

    private void UpdateSpriteRenderers()
    {
        frontMeshRenderer.material = emotionPairs[(int)currentEmotion].frontMaterial;
        backMeshRenderer.material = emotionPairs[(int)currentEmotion].backMaterial;
    }
}
