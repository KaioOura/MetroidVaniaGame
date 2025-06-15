using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Systems;
using Systems.SaveLoad;
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

    public Action<bool> OnInventoryInput;


    Vector2 GetInputValue(Vector2 value) => GameManager.Instance.IsGameplay ? value : Vector2.zero;
    float GetInputValue(float value) => GameManager.Instance.IsGameplay ? value : 0;
    
    
    public void HandleStateChanged(GameState state)
    {
        if (state is GameState.Gameplay) return;
        OnMoveInput?.Invoke(GetInputValue(Vector2.zero));
        OnJumpInput?.Invoke(false);
        OnAttackInput?.Invoke(false);

    }
    
    public void OnMove(InputValue ctx)
    {
        OnMoveInput?.Invoke(GetInputValue(ctx.Get<Vector2>()));
    }

    public void OnJump(InputValue ctx)
    {
        OnJumpInput?.Invoke(GetInputValue(ctx.Get<float>())  == 1);
    }

    public void OnAttack(InputValue ctx)
    {
        OnAttackInput?.Invoke(Mathf.Approximately(GetInputValue(ctx.Get<float>()), 1));
    }

    public void OnRangedAttack(InputValue ctx)
    {
        OnRangedAttackInput?.Invoke(Mathf.Approximately(GetInputValue(ctx.Get<float>()), 1));
    }

    public void OnDash(InputValue ctx)
    {
        OnDashInput?.Invoke(Mathf.Approximately(GetInputValue(ctx.Get<float>()), 1));
    }

    public void OnInteract(InputValue ctx)
    {
        OnInteractInput?.Invoke(Mathf.Approximately(GetInputValue(ctx.Get<float>()), 1));
    }

    public void OnInventory(InputValue ctx)
    {
        OnInventoryInput?.Invoke(Mathf.Approximately(GetInputValue(ctx.Get<float>()), 1));
    }

    public void OnSave(InputValue ctx)
    {
        SaveManager.SaveAll();
    }

    public void OnClear(InputValue ctx)
    {
        SaveManager.ClearSave();
    }
}