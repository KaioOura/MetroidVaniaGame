using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
    [SerializeField] private MovementData _movementData;
    [SerializeField] private MovementData _wallJumpMovementData;

    private AnimatorRef _animatorRef;
    private CharacterState _currentState;

    private Vector2 Move;
    private bool _isOnGround;

    private float _speed;

    private float _movementSpeedMultiplier = 0.1f;
    private Vector2 _colliderBoundsCenter;
    private Vector2 _colliderBoundsSize;

    private Vector2 _jumpForceMultiplier;

    public Rigidbody2D RB => _rb;

    public event Action<CharState> OnChangeStateChanging;
    public event Action<MovementData> OnUpdateJumpModifiers;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMovementModifiers(_movementData);
        _colliderBoundsCenter = _capsuleCollider2D.offset;
        _colliderBoundsSize = _capsuleCollider2D.size;
    }

    // Update is called once per frame
    void Update()
    {
        CalcMovement();
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
        if (_currentState.CharState is CharState.WallSliding && _isOnGround)
        {
            OnReceivedPhyscisChanging(_movementData.PhysicsOptions);
            UpdateMovementModifiers(_movementData);
            OnUpdateJumpModifiers?.Invoke(_movementData);
            OnChangeStateChanging.Invoke(CharState.Free);
            _animatorRef.Animator.Play(AnimatorRef.AnimationState.Idle.ToString());
            CheckFacing();
        }


        if (CanWalk())
            ApplyMovementVelocity();

        _animatorRef.Animator.SetFloat(AnimatorRef.MoveParam, Mathf.Abs(_rb.velocity.x));
        _animatorRef.Animator.SetFloat(AnimatorRef.VelYParam, _rb.velocity.y);
        _animatorRef.Animator.SetBool(AnimatorRef.OnGroundParam, _isOnGround);
    }

    void ApplyMovementVelocity()
    {
        FacingHandler(Move);

        if (_currentState.CharState is CharState.WallJumping)
        {
            _movementSpeedMultiplier = Mathf.Lerp(_movementSpeedMultiplier, 5, _wallJumpMovementData.SpeedMultiplier * Time.fixedDeltaTime);

            _rb.AddForce(new Vector2(Move.x * (_speed * Time.fixedDeltaTime) * _movementSpeedMultiplier, 0));


            float velX = Mathf.Clamp(_rb.velocity.x, -_wallJumpMovementData.MaxHorizontalVelocity, _wallJumpMovementData.MaxHorizontalVelocity);

            _rb.velocity = new Vector2(velX, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(Move.x * (_speed * Time.fixedDeltaTime), _rb.velocity.y);
        }

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

    public void FacingHandler(bool shouldFaceRight)
    {
        if (shouldFaceRight)
            _animatorRef.MainTransform.eulerAngles = new Vector2(_animatorRef.MainTransform.eulerAngles.x, 0);
        else
            _animatorRef.MainTransform.eulerAngles = new Vector2(_animatorRef.MainTransform.eulerAngles.x, 180);
    }

    public void CheckFacing()
    {
        FacingHandler(_animatorRef.IsFacingRight() ? false : true);
    }

    bool CancelWallJump()
    {
        return Move.x > 0 && _animatorRef.MainTransform.eulerAngles.y == 180 ||
        Move.x < 0 && _animatorRef.MainTransform.eulerAngles.y == 0;
    }

    public void UpdateMovementModifiers(MovementData movementData)
    {
        if (movementData == null)
            movementData = _movementData;

        _speed = movementData.Speed;
        _movementSpeedMultiplier = 0;
    }



    bool CanWalk()
    {
        return _currentState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping 
        or CharState.AirAttack or CharState.Falling or CharState.WallJumping;
    }

    public void OnReceivedPhyscisChanging(PhysicsOptions physicsOptions)
    {
        if (physicsOptions.StopHorizontalVelocity)
            _rb.velocity = new Vector2(0, _rb.velocity.y);

        if (physicsOptions.StopVerticalVelocity)
            _rb.velocity = new Vector2(_rb.velocity.x, 0);

        if (physicsOptions.UpdateRigidBodyContraints)
            _rb.constraints = physicsOptions.RigidbodyConstraints;

    }

    public void ApplyForce(Vector2 force, bool IsLocal)
    {
        bool facingRight = _animatorRef.MainTransform.eulerAngles.y == 0;
        int i = IsLocal == true ? facingRight == true ? 1 : -1 : 1; //Need to refactor


        _rb.AddForce(new Vector2(force.x * i, force.y), ForceMode2D.Impulse);
    }

    public void UpdateColliderSize(Vector3 center, Vector3 size)
    {
        if (size == Vector3.zero)
        {
            _capsuleCollider2D.size = _colliderBoundsSize;
            _capsuleCollider2D.offset = _colliderBoundsCenter;
            return;
        }

        _capsuleCollider2D.size = size;
        _capsuleCollider2D.offset = center;
            
    }

    public void IsOnGroundUpdate(bool isGround)
    {
        _isOnGround = isGround;
    }
}
