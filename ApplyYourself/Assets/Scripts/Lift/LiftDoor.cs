using System;
using Unity.Cinemachine;
using UnityEngine;

public class LiftDoor : MonoBehaviour
{
    [SerializeField] private float stayTime = 4f;
    [SerializeField] private Transform liftDoor;
    [SerializeField] private MoveLift moveLift;
    [SerializeField] private CinemachineCamera liftCamera;
    private Transform player;

    private float enterTime = 0f;

    private void Start()
    {
        liftDoor.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Player"))
        {
            enterTime = Time.time;
            liftCamera.Priority = 2;
            
            if(player == null)
                player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag($"Player"))
        {
            enterTime = 0f;
            liftCamera.Priority = 0;
        }
    }


    private void Update()
    {
        if (moveLift.IsFinished())
        {
            liftDoor.gameObject.SetActive(false);
        }
        else if (!moveLift.IsMoving() && Time.time - enterTime >= stayTime && enterTime != 0f)
        {
            liftDoor.gameObject.SetActive(true);
            moveLift.StartMoving();
        }
    }
}
