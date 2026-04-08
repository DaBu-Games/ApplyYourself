using UnityEngine;
using UnityEngine.Serialization;

public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private bool showGizmo;
    
    private Vector3 wallDirection;
    
    public Vector3 GetWallDirection => wallDirection;

    public bool IsTouchingWall()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, wallLayer);

        if (hits.Length > 0)
        {
            Collider closest = hits[0];

            Vector3 directionToWall = (closest.ClosestPoint(transform.position) - transform.position).normalized;

            wallDirection = -directionToWall;
            return true;
        }

        wallDirection = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;

        Gizmos.color = IsTouchingWall() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}