using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleStartState : PlayerGrappleState
{
    public PlayerGrappleStartState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetMouseButtonUp(0) || !playerGrapple.GrappleOnObject())
        {
            stateMachine.ChangeState(player.JumpSustainState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.LerpVelocityX(playerGrapple.endpoint.x * playerData.playerReelSpeed, 0.9f, false);
        player.LerpVelocityY(playerGrapple.endpoint.y * playerData.playerReelSpeed, 0.9f, false);
    }

}
