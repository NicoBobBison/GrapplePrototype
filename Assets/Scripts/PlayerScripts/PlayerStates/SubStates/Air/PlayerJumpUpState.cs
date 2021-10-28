using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpUpState : PlayerAirState
{
    public PlayerJumpUpState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityY(playerData.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time - startTime >= 0.2)
        {
            // Fix this by checking for ground and not allowing "autojumping"
            stateMachine.ChangeState(player.JumpSustainState);
        }
        if(player.MoveInput.y != 1)
        {
            player.SetVelocityY(3.0f);
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(playerData.movementSpeedX * player.MoveInput.x);
        player.SetVelocityY(playerData.jumpForce);
    }
}
