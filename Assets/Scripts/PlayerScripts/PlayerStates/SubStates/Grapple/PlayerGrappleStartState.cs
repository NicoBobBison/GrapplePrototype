using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleStartState : PlayerGrappleState
{
    Coroutine wait;
    float startDistance;

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
        Debug.Log("Start state");
        startDistance = Vector2.Distance(playerGrapple.lastHit.point, player.transform.position);
        wait = player.StartCoroutine(player.SlowToStop(playerData.grappleStallTime, 0.05f, true));
        player.SetGravity(0);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityX(player.CurrentVelocity.x / 1.3f);
        player.SetVelocityY(player.CurrentVelocity.y / 2);
        player.StopCoroutine(wait);
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
        if (!player.slowingFromGrapple)
        {
            if (playerGrapple.lastHit.collider.gameObject.layer != LayerMask.NameToLayer("Platform") &&
                playerGrapple.lastHit.collider.gameObject.layer != LayerMask.NameToLayer("GrapplePoint"))
            {
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) > 0.33f)
                {
                    //player.LerpVelocityX(playerGrapple.grappleDir.x * playerData.playerReelSpeed *
                    //    Vector2.Distance(playerGrapple.grapplePoint, player.transform.position), 0.9f, false);
                    //player.LerpVelocityY(playerGrapple.grappleDir.y * playerData.playerReelSpeed *
                    //    Vector2.Distance(playerGrapple.grapplePoint, player.transform.position), 0.9f, false);

                    player.SetVelocityX(playerGrapple.grappleDir.x * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)));
                    player.SetVelocityY(playerGrapple.grappleDir.y * (playerData.playerReelSpeed *
                        Vector2.Distance(playerGrapple.grapplePoint, player.transform.position)));
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
                    float xVel = playerGrapple.grappleDir.x * playerData.playerReelSpeed;
                    float yVel = playerGrapple.grappleDir.y * playerData.playerReelSpeed;
                    if (xVel < 8 && xVel > 0)
                    {
                        xVel = 8;
                    }
                    else if(xVel > -8 && xVel < 0)
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
                    player.SetVelocityX(xVel);
                    player.SetVelocityY(yVel);
                }
                else
                {
                    Debug.Log("End platform grapple. Direction is: " + playerGrapple.grappleDir);
                    player.AddForceTo(playerGrapple.grappleDir, 10);
                    playerGrapple.EndGrapple(player.GrappleAirState);
                }
            }
        }
        
    }
}