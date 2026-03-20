using UnityEngine;

public class BaseGrowable : MonoBehaviour
{
    [SerializeField] private bool fillIn;
    [SerializeField] private bool canUnGrow;
    public Vector2 UV { get; private set; }
    protected bool hasGrown = false;
    
    public bool FillIn => fillIn;
    public bool CanUnGrow => canUnGrow;
    
    void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            MossSurface surface = hit.collider.GetComponent<MossSurface>();
            UV = hit.textureCoord;

            if (surface != null)
            {
                surface.RegisterGrowable(this);
            }
        }
    }

    public virtual void Grow()
    {
        
    }

    public virtual void UnGrow()
    {
        
    }
}
