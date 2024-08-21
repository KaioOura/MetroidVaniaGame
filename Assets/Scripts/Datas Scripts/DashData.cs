using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashData", menuName = "ScriptableObjects/DashDataScriptableObject", order = 1)]

public class DashData : ScriptableObject
{
    [SerializeField] private float _dashForceX;
    [SerializeField] private float _dashForceY;
    [SerializeField] private float _dashTime;
    [SerializeField] private CapsuleCollider2D _dashCollider;

    public float DashForceX => _dashForceX;
    public float DashForceY => _dashForceY;
    public float DashTime => _dashTime;
    public CapsuleCollider2D DashCollider => _dashCollider;
}
