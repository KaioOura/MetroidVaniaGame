using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ActionData", menuName = "ScriptableObjects/ActionDataScriptableObject", order = 1)]
public class ActionData : ScriptableObject
{
    [Header("Action Data")]
    [SerializeField] protected AnimationClip _animationClip;
    [SerializeField] protected float _transitionDuration;
    [SerializeField] protected AnimatorRef.AnimationState _animationState;
    [SerializeField] protected CharState _characterStateToSet;

    [SerializeField] protected bool _overridePhysics;
    [SerializeField] protected PhysicsOptions _physicsOptions;
    [SerializeField] protected UpgradeEnum _upgradeNeeded;

    [SerializeField] List<FrameActionForces> _frameActionForces = new List<FrameActionForces>();
    [SerializeField] List<FrameActionVFX> _frameActionVFXs = new List<FrameActionVFX>();


    public AnimationClip AnimationClip => _animationClip;
    public float TransitionDuration => _transitionDuration;
    public AnimatorRef.AnimationState AnimationState => _animationState;

    public CharState CharacterStateToSet => _characterStateToSet;
    public bool OverridePhysics => _overridePhysics;
    public PhysicsOptions PhysicsOptions => _physicsOptions;

    public UpgradeEnum UpgradeNeeded => _upgradeNeeded;

    public List<FrameActionForces> FrameActionForces => _frameActionForces;
    public List<FrameActionVFX> FrameActionsVFXs => _frameActionVFXs;
}

[Serializable]
public class ActionMovementRelated
{
    [SerializeField] protected bool _stopHorizontalVelocity;
    [SerializeField] protected bool _stopVerticalVelocity;

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
    [SerializeField] private bool _localForce;

    [SerializeField] private Vector2 _force;


    public bool LocalForce => _localForce;
    public Vector2 Force => _force;

}

public class FrameAction
{
    public Vector2 ActionInterval;
}
