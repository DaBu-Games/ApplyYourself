using System;
using UnityEngine;

public class GrowableObject : MonoBehaviour
{
    public Vector2 UV { get; private set; }
    private bool hasGrown = false;

    private void Start()
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

    public void Grow()
    {
        if(hasGrown) 
            return;
        
        hasGrown = true;
        transform.localScale *= 1.5f;
        Debug.Log(name + " grew!");
    }
}
