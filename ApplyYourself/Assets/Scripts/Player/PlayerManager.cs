using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerValues playerValues;
    
    private StateMachine stateMachine;
    
    private IdleState idleState;
    private InAirState inAirState;
    private JumpingState jumpingState;
    private RunningState runningState;
    private WalkingState walkingState;

    private void Start()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(playerInput, playerValues);
        inAirState = new InAirState(playerInput, playerValues);
        jumpingState = new JumpingState(playerInput, playerValues);
        runningState = new RunningState(playerInput, playerValues);
        walkingState = new WalkingState(playerInput, playerValues);
        
        // idle transitions
        stateMachine.AddTransition(new Transition(
            idleState,
            inAirState,
            () => !playerInput.IsGrounded
        ));
        
        stateMachine.AddTransition(new Transition(
            idleState,
            jumpingState,
            () => playerInput.IsJumpBufferd() && playerInput.CanBufferJump()
        ));
        
        stateMachine.AddTransition(new Transition(
            idleState,
            runningState,
            () => playerInput.MoveInput != Vector2.zero && playerInput.IsHoldingRun
        ));
        
        stateMachine.AddTransition(new Transition(
            idleState,
            walkingState,
            () => playerInput.MoveInput != Vector2.zero
        ));
        
        // in air transitions
        stateMachine.AddTransition(new Transition(
            inAirState,
            idleState,
            () => playerInput.IsGrounded && playerInput.MoveInput == Vector2.zero
        ));
        
        stateMachine.AddTransition(new Transition(
            inAirState,
            jumpingState,
            () => playerInput.IsJumpBufferd() && playerInput.CanBufferJump() && !playerInput.IsJumping
        ));
        
        stateMachine.AddTransition(new Transition(
            inAirState,
            walkingState,
            () => playerInput.IsGrounded && playerInput.MoveInput != Vector2.zero
        ));
        
        stateMachine.AddTransition(new Transition(
            inAirState,
            runningState,
            () => playerInput.IsGrounded && playerInput.MoveInput != Vector2.zero && playerInput.IsHoldingRun
        ));
        
        
        // jumping transition
        stateMachine.AddTransition(new Transition(
            jumpingState,
            inAirState,
            () => !playerInput.IsGrounded
        ));
        
        // running transition
        stateMachine.AddTransition(new Transition(
            runningState,
            idleState,
            () => playerInput.MoveInput == Vector2.zero
        ));
        
        stateMachine.AddTransition(new Transition(
            runningState,
            walkingState,
            () => playerInput.MoveInput != Vector2.zero && !playerInput.IsHoldingRun
        ));
        
        stateMachine.AddTransition(new Transition(
            runningState,
            inAirState,
            () => !playerInput.IsGrounded
        ));
        
        stateMachine.AddTransition(new Transition(
            runningState,
            jumpingState,
            () => playerInput.IsJumpBufferd() && playerInput.CanBufferJump()
        ));
        
        // walking transition
        stateMachine.AddTransition(new Transition(
            walkingState,
            idleState,
            () => playerInput.MoveInput == Vector2.zero
        ));
        
        stateMachine.AddTransition(new Transition(
            walkingState,
            runningState,
            () => playerInput.MoveInput != Vector2.zero && playerInput.IsHoldingRun
        ));
        
        stateMachine.AddTransition(new Transition(
            walkingState,
            inAirState,
            () => !playerInput.IsGrounded
        ));
        
        stateMachine.AddTransition(new Transition(
            walkingState,
            jumpingState,
            () => playerInput.IsJumpBufferd() && playerInput.CanBufferJump()
        ));
        
        
        stateMachine.SwitchState(idleState);
    }

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }
}