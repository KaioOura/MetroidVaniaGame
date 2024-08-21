using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;


[Serializable]
public class ColliderInfos
{
    [SerializeField] private ColliderTypes _colliderTypes;
    [SerializeField] private bool _isActive;
    [SerializeField] private Vector2 _pos;
    [SerializeField] private float _rot;
    [SerializeField] private Vector2 _scale;

    public ColliderTypes ColliderTypes => _colliderTypes;
    public bool IsActive => _isActive;
    public Vector2 Pos => _pos;
    public float Rot => _rot;
    public Vector2 Scale => _scale;

    [ContextMenu("Copy Transform Values")]
    public void CopySelectedTransformPosition()
    {
        #if UNITY_EDITOR
        if (Selection.activeTransform != null)
        {
            _pos = Selection.activeTransform.localPosition;
            _scale = Selection.activeTransform.localScale;
            _rot = Selection.activeTransform.localEulerAngles.z;
            //Debug.Log($"Copied position: {copiedPosition}");
        }
        else
        {
            Debug.LogWarning("No transform selected in the Scene.");
        }
        #endif
    }
}

public enum ColliderTypes
{
    BoxCollider,
    SphereCollider,
    CapsuleCollider
}
