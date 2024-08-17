using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _ledgeCheckLayerMask;
    [SerializeField] private Transform _ledgeCheckTransform;
    [SerializeField] private Transform _wallCheckTransform;
    [SerializeField] private Transform _footPos;
    [SerializeField] private float _checkDistance;
    [SerializeField] private float _hangCoolDown;
    [SerializeField] private float _minDistY;

    [SerializeField] private Vector2 _offSet;
    [SerializeField] private ActionData _ledgeHangData;

    private CharacterState _characterState;
    private float _lastTimeHang = 0;

    public event Action<CharState> OnChangeStateChanging;
    public Action<PhysicsOptions> OnRequestPhysicsChanging;
    public event Action<ActionData, Action> OnLedgeHangPerformed;

    
    public void Init(CharacterState characterState)
    {
        _characterState = characterState;
    }

    void Update()
    {
        bool ledgeFound = Physics2D.Raycast(_ledgeCheckTransform.position, transform.parent.right, _checkDistance, _ledgeCheckLayerMask); 
        RaycastHit2D wallRay = Physics2D.Raycast(_wallCheckTransform.position, transform.parent.right, _checkDistance, _ledgeCheckLayerMask);

        HandleLedge(wallRay, ledgeFound);
    }

    void HandleLedge(RaycastHit2D wallRay, bool ledgeFound)
    {
        if (wallRay.collider == null || ledgeFound == true || !CanLedgeHang()) return;

        float wallDistX = wallRay.point.x - wallRay.transform.position.x;
        float wallDistY = wallRay.collider.bounds.extents.y + wallRay.collider.bounds.center.y;
        
        int facingRight = transform.parent.eulerAngles.y == 0 ? 1 : -1;

        Vector2 _hangPos = new Vector2(wallRay.point.x + (_offSet.x * facingRight), wallDistY + _offSet.y);

        RaycastHit2D groundBelowRay = Physics2D.Raycast(transform.position, transform.parent.up * -1, 2, _ledgeCheckLayerMask);


        if (_hangPos.y < groundBelowRay.point.y && wallRay.collider != groundBelowRay.collider) //Check if hang pos would get player inside ground
            return;
    
        OnChangeStateChanging?.Invoke(CharState.LedgeClimbing);
        OnLedgeHangPerformed?.Invoke(_ledgeHangData, null);
        OnRequestPhysicsChanging?.Invoke(_ledgeHangData.PhysicsOptions);

        transform.parent.position = _hangPos;

        //StartCoroutine(test(new Vector2(wallRay.point.x, wallRay.point.y)));
    }


    IEnumerator test(Vector2 vector2)
    {
        while (true)
        {
            transform.parent.position = new Vector2(vector2.x + (_offSet.x), vector2.y + _offSet.y);
            yield return null;
        }
    }
    bool CanLedgeHang()
    {
        bool isOnCooldown = Time.time < _lastTimeHang + _hangCoolDown;
        return _characterState.CharState is CharState.Jumping or CharState.DoubleJumping or CharState.Free && !isOnCooldown; 
    }

    public void JumpPerformed()
    {
        _lastTimeHang = Time.time;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //bool ledgeFound = Physics2D.Raycast(_ledgeCheckTransform.position, transform.parent.right, 1, _ledgeCheckLayerMask); 
        Gizmos.DrawRay(_wallCheckTransform.position, transform.parent.right * _checkDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(_ledgeCheckTransform.position, transform.parent.right * _checkDistance);

        Gizmos.DrawRay(transform.position, transform.parent.up * -1 * 2);
    }
}
