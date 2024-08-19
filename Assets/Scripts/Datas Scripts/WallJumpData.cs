using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallJumpData", menuName = "ScriptableObjects/WallJumpDataScriptableObject", order = 1)]
public class WallJumpData : ScriptableObject
{
    [SerializeField] private LayerMask _wallCheckLayerMask;
    [SerializeField] private float _checkDistance = 0.5f;
    [SerializeField] private float _offSetYEmptySpace = 0.5f;

    public LayerMask WallCheckLayerMask => _wallCheckLayerMask;
    public float CheckDistance => _checkDistance;
    public float OffSetYEmptySpace => _offSetYEmptySpace;
}
