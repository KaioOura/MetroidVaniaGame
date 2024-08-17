using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private LedgeDetector _ledgeDetector;
    [SerializeField] private Combat _combat;
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
        InitCombat();
        InitLedgeDetector();
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
        _inputReader.OnJumpInput -= _movement.OnJumpInput;
        _inputReader.OnAttackInput -= _combat.OnAttackInput;

        _combat.OnRequestStateChanging -= ChangeCurrentState;
        _combat.OnRequestPhysicsChanging -= _movement.OnReceivedPhyscisChanging;
        _combat.OnAttackPerformed -= _frameActionManager.OnActionReceived;

        _movement.OnChangeStateChanging -= ChangeCurrentState;


        _frameActionManager.OnFrameUpdate -= _combat.UpdateCurrentFrame;
        _frameActionManager.OnApplyForce -= _movement.ApplyForce;

        _ledgeDetector.OnChangeStateChanging -= ChangeCurrentState;
        _ledgeDetector.OnLedgeHangPerformed -= _frameActionManager.OnActionReceived;
        _ledgeDetector.OnRequestPhysicsChanging -= _movement.OnReceivedPhyscisChanging;
    }

    void InitInput()
    {
        _inputReader.OnMoveInput += _movement.OnMoveInput;
        _inputReader.OnJumpInput += _movement.OnJumpInput;
        _inputReader.OnAttackInput += _combat.OnAttackInput;
    }

    void InitMovement()
    {
        _movement.Init(_animatorRef, _characterState);
        _movement.OnChangeStateChanging += ChangeCurrentState;
    }

    void InitLedgeDetector()
    {
        _ledgeDetector.Init(_characterState);
        _ledgeDetector.OnChangeStateChanging += ChangeCurrentState;
        _ledgeDetector.OnLedgeHangPerformed += _frameActionManager.OnActionReceived;
        _ledgeDetector.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        OnJumpPerformed += _ledgeDetector.JumpPerformed;
    }

    void InitCombat()
    {
        _combat.Init(_upgradeManager);

        _combat.OnRequestStateChanging += ChangeCurrentState;
        _combat.OnRequestPhysicsChanging += _movement.OnReceivedPhyscisChanging;
        _combat.OnAttackPerformed += _frameActionManager.OnActionReceived;
    }

    void InitFrameActionManager()
    {
        _frameActionManager.Init(_animatorRef);

        _frameActionManager.OnApplyForce += _movement.ApplyForce;

        _frameActionManager.OnFrameUpdate += _combat.UpdateCurrentFrame;
    }

    public void ChangeCurrentState(CharState characterState)
    {
        _characterState.CharState = characterState;

        switch(_characterState.CharState)
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
            case CharState.Jumping:
            {
                OnJumpPerformed?.Invoke();
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
    AirAttack
}
