using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private MovementData _movementData;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private FootCollider _footCollider;


    private AnimatorRef _animatorRef;
    private CharacterState _currentState;

    private Vector2 Move;
    private float _pressingJumpTimeTracker;

    public event Action<CharState> OnChangeStateChanging;

    // Start is called before the first frame update
    void Start()
    {
        _footCollider.FootRadius = _movementData.FootRadius;
    }

    // Update is called once per frame
    void Update()
    {
        CalcMovement();
    }

    void FixedUpdate()
    {
        //
        HandleFalling();
    }

    public void Init(AnimatorRef animatorRef, CharacterState currentState)
    {
        _animatorRef = animatorRef;
        _currentState = currentState;
    }

    public void OnMoveInput(Vector2 input)
    {
        Move = input;
    }

    void CalcMovement()
    {
        if (CanWalk())
            ApplyMovementVelocity();

        _animatorRef.Animator.SetFloat(AnimatorRef.MoveParam, Mathf.Abs(_rb.velocity.x));
        _animatorRef.Animator.SetFloat(AnimatorRef.VelYParam, _rb.velocity.y);
        _animatorRef.Animator.SetBool(AnimatorRef.OnGroundParam, _footCollider.IsOnGround);
    }

    void ApplyMovementVelocity()
    {
        FacingHandler(Move);

        _rb.velocity = new Vector2(Move.x * (_movementData.Speed * Time.fixedDeltaTime), _rb.velocity.y);
    }

    void FacingHandler(Vector2 dir)
    {
        if (_currentState.CharState is CharState.Attack or CharState.AirAttack)
            return;

        if (dir.x > 0.1f && _animatorRef.MainTransform.eulerAngles.y != 0)
            _animatorRef.MainTransform.eulerAngles = new Vector2(_animatorRef.MainTransform.eulerAngles.x, 0);
        else if (dir.x < -0.1f && _animatorRef.MainTransform.eulerAngles.y != 180)
            _animatorRef.MainTransform.eulerAngles = new Vector2(_animatorRef.MainTransform.eulerAngles.x, 180);
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
            if (_rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / _movementData.JumpCutForce);
            }

        }
        else
            Jump(CanJump(), CanDoubleJump());

    }

    void Jump(bool CanJump, bool CanDoubleJump)
    {
        if (!CanJump && !CanDoubleJump)
            return;

        _rb.AddForce(Vector2.up * _movementData.JumpForce, ForceMode2D.Impulse);
        _pressingJumpTimeTracker = Time.time + _movementData.MaxTimeGettingJumpForce;
        _animatorRef.Animator.Play(AnimatorRef.JumpState);
        OnChangeStateChanging?.Invoke(CanDoubleJump ? CharState.DoubleJumping : CharState.Jumping);
    }

    void HandleFalling()
    {
        if (_rb.velocity.y > 0 && Time.time >= _pressingJumpTimeTracker)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / _movementData.JumpCutForce);
        }

        if (_rb.velocity.y < 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y <= -_movementData.MaxFallVelocity ? -_movementData.MaxFallVelocity : _rb.velocity.y * _movementData.FallAdditionalForce);   
        }
    }

    bool CanJump()
    {
        return _footCollider.IsOnGround && _currentState.CharState == CharState.Free;
    }

    bool CanDoubleJump()
    {
        return _currentState.CharState == CharState.Jumping && _upgradeManager.HasUpgrade(UpgradeEnum.DoubleJump);
    }

    bool CanWalk()
    {
        return _currentState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping or CharState.AirAttack;
    }

    public void OnStopVelocity(bool isHorizontal, bool isVertical)
    {
        if (isHorizontal)
            _rb.velocity = new Vector2(0, _rb.velocity.y);

        if (isVertical)
            _rb.velocity = new Vector2(_rb.velocity.x , 0);

    }

    public void ApplyForce(Vector2 force, bool IsLocal)
    {
        bool facingRight = _animatorRef.MainTransform.eulerAngles.y == 0;
        int i = IsLocal == true ? facingRight == true ? 1 : -1 : 1; //Need to refactor


        _rb.AddForce(force * i, ForceMode2D.Impulse);
    }
}
