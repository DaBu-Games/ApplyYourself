using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float range;
    [SerializeField] private bool showGizmo;
    
    public bool IsGrounded { get; private set; }
    public float LastOnGroundTime { get; private set; }

    private void Update()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, range, _groundLayer))
        {
            IsGrounded = true;
            LastOnGroundTime = Time.time;
        }
        else
        {
            IsGrounded = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        if(!showGizmo)
            return;
        
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * range);
    }
    
    public void SetRange(float setRange) => range = setRange;
}