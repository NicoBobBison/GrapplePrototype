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
        if (Input.GetMouseButtonUp(0))
        {
            stateMachine.ChangeState(player.JumpSustainState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.LerpVelocityX(player.GetMouseDirection().x * playerData.playerReelSpeed, 0.9f, false);
        player.LerpVelocityY(player.GetMouseDirection().y * playerData.playerReelSpeed, 0.9f, false);
    }

}
