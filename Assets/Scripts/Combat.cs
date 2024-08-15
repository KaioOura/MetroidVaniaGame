using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private WeaponData _currentWeapon;
    [SerializeField] private FootCollider _footCollider;

    private AnimatorRef _animatorRef;
    private UpgradeManager _upgradeManager;
    private IEnumerator FrameCounter;
    private int CurrentFrame;
    private bool _isAttacking;
    private int _attackIndex;

    private AttackData _currentAttackData;
    private AttackData[] _currentAttackDataList;

    public Action<CharState> OnRequestStateChanging;
    public Action<bool, bool> OnRequestStopVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(AnimatorRef animatorRef, UpgradeManager upgradeManager)
    {
        _animatorRef = animatorRef;
        _upgradeManager = upgradeManager;
    }

    public void OnAttackInput(bool pressing)
    {
        //TO DO Input buffer

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
        _animatorRef.AnimatorOverrideController[AnimatorRef.AttackState] = attackData.AnimationClip;
        _animatorRef.Animator.CrossFadeInFixedTime(AnimatorRef.AttackState, attackData.TransitionDuration);

        _attackIndex = _attackIndex >= _currentAttackDataList.Length - 1 ? 0 : _attackIndex + 1;

        RequestAttackAction(attackData);

        if (FrameCounter != null)
            StopCoroutine(FrameCounter);

        FrameCounter = CountFramesRoutine(attackData, AnimatorRef.AttackState, attackData.TransitionDuration);
        StartCoroutine(FrameCounter);
    }

    IEnumerator CountFramesRoutine(AttackData attackData, string desiredAnimationState, float transitionDurantion)
    {
        float maxFrame = attackData.AnimationClip.length * attackData.AnimationClip.frameRate;
        int frame = 0;
        float time = 0;
        float normalizedTransitionTime = 0;
        CurrentFrame = 0;

        _isAttacking = true;
        _currentAttackData = attackData;

        if (transitionDurantion > 0)
        {
            yield return new WaitUntil(() => _animatorRef.Animator.IsInTransition(0));

            while (_animatorRef.Animator.IsInTransition(0)) //!_animatorRef.Animator.GetCurrentAnimatorStateInfo(0).IsName(desiredAnimationState))
            {
                time += Time.deltaTime;
                normalizedTransitionTime = time / transitionDurantion;
                //Debug.Log("Normalized time: " +  normalizedTransitionTime);            
                //float normalizedTime = 1 - _animatorRef.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                //timeTrasitioning += Time.deltaTime;
                CurrentFrame = frame = (int)(normalizedTransitionTime / (1 / (transitionDurantion * attackData.AnimationClip.frameRate)) + 1);
                //Debug.Log(frame);
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
        //float maxFrameCorrection = (attackData.AnimationClip.length - timeTrasitioning) * attackData.AnimationClip.frameRate;

        while (frame < Math.Ceiling(maxFrame))
        {
            float normalizedTime = _animatorRef.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //Debug.Log("Normalized: " +  normalizedTime);
            CurrentFrame = frame = (int)(normalizedTime / (1 / maxFrame) + 1);
            //Debug.Log(frame);
            yield return null;
        }


        _isAttacking = false;
        _attackIndex = 0;
        _currentAttackData = null;
    }

    private void RequestAttackAction(AttackData attackData)
    {
        OnRequestStateChanging?.Invoke(attackData.AttackMovementRelated.CharacterStateToSet);

        OnRequestStopVelocity?.Invoke(attackData.AttackMovementRelated.StopHorizontalVelocity, attackData.AttackMovementRelated.StopVerticalVelocity);
        //TO DO External Forces
    }
}
