using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        player.CheckIfShouldFlip((int)input.x);
        if(input.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        var below = GetObjectBelow();
        float xVelBelow = 0;
        if(below != null)
        {
            xVelBelow = GetObjectBelow().GetComponent<Rigidbody2D>().velocity.x;
        }
        
        player.LerpVelocityX((playerData.movementSpeedX * player.MoveInput.x) + xVelBelow, 0.15f, false);
    }
    private GameObject GetObjectBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down, 1, LayerMask.GetMask("Ground", "Platform", "MovingPlatform"));
        if(hit)
            return hit.collider.gameObject;
        return null;
    }
       
}
