using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageInfos
{
    [SerializeField] private float _damage;
    [SerializeField] private bool _applyStagger;
    [SerializeField] private bool _localForces;
    [SerializeField] private Vector2 _forces;

    public float Damage => _damage;
    public bool ApplyStagger => _applyStagger;
    public bool LocalForces => _localForces;
    public Vector2 Forces => _forces;

}
