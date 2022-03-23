using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 input;
    protected bool holdingUp = false;
    protected bool isGrounded = true;
    public PlayerGroundedState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        if(input.y == 1)
        {
            holdingUp = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        input = player.MoveInput;
        if(input.y != 1)
        {
            holdingUp = false;
        }
        if (input.y == 1 && isGrounded && !holdingUp)
        {
            stateMachine.ChangeState(player.JumpUpState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.CurrentVelocity.y < playerData.maxFallSpeed)
        {
            player.SetVelocityY(playerData.maxFallSpeed);
        }
    }
}
