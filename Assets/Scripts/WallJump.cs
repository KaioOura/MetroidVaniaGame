using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private Transform _wallCheckTransform;
    [SerializeField] private WallJumpData _wallJumpData;
    [SerializeField] private MovementData _wallMovementData;
    [SerializeField] private ActionData _wallSlideActionData;
    private bool _isOnGround;
    private CharacterState _characterState;
    
    public event Action<CharState> OnStateChanging;
    public event Action<MovementData> OnUpdateMovementSettings;
    public event Action<ActionData, Action> OnWallSlidePerformed;
    
    public void Init(CharacterState characterState)
    {
        _characterState = characterState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanCheckWall())
            return;

        //cast ray to check wall
        bool isCloseToWall = Physics2D.Raycast(_wallCheckTransform.position, transform.parent.right, _wallJumpData.CheckDistance, _wallJumpData.WallCheckLayerMask);
        if (!isCloseToWall)
            return;

        HandleWallJump();
    }

    void HandleWallJump()
    {
        OnStateChanging?.Invoke(CharState.WallSliding);
        OnUpdateMovementSettings?.Invoke(_wallMovementData);
        OnWallSlidePerformed.Invoke(_wallSlideActionData, null);
    }

    bool CanCheckWall()
    {
        return _characterState.CharState is CharState.Jumping or CharState.DoubleJumping && !_isOnGround;
    }

    public void IsOnGroundUpdate(bool IsOnGround)
    {
        _isOnGround = IsOnGround;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        bool ledgeFound = Physics2D.Raycast(_wallCheckTransform.position, transform.parent.right, 1, _wallJumpData.WallCheckLayerMask); 
        Gizmos.DrawRay(_wallCheckTransform.position, transform.parent.right * _wallJumpData.CheckDistance);
    }
}
