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
        Debug.Log("Enter grapple start");
        startDistance = Vector2.Distance(playerGrapple.lastHitPoint, player.transform.position);
        if (playerData.slowBeforeGrapple)
        {
            wait = player.StartCoroutine(player.SlowToStop(playerData.grappleStallTime, 0.09f, true));
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
        Debug.Log("Exit grapple");
        player.SetVelocityX(player.CurrentVelocity.x / 2);
        player.SetVelocityY(player.CurrentVelocity.y / 2);
        if (playerData.slowBeforeGrapple)
        {
            player.StopCoroutine(wait);
        }
        player.slowingFromGrapple = false;
        if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) < 0.75f)
        {
            player.CollideDuringGrapplePS.Play();
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
            if (Vector2.Distance(player.transform.position, playerGrapple.grapplePoint) > startDistance + 0.5f)
            {
                player.StateMachine.ChangeState(player.GrappleAirState);
            }
            if (!player.IsInLayerMask(playerGrapple.lastHitObject, playerGrapple.grappleDontSlow))
            {
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) > 0.5f)
                {
                    player.SetVelocityX(Mathf.Clamp(direction.x * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)), -playerData.playerReelSpeed, playerData.playerReelSpeed));
                    player.SetVelocityY(Mathf.Clamp(direction.y * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)), -playerData.playerReelSpeed, playerData.playerReelSpeed));
                }
                else
                {
                    playerGrapple.EndGrapple(player.JumpSustainState);
                }
            }
            else
            {
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) > 0.3f)
                {
                    float xVel = direction.x * playerData.playerReelSpeed;
                    float yVel = direction.y * playerData.playerReelSpeed;
                    if (xVel < 8 && xVel > 0)
                    {
                        xVel = 8;
                    }
                    else if (xVel > -8 && xVel < 0)
                    {
                        xVel = -8;
                    }
                    if (yVel < 8 && yVel > 0)
                    {
                        yVel = 8;
                    }
                    else if (yVel > -8 && yVel < 0)
                    {
                        yVel = -8;
                    }
                    player.SetVelocityX(Mathf.Clamp(xVel, -playerData.playerReelSpeed, playerData.playerReelSpeed));
                    player.SetVelocityY(Mathf.Clamp(yVel, -playerData.playerReelSpeed, playerData.playerReelSpeed));
                }
                else
                {
                    playerGrapple.EndGrapple(player.GrappleAirState);
                }
            }
        }
    }
}