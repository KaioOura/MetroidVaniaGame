using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementScriptableObject", order = 1)]
public class MovementData : ScriptableObject
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpHorizontalForce;
    [SerializeField] private float _jumpCutForce;
    [SerializeField] private float _fallAdditionalForce;
    [SerializeField] private float _maxFallVelocity;
    [SerializeField] private float _maxTimeGettingJumpForce;
    [SerializeField] private float _maxTimeWallJump;
    [SerializeField] private float _maxHorizontalVelocity;
    [SerializeField] private PhysicsOptions _physicsOptions;

    public float Speed => _speed;
    public float SpeedMultiplier => _speedMultiplier;
    public float JumpForce => _jumpForce;
    public float JumpHorizontalForce => _jumpHorizontalForce;
    public float JumpCutForce => _jumpCutForce;
    public float FallAdditionalForce => _fallAdditionalForce;
    public float MaxFallVelocity => _maxFallVelocity;
    public float MaxTimeGettingJumpForce => _maxTimeGettingJumpForce;
    public float MaxTimeWallJump => _maxTimeWallJump;
    public float MaxHorizontalVelocity => _maxHorizontalVelocity;
    public PhysicsOptions PhysicsOptions => _physicsOptions;
}
