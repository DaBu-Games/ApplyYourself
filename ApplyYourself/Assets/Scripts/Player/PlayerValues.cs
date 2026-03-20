using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "Player/values")]
public class PlayerValues : ScriptableObject
{
    [Header("Rotation")] 
    public float RotationSpeed = 10f;
    
    [Header("Walk values")]
    public float MaxWalkSpeed = 6f;
    public float WalkAcceleration = 1.5f;
    
    [Header("Idle values")]
    public float Deceleration = 1.5f;
    
    [Header("Run Values")]
    public float MaxRunSpeed = 2f;
    public float RunAcceleration = 1.5f;
    
    [Header("Jump values")]
    public float JumpInputBufferTime = 0.15f;
    public float LeaveGroundBufferTime = 0.15f;
    public float JumpForce = 1.5f;
    
    [Header("Climb values")]
    public float MaxClimbSpeed = 6f;
    public float ClimbAcceleration = 1.5f;
    public float ClimbStickForce = 1.5f;
    
    [Header("physics values")]
    public float Gravity = -9.81f;
    public float FallingGravity = -12.85f;
}
