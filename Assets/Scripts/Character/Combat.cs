using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Combat : MonoBehaviour
{
    [SerializeField] private FootCollider _footCollider;
    [SerializeField] private FrameActionManager _frameActionManager;

    private WeaponData _currentMelee;
    private WeaponData _currentRanged;

    private UpgradeManager _upgradeManager;
    private CharacterState _characterState;
    private int CurrentFrame;
    private bool _isAttacking;
    private int _attackIndex;

    private ActionData _currentActionData;
    private ActionData[] _currentAttackDataList;


    public Action<ActionData, Action> OnAttackPerformed;
    public Action<CharState> OnRequestStateChanging;
    public Action<PhysicsOptions> OnRequestPhysicsChanging;

    public void Init(UpgradeManager upgradeManager, CharacterState characterState)
    {
        _upgradeManager = upgradeManager;
        _characterState = characterState;
    }

    public void UpdateCurrentWeapon(ItemData itemData)
    {
        WeaponData weaponData = itemData as WeaponData;

        if (weaponData == null) return;

        if (weaponData.WeaponType is WeaponType.Melee)
            _currentMelee = weaponData;
        else
            _currentRanged = weaponData;
    }

    public void UnEquipItem(ItemData itemData)
    {
        switch (itemData)
        {
            case WeaponData weaponData:
                if (weaponData.WeaponType is WeaponType.Melee)
                    _currentMelee = null;
                else
                    _currentRanged = null;
                
                break;
        }
    }

    public void OnAttackInput(bool pressing)
    {
        if (pressing)
            InputBuffer.Add(() => CheckAttack(isMelee: true));
    }

    public void OnRangedAttackInput(bool pressing)
    {
        if (pressing)
            InputBuffer.Add(() => CheckAttack(isMelee: false));
    }

    void CheckAttack(bool isMelee)
    {
        if (!CanAttack(isMelee))
            return;

        ActionData attackData = GetAttackData(isMelee);

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

    bool CanAttack(bool isMelee)
    {
        var weapon = isMelee ? _currentMelee : _currentRanged;

        if (weapon == null)
            return false;

        if (isMelee)
            return _characterState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping
                or CharState.Falling or CharState.Attack or CharState.AirAttack;
        else
            return _characterState.CharState is CharState.Free or CharState.Jumping or CharState.DoubleJumping
                or CharState.Falling or CharState.Attack or CharState.AirAttack;
    }

    ActionData GetAttackData(bool isMelee)
    {
        if (isMelee)
            _currentAttackDataList = _footCollider.IsOnGround
                ? _currentMelee.ActionAttackDatas
                : _currentMelee.ActionAirAttackDatas;
        else
            _currentAttackDataList = _footCollider.IsOnGround
                ? _currentRanged.ActionAttackDatas
                : _currentRanged.ActionAirAttackDatas;

        if (!_isAttacking)
            return _currentAttackDataList[0];

        var attackData = (_attackIndex < _currentAttackDataList.Length)
            ? _currentAttackDataList[_attackIndex]
            : _currentAttackDataList[0];

        return attackData;
    }

    bool CheckComboConnection(ActionData nextAttackData)
    {
        if (_currentActionData == null)
            return false;

        if (!_upgradeManager.HasUpgrade(nextAttackData.UpgradeNeeded))
            return false;

        return CurrentFrame >= _currentActionData.ComboConnectionRange.x &&
               CurrentFrame <= _currentActionData.ComboConnectionRange.y;
    }

    void PerformAttack(ActionData actionData)
    {
        _attackIndex = _attackIndex >= _currentAttackDataList.Length - 1 ? 0 : _attackIndex + 1;

        OnRequestStateChanging?.Invoke(actionData.CharacterStateToSet);
        OnRequestPhysicsChanging?.Invoke(actionData.PhysicsOptions);

        _isAttacking = true;


        _currentActionData = actionData;
        OnAttackPerformed?.Invoke(actionData, OnEndAttack);
    }

    public void UpdateCurrentFrame(int frame)
    {
        CurrentFrame = frame;
    }

    private void OnEndAttack()
    {
        _isAttacking = false;
        _attackIndex = 0;
        _currentActionData = null;
    }
}