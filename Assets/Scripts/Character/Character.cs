using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private FootCollider _footCollider;
    [SerializeField] private LedgeDetector _ledgeDetector;
    [SerializeField] private Jump _jump;
    [SerializeField] private WallJump _wallSlide;
    [SerializeField] private Dash _dash; 
    [SerializeField] private Combat _combat;
    [SerializeField] private ColliderCreator _colliderCreator;
    [SerializeField] private FrameActionManager _frameActionManager;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private AnimatorRef _animatorRef;
    [SerializeField] private UpgradeManager _upgradeManager;

    private CharacterState _characterState;

    private event Action OnJumpPerformed;

    void Awake()
    {
        _characterState = new CharacterState();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitInput();
        InitMovement();
        InitJump();
        InitCombat();
        InitLedgeDetector();
        InitWallSlide();
        InitDash();
        InitFrameActionManager();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        OnJumpPerformed -= _ledgeDetector.JumpPerformed;

        _inputReader.OnMoveInput -= _movement.OnMoveInput;
        _inputReader.OnAttackInput -= _combat.OnAttackInput;

        _combat.OnRequestStateChanging -= ChangeCurrentState;
        _combat.OnRequestPhysicsChanging -= _movement.OnReceivedPhyscisChanging;
        _combat.OnAttackPerformed -= _frameActionManager.OnActionReceived;

        _movement.OnChangeStateChanging -= ChangeCurrentState;
        _footCollider.OnIsOnGroundUpdate -= _movement.IsOnGroundUpdate;

        _jump.OnChangeStateChanging -= ChangeCurrentState;
        _footCollider.OnIsOnGroundUpdate -= _jump.IsOnGroundUpdate;

        _frameActionManager.OnFrameUpdate -= _combat.UpdateCurrentFrame;
        _frameActionManager.OnApplyForce -= _movement.ApplyForce;

        _ledgeDetector.OnChangeStateChanging -= ChangeCurrentState;
        _ledgeDetector.OnLedgeHangPerformed -= _frameActionManager.OnActionReceived;
        _ledgeDetector.OnRequestPhysicsChanging -= _movement.OnReceivedPhyscisChanging;

        _wallSlide.OnStateChanging -= ChangeCurrentState;
        _wallSlide.OnUpdateMovementSettings -= _movement.UpdateMovementModifiers;
    }

    void InitInput()
    {
        _inputReader.OnMoveInput += _movement.OnMoveInput;
        _inputReader.OnJumpInput += _jump.OnJumpInput;
        _inputReader.OnAttackInput += _combat.OnAttackInput;
        _inputReader.OnDashInput += _dash.OnDashInput;
    }

    void InitMovement()
    {
        _movement.Init(_animatorRef, _characterState);
        _movement.OnChangeStateChanging += ChangeCurrentState;
        _movement.OnUpdateJumpModifiers += _jump.UpdateJumpModifiers;
        _footCollider.OnIsOnGroundUpdate += _movement.IsOnGroundUpdate;
    }

    void InitJump()
    {
        _jump.Init(_characterState);
        _jump.OnChangeStateChanging += ChangeCurrentState;
        _jump.OnUpdateMovementModifiers += _movement.UpdateMovementModifiers;
        _jump.OnJumpPerformed += _frameActionManager.OnActionReceived;
        _jump.OnWallJumpPerformed += _frameActionManager.OnActionReceived;
        _jump.OnRequestFacingChange += _movement.FacingHandler;
        _jump.OnChangeStateChanging += ChangeCurrentState;
        _footCollider.OnIsOnGroundUpdate += _jump.IsOnGroundUpdate;
    }

    void InitLedgeDetector()
    {
        _ledgeDetector.Init(_characterState, _movement.RB);
        _ledgeDetector.OnChangeStateChanging += ChangeCurrentState;
        _ledgeDetector.OnLedgeHangPerformed += _frameActionManager.OnActionReceived;
        _ledgeDetector.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        OnJumpPerformed += _ledgeDetector.JumpPerformed;
        OnJumpPerformed += _wallSlide.JumpPerformed;
        _footCollider.OnIsOnGroundUpdate += _ledgeDetector.IsOnGroundUpdate;
    }

    void InitWallSlide()
    {
        _wallSlide.Init(_characterState);
        _wallSlide.OnStateChanging += ChangeCurrentState;
        _wallSlide.OnUpdateMovementSettings += _movement.UpdateMovementModifiers;
        _wallSlide.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        _wallSlide.OnUpdateMovementSettings += _jump.UpdateJumpModifiers;
        _wallSlide.OnRequestStateChanging += _frameActionManager.OnActionReceived;
        _wallSlide.OnWallSlidePerformed += _jump.ResetJumpCount;
        _wallSlide.OnWallSlidePerformed += _dash.OnDashCountReset;
    }

    void InitDash()
    {
        _dash.Init(_characterState);
        _dash.OnDashPerformed += _frameActionManager.OnActionReceived;
        _dash.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        _dash.OnRequestMovementReset += _movement.UpdateMovementModifiers;
        _dash.OnRequestMovementReset += _jump.UpdateJumpModifiers;
        _dash.OnRequestFacingChange += _movement.FacingHandler;
        _dash.OnRequestHitBoxChanging += _movement.UpdateColliderSize;
        _footCollider.OnIsOnGroundUpdate += _dash.IsOnGroundUpdate;

    }

    void InitCombat()
    {
        _combat.Init(_upgradeManager, _characterState);

        _combat.OnRequestStateChanging += ChangeCurrentState;
        //_combat.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        _combat.OnAttackPerformed += _frameActionManager.OnActionReceived;
    }

    void InitFrameActionManager()
    {
        _frameActionManager.Init(_animatorRef);

        _frameActionManager.OnRequestStateChanging += ChangeCurrentState;
        
        _frameActionManager.OnApplyForce += _movement.ApplyForce;
        _frameActionManager.OnOverridePhysics += _movement.OnReceivedPhyscisChanging;

        _frameActionManager.OnFrameUpdate += _combat.UpdateCurrentFrame;

        _frameActionManager.OnRequestCollider += _colliderCreator.OnReceivedColliderRequest;
    }

    public void ChangeCurrentState(CharState characterState)
    {
        _characterState.CharState = characterState;

        switch (_characterState.CharState)
        {
            case CharState.Free:
                {
                    // _movement.ChangeCurrentMoveType(MovementTypes.Free);
                    break;
                }
            case CharState.Attack:
                {
                    // _movement.ChangeCurrentMoveType(MovementTypes.Restricted);
                    break;
                }
            case CharState.Jumping or CharState.DoubleJumping:
                {
                    OnJumpPerformed?.Invoke();
                    break;
                }
            case CharState.Falling:
                {   
                    _movement.UpdateMovementModifiers(null);
                    break;
                }
            
        }
    }

}

[Serializable]
public class CharacterState
{
    [SerializeField]
    public CharState CharState;
}

public enum CharState
{
    Free,
    Jumping,
    DoubleJumping,
    LedgeClimbing,
    Attack,
    AirAttack,
    WallSliding,
    WallJumping,
    Falling,
    Dashing
}
