using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementScriptableObject", order = 1)]
public class MovementData : ScriptableObject
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCutForce;
    [SerializeField] private float _fallAdditionalForce;
    [SerializeField] private float _maxFallVelocity;
    [SerializeField] private float _maxTimeGettingJumpForce;

    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float JumpCutForce => _jumpCutForce;
    public float FallAdditionalForce => _fallAdditionalForce;
    public float MaxFallVelocity => _maxFallVelocity;
    public float MaxTimeGettingJumpForce => _maxTimeGettingJumpForce;
}
