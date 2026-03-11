using System;
using UnityEngine;

public class BowlingBall : MossPainter
{
    [SerializeField] private float minSize = 1f;
    [SerializeField] private float maxSize = 10f;
    [SerializeField] private int growForEveryPixel = 50;
    [SerializeField] private float growAmount = 5;
    [SerializeField] private float growthSpeed = 2f;
    
    private int changedPixels = 0;
    private float targetScale = 1f;
    private float currentScale = 1f;

    private void Start()
    {
        SetSize();
        SetRaycastValues();
    }

    private void SetRaycastValues()
    {
        float scale = transform.localScale.x + 0.01f;
        groundCheck.SetRange(scale);
        rayCastDistance = scale;
    }
    
    protected override void PaintOnSurface(RaycastHit hit)
    {
        changedPixels += currentSurface.PaintCircle(
            hit.textureCoord,
            transform.localScale.x / 3,
            drawWhite
        );
        
        SetSize();
        SetRaycastValues();
    }

    private void SetSize()
    {
        int growthSteps = changedPixels / growForEveryPixel;
        targetScale = Mathf.Lerp(minSize, maxSize, (float)growthSteps * growAmount / maxSize);
        targetScale = Mathf.Clamp(targetScale, minSize, maxSize);
        
        currentScale = Mathf.MoveTowards(currentScale, targetScale, growthSpeed * Time.deltaTime);
        transform.localScale = Vector3.one * currentScale;
    }
    
    public void ResetSize()
    {
        changedPixels = 0;
        targetScale = minSize;
        currentScale = minSize;
        transform.localScale = Vector3.one * minSize;
    }
}
