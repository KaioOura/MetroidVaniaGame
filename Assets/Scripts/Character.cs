using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private Combat _combat;
    [SerializeField] private FrameActionManager _frameActionManager;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private AnimatorRef _animatorRef;
    [SerializeField] private UpgradeManager _upgradeManager;

    private CharacterState _characterState;
    
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
        InitFrameActionManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        _inputReader.OnMoveInput -= _movement.OnMoveInput;
        _inputReader.OnJumpInput -= _movement.OnJumpInput;
        _inputReader.OnAttackInput -= _combat.OnAttackInput;

        _combat.OnRequestStateChanging -= ChangeCurrentState;
        _combat.OnRequestStopVelocity -= _movement.OnStopVelocity;
        _combat.OnAttackPerformed -= _frameActionManager.OnActionReceived;

        _movement.OnChangeStateChanging -= ChangeCurrentState;


        _frameActionManager.OnFrameUpdate -= _combat.UpdateCurrentFrame;
        _frameActionManager.OnApplyForce -= _movement.ApplyForce;
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

    void InitCombat()
    {
        _combat.Init(_upgradeManager);

        _combat.OnRequestStateChanging += ChangeCurrentState;
        _combat.OnRequestStopVelocity += _movement.OnStopVelocity;
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
    Attack,
    AirAttack
}
