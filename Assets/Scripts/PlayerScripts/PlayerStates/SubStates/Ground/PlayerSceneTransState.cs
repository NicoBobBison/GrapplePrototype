using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneTransState : PlayerState
{
    public PlayerSceneTransState(PlayerControls player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
