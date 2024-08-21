using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField] private Transform _ledgeCheckTransform;
    [SerializeField] private Transform _wallCheckTransform;
    [SerializeField] private ActionData _ledgeActionData;
    [SerializeField] private LedgeData _ledgeData;

    private CharacterState _characterState;
    private float _lastTimeHang = 0;
    private bool _isOnGround;
    private Rigidbody2D _rb;

    public event Action<CharState> OnChangeStateChanging;
    public Action<PhysicsOptions> OnRequestPhysicsChanging;
    public event Action<ActionData, Action> OnLedgeHangPerformed;


    public void Init(CharacterState characterState, Rigidbody2D rb)
    {
        _characterState = characterState;
        _rb = rb;
    }

    void Update()
    {
        if (!CanLedgeCheck()) return;

        bool ledgeFound = Physics2D.Raycast(_ledgeCheckTransform.position, transform.parent.right, _ledgeData.CheckDistance, _ledgeData.LedgeCheckLayerMask);
        RaycastHit2D wallRay = Physics2D.Raycast(_wallCheckTransform.position, transform.parent.right, _ledgeData.CheckDistance, _ledgeData.LedgeCheckLayerMask);
        Debug.DrawRay(_ledgeCheckTransform.position, _ledgeCheckTransform.TransformDirection(Vector2.right) * _ledgeData.CheckDistance, Color.black);

        HandleLedge(wallRay, ledgeFound);
    }

    void HandleLedge(RaycastHit2D wallRay, bool ledgeFound)
    {
        if (wallRay.collider == null || ledgeFound == true) return;

        //Calculate hang position
        float wallDistX = wallRay.point.x - wallRay.transform.position.x;
        float wallDistY = wallRay.collider.bounds.extents.y + wallRay.collider.bounds.center.y;

        int facingRight = transform.parent.eulerAngles.y == 0 ? 1 : -1;

        Vector2 _hangPos = new Vector2(wallRay.point.x + (_ledgeData.OffSet.x * facingRight), wallDistY + _ledgeData.OffSet.y);

        RaycastHit2D groundBelowRay = Physics2D.Raycast(transform.position, transform.parent.up * -1, 2, _ledgeData.LedgeCheckLayerMask);

        if (groundBelowRay.collider != null)
        {
            if (_hangPos.y < groundBelowRay.point.y && wallRay.collider != groundBelowRay.collider) //Check if hang pos would get player inside ground
                return;
        }


        OnChangeStateChanging?.Invoke(CharState.LedgeClimbing);
        OnLedgeHangPerformed?.Invoke(_ledgeActionData, null);
        OnRequestPhysicsChanging?.Invoke(_ledgeActionData.PhysicsOptions);

        transform.parent.position = _hangPos;
    }

    bool CanLedgeCheck()
    {
        bool isOnCooldown = Time.time < _lastTimeHang + _ledgeData.HangCoolDown;
        return _characterState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping or CharState.Falling or CharState.WallJumping or CharState.Dashing
        && !isOnCooldown && VelCheck();
    }

    bool VelCheck() //Used to check if player Y when dashing.
    {
        return _characterState.CharState is CharState.Dashing ? true : Mathf.Abs(_rb.velocity.y) > 0.1f;
    }

    public void JumpPerformed()
    {
        _lastTimeHang = Time.time;
    }

    public void IsOnGroundUpdate(bool IsOnGround)
    {
        _isOnGround = IsOnGround;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(_wallCheckTransform.position, transform.parent.right * _ledgeData.CheckDistance);

    //     Gizmos.color = Color.white;
    //     Gizmos.DrawRay(_ledgeCheckTransform.position, transform.parent.right * _ledgeData.CheckDistance);

    //     Gizmos.DrawRay(transform.position, transform.parent.up * -1 * 2);
    // }
}
