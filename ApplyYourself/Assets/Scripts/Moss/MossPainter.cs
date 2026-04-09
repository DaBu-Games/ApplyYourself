using System;
using UnityEngine;

public class MossPainter : MonoBehaviour
{
    [SerializeField] protected GroundCheck groundCheck;
    [SerializeField] private float paintDistance = 0.3f;
    [SerializeField] protected float rayCastDistance = 1.5f;
    [SerializeField] protected float brushRadiusWorld = 0.2f;
    [SerializeField] private bool showGizmo;
    [SerializeField] protected bool drawWhite;

    private Vector3 lastPaintPos;
    protected MossSurface currentSurface;
    private Collider currentCollider;

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, lastPaintPos) < paintDistance || !groundCheck.IsGrounded)
            return;

        if (!Physics.Raycast(
                transform.position,
                Vector3.down,
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

        PaintOnSurface(hit);

        lastPaintPos = transform.position;
    }

    protected virtual void PaintOnSurface(RaycastHit hit)
    {
        currentSurface.PaintCircle(
            hit.textureCoord,
            brushRadiusWorld,
            drawWhite
        );
    }
    
    private void OnDrawGizmos()
    {
        if(!showGizmo)
            return;
        
        Gizmos.color = Color.purple;
        Gizmos.DrawRay(transform.position, Vector3.down * rayCastDistance);
    }

}
