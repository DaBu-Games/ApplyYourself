using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeInOut : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    
    private CanvasGroup _group;
    private Coroutine routine;
    
    private void Awake()
    { 
        _group = GetComponent<CanvasGroup>();
        
        _group.alpha = 0f;
    }
    
    public IEnumerator FadeIn()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            _group.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        _group.alpha = end;
    }
}