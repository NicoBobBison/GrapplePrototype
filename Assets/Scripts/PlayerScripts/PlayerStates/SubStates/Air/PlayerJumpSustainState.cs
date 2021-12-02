using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSustainState : PlayerAirState
{
    float originalGravity;
    public PlayerJumpSustainState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravity(playerData.baseGravity);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time - startTime >= 0.2f)
        {
            stateMachine.ChangeState(player.FallState);
        }
        if (isGrounded)
        {
            if (input.x == 1 || input.x == -1)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(player.CurrentVelocity.y >=-0.2 && player.CurrentVelocity.y <= 0.2)
        {
            player.SetGravity(1.5f);
        }
        else
        {
            player.SetGravity(playerData.baseGravity);
        }
        if (player.slowingFromGrapple)
        {
            player.SetGravity(0);
        }
        else
        {
            player.SetGravity(playerData.baseGravity);
        }
    }
}
