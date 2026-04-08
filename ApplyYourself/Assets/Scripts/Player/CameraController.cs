using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Cinemachine reference")]
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CinemachineOrbitalFollow orbital;
    
    [Header("Look")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 13f;
    
    private Vector2 scrollDate;
    
    private float targetZoom;
    private float currentZoom;

    private void Start()
    {
        targetZoom = orbital.Radius;
        currentZoom = targetZoom;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        scrollDate = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        SetTargetZoom();
        
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, zoomLerpSpeed * Time.deltaTime);
        orbital.Radius = currentZoom;
    }


    private void SetTargetZoom()
    {
        if(scrollDate.y == 0f || !orbital)
            return;
        
        targetZoom = Mathf.Clamp(orbital.Radius - scrollDate.y * zoomSpeed, minDistance, maxDistance);
        scrollDate = Vector2.zero;
    }
}
