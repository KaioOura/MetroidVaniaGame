using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public Action<Vector2> OnMoveInput;
    public Action<bool> OnJumpInput;
    public Action<bool> OnAttackInput;
    public Action<bool> OnRangedAttackInput;
    public Action<bool> OnDashInput;
    public Action<bool> OnInteractInput;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMove(InputValue ctx)
    {
        OnMoveInput?.Invoke(ctx.Get<Vector2>());
    }

    public void OnJump(InputValue ctx)
    {
        OnJumpInput?.Invoke(ctx.Get<float>() == 1);
    }

    public void OnAttack(InputValue ctx)
    {
        OnAttackInput?.Invoke(ctx.Get<float>() == 1);
    }

    public void OnRangedAttack(InputValue ctx)
    {
        OnRangedAttackInput?.Invoke(ctx.Get<float>() == 1);
    }

    public void OnDash(InputValue ctx)
    {
        OnDashInput?.Invoke(ctx.Get<float>() == 1);
    }

    public void OnInteract(InputValue ctx)
    {
        OnInteractInput?.Invoke(ctx.Get<float>() == 1);
    }
}
