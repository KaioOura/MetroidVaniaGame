using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float _dashCooldown = 1;
    [SerializeField] private DashData _dashData;
    [SerializeField] private Vector2 _dashGroundCenter;
    [SerializeField] private Vector2 _dashGroundSize;
    [SerializeField] private ActionData _dashActionData;
    [SerializeField] private ActionData _rollActionData;
    [SerializeField] private PhysicsOptions _physicsOptionsReset;

    private CharacterState _characterState;
    private bool _isOnGround;
    private float _coolDownTracker;
    private int _dashCounter = 1;

    public event Action<CharState> OnRequestStateChaging;
    public event Action<ActionData, Action> OnDashPerformed;
    public event Action<PhysicsOptions> OnRequestPhysicsChanging;
    public event Action<MovementData> OnRequestMovementReset;
    public event Action<bool> OnRequestFacingChange;
    public event Action<Vector3, Vector3> OnRequestHitBoxChanging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(CharacterState characterState)
    {
        _characterState = characterState;
    }

    public void OnDashInput(bool isPressing)
    {
        if (!isPressing || !CanDash())
            return;

        //Do Dash
        DoDash();
    }

    void DoDash()
    {

        ActionData actionData = _isOnGround ? _rollActionData : _dashActionData;

        
        if (_isOnGround)
            OnRequestHitBoxChanging?.Invoke(_dashGroundCenter, _dashGroundSize);

        if (_characterState.CharState is CharState.WallSliding)
        {
            bool faceRight = transform.parent.eulerAngles.y == 180 ? true : false;
            OnRequestFacingChange?.Invoke(faceRight);
        }
            



        //OnRequestStateChaging?.Invoke(actionData.CharacterStateToSet);
        OnDashPerformed?.Invoke(actionData, OnEndDash);
        _coolDownTracker = Time.time;
        _dashCounter = 0;
    }

    bool CanDash()
    {
        return _characterState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping or CharState.Falling or CharState.WallJumping or CharState.WallSliding
        && Time.time >= _coolDownTracker + _dashCooldown
        && HasDashCount();
    }

    public void IsOnGroundUpdate(bool IsOnGround)
    {
        _isOnGround = IsOnGround;
        if (_isOnGround)
            OnDashCountReset();
    }

    public void OnDashCountReset()
    {
        _dashCounter = 1;
    }

    bool HasDashCount()
    {
        return _dashCounter == 1;
    }

    void OnEndDash()
    {
        //if (_characterState.CharState is not CharState.Dashing)
            //return;

        OnRequestPhysicsChanging?.Invoke(_physicsOptionsReset);
        OnRequestMovementReset?.Invoke(null);
        OnRequestHitBoxChanging?.Invoke(Vector3.zero, Vector3.zero);
    }
}
