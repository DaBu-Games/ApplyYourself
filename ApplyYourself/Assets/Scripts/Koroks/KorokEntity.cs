using System.Collections;
using UnityEngine;

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
    
    private float currentSpeed;
    private float cheeringHeight;
    private bool isCheering = false;
    private Vector3 startPosition;
    private Coroutine cheeringCoroutine;

    private void Awake()
    {
        UpdateSpriteRenderers();
    }

    public void Initialize(float speed, float height)
    {
        currentSpeed = speed;
        cheeringHeight = height;
        startPosition = transform.position;
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

    public void StartCheering()
    {
        if (!isCheering)
        {
            isCheering = true;
            cheeringCoroutine = StartCoroutine(Cheering());
        }
    }

    public void StopCheering()
    {
        if (isCheering)
        {
            isCheering = false;
            
            if (cheeringCoroutine != null)
            {
                StopCoroutine(cheeringCoroutine);
                cheeringCoroutine = null;
            }
            
            transform.position = startPosition;
        }
    }

    private IEnumerator Cheering()
    {
        while (isCheering)
        {
            float sinValue = Mathf.Sin(Time.time * currentSpeed);
            float yOffset = Mathf.Abs(sinValue) * cheeringHeight;
            
            transform.position = startPosition + new Vector3(0, yOffset, 0);
            
            yield return null;
        }
    }
}
