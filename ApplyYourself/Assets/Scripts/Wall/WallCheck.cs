using UnityEngine;
using UnityEngine.Serialization;

public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float range;
    [SerializeField] private bool showGizmo;
    
    private Vector3 wallDirection;
    
    public Vector3 GetWallDirection => wallDirection;

    public bool IsTouchingWall()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range, wallLayer))
        {
            wallDirection = hit.normal;
        }
        else
            wallDirection = Vector3.zero;
        
        return wallDirection != Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if(!showGizmo)
            return;
        
        Gizmos.color = IsTouchingWall() ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
    
}