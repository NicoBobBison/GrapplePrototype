using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    protected Vector2 input;
    protected bool isGrounded;
    public PlayerAirState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.CheckIfShouldFlip((int)input.x);
        input = player.MoveInput;
        if(player.CurrentVelocity.y < playerData.maxFallSpeed)
        {
            player.SetVelocityY(playerData.maxFallSpeed);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.LerpVelocityX(playerData.movementSpeedX * player.MoveInput.x, 0.15f, true);
    }
}
