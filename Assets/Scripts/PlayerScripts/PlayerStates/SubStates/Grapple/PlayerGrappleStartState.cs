using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleStartState : PlayerGrappleState
{
    Coroutine wait;
    float startDistance;
    Vector2 direction;

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
        Debug.Log("Enter grapple");
        AudioManager.instance.Play("GrappleSound");
        startDistance = Vector2.Distance(playerGrapple.lastHitPoint, player.transform.position);
        if (playerData.slowBeforeGrapple)
        {
            wait = player.StartCoroutine(player.FullStop(playerData.grappleStallTime, true));
        }
        else
        {
            player.SetVelocityX(0);
            player.SetVelocityY(0);
        }
        player.SetGravity(0);
        direction = player.MoveInput;
        direction.Normalize();

    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityX(player.CurrentVelocity.x / 2);
        player.SetVelocityY(player.CurrentVelocity.y / 2);
        if (playerData.slowBeforeGrapple && wait != null)
        {
            player.StopCoroutine(wait);
        }
        player.slowingFromGrapple = false;
        if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) < 0.75f)
        {
            //player.CollideDuringGrapplePS.Play();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!playerData.slowBeforeGrapple || (playerData.slowBeforeGrapple && !player.slowingFromGrapple))
        {
            playerGrapple.UpdateGrappleDir();

            if (Vector2.Distance(player.transform.position, playerGrapple.grapplePoint) > startDistance + 0.5f)
            {
                player.StateMachine.ChangeState(player.GrappleAirState);
            }
            if(playerGrapple.lastHitObject.layer == LayerMask.NameToLayer("Chains"))
            {
                player.SetVelocityX(Mathf.Clamp(playerGrapple.grappleDir.x * playerData.playerReelSpeed, -playerData.playerReelSpeed, playerData.playerReelSpeed));
                player.SetVelocityY(Mathf.Clamp(playerGrapple.grappleDir.y * playerData.playerReelSpeed, -playerData.playerReelSpeed, playerData.playerReelSpeed));
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) < 0.5f)
                {
                    playerGrapple.EndGrapple(player.JumpSustainState);

                }
            }
            else
            {
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) > 0.5f)
                {
                    player.SetVelocityX(Mathf.Clamp(playerGrapple.grappleDir.x * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)), -playerData.playerReelSpeed, playerData.playerReelSpeed));
                    player.SetVelocityY(Mathf.Clamp(playerGrapple.grappleDir.y * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)), -playerData.playerReelSpeed, playerData.playerReelSpeed));
                }
                else
                {
                    playerGrapple.EndGrapple(player.JumpSustainState);
                }
            }
            
        }
    }
}