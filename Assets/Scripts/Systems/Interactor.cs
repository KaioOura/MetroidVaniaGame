using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable _currentTarget;

    public IInteractable CurrentTarget => _currentTarget;

    public event Action<IInteractable> OnTargetChanged;

    private void Update()
    {
        UpdateCurrentTarget();
    }

    private void UpdateCurrentTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableMask);

        IInteractable best = null;
        int bestPriority = int.MinValue;
        float bestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IInteractable interactable))
            {
                int priority = interactable.GetPriority();
                float distance = (hit.transform.position - transform.position).sqrMagnitude;

                if (priority > bestPriority || (priority == bestPriority && distance < bestDistance))
                {
                    best = interactable;
                    bestPriority = priority;
                    bestDistance = distance;
                }
            }
        }

        if (best != _currentTarget)
        {
            _currentTarget?.Deselect();
            _currentTarget = best;
            _currentTarget?.Select();
            OnTargetChanged?.Invoke(_currentTarget);
        }
    }

    public void TryInteract(bool isPressing)
    {
        _currentTarget?.OnInteract(transform.parent);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
#endif
}
