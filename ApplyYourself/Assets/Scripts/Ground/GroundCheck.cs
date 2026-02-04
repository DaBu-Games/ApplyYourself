using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] float _radius = 0.3f;
    
    public bool IsGrounded { get; private set; }
    public float LastOnGroundTime { get; private set; }

    private void Update()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            _radius,
            _groundLayer,
            QueryTriggerInteraction.Ignore
        );

        if (hits.Length > 0)
        {
            IsGrounded = true;
            LastOnGroundTime = Time.time;
        }
        else
            IsGrounded = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}