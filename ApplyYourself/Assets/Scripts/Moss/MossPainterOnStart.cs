using System;
using UnityEngine;

public class MossPainterOnStart : MonoBehaviour
{
    [SerializeField] private float paintRadius = 2f;
    [SerializeField] private bool showGizmo = true;
    
    private MossSurface currentSurface;

    private void Start()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
        if (!Physics.Raycast(
                pos,
                Vector3.down,
                out RaycastHit hit,
                5f))
            return;
        
        if (!hit.collider.TryGetComponent(out currentSurface))
        {
            currentSurface = null;
            return;
        }
        
        currentSurface.PaintCircle(
            hit.textureCoord,
            paintRadius,
            true
        );
    }
    
    private void OnDrawGizmos()
    {
        if(!showGizmo)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, paintRadius);
    }
}
