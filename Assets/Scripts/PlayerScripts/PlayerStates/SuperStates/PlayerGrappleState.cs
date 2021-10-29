using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleState : PlayerState
{
    public PlayerGrapple playerGrapple;
    public PlayerGrappleState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerGrapple = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
        player.SetGravity(0);
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
    }
}
