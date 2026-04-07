using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLift : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private ScrollRect creditsScrollRect;
    [SerializeField] private float totalTime;
    
    private Rigidbody rb;
    private UIFadeInOut uiFadeInOut;
    
    private bool startMoving = false;
    private bool isFinished = false;
    private float distance;
    private float moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        distance = Vector3.Distance(transform.position, target.position); 
        moveSpeed = distance / totalTime;
        
        creditsScrollRect.gameObject.SetActive(false);
        uiFadeInOut = creditsScrollRect.gameObject.GetComponent<UIFadeInOut>();
    }
    
    public void StartMoving()
    {
        StartCoroutine(StartSequence());
    }
    
    private IEnumerator StartSequence()
    {
        creditsScrollRect.gameObject.SetActive(true);
        
        yield return null;
        
        yield return StartCoroutine(uiFadeInOut.FadeIn());
        
        yield return null;
        
        startMoving = true;
    }

    private IEnumerator StopSequence()
    {
        startMoving = false;
        isFinished = true;
        
        yield return StartCoroutine(uiFadeInOut.FadeOut());
        
        creditsScrollRect.gameObject.SetActive(false);
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
        
        float currentDistance = Vector3.Distance(newPosition, target.position);
        float progress = Mathf.Clamp01(currentDistance / distance);

        creditsScrollRect.verticalNormalizedPosition = progress;

        if (currentDistance <= 0.01f)
        {
            StartCoroutine(StopSequence());
        }
    }
}
