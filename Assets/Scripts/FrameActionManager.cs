using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrameActionManager : MonoBehaviour
{
    public int myFrame;

    public event Action<Vector2> OnApplyForce;

    public void ReceiveFrame(int frame)
    {
        myFrame = frame;
    }

    public void FrameActions(AttackData attackData)
    {
        if (attackData.FrameActionForces.Count > 0)
        {
            foreach (var action in attackData.FrameActionForces)
            {
                IEnumerator routine = FrameActionRoutine(action.ActionInterval, () =>OnApplyForce?.Invoke(action.Force));
                Debug.Log(action.ActionInterval);
                StartCoroutine(routine);
            }

        }

        if (attackData.FrameActionsVFXs.Count > 0)
        {
            foreach (var action in attackData.FrameActionsVFXs)
            {
                //IEnumerator routine = FrameActionRoutine(action.ActionInterval, () => OnApplyForce(action.Force));
                //StartCoroutine(routine);
            }
        }
    }


    IEnumerator FrameActionRoutine(Vector2 interval, Action performAction)
    {
        while (true)
        {
            if (myFrame >= interval.x && myFrame <= interval.y)
            {
                // Executa a ação passada como parâmetro
                performAction?.Invoke();
            }

            yield return null;
        }
    }
}
