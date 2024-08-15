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

    [SerializeField] List<FrameActionForces> _frameActionForces = new List<FrameActionForces>();
    [SerializeField] List<FrameActionVFX> _frameActionVFXs = new List<FrameActionVFX>();


    public AnimationClip AnimationClip => _animationClip;
    public float TransitionDuration => _transitionDuration;

    public Vector2 ComboConnectionRange => _comboConnectionRange;

    public AttackMovementRelated AttackMovementRelated => _attackMovementRelated;

    public UpgradeEnum UpgradeNeeded => _upgradeNeeded;

    public List<FrameActionForces> FrameActionForces => _frameActionForces;
    public List<FrameActionVFX> FrameActionsVFXs => _frameActionVFXs;
}

[Serializable]
public class AttackMovementRelated
{
    [SerializeField] private CharState _characterStateToSet;
    [SerializeField] private bool _stopHorizontalVelocity;
    [SerializeField] private bool _stopVerticalVelocity;

    public CharState CharacterStateToSet => _characterStateToSet;
    public bool StopHorizontalVelocity => _stopHorizontalVelocity;
    public bool StopVerticalVelocity => _stopVerticalVelocity;
}

[Serializable]
public class FrameActionVFX : FrameAction
{
    [SerializeField]
    private float _force;

    public float Force => _force;
}

[Serializable]
public class FrameActionForces : FrameAction
{
    [SerializeField]
    private Vector2 _force;

    public Vector2 Force => _force;

}

public class FrameAction
{
    public Vector2 ActionInterval;
}
