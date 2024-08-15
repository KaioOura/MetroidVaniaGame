using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/AttackDataScriptableObject", order = 1)]
public class AttackData : ScriptableObject
{
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private float _transitionDuration;

    [SerializeField] private Vector2 _comboConnectionRange;

    [SerializeField] private AttackMovementRelated _attackMovementRelated;

    [SerializeField] private UpgradeEnum _upgradeNeeded;


    public AnimationClip AnimationClip => _animationClip;
    public float TransitionDuration => _transitionDuration;

    public Vector2 ComboConnectionRange => _comboConnectionRange;

    public AttackMovementRelated AttackMovementRelated => _attackMovementRelated;

    public UpgradeEnum UpgradeNeeded => _upgradeNeeded;
}

[Serializable]
public class AttackMovementRelated
{
    [SerializeField] private CharState _characterStateToSet;
    [SerializeField] private bool _stopHorizontalVelocity;
    [SerializeField] private bool _stopVerticalVelocity;
    [SerializeField] private AttackForces[] _attackForces;

    public CharState CharacterStateToSet => _characterStateToSet;
    public bool StopHorizontalVelocity => _stopHorizontalVelocity;
    public bool StopVerticalVelocity => _stopVerticalVelocity;
    public AttackForces[] AttackForces => _attackForces;
}

[Serializable]
public class AttackForces
{
    private float _force;
    private float _timeApplied;

    public float Force => _force;
    public float TimeApplied => _timeApplied;
}
