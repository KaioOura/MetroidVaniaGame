using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRef : MonoBehaviour
{   

    public enum AnimationState
    {
        Jump,
        AttackDefault,
        LedgeHang,
        WallSlide,
        Idle,
        WallJump

    }
    public static string MoveParam = "Move";
    public static string VelYParam = "VelY";
    public static string OnGroundParam = "IsOnGround";

    public static string JumpState = "Jump";
    public static string AttackState = "AttackDefault";

    [SerializeField]
    private Transform _mainTransform;

    [SerializeField] private AnimatorOverrideController _animatorOverrideController;

    private Animator _animator;

    public Transform MainTransform
    {
        get => _mainTransform;
        set => _mainTransform = value;
    }

    public Animator Animator => _animator;
    public AnimatorOverrideController AnimatorOverrideController => _animatorOverrideController;

    void Awake()
    {
        try
        {
            _animator = GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError("Animator not found");
        }
        
        if (_mainTransform == null)
            Debug.LogError("Variable MainTransform not assigned");
    }

    public bool IsFacingRight()
    {
        return MainTransform.eulerAngles.y == 0;
    }
}
