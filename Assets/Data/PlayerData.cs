using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementSpeedX = 3f;

    [Header("Jumping")]
    public float baseGravity = 2.0f;
    public float jumpForce = 5f;
    public float maxFallSpeed = -6f;

    [Header("Dashing")]
    public float maxDashSpeed = 5f;
    public float dashTime = 1f;
    public float distanceBetweenAfterImages = 0.05f;

    [Header("Stamina")]
    public float staminaRechargeRate = 0.1f;
    public float staminaDrainRate = 0.1f;
    public float maxStamina = 100;

    [Header("Grapple")]
    public float playerReelSpeed = 3;
    public float playerPullSpeed = 5;
    public float grappleStallTime = 0.2f;
    public float grappleMaxDistance = 5;
    public float chainGrabDistance = 1;
    public float grappleWidth = 0.1f;
    public Color grappleColor;
    public bool slowBeforeGrapple = false;
    public bool unlockedGrapple = false;
    
}
