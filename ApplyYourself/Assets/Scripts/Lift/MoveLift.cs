using System;
using UnityEngine;

public class MoveLift : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float totalTime;
    
    private Rigidbody rb;
    private bool startMoving = false;
    private bool isFinished = false;
    private float distance;
    private float moveSpeed;
    private float startTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distance = Vector3.Distance(transform.position, target.position);
        moveSpeed = distance / totalTime;
    }

    public void StartMoving()
    {
        startMoving = true;
        startTime = Time.time;
    }
    
    public bool IsMoving() => startMoving;
    public bool IsFinished() => isFinished;

    private void Update()
    {
        if(!startMoving)
            return;
        
        Vector3 newPosition = Vector3.MoveTowards(
            rb.position,
            target.position,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
        
        if (Vector3.Distance(rb.position, target.position) <= 0.01f)
        {
            startMoving = false;
            isFinished = true;
        }
    }
}
