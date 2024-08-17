using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private WeaponData _currentWeapon;
    [SerializeField] private FootCollider _footCollider;
    [SerializeField] private FrameActionManager _frameActionManager;

    private UpgradeManager _upgradeManager;
    private int CurrentFrame;
    private bool _isAttacking;
    private int _attackIndex;

    private AttackData _currentAttackData;
    private AttackData[] _currentAttackDataList;


    public Action<AttackData, Action> OnAttackPerformed;
    public Action<CharState> OnRequestStateChanging;
    public Action<PhysicsOptions> OnRequestPhysicsChanging;

    public void Init(UpgradeManager upgradeManager)
    {
        _upgradeManager = upgradeManager;
    }

    public void OnAttackInput(bool pressing)
    {
        if (pressing)
            InputBuffer.Add(CheckAttack);
    }

    void CheckAttack()
    {   
        AttackData attackData = GetAttackData();

        if (_isAttacking)
        {
            if (CheckComboConnection(attackData))
                PerformAttack(attackData);

            return;
        }

        if (!_upgradeManager.HasUpgrade(attackData.UpgradeNeeded))
            return;

        PerformAttack(attackData);
    }

    AttackData GetAttackData()
    {
        _currentAttackDataList = _footCollider.IsOnGround ? _currentWeapon.AttackDatas : _currentWeapon.AirAttackDatas;

        if (!_isAttacking)
            return _currentAttackDataList[0];
    
        return _currentAttackDataList[_attackIndex];
    }

    bool CheckComboConnection(AttackData nextAttackData)
    {
        if (_currentAttackData == null)
            return false;

        if (!_upgradeManager.HasUpgrade(nextAttackData.UpgradeNeeded))
            return false;

        return CurrentFrame >= _currentAttackData.ComboConnectionRange.x && 
        CurrentFrame <= _currentAttackData.ComboConnectionRange.y;
    }

    void PerformAttack(AttackData attackData)
    {
        _attackIndex = _attackIndex >= _currentAttackDataList.Length - 1 ? 0 : _attackIndex + 1;

        OnRequestStateChanging?.Invoke(attackData.CharacterStateToSet);
        OnRequestPhysicsChanging?.Invoke(attackData.PhysicsOptions);

        _isAttacking = true;
        _currentAttackData = attackData;
        OnAttackPerformed?.Invoke(attackData, OnEndAttack);
    }

    public void UpdateCurrentFrame(int frame)
    {
        CurrentFrame = frame;
    }
    private void OnEndAttack()
    {
        _isAttacking = false;
        _attackIndex = 0;
        _currentAttackData = null;
    }
}
