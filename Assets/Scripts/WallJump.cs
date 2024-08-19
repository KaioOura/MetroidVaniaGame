using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private Transform _wallCheckTransform;
    [SerializeField] private WallJumpData _wallJumpData;
    [SerializeField] private MovementData _wallSlideMovementData;
    [SerializeField] private ActionData _wallSlideActionData;
    [SerializeField] private ActionData _idleActionData;
    [SerializeField] private LayerMask _wallCheckLayerMask;
    [SerializeField] private float _checkDistance = 0.4f;

    public LayerMask WallCheckLayerMask => _wallCheckLayerMask;

    private bool _isOnGround;
    private CharacterState _characterState;

    public event Action<CharState> OnStateChanging;
    public Action<PhysicsOptions> OnRequestPhysicsChanging;
    private float _lastTimeWallSlide;

    public event Action<MovementData> OnUpdateMovementSettings;
    public event Action<ActionData, Action> OnRequestStateChanging;

    public void Init(CharacterState characterState)
    {
        _characterState = characterState;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanCheckWallSlide())
        {
            bool isCloseToWall1 = Physics2D.Raycast(new Vector2(_wallCheckTransform.position.x, _wallCheckTransform.position.y + _wallJumpData.OffSetYEmptySpace), transform.parent.right, 0.4f, _wallCheckLayerMask);
            if (isCloseToWall1)
                return;
            OnRequestStateChanging?.Invoke(_idleActionData, null);
            OnUpdateMovementSettings?.Invoke(null);
            _lastTimeWallSlide = Time.time + 0.5f;            

            return;
        }

        if (!CanCheckWall())
            return;

        //cast ray to check wall
        bool isCloseToWall = Physics2D.Raycast(_wallCheckTransform.position, transform.parent.right, 0.4f, _wallCheckLayerMask);
        if (!isCloseToWall)
            return;

        HandleWallSlide();
    }

    void HandleWallSlide()
    {
        OnStateChanging?.Invoke(CharState.WallSliding);
        OnUpdateMovementSettings?.Invoke(_wallSlideMovementData);
        OnRequestPhysicsChanging.Invoke(_wallSlideActionData.PhysicsOptions);
        OnRequestStateChanging.Invoke(_wallSlideActionData, null);
    }

    bool CanCheckWallSlide()
    {
        return _characterState.CharState is CharState.WallSliding;
    }

    bool CanCheckWall()
    {
        bool isOnCooldown = Time.time < _lastTimeWallSlide + 0.2f;
        return _characterState.CharState is CharState.Jumping or CharState.DoubleJumping or CharState.Falling 
        && !_isOnGround && !isOnCooldown;
    }

    public void JumpPerformed()
    {
        _lastTimeWallSlide = Time.time;
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
        Gizmos.DrawRay(new Vector2(_wallCheckTransform.position.x, _wallCheckTransform.position.y + _wallJumpData.OffSetYEmptySpace), transform.parent.right * _wallJumpData.CheckDistance);
    }
}
