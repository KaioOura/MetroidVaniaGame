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
    public event Action<PhysicsOptions> OnOverridePhysics;
    public event Action<ColliderInfos> OnRequestCollider;
    public event Action OnRequestArrowShoot;
    public Action<CharState> OnRequestStateChanging;


    public void Init(AnimatorRef animatorRef)
    {
        _animatorRef = animatorRef;
        OverrideAnimations();
    }

    void OverrideAnimations()
    {
        _animatorRef.Animator.runtimeAnimatorController = _animatorRef.AnimatorOverrideController;
        var overrides = new AnimationClipOverrides(_animatorRef.AnimatorOverrideController.overridesCount);
        _animatorRef.AnimatorOverrideController.GetOverrides(overrides);

        for (int y = 0; y < overrides.Count; y++)
        {
            for (int i = 0; i < Enum.GetNames(typeof(AnimatorRef.AnimationState)).Length; i++)
            {
                var stateName = (AnimatorRef.AnimationState)i;
                string name = stateName.ToString();
                string key = GetCleanKey(overrides[y].Key.ToString());

                if (key == name)
                    overrides[stateName.ToString()] = overrides[y].Value;
            }
        }

        _animatorRef.AnimatorOverrideController.ApplyOverrides(overrides);
    }

    public void OnActionReceived(ActionData ActionData, Action EndAttack)
    {
        _animatorRef.AnimatorOverrideController[ActionData.AnimationState.ToString()] = ActionData.AnimationClip;
        _animatorRef.Animator.CrossFadeInFixedTime(ActionData.AnimationState.ToString(), ActionData.TransitionDuration);
        OnRequestStateChanging?.Invoke(ActionData.CharacterStateToSet);

        StopAllCoroutines();
        FrameActions(ActionData);

        if (FrameCounter != null)
            StopCoroutine(FrameCounter);
        FrameCounter = CountFramesRoutine(ActionData, ActionData.AnimationState.ToString(), ActionData.TransitionDuration, EndAttack);
        StartCoroutine(FrameCounter);
    }

    IEnumerator CountFramesRoutine(ActionData attackData, string desiredAnimationState, float transitionDurantion, Action EndAttack)
    {
        float maxFrame = attackData.AnimationClip.length * attackData.AnimationClip.frameRate;
        int frame = 0;
        float time = 0;
        float normalizedTransitionTime = 0;
        CurrentFrame = 0;

        //Debug.Log(CurrentFrame);

        if (transitionDurantion > 0)
        {
            yield return new WaitUntil(() => _animatorRef.Animator.IsInTransition(0));

            while (_animatorRef.Animator.IsInTransition(0)) //!_animatorRef.Animator.GetCurrentAnimatorStateInfo(0).IsName(desiredAnimationState))
            {
                time += Time.deltaTime;
                normalizedTransitionTime = time / transitionDurantion;
                CurrentFrame = frame = (int)(normalizedTransitionTime / (1 / (transitionDurantion * attackData.AnimationClip.frameRate)) + 1);
                //Debug.Log("Counting frame:" + frame);
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();

        while (frame < Math.Floor(maxFrame))
        {
            float normalizedTime = _animatorRef.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            CurrentFrame = frame = (int)(normalizedTime / (1 / maxFrame) + 1);
            //Debug.Log(CurrentFrame);
            //Debug.Log("Max frame:" + Math.Floor(maxFrame));
            //Debug.Log("Counting frame:" + frame);
            yield return null;
        }

        //Debug.Log("End anim");
        StopAllCoroutines();
        EndAttack?.Invoke();
    }

    public void FrameActions(ActionData ActionData)
    {
        if (ActionData.OverridePhysics)
        {
            OnOverridePhysics?.Invoke(ActionData.PhysicsOptions);
        }

        if (ActionData.FrameActionForces.Count > 0)
        {
            foreach (var action in ActionData.FrameActionForces)
            {
                IEnumerator routine = FrameActionRoutine(action.ActionInterval, () => OnApplyForce?.Invoke(action.Force, action.LocalForce));
                //Debug.Log(action.ActionInterval);
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

        AttackData attackData = ActionData as AttackData;

        if (attackData != null)
        {
            if (attackData.FrameActionHitboxes.Length > 0)
            {
                foreach (var attack in attackData.FrameActionHitboxes)
                {
                    IEnumerator routine = FrameActionRoutine(attack.ActionInterval, () => OnRequestCollider?.Invoke(attack.ColliderInfos));
                    //Debug.Log(action.ActionInterval);
                    StartCoroutine(routine);
                }
            }
        }

        RangedAttackData rangedAttackData = ActionData as RangedAttackData;

        if (rangedAttackData != null)
        {
            IEnumerator routine = FrameActionRoutine(rangedAttackData.ShotFrame.ActionInterval, () => OnRequestArrowShoot?.Invoke());
            //Debug.Log(action.ActionInterval);
            StartCoroutine(routine);
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
                //Debug.Log("Action");
                yield break;
            }

            yield return null;
        }
    }

    string GetCleanKey(string key)
    {
        int index = key.IndexOf('(');
        if (index >= 0)
        {
            key = key.Substring(0, index);
        }
        int index1 = key.IndexOf(' ');
        if (index >= 0)
        {
            key = key.Substring(0, index1);
        }

        return key;
    }
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
