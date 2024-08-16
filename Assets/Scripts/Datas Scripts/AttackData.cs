using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/AttackDataScriptableObject", order = 1)]
public class AttackData : ActionData
{
    [Header("Attack Data")]

    [SerializeField] protected float _damage;
}
