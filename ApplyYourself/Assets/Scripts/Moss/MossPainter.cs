using System;
using UnityEngine;

public class MossPainter : MonoBehaviour
{
    [SerializeField] private float paintDistance = 0.3f;
    [SerializeField] private float rayCastDistance = 1.5f;
    [SerializeField] private float brushRadiusWorld = 0.2f;

    private Vector3 lastPaintPos;
    private MossSurface currentSurface;
    private Collider currentCollider;

    private void Update()
    {
        if (Vector3.Distance(transform.position, lastPaintPos) < paintDistance)
            return;

        if (!Physics.Raycast(
                transform.position,
                -transform.up,
                out RaycastHit hit,
                rayCastDistance))
            return;

        if (hit.collider != currentCollider)
        {
            currentCollider = hit.collider;

            if (!hit.collider.TryGetComponent(out currentSurface))
            {
                currentSurface = null;
                return;
            }
        }
        
        if (!currentSurface)
            return;
        

        currentSurface.PaintCircle(
            hit.textureCoord,
            brushRadiusWorld
        );

        lastPaintPos = transform.position;
    }

}
