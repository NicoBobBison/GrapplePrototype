using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleSlowdownState : PlayerGrappleState
{
    Coroutine co;
    public PlayerGrappleSlowdownState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        co = player.StartCoroutine(player.SlowToStop(2, 0.03f, false));
    }

    public override void Exit()
    {
        base.Exit();
        player.StopCoroutine(co);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Vector2.Distance(playerGrapple.lastHit.point, player.transform.position) < 1 || Time.time - startTime >= 2)
        {
            PlayerGrapple pg = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
            pg.SetGrappleState(PlayerGrapple.GrapplingState.unattached);
            stateMachine.ChangeState(player.JumpSustainState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
