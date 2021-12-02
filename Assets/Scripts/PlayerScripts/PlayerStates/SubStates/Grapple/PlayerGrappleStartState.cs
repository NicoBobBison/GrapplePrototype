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
        startDistance = Vector2.Distance(playerGrapple.lastHit.point, player.transform.position);
        wait = player.StartCoroutine(player.SlowToStop(playerData.grappleStallTime, 0.05f, true));
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
        if (!player.slowingFromGrapple)
        {
            if (Input.GetMouseButtonUp(0))
            {
                playerGrapple.SlowThenEndGrapple();
            }
        }
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
                    playerGrapple.EndGrapple();
                }
            }
            else
            {
                if (Vector2.Distance(playerGrapple.grapplePoint, player.transform.position) > 0.6f)
                {
                    float xVel = playerGrapple.grappleDir.x * playerData.playerReelSpeed * startDistance;
                    if(xVel < 8 && xVel > 0)
                    {
                        Mathf.Clamp(xVel, 8, 100);
                    }
                    else if(xVel > -8 && xVel <= 0)
                    {
                        Mathf.Clamp(xVel, -8, -100);
                    }
                    float yVel = playerGrapple.grappleDir.y * playerData.playerReelSpeed * startDistance;
                    if (yVel < 8 && yVel > 0)
                    {
                        Mathf.Clamp(yVel, 8, 100);
                    }
                    else if (yVel > -8 && yVel <= 0)
                    {
                        Mathf.Clamp(yVel, -8, -100);
                    }
                    player.SetVelocityX(xVel);
                    player.SetVelocityY(yVel);
                }
                else
                {
                    playerGrapple.EndGrapple();
                }
            }
        }
        
    }
}