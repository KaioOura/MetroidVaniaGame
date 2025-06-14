using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Jump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private MovementData _standardMovementData;
    [SerializeField] private MovementData _wallJumpMovementData;
    [SerializeField] private ActionData _jumpActionData;
    [SerializeField] private ActionData _wallJumpActionData;

    private CharacterState _currentState;

    private float _wallJumpTimeTracker;
    private float _pressingJumpTimeTracker;
    private bool _isOnGround;
    private float _jumpCounts;
    private float _jumpCutForce;
    private float _jumpForce;
    private float _jumpHorizontalForce;
    private float _maxTimeGettingJumpForce;
    private float _fallAdditionalForce;
    private float _maxFallVelocity;

    private IEnumerator _wallJumpCoroutine;

    public Rigidbody2D RB => _rb;

    public event Action<CharState> OnChangeStateChanging;
    public event Action<ActionData, Action> OnJumpPerformed;
    public event Action<ActionData, Action> OnWallJumpPerformed;
    public event Action<bool> OnRequestFacingChange;
    public event Action<MovementData> OnUpdateMovementModifiers;

    public void Init(CharacterState currentState)
    {
        _currentState = currentState;

        UpdateJumpModifiers(_standardMovementData);
    }

    void FixedUpdate()
    {
        if (!CanHandleFalling())
            return;

        HandleFalling();
    }

    void HandleFalling()
    {
        if (_rb.linearVelocity.y > 0 && Time.time >= _pressingJumpTimeTracker)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y / _jumpCutForce);
        }

        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y <= -_maxFallVelocity ? -_maxFallVelocity : _rb.linearVelocity.y * _fallAdditionalForce);
        }
    }

    public void OnJumpInput(bool isPressing)
    {
        HandleJump(isPressing);
    }

    void HandleJump(bool isPressing)
    {
        if (!isPressing)
        {
            //Cortar impulso
            if (_rb.linearVelocity.y > 0 && _currentState.CharState is not CharState.WallJumping)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y / _jumpCutForce);

        }
        else
            GetJump();
    }

    void GetJump()
    {
        if (CanWallJump())
        {
            WallJump();
            return;
        }

        if (CanDoubleJump())
        {
            DoJump(isDoubleJump: true);
            _jumpCounts = 2;
            return;
        }

        if (CanLedgeJump())
        {
            DoJump();
            return;
        }

        if (CanJump())
        {
            DoJump();
            return;
        }

        return;
    }
    void DoJump(bool isDoubleJump = false)
    {
        OnUpdateMovementModifiers(_standardMovementData);
        UpdateJumpModifiers(_standardMovementData);
        ResetJumpCount();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _pressingJumpTimeTracker = Time.time + _maxTimeGettingJumpForce;
        OnJumpPerformed?.Invoke(_jumpActionData, null);
        OnChangeStateChanging?.Invoke(isDoubleJump ? CharState.DoubleJumping : CharState.Jumping);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        //Debug.Log("Jump");
    }

    void WallJump()
    {
        OnUpdateMovementModifiers(_wallJumpMovementData);
        UpdateJumpModifiers(_wallJumpMovementData);
        ResetJumpCount();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        bool faceRight = transform.parent.eulerAngles.y == 180 ? true : false;

        OnRequestFacingChange?.Invoke(faceRight);

        int i = faceRight ? 1 : -1;
        Vector2 wallJumpForce = new Vector2(_jumpHorizontalForce * i, _jumpForce);

        _rb.AddForce(wallJumpForce, ForceMode2D.Impulse);
        _pressingJumpTimeTracker = Time.time + _maxTimeGettingJumpForce;

        OnChangeStateChanging?.Invoke(CharState.WallJumping);
        OnWallJumpPerformed?.Invoke(_wallJumpActionData, null);

        CalcWallJumpTime();
    }

    bool CanJump()
    {
        return _isOnGround && _currentState.CharState is CharState.Free;
    }

    bool CanDoubleJump()
    {
        return _currentState.CharState is CharState.Jumping or CharState.Falling or CharState.WallJumping or CharState.Dashing
        && _upgradeManager.HasUpgrade(UpgradeEnum.DoubleJump)
        && _jumpCounts < 2;
    }

    bool CanLedgeJump()
    {
        return !_isOnGround && _currentState.CharState is CharState.LedgeClimbing;
    }

    bool CanWallJump()
    {
        return !_isOnGround && _currentState.CharState is CharState.WallSliding;
    }

    bool CanHandleFalling()
    {
        return _currentState.CharState is not CharState.WallJumping;
    }


    public void UpdateJumpModifiers(MovementData movementData)
    {
        if (movementData == null)
            movementData = _standardMovementData;

        _jumpForce = movementData.JumpForce;
        _jumpHorizontalForce = movementData.JumpHorizontalForce;
        _jumpCutForce = movementData.JumpCutForce;
        _fallAdditionalForce = movementData.FallAdditionalForce;
        _maxFallVelocity = movementData.MaxFallVelocity;
        _maxTimeGettingJumpForce = movementData.MaxTimeGettingJumpForce;
    }

    void CalcWallJumpTime()
    {
        if (_wallJumpCoroutine != null)
            StartCoroutine(_wallJumpCoroutine);

        _wallJumpCoroutine = WallJumpRoutine();
        StartCoroutine(_wallJumpCoroutine);
    }

    IEnumerator WallJumpRoutine()
    {
        _wallJumpTimeTracker = Time.time;

        while (Time.time < _wallJumpTimeTracker + _wallJumpMovementData.MaxTimeWallJump)
        {
            yield return null;
        }

        if (_currentState.CharState is not CharState.WallJumping)
            yield break;

        OnEndWallJump();
        ResetModifiers();
    }


    void OnEndWallJump()
    {
        OnChangeStateChanging.Invoke(CharState.Falling);
    }
    void ResetModifiers()
    {
        OnUpdateMovementModifiers(_standardMovementData);
        UpdateJumpModifiers(_standardMovementData);
    }

    public void ResetJumpCount()
    {
        _jumpCounts = 1;
    }

    public void IsOnGroundUpdate(bool isGround)
    {
        _isOnGround = isGround;

        if (isGround)
            _jumpCounts = 0;
    }
}
