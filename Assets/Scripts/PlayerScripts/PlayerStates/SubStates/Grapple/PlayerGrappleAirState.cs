using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleAirState : PlayerGrappleState
{
    public PlayerGrappleAirState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (player.CheckIfGrounded())
        {
            if(player.MoveInput.x != 0)
            {
                player.StateMachine.ChangeState(player.MoveState);
            }
            else
            {
                player.StateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravity(playerData.baseGravity);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.LerpVelocityX(playerData.movementSpeedX * player.MoveInput.x + player.CurrentVelocity.x, 0.15f, true);
        if (player.CurrentVelocity.y < playerData.maxFallSpeed)
        {
            player.SetVelocityY(playerData.maxFallSpeed);
        }
    }
}
