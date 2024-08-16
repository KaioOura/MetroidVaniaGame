using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrameActionManager : MonoBehaviour
{

    private AnimatorRef _animatorRef;
    private IEnumerator FrameCounter;

    private int currentFrame;
    private int CurrentFrame 
    {
        get 
        {
            return currentFrame;
        }
        set
        {
            currentFrame = value;
            OnFrameUpdate?.Invoke(CurrentFrame);
        }
    }

    public event Action<int> OnFrameUpdate;
    public event Action<Vector2, bool> OnApplyForce;


    public void Init(AnimatorRef animatorRef)
    {
        _animatorRef = animatorRef;
    }

    public void OnActionReceived(ActionData ActionData, Action EndAttack)
    {
        _animatorRef.AnimatorOverrideController[AnimatorRef.AttackState] = ActionData.AnimationClip;
        _animatorRef.Animator.CrossFadeInFixedTime(AnimatorRef.AttackState, ActionData.TransitionDuration);

        StopAllCoroutines();
        FrameActions(ActionData);

        if (FrameCounter != null)
            StopCoroutine(FrameCounter);
        FrameCounter = CountFramesRoutine(ActionData, AnimatorRef.AttackState, ActionData.TransitionDuration, EndAttack);
        StartCoroutine(FrameCounter);
    }

    IEnumerator CountFramesRoutine(ActionData attackData, string desiredAnimationState, float transitionDurantion, Action EndAttack)
    {
        float maxFrame = attackData.AnimationClip.length * attackData.AnimationClip.frameRate;
        int frame = 0;
        float time = 0;
        float normalizedTransitionTime = 0;
        CurrentFrame = 0;

        if (transitionDurantion > 0)
        {
            yield return new WaitUntil(() => _animatorRef.Animator.IsInTransition(0));

            while (_animatorRef.Animator.IsInTransition(0)) //!_animatorRef.Animator.GetCurrentAnimatorStateInfo(0).IsName(desiredAnimationState))
            {
                time += Time.deltaTime;
                normalizedTransitionTime = time / transitionDurantion;
                CurrentFrame = frame = (int)(normalizedTransitionTime / (1 / (transitionDurantion * attackData.AnimationClip.frameRate)) + 1);
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();

        while (frame < Math.Ceiling(maxFrame))
        {
            float normalizedTime = _animatorRef.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            CurrentFrame = frame = (int)(normalizedTime / (1 / maxFrame) + 1);
            yield return null;
        }

        StopAllCoroutines();
        EndAttack?.Invoke();
    }

    public void FrameActions(ActionData ActionData)
    {
        if (ActionData.FrameActionForces.Count > 0)
        {
            foreach (var action in ActionData.FrameActionForces)
            {
                IEnumerator routine = FrameActionRoutine(action.ActionInterval, () => OnApplyForce?.Invoke(action.Force, action.LocalForce));
                Debug.Log(action.ActionInterval);
                StartCoroutine(routine);
            }

        }

        if (ActionData.FrameActionsVFXs.Count > 0)
        {
            foreach (var action in ActionData.FrameActionsVFXs)
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
            if (currentFrame >= interval.x && currentFrame <= interval.y)
            {
                // Executa a ação passada como parâmetro
                performAction?.Invoke();
            }

            yield return null;
        }
    }
}
