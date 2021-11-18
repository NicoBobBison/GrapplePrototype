using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleStartState : PlayerGrappleState
{
    Coroutine wait;
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
        wait = player.StartCoroutine(player.SlowToStop(playerData.grappleStallTime));
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityX(player.CurrentVelocity.x / 1.3f);
        player.SetVelocityY(player.CurrentVelocity.y / 2);
        player.EndCoroutine(wait);
        player.slowingFromGrapple = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!player.slowingFromGrapple)
        {
            if (Input.GetMouseButtonUp(0) || Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) < 0.75f)
            {
                PlayerGrapple pg = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
                pg.SetGrappleState(PlayerGrapple.GrapplingState.unattached);
                stateMachine.ChangeState(player.JumpSustainState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!player.slowingFromGrapple)
        {
            player.LerpVelocityX(playerGrapple.grappleDir.x * playerData.playerReelSpeed, 0.9f, false);
            player.LerpVelocityY(playerGrapple.grappleDir.y * playerData.playerReelSpeed, 0.9f, false);
        }
    }
}
