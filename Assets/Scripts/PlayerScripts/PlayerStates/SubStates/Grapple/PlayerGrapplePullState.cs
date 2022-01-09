using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplePullState : PlayerGrappleState
{
    PlayerGrapple pg;
    public PlayerGrapplePullState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        pg = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
        Pullable pull = pg.lastHitObject.GetComponent<Pullable>();
        pull.StopAllCoroutines();
        pull.StartCoroutine(pull.Transition());
        player.SetGravity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetMouseButtonUp(0) || Time.time - startTime >= 0.3)
        {
            pg.SetGrappleState(PlayerGrapple.GrapplingState.unattached);
            stateMachine.ChangeState(player.JumpSustainState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }
}
