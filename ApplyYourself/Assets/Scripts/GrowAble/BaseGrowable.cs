using System;
using UnityEngine;

public class BaseGrowable : MonoBehaviour
{
    [SerializeField] private bool showGizmo = true;
    [Header("Grow settings")]
    [SerializeField] private bool fillIn;
    [SerializeField] private bool canUnGrow;
    public Vector2 UV { get; private set; }
    public bool HasGrown { get; private set; }
    
    public bool FillIn => fillIn;
    public bool CanUnGrow => canUnGrow;
    public Action OnGrowthChanged;
    
    void Start()
    {
        if (GetComponent<MossPainterOnStart>())
        {
            HasGrown = true;
            OnGrowthChanged?.Invoke();
            return;
        }
        
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 5f))
        {
            MossSurface surface = hit.collider.GetComponent<MossSurface>();
            UV = hit.textureCoord;

            if (surface != null)
            {
                surface.RegisterGrowable(this);
            }
        }
        
        HasGrown = false;
    }

    public void Grow()
    {
        if (!HasGrown)
        {
            HasGrown = true;
            OnGrowthChanged?.Invoke();
        }
    }

    public void UnGrow()
    {
        if (HasGrown && canUnGrow)
        {
            HasGrown = false;
            OnGrowthChanged?.Invoke();
        }
    }
    
    private void OnDrawGizmos()
    {
        if(!showGizmo)
            return;
        
        Vector3 rayStart = new Vector3(
            transform.position.x, 
            transform.position.y + 2f, 
            transform.position.z
        );
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rayStart, 0.2f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayStart, rayStart + Vector3.down * 5f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rayStart + Vector3.down * 5f, 0.2f);
    }
}
